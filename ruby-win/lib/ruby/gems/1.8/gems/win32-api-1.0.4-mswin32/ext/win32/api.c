#include <ruby.h>
#include <windows.h>

#define MAX_BUF 1024
#define WINDOWS_API_VERSION "1.0.4"

#define _T_VOID     0
#define _T_LONG     1
#define _T_POINTER  2
#define _T_INTEGER  3
#define _T_CALLBACK 4

VALUE cAPIError, cCallbackError;

/* Helper function that converts the error number returned by GetLastError()
 * into a human readable string.  Internal use only.
 */
char* StringError(DWORD dwError){
   LPVOID lpMsgBuf;
   static char buf[MAX_PATH];
   DWORD dwLen;

   /* Assume ASCII error messages from the Windows API */
   dwLen = FormatMessageA(
      FORMAT_MESSAGE_ALLOCATE_BUFFER |
      FORMAT_MESSAGE_FROM_SYSTEM |
      FORMAT_MESSAGE_IGNORE_INSERTS,
      NULL,
      dwError,
      MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
      (LPSTR)&lpMsgBuf,
      0,
      NULL
   );

   if(!dwLen)
      rb_raise(cAPIError, "Attempt to format message failed");

   memset(buf, 0, MAX_PATH);

   /* remove \r\n */
#ifdef HAVE_STRNCPY_S
   strncpy_s(buf, MAX_PATH, lpMsgBuf, dwLen - 2);
#else
   strncpy(buf, lpMsgBuf, dwLen - 2);
#endif

   LocalFree(lpMsgBuf);

   return buf;
}

/*
 * call-seq:
 *    Win32::API::Callback.new(prototype, return='L')
 * 
 * Creates and returns a new Win32::API::Callback object. The +function+ is
 * the name of the Windows function.
 * 
 * The +prototype+ is the function prototype for the callback function. This
 * is a string. The possible valid characters are 'I' (integer), 'L' (long),
 * 'V' (void), or 'P' (pointer). Unlike API objects, API::Callback objects
 * do not have a default prototype.
 * 
 * The +return+ argument is the return type for the callback function. The
 * valid characters are the same as for the +prototype+. The default is
 * 'L' (long).
 */
static VALUE callback_init(int argc, VALUE* argv, VALUE self)
{
   VALUE v_proto, v_return, v_proc;
   int i;
    
   rb_scan_args(argc, argv, "11", &v_proto, &v_return);
   
   /* Validate prototype characters */
   for(i = 0; i < RSTRING(v_proto)->len; i++){
      switch(RSTRING(v_proto)->ptr[i]){
         case 'I': case 'L': case 'P': case 'V':
            break;
         default:
            rb_raise(cCallbackError, "Illegal prototype '%c'",
               RSTRING(v_proto)->ptr[i]);
      }
   }

   if(NIL_P(v_return) || RARRAY(v_return)->len == 0)
      v_return = rb_str_new2("L");

   if(rb_block_given_p())
      v_proc = rb_block_proc();
   else
      v_proc = Qnil;

    rb_iv_set(self, "@function", v_proc);
    rb_iv_set(self, "@prototype", v_proto);
    rb_iv_set(self, "@return_type", v_return);
    
    return self;
}


typedef struct {
    HANDLE library;
    FARPROC function;
    int return_type;
    int prototype[16];
} Win32API;

static void api_free(Win32API* ptr){
   if(ptr->library)
      FreeLibrary(ptr->library);

   free(ptr);
}

static VALUE api_allocate(VALUE klass){
   Win32API* ptr = malloc(sizeof(Win32API));
   return Data_Wrap_Struct(klass, 0, api_free, ptr);
}

/*
 * call-seq:
 *    Win32::API.new(function, prototype='V', return='L', dll='kernel32')
 * 
 * Creates and returns a new Win32::API object. The +function+ is the name
 * of the Windows function.
 * 
 * The +prototype+ is the function prototype for +function+. This can be a
 * string or an array of characters.  The possible valid characters are 'I'
 * (integer), 'L' (long), 'V' (void), 'P' (pointer), or 'K' (callback).
 * The default is void ('V').
 * 
 * The +return+ argument is the return type for the function.  The valid
 * characters are the same as for the +prototype+.  The default is 'L' (long).
 * 
 * The +dll+ is the name of the DLL file that the function is exported from.
 * The default is 'kernel32'.
 * 
 * If the function cannot be found then an API::Error is raised (a subclass
 * of RuntimeError).
 */
