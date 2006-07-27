//
using System;
using Gtk;
using GtkSharp;
using Glade;


namespace UDonkey.GUI
{
  public class LoadDBFormCon 
  {
    #region Glade Widgets
    [Widget] Gtk.RadioButton remoteRadioButton;
    [Widget] Gtk.RadioButton openXMLRadioButton;
    [Widget] Gtk.RadioButton closeRadioButton;
    [Widget] Gtk.Window LoadDBForm;
    [Widget] Gtk.Button executeButton;


    #endregion

    public LoadDBFormCon() 
    {
      Application.Init();
      Glade.XML gxml = new Glade.XML("udonkey.glade", "LoadDBForm", null);
      gxml.Autoconnect (this);
      Application.Run();
    }

    protected void onExecuteButtonClicked (object o, EventArgs args)
    {
      int i=0;
      if(remoteRadioButton.Active){
        i=1;
      } else if(openXMLRadioButton.Active){
        i=2;
      } else if(closeRadioButton.Active)
      {
        i=3;
      }
      System.Console.WriteLine(""+i);
    }
    [STAThread]
    static void Main(string[] args) 
    {
      new LoadDBFormCon();
    }
  }
}
