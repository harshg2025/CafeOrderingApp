namespace CafeOrderingApp.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public int CafeId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string Category { get; set; } // e.g. Snack, Beverage, Dessert

        public string ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Optional: Navigation property if needed in EF Core
        // public Cafe Cafe { get; set; }
    }
}
