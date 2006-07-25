

namespace UDonkey.Logic.SystemDependent
{
  int Column;
  class CoulumnEventWrapper 
  {
    CoulumnEventWrapper( System.Windows.Forms.ColumnClickEventArgs e){
      Column = e.Column;

    } 
  }
}
