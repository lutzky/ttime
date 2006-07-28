using System;
using Gtk;
using GtkSharp;
using Glade;


namespace UDonkey.GUI
{
  class CommonDialogs{
    public static int errorDialog(string error){

      MessageDialog md = new MessageDialog (null, 
          DialogFlags.DestroyWithParent,
          MessageType.Error, 
          ButtonsType.Close, error);

      int result = md.Run ();
      md.Destroy();
      return result;
    }
  }
}
