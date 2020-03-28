using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HPlusSport.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Sku { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(19, 2)")]
        public decimal Price { get; set; }

        public bool isAvailable { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

    }
}