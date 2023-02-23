using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.ViewModels
{
    public class DistrictVM
    {
        public short Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "İlçe adı min 2 mak 50 karakter olmalıdır!")]
        public string Name { get; set; }
        public sbyte CityId { get; set; }
        public CityVM? City { get; set; }

    }
}
