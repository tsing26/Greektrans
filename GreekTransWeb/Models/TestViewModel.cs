using GreekTrans;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace GreekTransWeb.Models
{
    public class TestViewModel
    {
        [Display(Name = "测试条目")]
        [Required(ErrorMessage = "请输入测试条目")]
        public string? InputLines { get; set; }


        // 测试结果信息
        public List<TestResult>? TestResults { get; set; }

        // 错误信息
        public string? ErrorInfo { get; set; }

        public void Test()
        {
            if (InputLines == null)
                return;
            List<TestResult> results = new List<TestResult>();
            string[] lines = InputLines.Split("\r\n");
            int index = 0;
            foreach (var line in lines)
            {
                if (line.Contains('→') == false)
                    continue;

                GreekTransliter.ParseTwoPart(line,
    "→",
    out string source,
    out string target);
                bool ancient = false;
                if (source.EndsWith("*"))
                {
                    source = source.Substring(0, source.Length - 1);
                    ancient = true;
                }

                try
                {
                    StringBuilder debugInfo = new StringBuilder();
                    var result = GreekTransliter.TransliterWord(source, ancient, debugInfo);
                    results.Add(new TestResult
                    {
                        Source = source,
                        Expect = target,
                        Target = result,
                        DebugInfo = debugInfo.ToString(),
                    });

                    /*
                    bool ok = target == result;
                    string output = $"{(index + 1)}) {(ok ? "v " : "x ")} {source} --> {target} ({(ok ? "OK" : "expect:" + result)})";
                    test_result.AppendLine($"<div>{HttpUtility.HtmlEncode(output)}</div>");
                    test_result.AppendLine($"<div class='align-content-end'>{HttpUtility.HtmlEncode(debugInfo.ToString())}</div>");
                    */
                }
                catch (Exception ex)
                {
                    results.Add(new TestResult
                    {
                        Source = source,
                        Expect = target,
                        Target = null,
                        ErrorInfo = ex.Message,
                    });
                }
                index++;
            }

            TestResults = results;
        }
    }

    public class TestResult
    {
        public string? Source { get; set; }

        public string? Expect { get; set; }

        public string? Target { get; set; }

        public string? DebugInfo { get; set; }

        public string? ErrorInfo { get; set; }
    }
}
