namespace CafeOrderingApp.Models
{
   
        public class Feedback
        {
            public int FeedbackId { get; set; }
            public int UserId { get; set; }
            public int CafeId { get; set; }
            public int OrderId { get; set; }
            public string FeedbackText { get; set; }
            public float Rating { get; set; }
            public DateTime CreatedAt { get; set; }
        }

}
