using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.Models
{
    public class dtoImage
    {
        [Required]
        public string email { get; set; }
        [Required]
        public IFormFile image { get; set; }
    }
}
