using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.ViewModels
{
    public class UBilisimApiModel
    {
        //https://api.ubilisim.com/postakodu/il/34
        //apiden gelen response'u tutacak classtır
        //"success": true,
        //"status": "ok",
        //"dataupdatedate": "2023-01-30",
        //"description": "PTT tarafından günlük olarak çekilerek güncellenen posta kodlarıdır.",
        //"pagecreatedate": "2023-01-30 08:56:02",
        //"postakodu": []
        public bool success { get; set; }
        public string status { get; set; }
        public string dataupdatedate { get; set; }
        public string description { get; set; }
        public string pagecreatedate { get; set; }

        //"postakodu": [] //json'da [] ARRAY'i ifade eder
      public List<UBilisimApiPostalCodeModel> postakodu { get; set; }


    }
}
