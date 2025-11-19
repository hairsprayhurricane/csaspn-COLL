using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using csaspn_COLL.Models.Controllers;

namespace csaspn_COLL.Pages 
{
    public class AddTripModel : PageModel
    {
        [BindProperty]
        public string Destination { get; set; } = string.Empty;

        [BindProperty]
        public DateTime TravelDate { get; set; }

        [BindProperty]
        public int Travelers { get; set; }

        [BindProperty]
        public bool IsBooked { get; set; }

        [BindProperty]
        public List<string> Attractions { get; set; } = new List<string>();

        public void OnGet()
        {
            TravelDate = DateTime.Today;
        }
        
        public IActionResult OnPost()
        {
            var cleanedAttractions = new List<string>();
            if (Attractions != null)
            {
                foreach (var item in Attractions)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        cleanedAttractions.Add(item.Trim());
                    }
                }
            }

            var controller = new TripController();
            controller.AddTrip(Destination, TravelDate, Travelers, IsBooked, cleanedAttractions);

            var tripData = new
            {
                Destination,
                TravelDate,
                Travelers,
                IsBooked,
                Attractions = cleanedAttractions
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(tripData, options);

            var isEmpty = string.IsNullOrWhiteSpace(Destination)
                          && Travelers == 0
                          && !IsBooked
                          && cleanedAttractions.Count == 0;

            var baseDir = Path.Combine(Directory.GetCurrentDirectory(), "Pages", "TrashCan");
            var dir = isEmpty
                ? Path.Combine(baseDir, "ULTRAtrashCan")
                : baseDir;

            Directory.CreateDirectory(dir);

            var fileName = $"trip_{DateTime.Now:yyyyMMdd_HHmmss_fff}.json";
            var path = Path.Combine(dir, fileName);

            System.IO.File.WriteAllText(path, json);

            return RedirectToPage("Index");
        }
    }
}
