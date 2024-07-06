using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.Models
{
    public class dtoUpdateUser
    {

 
        public string? fullname { get; set; }
        public string? password { get; set; }
        public string? id { get; set; }
        public string? email { get; set; }

        public string? phoneNumber { get; set; }
        public string? verfiyCode { get; set; }
        public string? title { get; set; }
        public string? addries { get; set; }
        public string? location { get; set; }
        public string? typeOfUser { get; set; }
        public string? typeOfCam { get; set; }
        public string? gender { get; set; }
        public string? NationalId { get; set; }
        public string? Nationality { get; set; }
        public string? salary { get; set; }
        public string? lastWork { get; set; }
        public string? perHourTask { get; set; }
        public int? rate { get; set; }
    }
}
