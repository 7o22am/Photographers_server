using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.Models
{
    public class dtoConfirmEmail
    {
        [Required]
            public string email { get; set; }
        [Required]
        public string code { get; set; }
    }
}
