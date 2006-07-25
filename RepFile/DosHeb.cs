using System;
using System.IO;
using System.Text;

namespace UDonkey.RepFile
{
	/// <summary>
	/// DosHeb reads DosHeb file's content andtranslate it to Unicode.
	/// It provides some other related services
	/// </summary>
	public class DosHeb
	{
	// boazg: imho this is unclean
        #region Translate Table and EOL
        //http://www.kostis.net/charsets/cp862.htm
        static char[] TRANSLATE_TABLE = {
                                            /*  0*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /* 10*/'\n',' ',' ','\r',' ',' ',' ',' ',' ',' ',
                                            /* 20*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /* 30*/' ',' ',' ','!','\'','#','$','%','&','\'',
                                            /* 40*/')','(','*','+',',','-','.','/','0','1',
                                            /* 50*/'2','3','4','5','6','7','8','9',':',';',
                                            /* 60*/'<','=','>','?','@','A','B','C','D','E',
                                            /* 70*/'F','G','H','I','J','K','L','M','N','O',
                                            /* 80*/'P','Q','R','S','T','U','V','W','X','Y',
                                            /* 90*/'Z','[','\\',']','^','_','\'','a','b','c',
                                            /*100*/'d','e','f','g','h','i','j','k','l','m',
                                            /*110*/'n','o','p','q','r','s','t','u','v','w',
                                            /*120*/'x','y','z','{','|','}','~',' ','א','ב',
                                            /*130*/'ג','ד','ה','ו','ז','ח','ט','י','כ','ך',
                                            /*140*/'ל','מ','ם','נ','ן','ס','ע','פ','ף','צ',
                                            /*150*/'ץ','ק','ר','ש','ת',' ',' ',' ',' ',' ',
                                            /*160*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*170*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*180*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*190*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*200*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*210*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*220*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*230*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*240*/' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',
                                            /*250*/' ',' ',' ',' ',' ',' '};
        static char[] END_OF_LINE = {'\r','\n'};
        #endregion Translate Table and EOL
        #region Static Methods
        /// <summary>
        /// Reads DosHeb file's content andtranslate it to Unicode
        /// </summary>
        /// <param name="fileName">File that contains DosHeb text</param>
        /// <returns>Files input translated to Unicode</returns>
        public static string Translate( string fileName )
        {
            FileStream fileReader = File.OpenRead( fileName ) ;
            byte[] fileBytes = new byte[fileReader.Length];
            //Read the file as bytes to avoid encoding and ruin it
            fileReader.Read( fileBytes, 0, fileBytes.Length );
            fileReader.Close();

            //Translate the file's bytes
            StringBuilder builder = new StringBuilder();
            for( int i = 0 ; i < fileBytes.Length ; ++i )
            {
                //Get the right char from the table by the byte value
                builder.Append( TRANSLATE_TABLE[ fileBytes[i] ] ) ;
            }
            return builder.ToString();
        }
        /// <summary>
        /// Revert the string
        /// </summary>
        /// <param name="input">The string to revert</param>
        /// <returns>The reverted string</returns>
        public static string Revert( string input )
        {
            StringBuilder builder = new StringBuilder();
            DosHeb.Revert( input , builder );
            return builder.ToString();
        }
        public static string Revert( string input, StringBuilder builder )
        {
            char[] tempChars = input.ToCharArray();
            Array.Reverse( tempChars );
            builder.Append( tempChars );
            return builder.ToString();
        }
        public static string RevertLines( string input )
        {
            StringReader translatedReader = new StringReader( input );
            StringBuilder builder = new StringBuilder();
            for( string line = translatedReader.ReadLine();
                line != null;
                line = translatedReader.ReadLine() )
            {
                DosHeb.Revert( line, builder );
                //Add \r\n to the end of each line
                builder.Append( END_OF_LINE );
            }
            return builder.ToString();
        }
        #endregion Static Methods
        #region Constructors
        /// <summary>
        /// Private Constructor to prevent instances of this class
        /// </summary>
		private DosHeb()
		{
		}
        #endregion Constructors
	}
}
