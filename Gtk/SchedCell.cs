
using System;
using Gtk;
using Gdk;
using Pango;

class TextViewSample
{

     static void Main ()
     {
          new TextViewSample ();
     }

     TextViewSample ()
     {

          Application.Init ();
          TextTag tag = new TextTag("bold-large");
          tag.Size=12000;
          Gtk.Window win = new Gtk.Window ("TextViewSample");
          win.DeleteEvent += new DeleteEventHandler (OnWinDelete);
          win.SetDefaultSize (200,75);
          
          Gtk.TextView view;
          Gtk.TextBuffer buffer;

          view = new Gtk.TextView ();
          buffer = view.Buffer;

          buffer.Text = "חדו\"א 1ת\nמרצה: פרופסור רשע ט. הור";
          TextIter itr = buffer.GetIterAtOffset(0);
          TextIter itr2 = buffer.GetIterAtOffset(0);
          itr2.ForwardLine();
          buffer.TagTable.Add(tag);
          buffer.ApplyTag(tag,itr,itr2);
          itr = buffer.GetIterAtOffset(0);
          buffer.Insert(itr,"  ");
          itr = buffer.GetIterAtOffset(0);
          buffer.InsertPixbuf(itr,new Pixbuf("images/sphereRed.png"));


          win.Add (view);
          win.ShowAll ();

          Application.Run ();
     }

     void OnWinDelete (object obj, DeleteEventArgs args)
     {
          Application.Quit ();
     }

     void FormatBuffer(TextBuffer buffer){
          //Pango.FontDescription fd = new FontDescription();
          //fd.Weight = Weight.Heavy;
          //tag.FontDesc = fd;
     }
}
    
