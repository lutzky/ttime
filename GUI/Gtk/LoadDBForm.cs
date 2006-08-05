//
using System;
using Gtk;
using GtkSharp;
using Glade;
using UDonkey.DB;
using UDonkey.Logic;

namespace UDonkey.GUI
{
  public class LoadDBForm 
  {
#region Glade Widgets
    [Widget] RadioButton remoteRadioButton;
    [Widget] RadioButton openXMLRadioButton;
    [Widget] RadioButton closeRadioButton;
    [Widget] Window LoadDBFormWindow;
    [Widget] Button executeButton;
#endregion

    FileSelection fs; // bad news - fileChooser is out of
                      // the question (not in ubuntu)
	
		private CourseDB cDB;
		private string msWorkingFolder;
		private const string RESOURCES_GROUP = "CourseDB";

    public static void Start(CourseDB courseDB, string workingfolder){
      new LoadDBForm(courseDB,workingfolder);
    }
		public LoadDBForm(CourseDB courseDB, string workingfolder)
    {
			msWorkingFolder = workingfolder;
			cDB	= courseDB;

      Application.Init();
      Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "LoadDBFormWindow", null);
      gxml.Autoconnect (this);
      Application.Run();
    }

    protected void onExecuteButtonClicked (object o, EventArgs args)
    {
      int i=0;
      if(remoteRadioButton.Active){
        updateFromWeb(); 
      } else if(openXMLRadioButton.Active){
        // parse db from rep or XML
          fs = new FileSelection ("Choose a file");
          fs.Response += new ResponseHandler(FileOk);
          fs.Run();
      } else if(closeRadioButton.Active)
      {
        Environment.Exit(0);
      }
      return;
    }


// finction uses things not implemented in currnt disto's
/*
    string getFileName(){
      string title = Catalog.GetString ("Open");
      Gtk.FileChooserAction action = Gtk.FileChooserAction.open;
      FileChooserDialog fileDialog = new FileChooserDialog (
          title,
          null,
          action);
      fileDialog.SetCurrentFolder(System.IO.Directory.GetCurrentDirectory());

        // zip filter
      FileFilter zipFilter = new FileFilter();
      zipFilter.Name = "Zipped Rep File(*.zip)";
      zipFilter.AddMimeType("application/zip");

      //DataBase filter
      FileFilter dbFilter = new FileFilter();
      dbFilter.Name = "DataBase(*.xml)";
      dbFilter.AddMimeType("text/xml");

      // allfiles
      FileFilter allFilter = new FileFilter();
      allFilter.Name = "Rep File / all files";
      allFilter.AddPattern("*");

      //apply filter
      fileDialog.AddFilter(zipFilter);
      fileDialog.AddFilter(dbFilter);
      fileDialog.AddFilter(allFilter);

      fileDialog.AddButton (Stock.Cancel, ResponseType.Cancel);
      fileDialog.AddButton (Stock.Open, ResponseType.Ok);

      // show dialog and wait for results
      int ret = fileDialog.run();

      if ((ResponseType) ret == ResponseType.Ok) {
        // user pressed ok. we will call closing func
        return fileDialog.uri; // bad name, from UDnk
      }
      return null;

    }
    */

    // ok button presser
		private void FileOk(object o, ResponseArgs args)
		{
			LoadDBFormWindow.HideAll();
      fs.HideAll();
      string fileName = fs.Filename;
			try
			{
        LoadDBFormLogic.updateFromFile(cDB,msWorkingFolder,fileName);
				LoadDBFormWindow.Destroy();
			}
			catch(System.IO.FileNotFoundException)
			{
				CommonDialogs.errorDialog("File Not Found");
        LoadDBFormWindow.ShowAll();
			}
		}

    // do whatever to update the we forms
    // FIXME: to much logic here for my tase, 
    // move some to Logic
    void updateFromWeb(){
      	try
				{
					LoadDBFormWindow.HideAll();
					try {
					  LoadDBFormLogic.updateFromWeb(cDB,msWorkingFolder);
					}
					catch(System.Net.WebException)
					{
            // these resource things are just bad!!!
						CommonDialogs.errorDialog(Resources.String( RESOURCES_GROUP, "InternetFailedMessage1" ));
            Environment.Exit(0);
						return;
					}
					LoadDBFormWindow.Destroy();
					return;
				}
				catch
				{
					CommonDialogs.errorDialog("AutoUpdate Failed");
          LoadDBFormWindow.ShowAll();
				}

    }


    /*
       [STAThread]
       static void Main(string[] args) 
       {
       new LoadDBFormCon();
       }
       */
  }
}
