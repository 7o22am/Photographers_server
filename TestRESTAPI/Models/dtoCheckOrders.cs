using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.Models
{
    public class dtoCheckOrders
    {
        [Required]
        public string photographer { get; set; }
        [Required]
        public string OrderData { get; set; }
    }
}
