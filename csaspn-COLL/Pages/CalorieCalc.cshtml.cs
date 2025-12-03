using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace csaspn_COLL.Pages 
{
    public class CalorieCalc : PageModel
    {
        [BindProperty]
        public string FoodName { get; set; } = string.Empty;

        [BindProperty]
        public int CalorieAmountPerKg { get; set; }

        [BindProperty]
        public int GrammAmount { get; set; }

        public List<(string Name, int Calories)> AddedFoods { get; set; } = new List<(string, int)>();
        public int TotalCalories => AddedFoods.Sum(f => f.Calories);

        public void OnGet()
        {
        }
        
        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(FoodName) || CalorieAmountPerKg <= 0 || GrammAmount <= 0)
            {
                TempData["Error"] = "Заполните все поля!";
                return Page();
            }

            int totalCalories = CalorieAmountPerKg * GrammAmount / 1000;
            
            AddedFoods.Add((FoodName, totalCalories));
            
            TempData["Message"] = $"Добавлено: {FoodName} - {totalCalories} ккал";
            
            FoodName = string.Empty;
            CalorieAmountPerKg = 0;
            GrammAmount = 0;

            return Page();
        }
    }
}