static VALUE api_init(int argc, VALUE* argv, VALUE self)
{
   HMODULE hLibrary;
   FARPROC fProc;
   Win32API* ptr;
   int i;
   VALUE v_proc, v_proto, v_return, v_dll, v_bool, v_name;
   
   rb_scan_args(argc, argv, "13", &v_proc, &v_proto, &v_return, &v_dll);
   
   Data_Get_Struct(self, Win32API, ptr);

   /* Convert a string prototype to an array of characters */
   if(rb_respond_to(v_proto, rb_intern("split")))
      v_proto = rb_str_split(v_proto, "");

   /* Set an arbitrary limit of 16 parameters */
   if(16 < RARRAY(v_proto)->len)
      rb_raise(rb_eArgError, "too many parameters: %d\n", RARRAY(v_proto)->len);

   /* Convert a nil or empty prototype to 'V' (void) automatically */
   if(NIL_P(v_proto) || RARRAY(v_proto)->len == 0){
      v_proto = rb_ary_new();
      rb_ary_push(v_proto, rb_str_new2("V"));
   }
   
   /* Set the default dll to 'kernel32' */
   if(NIL_P(v_dll))
   	v_dll = rb_str_new2("kernel32");
   	
   /* Set the default return type to 'L' (DWORD) */
   if(NIL_P(v_return))
   	v_return = rb_str_new2("L");

   SafeStringValue(v_dll);
   SafeStringValue(v_proc);

   hLibrary = LoadLibrary(TEXT(RSTRING(v_dll)->ptr));

   /* The most likely cause of failure is a bad DLL load path */
   if(!hLibrary){
      rb_raise(cAPIError, "LoadLibrary() function failed for '%s': %s",
         RSTRING(v_dll)->ptr,
         StringError(GetLastError())
      );
   }

   ptr->library = hLibrary;

   /* Attempt to get the function.  If it fails, try again with an 'A'
    * appended.  If that fails, try again with a 'W' appended.  If that
    * still fails, raise an API::Error.
    */
   fProc = GetProcAddress(hLibrary, TEXT(RSTRING(v_proc)->ptr));

   if(!fProc){
      VALUE v_ascii = rb_str_new3(v_proc);
      v_ascii = rb_str_cat(v_ascii, "A", 1);
      fProc = GetProcAddress(hLibrary, TEXT(RSTRING(v_ascii)->ptr));

      if(!fProc){
         VALUE v_unicode = rb_str_new3(v_proc);
         v_unicode = rb_str_cat(v_unicode, "W", 1);
         fProc = GetProcAddress(hLibrary, TEXT(RSTRING(v_unicode)->ptr));

         if(!fProc){
            rb_raise(
               cAPIError,
               "GetProcAddress() failed for '%s', '%s' and '%s': %s",
               RSTRING(v_proc)->ptr,
               RSTRING(v_ascii)->ptr,
               RSTRING(v_unicode)->ptr,
               StringError(GetLastError())
            );
         }
      }
   }

   ptr->function = fProc;

   /* Push the numeric prototypes onto our int array for later use. */
   for(i = 0; i < RARRAY(v_proto)->len; i++){
      SafeStringValue(RARRAY(v_proto)->ptr[i]);
      switch(*(char*)StringValuePtr(RARRAY(v_proto)->ptr[i])){
         case 'L':
            ptr->prototype[i] = _T_LONG;
            break;
         case 'P':
            ptr->prototype[i] = _T_POINTER;
            break;
         case 'I': case 'B':
            ptr->prototype[i] = _T_INTEGER;
            break;
         case 'V':
            ptr->prototype[i] = _T_VOID;
            break;
         case 'K':
            ptr->prototype[i] = _T_CALLBACK;
            break;
         default:
            rb_raise(cAPIError, "Illegal prototype '%s'", RARRAY(v_proto)->ptr[i]);
      }
   }

   /* Store the return type for later use.  Automatically convert empty
    * strings or nil to type void.
    */
   if(NIL_P(v_return) || RSTRING(v_return)->len == 0){
      v_return = rb_str_new2("V");
      ptr->return_type = _T_VOID;
   }
   else{
      SafeStringValue(v_return);
      switch(*RSTRING(v_return)->ptr){
         case 'L':
            ptr->return_type = _T_LONG;
            break;
         case 'P':
            ptr->return_type = _T_POINTER;
            break;
         case 'I': case 'B':
            ptr->return_type = _T_INTEGER;
            break;
         case 'V':
            ptr->return_type = _T_VOID;
            break;
         default:
            rb_raise(cAPIError, "Illegal prototype '%s'", RARRAY(v_proto)->ptr[i]);
      }
   }

   rb_iv_set(self, "@dll_name", v_dll);
   rb_iv_set(self, "@function_name", v_proc);
   rb_iv_set(self, "@prototype", v_proto);
   rb_iv_set(self, "@return_type", v_return);

   return self;
}

