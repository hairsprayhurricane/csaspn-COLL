using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace csaspn_COLL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceDetectorController : ControllerBase
    {
        /*private static DeviceDetectorController _instance;
        public static DeviceDetectorController Instance { get { return _instance; } } не сегодня брат, сегодня я жертвую 3кб памяти, но завтра....*/
        [HttpGet]
        public IActionResult Get()
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            if (string.IsNullOrEmpty(userAgent))
            {
                return BadRequest("User-Agent header is missing.");
            }

            var result = ParseUserAgent(userAgent);
            return Ok(result);
        }

        public class DeviceData
        {
            public string userAgent { get; set; } = string.Empty;
            public string browser { get; set; } = string.Empty;
            public string device { get; set; } = string.Empty;
            public string os { get; set; } = string.Empty;

            public DeviceData(string userAgent, string browser, string device, string os)
            {
                this.userAgent = userAgent;
                this.browser = browser;
                this.device = device;
                this.os = os;
            }


        }

        public static string ParseUserAgent(string userAgent, bool getWrapper = false)
        {
            string browser = DetectBrowser(userAgent);
            string device = DetectDevice(userAgent);
            string os = DetectOS(userAgent);

            var data = new DeviceData(userAgent, browser, device, os);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string json = "-";
            if (getWrapper)
            {
                var wrapper = new { person = data };
                json = JsonSerializer.Serialize(wrapper, options);
            } else
            {
                json = JsonSerializer.Serialize(data, options);
            }

            return json;
        }

        private static string DetectBrowser(string userAgent)
        {
            if (Regex.IsMatch(userAgent, @"Edg/\d+", RegexOptions.IgnoreCase))
                return "Edge";
            else if (Regex.IsMatch(userAgent, @"Chrome/\d+", RegexOptions.IgnoreCase))
                return "Chrome";
            else if (Regex.IsMatch(userAgent, @"Firefox/\d+", RegexOptions.IgnoreCase))
                return "Firefox";
            else if (Regex.IsMatch(userAgent, @"Safari/\d+", RegexOptions.IgnoreCase) &&
                     !Regex.IsMatch(userAgent, @"Chrome|Edg", RegexOptions.IgnoreCase))
                return "Safari";
            else if (!Regex.IsMatch(userAgent, @"Mozilla/5\.0", RegexOptions.IgnoreCase) ||
                     Regex.IsMatch(userAgent, @"\b(bot|crawl|spider|agent)\b", RegexOptions.IgnoreCase))
                return "Other";

            return "I dont think he is using a browser, i think he is a net runner or a phantom or shit";
        }

        private static string DetectDevice(string userAgent)
        {
            if (Regex.IsMatch(userAgent, @"Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini", RegexOptions.IgnoreCase))
                return "Mobile";

            if (Regex.IsMatch(userAgent, @"\b(Googlebot|Bingbot|Slurp|DuckDuckBot|facebookexternalhit|twitterbot|bot|crawl|spider)\b", RegexOptions.IgnoreCase) ||
                !Regex.IsMatch(userAgent, @"Mozilla/5\.0", RegexOptions.IgnoreCase))
                return "Bot";

            return "Desktop";
        }

        private static string DetectOS(string userAgent)
        {
            if (Regex.IsMatch(userAgent, @"Windows NT\s*(5\.1|6\.0|6\.1|6\.2|6\.3|10\.0)", RegexOptions.IgnoreCase))
                return "Windows";
            if (Regex.IsMatch(userAgent, @"Macintosh|Mac OS X", RegexOptions.IgnoreCase))
                return "macOS";
            if (Regex.IsMatch(userAgent, @"Linux", RegexOptions.IgnoreCase))
                return "Linux";
            if (Regex.IsMatch(userAgent, @"iPhone|iPad|iPod", RegexOptions.IgnoreCase))
                return "iOS";
            if (Regex.IsMatch(userAgent, @"Android", RegexOptions.IgnoreCase))
                return "Android";

            return "Linux.";
        }
    }
}
