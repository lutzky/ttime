using UDonkey;
using UDonkey.Logic;
using UDonkey.DB;
using UDonkey.RepFile;

namespace UDonkey.Logic{
  // this class contains 2 static methods used in LoadDBForm classes
  public class LoadDBFormLogic {
    static public void updateFromWeb(CourseDB cDB, string msWorkingFolder) {
      cDB.AutoUpdate();
      RepToXML.Convert("REPY", msWorkingFolder + "\\" + CourseDB.DEFAULT_DB_FILE_NAME);
      cDB.Load( Constants.MainDB);

      return;
    }
    public static void updateFromFile(CourseDB cDB, string msWorkingFolder,string fileName){
      if (fileName.EndsWith("REPFILE.zip")){
        cDB.OpenLocalZip(fileName);
        RepToXML.Convert("REPY", msWorkingFolder + "\\" + CourseDB.DEFAULT_DB_FILE_NAME);
      }
      else if (fileName.EndsWith("REPY"))
      {
        RepToXML.Convert("REPY", msWorkingFolder + "\\" + CourseDB.DEFAULT_DB_FILE_NAME);
      }
      else if (fileName.EndsWith(Constants.MainDB))
      {
        System.IO.File.Copy(fileName ,msWorkingFolder + "\\" + CourseDB.DEFAULT_DB_FILE_NAME, true);
      }
      cDB.Load( CourseDB.DEFAULT_DB_FILE_NAME );

    }
  }
}

