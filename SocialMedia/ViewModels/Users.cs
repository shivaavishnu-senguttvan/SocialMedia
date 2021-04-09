using System.ComponentModel.DataAnnotations;

namespace SocialMedia.ViewModels
{
    public class Users
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
