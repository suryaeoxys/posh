using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.Resources;

namespace Posh_TRPT.Controllers
{
    [Route(GlobalConstants.GlobalValues.ControllerRoute)]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        public AboutController(IWebHostEnvironment environment)
        {

            _environment = environment;

        }
        [AllowAnonymous]
        [HttpGet]
        public ContentResult TermConditions()
        {
            string wwwPath = _environment.WebRootPath;
            string connectionHtmlPage = Path.Combine(string.Concat(wwwPath, GlobalResourceFile.AssetsHtml));
            var html = System.IO.File.ReadAllText(connectionHtmlPage);
            return base.Content(html, "text/html");
        }

        [AllowAnonymous]
        [HttpGet]
        //[Route("Privacy_Policy")]
        public IActionResult PrivacyPolicy()
        {
            string wwwPath = _environment.WebRootPath;
            string connectionHtmlPage = Path.Combine(string.Concat(wwwPath, "\\about\\PrivacyPolicy\\PrivacyPolicy.pdf"));
            var html = System.IO.File.ReadAllText(connectionHtmlPage);



            var fileInfo = new System.IO.FileInfo(connectionHtmlPage);
            Response.ContentType = "application/pdf";
           
            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(connectionHtmlPage), "application/pdf");

        }
    }
}
