using Microsoft.AspNetCore.Mvc;
using CafeOrderingApp.Models;
using Microsoft.Data.SqlClient;


namespace CafeOrderingApp.Controllers
{
    

    public class CafeController : Controller
    {
        private readonly string _connectionString = "Server=LAPTOP-NEJRIJP3;Database=CafeBookingApp;Trusted_Connection=True;TrustServerCertificate=True";

        public IActionResult Nearby()
        {
            // Get user location from session
            var latStr = HttpContext.Session.GetString("Latitude");
            var lngStr = HttpContext.Session.GetString("Longitude");

            if (string.IsNullOrEmpty(latStr) || string.IsNullOrEmpty(lngStr))
            {
                ViewBag.Message = "Location not set. Please update your location.";
                return View(new List<Cafe>());
            }

            double userLat = Convert.ToDouble(latStr);
            double userLng = Convert.ToDouble(lngStr);

            List<Cafe> cafes = new List<Cafe>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT TOP 9 *, 
                       (6371 * ACOS(COS(RADIANS(@UserLat)) * COS(RADIANS(Latitude)) *
                       COS(RADIANS(Longitude) - RADIANS(@UserLng)) +
                       SIN(RADIANS(@UserLat)) * SIN(RADIANS(Latitude)))) AS Distance
                FROM Cafe
                ORDER BY Distance";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserLat", userLat);
                cmd.Parameters.AddWithValue("@UserLng", userLng);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cafes.Add(new Cafe
                    {
                        CafeId = Convert.ToInt32(reader["CafeId"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Speciality = reader["Speciality"].ToString(),
                        Ratings = Convert.ToDouble(reader["Ratings"]),
                        Address = reader["Address"].ToString(),
                        City = reader["City"].ToString(),
                        Latitude = Convert.ToDouble(reader["Latitude"]),
                        Longitude = Convert.ToDouble(reader["Longitude"]),
                        ImageUrl = reader["ImageUrl"].ToString()
                    });
                }
            }

            return View(cafes);
        }

        public IActionResult Search(string term)
        {
            List<Cafe> cafes = new List<Cafe>();

            if (string.IsNullOrWhiteSpace(term))
            {
                ViewBag.Message = "Please enter a search term.";
                return View("Nearby", cafes);
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT TOP 20 * 
            FROM Cafe
            WHERE Name LIKE @term OR City LIKE @term OR Address LIKE @term";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@term", "%" + term + "%");

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cafes.Add(new Cafe
                    {
                        CafeId = Convert.ToInt32(reader["CafeId"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Speciality = reader["Speciality"].ToString(),
                        Ratings = Convert.ToDouble(reader["Ratings"]),
                        Address = reader["Address"].ToString(),
                        City = reader["City"].ToString(),
                        Latitude = Convert.ToDouble(reader["Latitude"]),
                        Longitude = Convert.ToDouble(reader["Longitude"]),
                        ImageUrl = reader["ImageUrl"].ToString()
                    });
                }
            }

            ViewBag.SearchTerm = term;
            return View("Nearby", cafes); // Reuse your existing view
        }


        public IActionResult Details(int id)
        {
            Cafe cafe = null;
            List<Menu> menuList = new List<Menu>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Get cafe info
                SqlCommand cafeCmd = new SqlCommand("SELECT * FROM Cafe WHERE CafeId = @CafeId", con);
                cafeCmd.Parameters.AddWithValue("@CafeId", id);
                using (SqlDataReader reader = cafeCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cafe = new Cafe
                        {
                            CafeId = id,
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Speciality = reader["Speciality"].ToString(),
                            Ratings = Convert.ToDouble(reader["Ratings"]),
                            Address = reader["Address"].ToString(),
                            City = reader["City"].ToString(),
                            Latitude = Convert.ToDouble(reader["Latitude"]),
                            Longitude = Convert.ToDouble(reader["Longitude"]),
                            ImageUrl = reader["ImageUrl"].ToString()
                        };
                    }
                }