typedef struct {
   DWORD params[1];
} PARAM;

static VALUE ActiveCallback; 

DWORD CallbackFunction(PARAM param)
{
    VALUE v_proto, v_return, v_proc, v_retval;
    VALUE argv[16];
    int i, argc;
    char *a_proto;
    char *a_return;
    
    if(!NIL_P(ActiveCallback)){
        v_proto = rb_iv_get(ActiveCallback, "@prototype");
        a_proto = RSTRING(v_proto)->ptr;
        
        v_return = rb_iv_get(ActiveCallback, "@return_type");
        a_return = RSTRING(v_return)->ptr;
        
        v_proc = rb_iv_get(ActiveCallback, "@function");
        argc = RSTRING(v_proto)->len;
        
        for(i=0; i < RSTRING(v_proto)->len; i++){
           argv[i] = Qnil;
           switch(a_proto[i]){
  	           case 'L':
  	              argv[i] = ULONG2NUM(param.params[i]); 
  	              break;
  	           case 'P':
  	              if(param.params[i])
  	                 argv[i] = rb_str_new2((char *)param.params[i]);
  	              break;
  	           case 'I':
  	              argv[i] = INT2NUM(param.params[i]);
  	              break;
  	           default:
  	              rb_raise(cCallbackError, "Illegal prototype '%c'", a_proto[i]);
            }
        }
        
        v_retval = rb_funcall2(v_proc, rb_intern("call"), argc, argv);
        
        /* Handle true and false explicitly, as some CALLBACK functions
         * require TRUE or FALSE to break out of loops, etc.
         */
        if(v_retval == Qtrue) 
           return TRUE;
        else if(v_retval == Qfalse) 
           return FALSE;
        
        switch (*a_return) {
           case 'I':
              return NUM2INT(v_retval);
              break;
           case 'L':
              return NUM2ULONG(v_retval);    
              break;
           case 'P':
	           if(NIL_P(v_retval)){
	              return 0;
	           }
	           else if (FIXNUM_P(v_retval)){
	              return NUM2ULONG(v_retval);
	           }
	           else{
	              StringValue(v_retval);
	              rb_str_modify(v_retval);
	              return (unsigned long)StringValuePtr(v_retval);
	           }		    
	           break;
        }        
    }
    
    return 0;
}

/*
 * call-seq:
 *    Win32::API#call(arg1, arg2, ...)
 * 
 * Calls the function pointer with the given arguments (if any). Note that,
 * while this method will catch some prototype mismatches (raising a TypeError
 * in the process), it is not fulproof.  It is ultimately your job to make
 * sure the arguments match the +prototype+ specified in the constructor.
 * 
 * For convenience, nil is converted to NULL, true is converted to TRUE (1)
 * and false is converted to FALSE (0).
 */
