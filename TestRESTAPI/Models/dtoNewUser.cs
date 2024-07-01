using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.Models
{
    public class dtoNewUser
    {
        [Required]
        public string fullname { get; set; }
        public string? password { get; set; }

        [Required]
        public string email { get; set; }

        public string? phoneNumber { get; set; }
        public string? verfiyCode { get; set; }
        public string? title { get; set; }
        public string? addries { get; set; }
        public string? location { get; set; } = "";
        public string? typeOfUser { get; set; }
        public string? typeOfCam { get; set; }
        public string? gender { get; set; }
        public string? NationalId { get; set; }
        public string? Nationality { get; set; }
        public string? salary { get; set; }
        public string? lastWork { get; set; }
        public string? perHourTask { get; set; }
        public IFormFile? image { get; set; }
        public string? rate { get; set; }
        public string? EmailConfirmed { get; set; } 

        public string? idTokn { get; set; }
        public string? provider { get; set; }
    }
}
