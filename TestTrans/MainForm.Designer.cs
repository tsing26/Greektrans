namespace TestTrans
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuItem_file = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_unicodeTool = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_greek_transliteration = new System.Windows.Forms.TabPage();
            this.button_greek_clearTransliterated = new System.Windows.Forms.Button();
            this.button_greek_clearOrigin = new System.Windows.Forms.Button();
            this.checkBox_greek_ancient = new System.Windows.Forms.CheckBox();
            this.button_greek_transliter = new System.Windows.Forms.Button();
            this.textBox_greek_transliterated = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_greek_origin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_greek_transliteration.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_file});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(933, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuItem_file
            // 
            this.MenuItem_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_unicodeTool,
            this.MenuItem_exit});
            this.MenuItem_file.Name = "MenuItem_file";
            this.MenuItem_file.Size = new System.Drawing.Size(97, 32);
            this.MenuItem_file.Text = "文件(&F)";
            // 
            // MenuItem_unicodeTool
            // 
            this.MenuItem_unicodeTool.Name = "MenuItem_unicodeTool";
            this.MenuItem_unicodeTool.Size = new System.Drawing.Size(312, 40);
            this.MenuItem_unicodeTool.Text = "Unicode 工具(&U) ...";
            this.MenuItem_unicodeTool.Click += new System.EventHandler(this.MenuItem_unicodeTool_Click);
            // 
            // MenuItem_exit
            // 
            this.MenuItem_exit.Name = "MenuItem_exit";
            this.MenuItem_exit.Size = new System.Drawing.Size(312, 40);
            this.MenuItem_exit.Text = "退出(&X)";
            this.MenuItem_exit.Click += new System.EventHandler(this.MenuItem_exit_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.toolStrip1.Location = new System.Drawing.Point(0, 36);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(933, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Location = new System.Drawing.Point(0, 624);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(933, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_greek_transliteration);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 61);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(933, 563);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_greek_transliteration
            // 
            this.tabPage_greek_transliteration.Controls.Add(this.button_greek_clearTransliterated);
            this.tabPage_greek_transliteration.Controls.Add(this.button_greek_clearOrigin);
            this.tabPage_greek_transliteration.Controls.Add(this.checkBox_greek_ancient);
            this.tabPage_greek_transliteration.Controls.Add(this.button_greek_transliter);
            this.tabPage_greek_transliteration.Controls.Add(this.textBox_greek_transliterated);
            this.tabPage_greek_transliteration.Controls.Add(this.label2);
            this.tabPage_greek_transliteration.Controls.Add(this.textBox_greek_origin);
            this.tabPage_greek_transliteration.Controls.Add(this.label1);
            this.tabPage_greek_transliteration.Location = new System.Drawing.Point(4, 37);
            this.tabPage_greek_transliteration.Name = "tabPage_greek_transliteration";
            this.tabPage_greek_transliteration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_greek_transliteration.Size = new System.Drawing.Size(925, 522);
            this.tabPage_greek_transliteration.TabIndex = 0;
            this.tabPage_greek_transliteration.Text = "希腊文转写";
            this.tabPage_greek_transliteration.UseVisualStyleBackColor = true;
            // 
            // button_greek_clearTransliterated
            // 
            this.button_greek_clearTransliterated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_greek_clearTransliterated.Location = new System.Drawing.Point(813, 476);
            this.button_greek_clearTransliterated.Name = "button_greek_clearTransliterated";
            this.button_greek_clearTransliterated.Size = new System.Drawing.Size(104, 40);
            this.button_greek_clearTransliterated.TabIndex = 7;
            this.button_greek_clearTransliterated.Text = "清除";
            this.button_greek_clearTransliterated.UseVisualStyleBackColor = true;
            this.button_greek_clearTransliterated.Click += new System.EventHandler(this.button_greek_clearTransliterated_Click);
            // 
            // button_greek_clearOrigin
            // 
            this.button_greek_clearOrigin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_greek_clearOrigin.Location = new System.Drawing.Point(813, 177);
            this.button_greek_clearOrigin.Name = "button_greek_clearOrigin";
            this.button_greek_clearOrigin.Size = new System.Drawing.Size(104, 40);
            this.button_greek_clearOrigin.TabIndex = 4;
            this.button_greek_clearOrigin.Text = "清除";
            this.button_greek_clearOrigin.UseVisualStyleBackColor = true;
            this.button_greek_clearOrigin.Click += new System.EventHandler(this.button_greek_clearOrigin_Click);
            // 
            // checkBox_greek_ancient
            // 
            this.checkBox_greek_ancient.AutoSize = true;
            this.checkBox_greek_ancient.Location = new System.Drawing.Point(8, 223);
            this.checkBox_greek_ancient.Name = "checkBox_greek_ancient";
            this.checkBox_greek_ancient.Size = new System.Drawing.Size(80, 32);
            this.checkBox_greek_ancient.TabIndex = 2;
            this.checkBox_greek_ancient.Text = "古代";
            this.checkBox_greek_ancient.UseVisualStyleBackColor = true;
            // 
            // button_greek_transliter
            // 
            this.button_greek_transliter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_greek_transliter.Location = new System.Drawing.Point(813, 56);
            this.button_greek_transliter.Name = "button_greek_transliter";
            this.button_greek_transliter.Size = new System.Drawing.Size(104, 40);
            this.button_greek_transliter.TabIndex = 3;
            this.button_greek_transliter.Text = "转写";
            this.button_greek_transliter.UseVisualStyleBackColor = true;
            this.button_greek_transliter.Click += new System.EventHandler(this.button_greek_transliter_Click);
            // 
            // textBox_greek_transliterated
            // 
            this.textBox_greek_transliterated.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_greek_transliterated.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox_greek_transliterated.Location = new System.Drawing.Point(8, 304);
            this.textBox_greek_transliterated.Multiline = true;
            this.textBox_greek_transliterated.Name = "textBox_greek_transliterated";
            this.textBox_greek_transliterated.ReadOnly = true;
            this.textBox_greek_transliterated.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_greek_transliterated.Size = new System.Drawing.Size(799, 212);
            this.textBox_greek_transliterated.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 273);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 28);
            this.label2.TabIndex = 5;
            this.label2.Text = "转写后:";
            // 
            // textBox_greek_origin
            // 
            this.textBox_greek_origin.AcceptsReturn = true;
            this.textBox_greek_origin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_greek_origin.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox_greek_origin.Location = new System.Drawing.Point(8, 55);
            this.textBox_greek_origin.Multiline = true;
            this.textBox_greek_origin.Name = "textBox_greek_origin";
            this.textBox_greek_origin.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_greek_origin.Size = new System.Drawing.Size(799, 162);
            this.textBox_greek_origin.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "希腊文:";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 37);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(925, 522);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 646);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "测试转写";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_greek_transliteration.ResumeLayout(false);
            this.tabPage_greek_transliteration.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStrip toolStrip1;
        private StatusStrip statusStrip1;
        private TabControl tabControl1;
        private TabPage tabPage_greek_transliteration;
        private TabPage tabPage2;
        private ToolStripMenuItem MenuItem_file;
        private ToolStripMenuItem MenuItem_unicodeTool;
        private ToolStripMenuItem MenuItem_exit;
        private Button button_greek_transliter;
        private TextBox textBox_greek_transliterated;
        private Label label2;
        private TextBox textBox_greek_origin;
        private Label label1;
        private CheckBox checkBox_greek_ancient;
        private Button button_greek_clearTransliterated;
        private Button button_greek_clearOrigin;
    }
}