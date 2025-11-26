using csaspn_COLL.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace csaspn_COLL.Pages 
{
    public class IndexModel : PageModel
    {
        public string DeviceInfo { get; set; } = "";

        public void OnGet()
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            var result = DeviceDetectorController.ParseUserAgent(userAgent, getWrapper:true);
            Console.WriteLine(result); 

            //DeviceInfo = result;
        }
    }
}
