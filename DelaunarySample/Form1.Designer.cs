namespace DelaunarySample
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rotateButton = new System.Windows.Forms.Button();
			this.openGLControl1 = new SharpGL.OpenGLControl();
			((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).BeginInit();
			this.SuspendLayout();
			// 
			// rotateButton
			// 
			this.rotateButton.Location = new System.Drawing.Point(12, 495);
			this.rotateButton.Name = "rotateButton";
			this.rotateButton.Size = new System.Drawing.Size(75, 23);
			this.rotateButton.TabIndex = 1;
			this.rotateButton.Text = "rotate";
			this.rotateButton.UseVisualStyleBackColor = true;
			this.rotateButton.Click += new System.EventHandler(this.rotateButton_Click);
			// 
			// openGLControl1
			// 
			this.openGLControl1.DrawFPS = false;
			this.openGLControl1.Location = new System.Drawing.Point(12, 12);
			this.openGLControl1.Name = "openGLControl1";
			this.openGLControl1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
			this.openGLControl1.RenderContextType = SharpGL.RenderContextType.DIBSection;
			this.openGLControl1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
			this.openGLControl1.Size = new System.Drawing.Size(827, 477);
			this.openGLControl1.TabIndex = 2;
			this.openGLControl1.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl1_OpenGLDraw);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(851, 521);
			this.Controls.Add(this.openGLControl1);
			this.Controls.Add(this.rotateButton);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button rotateButton;
		private SharpGL.OpenGLControl openGLControl1;

	}
}