                // Get menu items for the cafe
                SqlCommand menuCmd = new SqlCommand("SELECT * FROM Menu WHERE CafeId = @CafeId", con);
                menuCmd.Parameters.AddWithValue("@CafeId", id);
                using (SqlDataReader reader = menuCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menuList.Add(new Menu
                        {
                            MenuId = Convert.ToInt32(reader["MenuId"]),
                            CafeId = id,
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Category = reader["Category"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            IsAvailable = Convert.ToBoolean(reader["IsAvailable"])
                        });
                    }
                }
            }

            ViewBag.MenuItems = menuList;
            return View(cafe);
        }


        [HttpPost]
        public IActionResult Book(Order order)
        {
            // Get logged-in user's ID from session
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Error"] = "Please login to book a cafe.";
                return RedirectToAction("Index", "Home");
            }

            order.UserId = userId.Value;
            order.Status = "Pending";
            order.CreatedAt = DateTime.Now;

            // Save order to database (example using ADO.NET)
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO [Order] 
                        (UserId, CafeId, BookingDate, BookingTime, NumberOfPeople, SpecialRequest, Status, CreatedAt,UID,TID)
                        VALUES 
                        (@UserId, @CafeId, @BookingDate, @BookingTime, @NumberOfPeople, @SpecialRequest, @Status, @CreatedAt, @UID, @TID)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", order.UserId);
                cmd.Parameters.AddWithValue("@CafeId", order.CafeId);
                cmd.Parameters.AddWithValue("@BookingDate", order.BookingDate);
                cmd.Parameters.AddWithValue("@BookingTime", order.BookingTime);
                cmd.Parameters.AddWithValue("@NumberOfPeople", order.NumberOfPeople);
                cmd.Parameters.AddWithValue("@SpecialRequest", (object?)order.SpecialRequest ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", order.Status);
                cmd.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
                cmd.Parameters.AddWithValue("@UID", order.UID);
                cmd.Parameters.AddWithValue("@TID", order.TID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            TempData["Success"] = "Your booking has been submitted!";
            return RedirectToAction("Details", new { id = order.CafeId });
        }

        // Display List of Cafes
        public IActionResult Index()
        {
            List<Cafe> cafes = new List<Cafe>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Cafe";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cafes.Add(new Cafe
                    {
                        CafeId = (int)reader["CafeId"],
                        Name = reader["Name"].ToString(),
                        Address = reader["Address"].ToString(),
                        Description = reader["Description"].ToString(),
                        Latitude = reader["Latitude"] != DBNull.Value ? (double)reader["Latitude"] : 0,
                        Longitude = reader["Longitude"] != DBNull.Value ? (double)reader["Longitude"] : 0,
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    });
                }
                reader.Close();
            }

            return View(cafes);
        }

        // GET: Create Cafe
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Cafe
        [HttpPost]
        public IActionResult Create(Cafe cafe)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Cafe (Name, Description, Address, City, Latitude, Longitude, ImageUrl)
                                 VALUES (@Name, @Description, @Address, @City, @Latitude, @Longitude, @ImageUrl)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", cafe.Name);
                cmd.Parameters.AddWithValue("@Description", cafe.Description);
                cmd.Parameters.AddWithValue("@Address", cafe.Address);
                cmd.Parameters.AddWithValue("@City", cafe.Address); // You can correct this if you have a City field separately
                cmd.Parameters.AddWithValue("@Latitude", cafe.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", cafe.Longitude);
                cmd.Parameters.AddWithValue("@ImageUrl", cafe.ImageUrl ?? (object)DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Cafe Added Successfully!";
            return RedirectToAction("Index");
        }

        // GET: Edit Cafe
        public IActionResult Edit(int id)
        {
            Cafe cafe = new Cafe();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Cafe WHERE CafeId = @CafeId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CafeId", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cafe.CafeId = (int)reader["CafeId"];
                    cafe.Name = reader["Name"].ToString();
                    cafe.Address = reader["Address"].ToString();
                    cafe.Description = reader["Description"].ToString();
                    cafe.Latitude = reader["Latitude"] != DBNull.Value ? (double)reader["Latitude"] : 0;
                    cafe.Longitude = reader["Longitude"] != DBNull.Value ? (double)reader["Longitude"] : 0;
                    cafe.ImageUrl = reader["ImageUrl"]?.ToString();
                }
                reader.Close();
            }

            return View(cafe);
        }

        // POST: Update Cafe
        [HttpPost]
        public IActionResult Edit(Cafe cafe)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Cafe 
                                 SET Name = @Name, Description = @Description, Address = @Address, City = @City,
                                     Latitude = @Latitude, Longitude = @Longitude, ImageUrl = @ImageUrl
                                 WHERE CafeId = @CafeId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CafeId", cafe.CafeId);
                cmd.Parameters.AddWithValue("@Name", cafe.Name);
                cmd.Parameters.AddWithValue("@Description", cafe.Description);
                cmd.Parameters.AddWithValue("@Address", cafe.Address);
                cmd.Parameters.AddWithValue("@City", cafe.Address); // Correct if City is different field
                cmd.Parameters.AddWithValue("@Latitude", cafe.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", cafe.Longitude);
                cmd.Parameters.AddWithValue("@ImageUrl", cafe.ImageUrl ?? (object)DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Cafe Updated Successfully!";
            return RedirectToAction("Index");
        }

        // POST: Delete Cafe
        [HttpPost]
        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Cafe WHERE CafeId = @CafeId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CafeId", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Cafe Deleted Successfully!";
            return RedirectToAction("Index");
        }


    }

}
