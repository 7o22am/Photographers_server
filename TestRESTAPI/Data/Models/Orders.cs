using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestRESTAPI.Data.Models
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string user { get; set; }
        [Required]
        public string photographer { get; set; }

        public string? photographerName { get; set; }
        [Required]
        public string OrderData { get; set; }
        [Required]
        public string typeOfTask { get; set; }
        public string? duration { get; set; }

         public int ? invoice { get; set; }

        public string? location { get; set; }
        public int? phoneNumber { get; set; }
        public string? stata { get; set; } = "pending";


        public string? PayStata { get; set; } = "pending";

    }
}
