using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.ViewModels
{
    public class UBilisimApiPostalCodeModel
    {
        //https://api.ubilisim.com/postakodu/il/34
        //apiden dönen json response içindeki "postakodu" array'i bu class ile karşılanacaktır
        // "postakodu": [
        //{
        //    "plaka": 47,
        //    "il": "MARDİN",
        //    "ilce": "ARTUKLU",
        //    "semt_bucak_belde": "GÖKÇE",
        //    "mahalle": "GÖKÇE MAH",
        //    "pk": "47420"
        //}
        public int plaka { get; set; }
        public string il { get; set; }
        public string ilce { get; set; }
        public string semt_bucak_belde { get; set; }
        public string mahalle { get; set; }
        public string pk { get; set; }

    }
}
