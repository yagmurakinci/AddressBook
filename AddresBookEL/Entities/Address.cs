using AddresBookEL.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddresBookEL.Entities
{
    [Table("Address")]
    public class Address:BaseEntity<int>
    {
        [Required]
        public string Title { get; set; }
        public int NeighborhoodId { get; set; }
        public string UserId { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string Details { get; set; }
        [ForeignKey("NeighborhoodId")]
        public virtual Neighborhood Neighborhood { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
    }
}
