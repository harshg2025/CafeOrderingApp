using CafeOrderingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CafeOrderingApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly string _connectionString = "Server=SUPRIKA;Database=CafeBookingApp;Trusted_Connection=True;TrustServerCertificate=True";

        public IActionResult MyBookings()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Error"] = "Please Login to View Orders";
                return RedirectToAction("Index", "Home");
            }

            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT o.*, c.Name AS CafeName 
                             FROM [Order] o
                             INNER JOIN Cafe c ON o.CafeId = c.CafeId
                             WHERE o.UserId = @UserId
                             ORDER BY o.BookingDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId.Value);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        CafeId = Convert.ToInt32(reader["CafeId"]),
                        BookingDate = Convert.ToDateTime(reader["BookingDate"]),
                        BookingTime = (TimeSpan)reader["BookingTime"],
                        NumberOfPeople = Convert.ToInt32(reader["NumberOfPeople"]),
                        SpecialRequest = reader["SpecialRequest"]?.ToString(),
                        Status = reader["Status"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        CafeName = reader["CafeName"].ToString(),
                        FeedbackText = reader["FeedbackText"].ToString()
                    });
                }
                reader.Close();
            }

            return View(orders);
        }


        [HttpPost]
        public IActionResult SubmitFeedback(int OrderId, int Rating, string Feedback)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [Order] SET Rating = @Rating, FeedbackText = @Feedback WHERE OrderId = @OrderId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderId", OrderId);
                cmd.Parameters.AddWithValue("@Rating", Rating);
                cmd.Parameters.AddWithValue("@Feedback", Feedback);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Feedback submitted successfully!";
            return RedirectToAction("MyBookings");
        }

    }
}
