using System.Net;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

using GreekTrans;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace GreekTransWeb.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    //[Route("greektrans/[controller]/[action]")]
    [Route("api/[action]")]
    [Route("api/v1/[action]")]

    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [SwaggerRequestExample(typeof(TransliterRequest), typeof(TransliterRequestExamples))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(TransliterResponseExamples))]
        [HttpPost]
        public TransliterResponse Transliter([FromBody] TransliterRequest request)
        {
            if (string.IsNullOrEmpty(request.Greek))
                return new TransliterResponse
                {
                    ErrorInfo = $"{nameof(request.Greek)} parameter should not be empty"
                };
            try
            {
                StringBuilder details = new StringBuilder();
                var result = GreekTransliter.TransliterString(request.Greek,
                    request.Ancient,
                    details);
                return new TransliterResponse
                {
                    Translitered = result,
                    Details = details?.ToString(),
                };
            }
            catch(Exception ex)
            {
                return new TransliterResponse
                {
                    ErrorInfo = $"exception: {ex.Message}",
                };
            }
        }

        [HttpGet]
        public TransliterResponse Transliter(string greek, bool ancient = false)
        {
            if (string.IsNullOrEmpty(greek))
                return new TransliterResponse
                {
                    ErrorInfo = $"{nameof(greek)} parameter should not be empty"
                };
            try
            {
                StringBuilder details = new StringBuilder();
                var result = GreekTransliter.TransliterString(greek,
                    ancient,
                    details);
                return new TransliterResponse
                {
                    Translitered = result,
                    Details = details?.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new TransliterResponse
                {
                    ErrorInfo = $"exception: {ex.Message}",
                };
            }
        }


        public class TransliterRequest
        {
            public string? Greek { get; set; }
            public bool Ancient { get; set; }
        }

        public class TransliterResponse
        {
            public string? Translitered { get; set; }

            public string? Details { get; set; }

            public string? ErrorInfo { get; set; }
        }

        public class TransliterResponseExamples : IExamplesProvider<TransliterResponse>
        {
            public TransliterResponse GetExamples()
            {
                return new TransliterResponse
                {
                    Translitered = "hagineō",
                    Details = "ἁ->ha γ->g ῑ->i ν->n έ->e ω->ō ",
                    ErrorInfo = null,
                };
            }
        }

        public class TransliterRequestExamples : IExamplesProvider<TransliterRequest>
        {
            public TransliterRequest GetExamples()
            {
                return new TransliterRequest
                {
                    Greek = "ἁγῑνέω",
                    Ancient = true,
                };
            }
        }
    }
}
