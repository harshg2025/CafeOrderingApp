namespace CafeOrderingApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int CafeId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public int NumberOfPeople { get; set; }
        public string? SpecialRequest { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UID { get; set; }
        public int TID { get; set; }

        public string? CafeName { get; set; }
        public string? FeedbackText { get; set; }
        public double? Rating { get; set; }

    }

}
