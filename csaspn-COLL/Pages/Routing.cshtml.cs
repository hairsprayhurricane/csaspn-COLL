using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace csaspn_COLL.Pages
{
    public class RoutingDemoModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Url { get; set; }

        public string InputUrl { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Id { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(Url))
            {
                ParseUrl(Url);
            }
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(Url))
            {
                ParseUrl(Url);
            }
            return Page();
        }

        private void ParseUrl(string url)
        {
            InputUrl = url.Trim();
            
            var path = InputUrl.Split('?')[0];
            
            var segments = path.Trim('/').Split('/');

            if (segments.Length > 0 && !string.IsNullOrEmpty(segments[0]))
            {
                Controller = segments[0];
            }
            else
            {
                Controller = "Home";
            }

            if (segments.Length > 1 && !string.IsNullOrEmpty(segments[1]))
            {
                Action = segments[1];
            }
            else if (segments.Length > 0)
            {
                Action = "Index";
            }

            if (segments.Length > 2 && !string.IsNullOrEmpty(segments[2]))
            {
                Id = segments[2];
            }
            else if (segments.Length > 1 && int.TryParse(segments[1], out _))
            {
                Id = segments[1];
                Action = "Details";
            }
        }
    }
}
