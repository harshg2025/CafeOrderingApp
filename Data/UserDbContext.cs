using CafeOrderingApp.Models;
using Microsoft.Data.SqlClient;

namespace CafeOrderingApp.Data
{
    public class UserDbContext
    {
        private readonly string cs = "Server=SUPRIKA;Database=CafeOrderingApp;Trusted_Connection;TrustServerCertificate=True;MultipleActiveResultSets=true";
        public User getUser(string Id)
        {
            User user = new User();
            using (SqlConnection conn=new SqlConnection(cs))
            {
                string query = "Select * from User where Id=@Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                }
            }


                return user;
        }
    }
}
