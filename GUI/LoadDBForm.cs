using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using UDonkey.DB;
using UDonkey.RepFile;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for loadDB.
	/// </summary>
	public class LoadDBForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RadioButton radioButton5;
		
		private CourseDB cDB;
		private string msWorkingFolder;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LoadDBForm(CourseDB courseDB, string workingfolder)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			msWorkingFolder = workingfolder;
			cDB	= courseDB;
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
			this.label1 = new System.Windows.Forms.Label();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.button1 = new System.Windows.Forms.Button();
			this.radioButton5 = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label1.Size = new System.Drawing.Size(208, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = " קובץ בסיס נתונים  לא נמצא. בחר אפשרות:";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(56, 56);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioButton2.Size = new System.Drawing.Size(168, 24);
			this.radioButton2.TabIndex = 2;
			this.radioButton2.Text = "בחר קובץ בסיס נתונים ";
			// 
			// radioButton4
			// 
			this.radioButton4.Checked = true;
			this.radioButton4.Location = new System.Drawing.Point(16, 32);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioButton4.Size = new System.Drawing.Size(208, 24);
			this.radioButton4.TabIndex = 4;
			this.radioButton4.TabStop = true;
			this.radioButton4.Text = "הורד מהרשת (מצריך חיבור אינטרנט)";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(16, 104);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(40, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "בצע";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// radioButton5
			// 
			this.radioButton5.Location = new System.Drawing.Point(80, 80);
			this.radioButton5.Name = "radioButton5";
			this.radioButton5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioButton5.Size = new System.Drawing.Size(144, 24);
			this.radioButton5.TabIndex = 6;
			this.radioButton5.Text = "סגור את התוכנה";
			// 
			// LoadDBForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(240, 134);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.radioButton5,
																		  this.button1,
																		  this.radioButton4,
																		  this.radioButton2,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "LoadDBForm";
			this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.Text = "טעינת בסיס נתונים";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			if ( radioButton2.Checked )
			{
				System.Windows.Forms.FileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
				fileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
				fileDialog.CheckFileExists = true;
				fileDialog.Filter = "Zipped Rep File(*.zip)|*.zip|Rep File|*|DataBase(*.xml)|*.xml";
				fileDialog.AddExtension = true;
				fileDialog.FilterIndex = 1;
				fileDialog.FileName = "";
				fileDialog.FileOk +=new CancelEventHandler(FileOk);             
				fileDialog.ShowDialog();
				return;
			}
			else if (radioButton4.Checked)
			{
				try
				{
					this.Hide();
					cDB.AutoUpdate();
					RepToXML.Convert("REPY", msWorkingFolder + "\\" + CourseDB.DEFAULT_DB_FILE_NAME);
					cDB.Load( "mainDB.xml" );
					this.Close();
					return;
				}
				catch
				{
					System.Windows.Forms.MessageBox.Show("AutoUpdate Failed");
				}
			}
			else if (radioButton5.Checked)
			{
				this.Close();
				return;
			}
			this.Show();
		}
		
		private void FileOk(object sender, CancelEventArgs e)
		{
			FileDialog fileDialog = (FileDialog) sender;
			this.Hide();
			string fileName = fileDialog.FileName;
			try
			{
				if (fileName.EndsWith("REPFILE.zip"))
				{
					cDB.OpenLocalZip(fileName);
					RepToXML.Convert("REPY", msWorkingFolder + "\\" + CourseDB.DEFAULT_DB_FILE_NAME);
				}
				else if (fileName.EndsWith("REPY"))
				{
					RepToXML.Convert("REPY", msWorkingFolder + "\\" + CourseDB.DEFAULT_DB_FILE_NAME);
				}
				else if (fileName.EndsWith("mainDB.xml"))
				{
					System.IO.File.Copy(fileName ,msWorkingFolder + "\\" + CourseDB.DEFAULT_DB_FILE_NAME, true);
				}
				cDB.Load( CourseDB.DEFAULT_DB_FILE_NAME );
				this.Close();
			}
			catch(System.IO.FileNotFoundException)
			{
				System.Windows.Forms.MessageBox.Show("File Not Found");
			}
			this.Show();
		}
	}
}
