using Microsoft.AspNetCore.Identity;

namespace TestRESTAPI.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string verfiyCode {  get; set; }
        public string? title { get; set; }
        public string? addries { get; set; }
        public string? location { get; set; }
        public string? typeOfUser { get; set; }
        public string? typeOfCam { get; set; }
        public string? gender { get; set; }
        public string? NationalId { get; set; }
        public string? Nationality { get; set; }
        public string? salary { get; set; }
        public string? link { get; set; }
        public string? perHourTask { get; set; }
        public byte[]? image { get; set; }
        public string? rate { get; set; } = "0";
        public string? idTokn { get; set; }
        public string? provider { get; set; }

    }
}
