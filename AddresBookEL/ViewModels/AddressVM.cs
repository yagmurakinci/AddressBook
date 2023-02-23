using AddresBookEL.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.ViewModels
{
    public class AddressVM
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string Title { get; set; }
        public int NeighborhoodId { get; set; }
        public string UserId { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Details { get; set; }
        public NeighborhoodVM? Neighborhood { get; set; }
        public  AppUser? AppUser { get; set; }
    }
}
