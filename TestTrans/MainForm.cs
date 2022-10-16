using GreekTrans;

namespace TestTrans
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void MenuItem_unicodeTool_Click(object sender, EventArgs e)
        {
            UnicodeToolDialog dialog = new UnicodeToolDialog();
            dialog.Show(this);
        }

        private void MenuItem_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_greek_transliter_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBox_greek_transliterated.Text = "";
                this.textBox_greek_transliterated.Text =
                    GreekTransliter.TransliterString(this.textBox_greek_origin.Text,
                    this.checkBox_greek_ancient.Checked);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, $"转写过程出现异常: {ex.Message}");
            }
        }

        private void button_greek_clearOrigin_Click(object sender, EventArgs e)
        {
            this.textBox_greek_origin.Text = "";
        }

        private void button_greek_clearTransliterated_Click(object sender, EventArgs e)
        {
            this.textBox_greek_transliterated.Text = "";
        }
    }
}