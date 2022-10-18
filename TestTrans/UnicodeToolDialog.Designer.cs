namespace TestTrans
{
    partial class UnicodeToolDialog
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
            if (disposing && (components != null))
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_text = new System.Windows.Forms.TextBox();
            this.textBox_unicode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_viewCode = new System.Windows.Forms.Button();
            this.button_viewText = new System.Windows.Forms.Button();
            this.button_clearText = new System.Windows.Forms.Button();
            this.button_clearUnicode = new System.Windows.Forms.Button();
            this.button_greak = new System.Windows.Forms.Button();
            this.button_normlizeText = new System.Windows.Forms.Button();
            this.button_collectionText = new System.Windows.Forms.Button();
            this.button_testCaseText = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "文字内容:";
            // 
            // textBox_text
            // 
            this.textBox_text.AcceptsReturn = true;
            this.textBox_text.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_text.Font = new System.Drawing.Font("Arial", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox_text.Location = new System.Drawing.Point(12, 51);
            this.textBox_text.Multiline = true;
            this.textBox_text.Name = "textBox_text";
            this.textBox_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_text.Size = new System.Drawing.Size(624, 206);
            this.textBox_text.TabIndex = 1;
            // 
            // textBox_unicode
            // 
            this.textBox_unicode.AcceptsReturn = true;
            this.textBox_unicode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_unicode.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox_unicode.Location = new System.Drawing.Point(12, 358);
            this.textBox_unicode.Multiline = true;
            this.textBox_unicode.Name = "textBox_unicode";
            this.textBox_unicode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_unicode.Size = new System.Drawing.Size(624, 206);
            this.textBox_unicode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 327);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "Unicode 内码:";
            // 
            // button_viewCode
            // 
            this.button_viewCode.Location = new System.Drawing.Point(12, 275);
            this.button_viewCode.Name = "button_viewCode";
            this.button_viewCode.Size = new System.Drawing.Size(280, 40);
            this.button_viewCode.TabIndex = 4;
            this.button_viewCode.Text = "查看内码 ↓\r\n";
            this.button_viewCode.UseVisualStyleBackColor = true;
            this.button_viewCode.Click += new System.EventHandler(this.button_viewCode_Click);
            // 
            // button_viewText
            // 
            this.button_viewText.Location = new System.Drawing.Point(298, 275);
            this.button_viewText.Name = "button_viewText";
            this.button_viewText.Size = new System.Drawing.Size(280, 40);
            this.button_viewText.TabIndex = 5;
            this.button_viewText.Text = "查看文字 ↑";
            this.button_viewText.UseVisualStyleBackColor = true;
            this.button_viewText.Click += new System.EventHandler(this.button_viewText_Click);
            // 
            // button_clearText
            // 
            this.button_clearText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearText.Location = new System.Drawing.Point(642, 55);
            this.button_clearText.Name = "button_clearText";
            this.button_clearText.Size = new System.Drawing.Size(146, 40);
            this.button_clearText.TabIndex = 6;
            this.button_clearText.Text = "清除";
            this.button_clearText.UseVisualStyleBackColor = true;
            this.button_clearText.Click += new System.EventHandler(this.button_clearText_Click);
            // 
            // button_clearUnicode
            // 
            this.button_clearUnicode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearUnicode.Location = new System.Drawing.Point(642, 358);
            this.button_clearUnicode.Name = "button_clearUnicode";
            this.button_clearUnicode.Size = new System.Drawing.Size(146, 40);
            this.button_clearUnicode.TabIndex = 7;
            this.button_clearUnicode.Text = "清除";
            this.button_clearUnicode.UseVisualStyleBackColor = true;
            this.button_clearUnicode.Click += new System.EventHandler(this.button_clearUnicode_Click);
            // 
            // button_greak
            // 
            this.button_greak.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_greak.Location = new System.Drawing.Point(642, 235);
            this.button_greak.Name = "button_greak";
            this.button_greak.Size = new System.Drawing.Size(146, 40);
            this.button_greak.TabIndex = 8;
            this.button_greak.Text = "Greek";
            this.button_greak.UseVisualStyleBackColor = true;
            this.button_greak.Click += new System.EventHandler(this.button_greak_Click);
            // 
            // button_normlizeText
            // 
            this.button_normlizeText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_normlizeText.Location = new System.Drawing.Point(642, 101);
            this.button_normlizeText.Name = "button_normlizeText";
            this.button_normlizeText.Size = new System.Drawing.Size(146, 40);
            this.button_normlizeText.TabIndex = 9;
            this.button_normlizeText.Text = "Normalize";
            this.button_normlizeText.UseVisualStyleBackColor = true;
            this.button_normlizeText.Click += new System.EventHandler(this.button_normlizeText_Click);
            // 
            // button_collectionText
            // 
            this.button_collectionText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_collectionText.Location = new System.Drawing.Point(642, 147);
            this.button_collectionText.Name = "button_collectionText";
            this.button_collectionText.Size = new System.Drawing.Size(146, 40);
            this.button_collectionText.TabIndex = 10;
            this.button_collectionText.Text = "集合态";
            this.button_collectionText.UseVisualStyleBackColor = true;
            this.button_collectionText.Click += new System.EventHandler(this.button_collectionText_Click);
            // 
            // button_testCaseText
            // 
            this.button_testCaseText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_testCaseText.Location = new System.Drawing.Point(642, 189);
            this.button_testCaseText.Name = "button_testCaseText";
            this.button_testCaseText.Size = new System.Drawing.Size(146, 40);
            this.button_testCaseText.TabIndex = 11;
            this.button_testCaseText.Text = "测试态";
            this.button_testCaseText.UseVisualStyleBackColor = true;
            this.button_testCaseText.Click += new System.EventHandler(this.button_testCaseText_Click);
            // 
            // UnicodeToolDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 576);
            this.Controls.Add(this.button_testCaseText);
            this.Controls.Add(this.button_collectionText);
            this.Controls.Add(this.button_normlizeText);
            this.Controls.Add(this.button_greak);
            this.Controls.Add(this.button_clearUnicode);
            this.Controls.Add(this.button_clearText);
            this.Controls.Add(this.button_viewText);
            this.Controls.Add(this.button_viewCode);
            this.Controls.Add(this.textBox_unicode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_text);
            this.Controls.Add(this.label1);
            this.Name = "UnicodeToolDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Unicode 工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox textBox_text;
        private TextBox textBox_unicode;
        private Label label2;
        private Button button_viewCode;
        private Button button_viewText;
        private Button button_clearText;
        private Button button_clearUnicode;
        private Button button_greak;
        private Button button_normlizeText;
        private Button button_collectionText;
        private Button button_testCaseText;
    }
}