
namespace SteganographyProject
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClick = new System.Windows.Forms.Button();
            this.lblClick = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.encryptButton = new System.Windows.Forms.Button();
            this.decryptButton = new System.Windows.Forms.Button();
            this.sourcePicture = new System.Windows.Forms.TextBox();
            this.sourceMessage = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.asmEncryptButton = new System.Windows.Forms.Button();
            this.asmDecryptButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClick
            // 
            this.btnClick.Location = new System.Drawing.Point(129, 72);
            this.btnClick.Name = "btnClick";
            this.btnClick.Size = new System.Drawing.Size(94, 29);
            this.btnClick.TabIndex = 0;
            this.btnClick.Text = "Enter";
            this.btnClick.UseVisualStyleBackColor = true;
            this.btnClick.Click += new System.EventHandler(this.btnClick_Click);
            // 
            // lblClick
            // 
            this.lblClick.AutoSize = true;
            this.lblClick.Font = new System.Drawing.Font("Segoe UI Black", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblClick.Location = new System.Drawing.Point(149, 385);
            this.lblClick.Name = "lblClick";
            this.lblClick.Size = new System.Drawing.Size(505, 35);
            this.lblClick.TabIndex = 1;
            this.lblClick.Text = "Welcome in steganography application!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(479, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(294, 335);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(42, 178);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(94, 29);
            this.encryptButton.TabIndex = 3;
            this.encryptButton.Text = "Encrypt";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // decryptButton
            // 
            this.decryptButton.Location = new System.Drawing.Point(42, 224);
            this.decryptButton.Name = "decryptButton";
            this.decryptButton.Size = new System.Drawing.Size(94, 29);
            this.decryptButton.TabIndex = 4;
            this.decryptButton.Text = "Decrypt";
            this.decryptButton.UseVisualStyleBackColor = true;
            this.decryptButton.Click += new System.EventHandler(this.decryptButton_Click);
            // 
            // sourcePicture
            // 
            this.sourcePicture.Location = new System.Drawing.Point(12, 27);
            this.sourcePicture.Name = "sourcePicture";
            this.sourcePicture.PlaceholderText = "Picture sourcepath";
            this.sourcePicture.Size = new System.Drawing.Size(339, 27);
            this.sourcePicture.TabIndex = 5;
            // 
            // sourceMessage
            // 
            this.sourceMessage.Location = new System.Drawing.Point(12, 132);
            this.sourceMessage.Name = "sourceMessage";
            this.sourceMessage.PlaceholderText = "Message";
            this.sourceMessage.Size = new System.Drawing.Size(339, 27);
            this.sourceMessage.TabIndex = 6;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(116, 280);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(118, 29);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save picture";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // asmEncryptButton
            // 
            this.asmEncryptButton.Location = new System.Drawing.Point(200, 178);
            this.asmEncryptButton.Name = "asmEncryptButton";
            this.asmEncryptButton.Size = new System.Drawing.Size(151, 29);
            this.asmEncryptButton.TabIndex = 8;
            this.asmEncryptButton.Text = "Encrypt using ASM";
            this.asmEncryptButton.UseVisualStyleBackColor = true;
            this.asmEncryptButton.Click += new System.EventHandler(this.asmEncryptButton_Click);
            // 
            // asmDecryptButton
            // 
            this.asmDecryptButton.Location = new System.Drawing.Point(200, 224);
            this.asmDecryptButton.Name = "asmDecryptButton";
            this.asmDecryptButton.Size = new System.Drawing.Size(151, 29);
            this.asmDecryptButton.TabIndex = 9;
            this.asmDecryptButton.Text = "Decrypt using ASM";
            this.asmDecryptButton.UseVisualStyleBackColor = true;
            this.asmDecryptButton.Click += new System.EventHandler(this.asmDecryptButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 342);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.asmDecryptButton);
            this.Controls.Add(this.asmEncryptButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.sourceMessage);
            this.Controls.Add(this.sourcePicture);
            this.Controls.Add(this.decryptButton);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblClick);
            this.Controls.Add(this.btnClick);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClick;
        private System.Windows.Forms.Label lblClick;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Button decryptButton;
        private System.Windows.Forms.TextBox sourcePicture;
        private System.Windows.Forms.TextBox sourceMessage;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button asmEncryptButton;
        private System.Windows.Forms.Button asmDecryptButton;
        private System.Windows.Forms.Label label1;
    }
}

