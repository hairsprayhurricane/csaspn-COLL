using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace csaspn_COLL.Pages
{
    public class FoodItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public double CaloriesPer100g { get; set; }
        public int GrammAmount { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;

        public double Calories => Math.Round(CaloriesPer100g * GrammAmount / 100, 1);
    }

    public class CalorieCalc : PageModel
    {
        private static readonly List<FoodItem> _foodItems = new(); // волтер я не буду вводить сессии волтер волтер все будут делить один список волтер это называется коллективная ответственность
        private static readonly object _lock = new();

        [BindProperty]
        [Required(ErrorMessage = "Введите название блюда")]
        [StringLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
        public string FoodName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Укажите калорийность")]
        [Range(0.1, 1000, ErrorMessage = "Калорийность должна быть от 0.1 до 1000 ккал")]
        public double CalorieAmountPer100g { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Укажите вес порции")]
        [Range(1, 10000, ErrorMessage = "Вес порции должен быть от 1 до 10000 грамм")]
        public int GrammAmount { get; set; }

        public List<FoodItem> AddedFoods => _foodItems;
        public double TotalCalories => _foodItems.Sum(f => f.Calories);
        public int TotalGrams => _foodItems.Sum(f => f.GrammAmount);

        public void OnGet()
        {
        }
        
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var newFood = new FoodItem
            {
                Name = FoodName.Trim(),
                CaloriesPer100g = CalorieAmountPer100g,
                GrammAmount = GrammAmount
            };

            lock (_lock)
            {
                _foodItems.Add(newFood);
            }
            
            TempData["Message"] = $"Добавлено: {FoodName} - {newFood.Calories} ккал";
            
            FoodName = string.Empty;
            CalorieAmountPer100g = 0;
            GrammAmount = 0;

            return RedirectToPage();
        }

        public IActionResult OnPostRemove(Guid id)
        {
            lock (_lock)
            {
                var itemToRemove = _foodItems.FirstOrDefault(f => f.Id == id);
                if (itemToRemove != null)
                {
                    _foodItems.Remove(itemToRemove);
                    TempData["Message"] = $"Удалено: {itemToRemove.Name}";
                }
            }

            return RedirectToPage();
        }

        public IActionResult OnPostClear()
        {
            lock (_lock)
            {
                _foodItems.Clear();
            }
            
            TempData["Message"] = "Список продуктов очищен";
            return RedirectToPage();
        }
    }
}
