namespace CafeOrderingApp.Models
{
    public class Cafe
    {
        public int CafeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Speciality { get; set; }
        public double Ratings { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUrl { get; set; }
    }

}
