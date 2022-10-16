using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GreekTransWeb.Models
{
    public class TransViewModel
    {
        // 待转换的希腊文
        [Display(Name = "希腊文")]
        [Required(ErrorMessage = "请输入希腊文")]
        public string? Greek { get; set; }

        // 是否为古代希腊文风格
        public bool Ancient { get; set; }

        // public bool OutputProcessInfo { get; set; }

        // 转换后的罗马化字符串
        public string? Translitered { get; set; }

        // 转换过程信息
        public string? ProcessInfo { get; set; }

        // 错误信息
        public string? ErrorInfo { get; set; }
    }
}
