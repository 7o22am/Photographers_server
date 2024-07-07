using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.Models
{
    public class dtoOrder
    {
 
        [Required]
        public string user { get; set; }
        [Required]
        public string photographer { get; set; }
        [Required]
        public string OrderData { get; set; }
        [Required]
        public string typeOfTask { get; set; }
        public string? duration { get; set; }

        public int? invoice { get; set; }
        public string? phoneNumber { get; set; }
        public string? PhotographerName { get; set; }
        public string? location { get; set; }  
    }
}
