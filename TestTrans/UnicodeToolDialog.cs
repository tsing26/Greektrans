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

        private void button_collectionText_Click(object sender, EventArgs e)
        {
            List<string> results = new List<string>();
            foreach (var line in this.textBox_text.Lines)
            {
                if (line.Contains("(") == false)
                    continue;
                string left;
                string right = "";
                int index = line.IndexOfAny(new char[] { ' ', '\t' });
                if (index != -1)
                {
                    left = line.Substring(0, index).Trim();
                    right = line.Substring(index + 1).Trim();
                }
                else
                    left = line.Trim();

                results.Add($"            \"\\u{left}\",   // {right}");
            }

            results.Add($"            // 共 {results.Count} 个");
            this.textBox_unicode.Text = String.Join("\r\n", results);
        }

        private void button_testCaseText_Click(object sender, EventArgs e)
        {
            //         [InlineData("αὗ→hau")]
            List<string> results = new List<string>();
            int number = 1;
            foreach (var line in this.textBox_text.Lines)
            {
                if (line.StartsWith("//"))
                {
                    results.Add($"            {line}");
                    continue;
                }

                if (line.Contains('→') == false)
                    continue;

                results.Add($"         [InlineData({number}, \"{line.Trim()}\")]");
                number++;
            }

            results.Add($"            // 共 {results.Count} 个");
            this.textBox_unicode.Text = String.Join("\r\n", results);
        }
    }
}
