using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for UDonkeyForm.
	/// </summary>
	public class UDonkeyForm : System.Windows.Forms.Form
	{
		private const string RESOURCES_GROUP = "UDonkeyForm";
		//private const string RESOURCES_TEXT = "text";
        private System.Windows.Forms.Button udonkeyOkButton;
        private System.Windows.Forms.Label udonkeyLabel;
        private System.Windows.Forms.PictureBox udonkeyPicture;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UDonkeyForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            //this.udonkeyLabel.Text = UDONKEY_STORY;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UDonkeyForm));
			this.udonkeyOkButton = new System.Windows.Forms.Button();
			this.udonkeyLabel = new System.Windows.Forms.Label();
			this.udonkeyPicture = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// udonkeyOkButton
			// 
			this.udonkeyOkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.udonkeyOkButton.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.udonkeyOkButton.Location = new System.Drawing.Point(248, 152);
			this.udonkeyOkButton.Name = "udonkeyOkButton";
			this.udonkeyOkButton.TabIndex = 3;
			this.udonkeyOkButton.Text = Resources.String( Resources.GeneralResource, "OK" );
			this.udonkeyOkButton.Click += new System.EventHandler(this.udonkeyOkButton_Click);
			// 
			// udonkeyLabel
			// 
			this.udonkeyLabel.Dock = System.Windows.Forms.DockStyle.Right;
			this.udonkeyLabel.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.udonkeyLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.udonkeyLabel.Location = new System.Drawing.Point(176, 0);
			this.udonkeyLabel.Name = "udonkeyLabel";
			this.udonkeyLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.udonkeyLabel.Size = new System.Drawing.Size(384, 182);
			this.udonkeyLabel.TabIndex = 4;
			this.udonkeyLabel.Text = Resources.String( RESOURCES_GROUP, "Text" );
			// udonkeyPicture
			// 
			this.udonkeyPicture.Dock = System.Windows.Forms.DockStyle.Left;
			this.udonkeyPicture.Image = ((System.Drawing.Image)(resources.GetObject("udonkeyPicture.Image")));
			this.udonkeyPicture.Location = new System.Drawing.Point(0, 0);
			this.udonkeyPicture.Name = "udonkeyPicture";
			this.udonkeyPicture.Size = new System.Drawing.Size(170, 182);
			this.udonkeyPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.udonkeyPicture.TabIndex = 5;
			this.udonkeyPicture.TabStop = false;
			// 
			// UDonkeyForm
			// 
			this.AcceptButton = this.udonkeyOkButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.udonkeyOkButton;
			this.ClientSize = new System.Drawing.Size(560, 182);
			this.Controls.Add(this.udonkeyOkButton);
			this.Controls.Add(this.udonkeyPicture);
			this.Controls.Add(this.udonkeyLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UDonkeyForm";
			this.Text = Resources.String( RESOURCES_GROUP, "Title" );
			this.ResumeLayout(false);

		}
		#endregion

        private void udonkeyOkButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
	}
}
