using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTrans
{
    public partial class UnicodeToolDialog : Form
    {
        public UnicodeToolDialog()
        {
            InitializeComponent();
        }

        private void button_viewCode_Click(object sender, EventArgs e)
        {
            StringBuilder text = new StringBuilder();
            foreach (var ch in this.textBox_text.Text)
            {
                //var hex = String.Format("{0,4:X}",(int)ch);
                //text.AppendLine(hex);
                string strHex = Convert.ToString((int)ch, 16);
                text.AppendLine(strHex.PadLeft(4, '0') + " (" + ch.ToString() + ")");
            }

            this.textBox_unicode.Text = text.ToString();
        }

        private void button_viewText_Click(object sender, EventArgs e)
        {
            string text = this.textBox_unicode.Text;
            text = text.Replace("\r\n", " ");
            var lines = text.Split(new char[] { ' ' });

            List<string> codes = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line.StartsWith("(") || line.StartsWith(")"))
                    continue;
                codes.Add(line);
            }

            StringBuilder result = new StringBuilder();
            foreach (var code in codes)
            {
                result.Append(Convert.ToChar(Convert.ToInt32(code, 16)));
            }

            this.textBox_text.Text = result.ToString();
        }

        private void button_clearText_Click(object sender, EventArgs e)
        {
            this.textBox_text.Text = "";
        }

        private void button_clearUnicode_Click(object sender, EventArgs e)
        {
            this.textBox_unicode.Text = "";
        }

        private void button_greak_Click(object sender, EventArgs e)
        {
            //  && ch <= 0x03fb
            StringBuilder text = new StringBuilder();
            for (int i = 0x037e; i <= 0x03fb; i++)
            {
                text.Append(Convert.ToChar(i));
            }
            text.AppendLine();
            for (int i = 0x1f00; i <= 0x1ffe; i++)
            {
                text.Append(Convert.ToChar(i));
            }

            this.textBox_text.Text = text.ToString();
        }

        private void button_normlizeText_Click(object sender, EventArgs e)
        {
            this.textBox_text.Text = this.textBox_text.Text.Normalize();
        }
    }
}
