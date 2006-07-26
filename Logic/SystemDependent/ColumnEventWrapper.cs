

namespace UDonkey.Logic.SystemDependent
{
  
  class CoulumnEventWrapper 
  {
    public int Column;
    CoulumnEventWrapper( System.Windows.Forms.ColumnClickEventArgs e){
        Column = e.Column;

     } 
  }
}
