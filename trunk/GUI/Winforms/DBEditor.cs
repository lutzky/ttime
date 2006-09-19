using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using UDonkey.Logic;
using UDonkey.RepFile;
using UDonkey.DB;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for DBEditor.
	/// </summary>
	public class DBEditor : System.Windows.Forms.UserControl
	{
		private const string RESOURCES_GROUP = "CourseDB";

		private UDonkey.DB.CourseDB cDB;
		private System.Windows.Forms.DataGrid dg1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DBEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
	

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dg1 = new System.Windows.Forms.DataGrid();
			((System.ComponentModel.ISupportInitialize)(this.dg1)).BeginInit();
			this.SuspendLayout();
			// 
			// dg1
			// 
			this.dg1.AlternatingBackColor = System.Drawing.SystemColors.Window;
			this.dg1.BackgroundColor = System.Drawing.Color.Lavender;
			this.dg1.CaptionText = "עורך הקורסים";
			this.dg1.DataMember = "";
			this.dg1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dg1.GridLineColor = System.Drawing.SystemColors.Control;
			this.dg1.HeaderBackColor = System.Drawing.SystemColors.Control;
			this.dg1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dg1.LinkColor = System.Drawing.SystemColors.HotTrack;
			this.dg1.Name = "dg1";
			this.dg1.PreferredColumnWidth = 100;
			this.dg1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.dg1.RowHeaderWidth = 20;
			this.dg1.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
			this.dg1.SelectionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.dg1.Size = new System.Drawing.Size(960, 517);
			this.dg1.TabIndex = 0;
			// 
			// DBEditor
			// 
			//this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(960, 517);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.dg1});
			this.Name = "DBEditor";
			this.Text = "DBEditor";
			this.Load += new System.EventHandler(this.DBEditor_Load);
			//this.Closing += new System.ComponentModel.CancelEventHandler(this.DBEditor_Closing);
			((System.ComponentModel.ISupportInitialize)(this.dg1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void DBEditor_Load(object sender, System.EventArgs e)
		{
			try
			{
				cDB=new CourseDB();
			}
			catch(System.IO.FileNotFoundException)
			{
				MessageBox.Show( null, Resources.String( RESOURCES_GROUP, "xsdFailedMessage1" ), Resources.String( RESOURCES_GROUP, "xsdFailedMessage2" ), MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
				return;
			}
			DataSet aDataSet = cDB.DataSet;
			dg1.SetDataBinding(aDataSet, "Faculty");
		}
		private void DBEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			cDB.SaveXMLData();
		}
	}
}
