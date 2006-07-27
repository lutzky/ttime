using System;
using System.Drawing;
using System.Windows.Forms;
using UDonkey.Logic;

namespace UDonkey.GUI
{
	/// <summary>
	/// ScheduleDataGridColumnStyle is used to give style to each Schedule columns.
	/// </summary>
	public class ScheduleDataGridColumnStyle : DataGridTextBoxColumn
	{
		/// <summary>
		/// The column's (day's) data
		/// </summary>
		private ScheduleDayData mDayData;
		/// <summary>
		/// Basic constructor
		/// </summary>
		/// <param name="dayData">The column's (day's) data</param>
		public ScheduleDataGridColumnStyle( ScheduleDayData dayData ): base()
		{
			mDayData = dayData;
			this.MappingName = mDayData.Day.ToString();
			this.HeaderText  = mDayData.Day.ToString();
		}
		/// <summary>
		/// This was override to not allow edit of a cell
		/// </summary>
		/// <param name="Source"></param>
		/// <param name="Rownum">Cell's row number</param>
		/// <param name="Bounds">Cells' bounds</param>
		/// <param name="ReadOnly">Cell's ReadOnly Property</param>
		/// <param name="InstantText"></param>
		/// <param name="CellIsVisible">Cell's Visible property</param>
		protected override void Edit(CurrencyManager Source ,int Rownum,Rectangle Bounds, bool ReadOnly,string InstantText, bool CellIsVisible) 
		{
		}

		/// <summary>
		/// Was overrided to paint the cell with it's style
		/// </summary>
		/// <param name="g"></param>
		/// <param name="Bounds"></param>
		/// <param name="Source"></param>
		/// <param name="RowNum"></param>
		protected override void Paint(Graphics g,Rectangle Bounds,CurrencyManager Source,int RowNum) 
		{
			Brush backBrush;
			Brush foreBrush;
			string text = string.Empty;
			IScheduleEntry entry = GetColumnValueAtRow(Source, RowNum) as IScheduleEntry;

			if( entry.IsEmpty )
			{//Empty Cell 
				foreBrush = new SolidBrush( Color.White );
                
				if( this.mDayData.FreeDayConstraint )
				{
					backBrush = new SolidBrush( Color.LightBlue );
				}
				else
				{
					backBrush = new SolidBrush( Color.White );
				}
			}
			else
			{//IScheduleEntryBucket, Take its defenitions for Back/Fore color
				// FIXME: need a way to color gui while having seperation
				//backBrush = new SolidBrush( entry.BackColor );
				//foreBrush = new SolidBrush( entry.ForeColor );
				backBrush = new SolidBrush( Color.White );
				foreBrush = new SolidBrush( Color.Black );
				text      = entry.ToString();
			} 
            
			//Use DataGridTextBoxColumn PaintText method to write the text
			
			this.PaintText( g, Bounds, text, backBrush, foreBrush, true );

			if ( RowNum == this.mDayData.StartHourRowConstraint )
			{//At the top of the StartHour
				g.DrawLine( new Pen( Color.Green, 3 ), Bounds.X, Bounds.Y, Bounds.Right, Bounds.Y );
			}
			if ( RowNum == this.mDayData.EndHourRowConstraint )
			{//At the buttom ot the EndHour
				g.DrawLine( new Pen( Color.Blue, 3 ), Bounds.X, Bounds.Y, Bounds.Right, Bounds.Y );
			}
		}

		/// <summary>
		/// Was overrided to paint the cell with it's style
		/// </summary>
		/// <param name="g"></param>
		/// <param name="Bounds"></param>
		/// <param name="Source"></param>
		/// <param name="RowNum"></param>
		/// <param name="AlignToRight"></param>
		protected override void Paint(Graphics g,Rectangle Bounds,CurrencyManager Source,int RowNum,bool AlignToRight) 
		{
			this.Paint( g, Bounds, Source,RowNum );	
		}
        /// <summary>
        /// Was overrided to paint the cell with it's style
        /// </summary>
        /// <param name="g"></param>
        /// <param name="Bounds"></param>
        /// <param name="Source"></param>
        /// <param name="RowNum"></param>
        /// <param name="BackBrush"></param>
        /// <param name="ForeBrush"></param>
        /// <param name="AlignToRight"></param>
        protected override void Paint(Graphics g,Rectangle Bounds,CurrencyManager Source,int RowNum, Brush BackBrush ,Brush ForeBrush ,bool AlignToRight) 
        {
            this.Paint( g, Bounds, Source,RowNum, AlignToRight );				
        }
		protected override int GetMinimumHeight()
		{
			return 40;
		}


	}
}