static VALUE api_call(int argc, VALUE* argv, VALUE self){
   VALUE v_proto, v_args, v_arg, v_return;
   Win32API* ptr;
   unsigned long return_value;
   int i = 0;

   struct{
      unsigned long params[16];
   } param;

   Data_Get_Struct(self, Win32API, ptr);

   rb_scan_args(argc, argv, "0*", &v_args);

   v_proto = rb_iv_get(self, "@prototype");

   /* For void prototypes, allow either no args or an explicit nil */
   if(RARRAY(v_proto)->len != RARRAY(v_args)->len){
      char* c = StringValuePtr(RARRAY(v_proto)->ptr[0]);
      if(!strcmp(c, "V")){
         rb_ary_push(v_args, Qnil);
      }
      else{
         rb_raise(rb_eArgError,
            "wrong number of parameters: expected %d, got %d",
            RARRAY(v_proto)->len, RARRAY(v_args)->len
         );
      }
   }

   for(i = 0; i < RARRAY(v_proto)->len; i++){
      v_arg = RARRAY(v_args)->ptr[i];

      /* Convert nil to NULL.  Otherwise convert as appropriate. */
      if(NIL_P(v_arg))
         param.params[i] = (unsigned long)NULL;
      else if(v_arg == Qtrue)
         param.params[i] = TRUE;
      else if(v_arg == Qfalse)
         param.params[i] = FALSE;
      else
         switch(ptr->prototype[i]){
            case _T_LONG:
               param.params[i] = NUM2ULONG(v_arg);
               break;
            case _T_INTEGER:
               param.params[i] = NUM2INT(v_arg);
               break;
            case _T_POINTER:
               if(FIXNUM_P(v_arg)){
                  param.params[i] = NUM2ULONG(v_arg);
               }
               else{
                  StringValue(v_arg);
                  rb_str_modify(v_arg);
                  param.params[i] = (unsigned long)StringValuePtr(v_arg);
               }
               break;
            case _T_CALLBACK:
               ActiveCallback = v_arg;
               param.params[i] = (LPARAM)CallbackFunction;	
               break;
            default:
               param.params[i] = NUM2ULONG(v_arg);
         }
   }

   /* Call the function, get the return value */
   return_value = ptr->function(param);

   /* Return the appropriate type based on the return type specified
    * in the constructor.
    */
   switch(ptr->return_type){
      case _T_INTEGER:
         v_return = INT2NUM(return_value);
         break;
      case _T_LONG:
         v_return = LONG2NUM(return_value);
         break;
      case _T_VOID:
         v_return = Qnil;
         break;
      case _T_POINTER:
         if(!return_value)
            v_return = Qnil;
         else
            v_return = rb_str_new2((TCHAR*)return_value);
         break;
      default:
         v_return = INT2NUM(0);
   }

   return v_return;
}

/* 
 * Wraps the Windows API functions in a Ruby interface.
 */
void Init_api(){
   VALUE mWin32, cAPI, cCallback;
   
   /* Modules and Classes */
   
   /* The Win32 module serves as a namespace only */
   mWin32 = rb_define_module("Win32");
   
   /* The API class encapsulates a function pointer to Windows API function */
   cAPI = rb_define_class_under(mWin32, "API", rb_cObject);
   
   /* The API::Callback class encapsulates a Windows CALLBACK function */ 
   cCallback = rb_define_class_under(cAPI, "Callback", rb_cObject);
   
   /* The API::Error class is raised if the constructor fails */
   cAPIError = rb_define_class_under(cAPI, "Error", rb_eRuntimeError);

   /* The API::Callback::Error class is raised if the constructor fails */
   cCallbackError = rb_define_class_under(cCallback, "Error", rb_eRuntimeError);

   /* Miscellaneous */
   rb_define_alloc_func(cAPI, api_allocate);
   
   /* Win32::API Instance Methods */
   rb_define_method(cAPI, "initialize", api_init, -1);
   rb_define_method(cAPI, "call", api_call, -1);

   rb_define_method(cCallback, "initialize", callback_init, -1);
   
   /* The name of the DLL that exports the API function */
   rb_define_attr(cAPI, "dll_name", 1, 0);
   
   /* The name of the function */
   rb_define_attr(cAPI, "function_name", 1, 0);
   
   /* The prototype, returned as an array of characters */
   rb_define_attr(cAPI, "prototype", 1, 0);
   
   /* The return type, returned as a single character, P, L, I, V or B */
   rb_define_attr(cAPI, "return_type", 1, 0);
   
   /* Win32::API::Callback Instance Methods */
   
   /* The prototype, returned as an array of characters */
   rb_define_attr(cCallback, "prototype", 1, 0);
      
   /* The return type, returned as a single character, P, L, I, V or B */
   rb_define_attr(cCallback, "return_type", 1, 0);
 
   /* Constants */
   
   /* 1.0.0: The version of this library, returned as a String */
   rb_define_const(cAPI, "VERSION", rb_str_new2(WINDOWS_API_VERSION));
}
