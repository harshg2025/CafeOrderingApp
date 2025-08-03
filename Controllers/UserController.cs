using CafeOrderingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CafeOrderingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly string _connectionString = "Server=LAPTOP-NEJRIJP3;Database=CafeBookingApp;Trusted_Connection=True;TrustServerCertificate=True";

        // Registration Form
        [HttpGet]
        public IActionResult Register() 
        {
            TempData["message"] = "Invalid email or password please register to login.";
            return RedirectToAction("Index", "Home"); 
        }

        // Handle Registration
        [HttpPost]
        public IActionResult Register(User user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO [User] (UserName, Email, Password, Latitude, Longitude) VALUES (@UserName, @Email, @Password, @Latitude, @Longitude)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password); // You should hash in production
                cmd.Parameters.AddWithValue("@Latitude", user.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", user.Longitude);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Login");
        }

        // Login Form
        [HttpGet]
        public IActionResult Login() => RedirectToAction("Index","Home");

        // Handle Login
        [HttpPost]
        public IActionResult Login(string email, string password, double latitude, double longitude)
        {
            User user = null;
            latitude= Convert.ToDouble(HttpContext.Session.GetString("Latitude"));
            longitude = Convert.ToDouble(HttpContext.Session.GetString("Longitude"));

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM [User] WHERE Email = @Email AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserName = reader["UserName"].ToString(),
                        Email = reader["Email"].ToString()
                    };
                }
                reader.Close();

                // ✅ Update location if login successful
                if (user != null)
                {
                    string updateQuery = "UPDATE [User] SET Latitude = @Latitude, Longitude = @Longitude WHERE UserId = @UserId";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@Latitude", latitude);
                    updateCmd.Parameters.AddWithValue("@Longitude", longitude);
                    updateCmd.Parameters.AddWithValue("@UserId", user.UserId);
                    updateCmd.ExecuteNonQuery();
                }
            }

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Index", "Home");
            }
            

                
            return RedirectToAction("Register","User");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult UpdateLocation([FromBody] UserLocation location)
        {
            // Store lat/lng in session
            HttpContext.Session.SetString("Latitude", location.Latitude.ToString());
            HttpContext.Session.SetString("Longitude", location.Longitude.ToString());

            return Ok();
        }

        // Support class
        public class UserLocation
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
