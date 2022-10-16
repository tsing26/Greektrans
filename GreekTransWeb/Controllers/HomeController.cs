using GreekTrans;
using GreekTransWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace GreekTransWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Trans(string? greek,
            bool ancient = false,
            bool output_debugInfo = false)
        {
            var model = new TransViewModel();
            model.Greek = greek;
            model.Ancient = ancient;

            // 显示出输入界面
            if (string.IsNullOrEmpty(greek))
                return View(model);

            try
            {
                StringBuilder? debugInfo = new StringBuilder();
                var result = GreekTransliter.TransliterString(greek,
                    ancient,
                    debugInfo);

                model.Translitered = result;
                model.ProcessInfo = debugInfo?.ToString();
                return View(model);
            }
            catch (TransliterException ex)
            {
                if (string.IsNullOrEmpty(ex.Word))
                    model.ErrorInfo = ex.Message;
                else
                    model.ErrorInfo = $"{ex.Message} (单词: '{ex.Word}')";
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Trans(TransViewModel model)
        {
            if (string.IsNullOrEmpty(model.Greek))
            {
                model.ErrorInfo = "请输入希腊文";
                return View(model); // TODO: 如何报错?
            }

            if (ModelState.IsValid)
            {
                try
                {
                    StringBuilder? debugInfo = new StringBuilder();
                    var result = GreekTransliter.TransliterString(model.Greek,
        model.Ancient,
        debugInfo);
                    model.Translitered = result;
                    model.ProcessInfo = debugInfo?.ToString();
                }
                catch (TransliterException ex)
                {
                    if (string.IsNullOrEmpty(ex.Word))
                        model.ErrorInfo = ex.Message;
                    else
                        model.ErrorInfo = $"{ex.Message} (单词: '{ex.Word}')";
                }
            }
            else
            {
                // TODO: 如何报错?
                model.ErrorInfo = "IsValid == false";
            }

            return View(model);
        }

        /*
        public IActionResult Privacy()
        {
            return View();
        }
        */

        [HttpGet]
        public ViewResult Test(string? inputLines)
        {
            var model = new TestViewModel();
            model.InputLines = inputLines;

            // 显示出输入界面
            if (string.IsNullOrEmpty(inputLines))
                return View(model);

            model.Test();
            return View(model);
        }

        [HttpPost]
        public IActionResult Test(TestViewModel model)
        {
            if (string.IsNullOrEmpty(model.InputLines))
            {
                model.ErrorInfo = "请输入测试条目";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                model.Test();
            }
            else
            {
                // TODO: 如何报错?
                model.ErrorInfo = "IsValid == false";
            }

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}