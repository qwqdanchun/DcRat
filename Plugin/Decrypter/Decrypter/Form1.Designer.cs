using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace DECF
{
    [DesignerGenerated()]
    public partial class Form1 : System.Windows.Forms.Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this._GroupBox1 = new System.Windows.Forms.GroupBox();
            this._txtMSG = new System.Windows.Forms.RichTextBox();
            this._GroupBox2 = new System.Windows.Forms.GroupBox();
            this._txtKey = new System.Windows.Forms.TextBox();
            this._btnDecrypt = new System.Windows.Forms.Button();
            this._GroupBox3 = new System.Windows.Forms.GroupBox();
            this._txtFiles = new System.Windows.Forms.RichTextBox();
            this._BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this._BackgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this._BackgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this._BackgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            this._GroupBox1.SuspendLayout();
            this._GroupBox2.SuspendLayout();
            this._GroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // _GroupBox1
            // 
            this._GroupBox1.Controls.Add(this._txtMSG);
            this._GroupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._GroupBox1.Location = new System.Drawing.Point(8, 28);
            this._GroupBox1.Margin = new System.Windows.Forms.Padding(2);
            this._GroupBox1.Name = "_GroupBox1";
            this._GroupBox1.Padding = new System.Windows.Forms.Padding(2);
            this._GroupBox1.Size = new System.Drawing.Size(297, 179);
            this._GroupBox1.TabIndex = 0;
            this._GroupBox1.TabStop = false;
            this._GroupBox1.Text = "Message";
            // 
            // _txtMSG
            // 
            this._txtMSG.BackColor = System.Drawing.SystemColors.Control;
            this._txtMSG.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtMSG.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtMSG.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._txtMSG.Location = new System.Drawing.Point(2, 16);
            this._txtMSG.Margin = new System.Windows.Forms.Padding(2);
            this._txtMSG.Name = "_txtMSG";
            this._txtMSG.ReadOnly = true;
            this._txtMSG.Size = new System.Drawing.Size(293, 161);
            this._txtMSG.TabIndex = 0;
            this._txtMSG.Text = "";
            // 
            // _GroupBox2
            // 
            this._GroupBox2.Controls.Add(this._txtKey);
            this._GroupBox2.Controls.Add(this._btnDecrypt);
            this._GroupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._GroupBox2.Location = new System.Drawing.Point(8, 251);
            this._GroupBox2.Margin = new System.Windows.Forms.Padding(2);
            this._GroupBox2.Name = "_GroupBox2";
            this._GroupBox2.Padding = new System.Windows.Forms.Padding(2);
            this._GroupBox2.Size = new System.Drawing.Size(681, 110);
            this._GroupBox2.TabIndex = 1;
            this._GroupBox2.TabStop = false;
            this._GroupBox2.Text = "Decryption Key";
            // 
            // _txtKey
            // 
            this._txtKey.BackColor = System.Drawing.SystemColors.Control;
            this._txtKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this._txtKey.Location = new System.Drawing.Point(4, 35);
            this._txtKey.Margin = new System.Windows.Forms.Padding(2);
            this._txtKey.Name = "_txtKey";
            this._txtKey.Size = new System.Drawing.Size(675, 20);
            this._txtKey.TabIndex = 1;
            this._txtKey.Text = "KEY";
            this._txtKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _btnDecrypt
            // 
            this._btnDecrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnDecrypt.Location = new System.Drawing.Point(251, 68);
            this._btnDecrypt.Margin = new System.Windows.Forms.Padding(2);
            this._btnDecrypt.Name = "_btnDecrypt";
            this._btnDecrypt.Size = new System.Drawing.Size(186, 30);
            this._btnDecrypt.TabIndex = 0;
            this._btnDecrypt.Text = "Decrypt";
            this._btnDecrypt.UseVisualStyleBackColor = true;
            this._btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // _GroupBox3
            // 
            this._GroupBox3.Controls.Add(this._txtFiles);
            this._GroupBox3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._GroupBox3.Location = new System.Drawing.Point(393, 28);
            this._GroupBox3.Margin = new System.Windows.Forms.Padding(2);
            this._GroupBox3.Name = "_GroupBox3";
            this._GroupBox3.Padding = new System.Windows.Forms.Padding(2);
            this._GroupBox3.Size = new System.Drawing.Size(297, 179);
            this._GroupBox3.TabIndex = 1;
            this._GroupBox3.TabStop = false;
            this._GroupBox3.Text = "Files";
            // 
            // _txtFiles
            // 
            this._txtFiles.BackColor = System.Drawing.SystemColors.Control;
            this._txtFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtFiles.DetectUrls = false;
            this._txtFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtFiles.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._txtFiles.Location = new System.Drawing.Point(2, 16);
            this._txtFiles.Margin = new System.Windows.Forms.Padding(2);
            this._txtFiles.Name = "_txtFiles";
            this._txtFiles.ReadOnly = true;
            this._txtFiles.Size = new System.Drawing.Size(293, 161);
            this._txtFiles.TabIndex = 1;
            this._txtFiles.Text = "";
            this._txtFiles.TextChanged += new System.EventHandler(this.txtFiles_TextChanged);
            // 
            // _BackgroundWorker1
            // 
            this._BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // _BackgroundWorker2
            // 
            this._BackgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker2_DoWork);
            // 
            // _BackgroundWorker3
            // 
            this._BackgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker3_DoWork);
            // 
            // _BackgroundWorker4
            // 
            this._BackgroundWorker4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker4_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(697, 382);
            this.ControlBox = false;
            this.Controls.Add(this._GroupBox3);
            this.Controls.Add(this._GroupBox2);
            this.Controls.Add(this._GroupBox1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Ransomware Decrypter";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this._GroupBox1.ResumeLayout(false);
            this._GroupBox2.ResumeLayout(false);
            this._GroupBox2.PerformLayout();
            this._GroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox _GroupBox1;

        internal System.Windows.Forms.GroupBox GroupBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _GroupBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_GroupBox1 != null)
                {
                }

                _GroupBox1 = value;
                if (_GroupBox1 != null)
                {
                }
            }
        }

        private System.Windows.Forms.RichTextBox _txtMSG;

        internal System.Windows.Forms.RichTextBox txtMSG
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtMSG;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtMSG != null)
                {
                }

                _txtMSG = value;
                if (_txtMSG != null)
                {
                }
            }
        }

        private System.Windows.Forms.GroupBox _GroupBox2;

        internal System.Windows.Forms.GroupBox GroupBox2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _GroupBox2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_GroupBox2 != null)
                {
                }

                _GroupBox2 = value;
                if (_GroupBox2 != null)
                {
                }
            }
        }

        private System.Windows.Forms.TextBox _txtKey;

        internal System.Windows.Forms.TextBox txtKey
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtKey;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtKey != null)
                {
                }

                _txtKey = value;
                if (_txtKey != null)
                {
                }
            }
        }

        private System.Windows.Forms.Button _btnDecrypt;

        internal System.Windows.Forms.Button btnDecrypt
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnDecrypt;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnDecrypt != null)
                {
                    _btnDecrypt.Click -= btnDecrypt_Click;
                }

                _btnDecrypt = value;
                if (_btnDecrypt != null)
                {
                    _btnDecrypt.Click += btnDecrypt_Click;
                }
            }
        }

        private System.Windows.Forms.GroupBox _GroupBox3;

        internal System.Windows.Forms.GroupBox GroupBox3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _GroupBox3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_GroupBox3 != null)
                {
                }

                _GroupBox3 = value;
                if (_GroupBox3 != null)
                {
                }
            }
        }

        private System.Windows.Forms.RichTextBox _txtFiles;

        internal System.Windows.Forms.RichTextBox txtFiles
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtFiles;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtFiles != null)
                {
                    _txtFiles.TextChanged -= txtFiles_TextChanged;
                }

                _txtFiles = value;
                if (_txtFiles != null)
                {
                    _txtFiles.TextChanged += txtFiles_TextChanged;
                }
            }
        }

        private System.ComponentModel.BackgroundWorker _BackgroundWorker1;

        internal System.ComponentModel.BackgroundWorker BackgroundWorker1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _BackgroundWorker1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_BackgroundWorker1 != null)
                {
                    _BackgroundWorker1.DoWork -= BackgroundWorker1_DoWork;
                }

                _BackgroundWorker1 = value;
                if (_BackgroundWorker1 != null)
                {
                    _BackgroundWorker1.DoWork += BackgroundWorker1_DoWork;
                }
            }
        }

        private System.ComponentModel.BackgroundWorker _BackgroundWorker2;

        internal System.ComponentModel.BackgroundWorker BackgroundWorker2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _BackgroundWorker2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_BackgroundWorker2 != null)
                {
                    _BackgroundWorker2.DoWork -= BackgroundWorker2_DoWork;
                }

                _BackgroundWorker2 = value;
                if (_BackgroundWorker2 != null)
                {
                    _BackgroundWorker2.DoWork += BackgroundWorker2_DoWork;
                }
            }
        }

        private System.ComponentModel.BackgroundWorker _BackgroundWorker3;

        internal System.ComponentModel.BackgroundWorker BackgroundWorker3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _BackgroundWorker3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_BackgroundWorker3 != null)
                {
                    _BackgroundWorker3.DoWork -= BackgroundWorker3_DoWork;
                }

                _BackgroundWorker3 = value;
                if (_BackgroundWorker3 != null)
                {
                    _BackgroundWorker3.DoWork += BackgroundWorker3_DoWork;
                }
            }
        }

        private System.ComponentModel.BackgroundWorker _BackgroundWorker4;

        internal System.ComponentModel.BackgroundWorker BackgroundWorker4
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _BackgroundWorker4;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_BackgroundWorker4 != null)
                {
                    _BackgroundWorker4.DoWork -= BackgroundWorker4_DoWork;
                }

                _BackgroundWorker4 = value;
                if (_BackgroundWorker4 != null)
                {
                    _BackgroundWorker4.DoWork += BackgroundWorker4_DoWork;
                }
            }
        }
    }
}