using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.Models
{
    public class dtoDeleteUser
    {
        [Required]
        public string email { get; set; }
    }
}
