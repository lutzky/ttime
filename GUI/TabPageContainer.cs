using System.Windows.forms;

namespace UDonkey.GUI {
 class TabPageContainer{
   // this is internal to the winforms part
   // GOD there has to be a better way!
   public TabPage page {get;};

   public TabPageContainer(string name){
     page = new TabPage(name);
   }
   public AddScheduleDataGrid(ScheduleDataGrid sch){
        page.Controls.Add( sch );
   }
 } 
}
