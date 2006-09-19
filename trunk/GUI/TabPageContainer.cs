using System.Windows.Forms;

namespace UDonkey.GUI {
 public class TabPageContainer{
   // this is internal to the winforms part
   // GOD there has to be a better way!
   public TabPage page; 

   public TabPageContainer(string name){
     page = new TabPage(name);
   }
   public void AddScheduleDataGrid(ScheduleDataGrid sch){
        page.Controls.Add( sch );
   }
 } 
}
