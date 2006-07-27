using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using UDonkey.RepFile;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for RepFileConvertForm.
	/// </summary>
	public class RepFileConvertForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RepFileConvertForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.label1.Text = string.Format("{0}\n{1}",
				"כעת מתבצעת המרת הקובץ",
				"התהליך לוקח כדקה וחצי ומתרחש רק פעם אחת");
			RepFile.RepToXML.StartConvertion += new EventHandler(RepToXML_StartConvertion);
			RepFile.RepToXML.Progress += new UDonkey.RepFile.RepToXML.ConvertProgress(RepToXML_Progress);
			
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
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(8, 56);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(296, 24);
			this.progressBar1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 32);
			this.label1.TabIndex = 1;
			// 
			// RepFileConvertForm
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(312, 94);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1,
																		  this.progressBar1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RepFileConvertForm";
			this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.Text = "המרת קובץ Rep";
			this.ResumeLayout(false);

		}
		#endregion

		private void RepToXML_Progress(int precentage)
		{
			this.progressBar1.Value = precentage;
			if( precentage == 100 )
			{
				this.Close();
			}
			this.Refresh();
		}

		private void RepToXML_StartConvertion(object sender, EventArgs e)
		{
			System.Threading.Thread thread = new System.Threading.Thread( 
				new System.Threading.ThreadStart( this.ShowNow ) );
			thread.Start();
		}

		private void ShowNow()
		{
			this.ShowDialog();
		}
	}
}
