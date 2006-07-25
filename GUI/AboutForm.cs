using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;


namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class AboutForm : System.Windows.Forms.Form
	{
		private const string RESOURCES_GROUP = "AboutForm";
        private System.Windows.Forms.PictureBox aboutPicture;
        private System.Windows.Forms.Label aboutLabel1;
        private System.Windows.Forms.Button aboutOkButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent(); 
            //this.aboutLabel1.Text = LABEL;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutForm));
			this.aboutPicture = new System.Windows.Forms.PictureBox();
			this.aboutLabel1 = new System.Windows.Forms.Label();
			this.aboutOkButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// aboutPicture
			// 
			this.aboutPicture.Dock = System.Windows.Forms.DockStyle.Top;
			this.aboutPicture.Image = ((System.Drawing.Image)(resources.GetObject("aboutPicture.Image")));
			this.aboutPicture.Location = new System.Drawing.Point(0, 0);
			this.aboutPicture.Name = "aboutPicture";
			this.aboutPicture.Size = new System.Drawing.Size(408, 88);
			this.aboutPicture.TabIndex = 0;
			this.aboutPicture.TabStop = false;
			// 
			// aboutLabel1
			// 
			this.aboutLabel1.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.aboutLabel1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.aboutLabel1.Location = new System.Drawing.Point(8, 88);
			this.aboutLabel1.Name = "aboutLabel1";
			this.aboutLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.aboutLabel1.Size = new System.Drawing.Size(400, 216);
			this.aboutLabel1.TabIndex = 1;
			this.aboutLabel1.Text = Resources.String( RESOURCES_GROUP, "Text" );
			// 
			// aboutOkButton
			// 
			this.aboutOkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.aboutOkButton.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.aboutOkButton.Location = new System.Drawing.Point(144, 304);
			this.aboutOkButton.Name = "aboutOkButton";
			this.aboutOkButton.TabIndex = 2;
			this.aboutOkButton.Text = Resources.String( Resources.GeneralResource, "OK" );
			this.aboutOkButton.Click += new System.EventHandler(this.aboutOkButton_Click);
			// 
			// AboutForm
			// 
			this.AcceptButton = this.aboutOkButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.aboutOkButton;
			this.ClientSize = new System.Drawing.Size(408, 334);
			this.Controls.Add(this.aboutOkButton);
			this.Controls.Add(this.aboutLabel1);
			this.Controls.Add(this.aboutPicture);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.FormBorderStyle = FormBorderStyle.Fixed3D;
			this.Text = Resources.String( RESOURCES_GROUP, "Title" );
			
			this.ResumeLayout(false);

		}
		#endregion

        private void aboutOkButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
	}
}
