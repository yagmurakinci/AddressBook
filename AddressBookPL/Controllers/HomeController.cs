using AddresBookEL.IdentityEntities;
using AddresBookEL.ViewModels;
using AddressBookBL.EmailSenderBusiness;
using AddressBookBL.Interfaces;
using AddressBookPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace AddressBookPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICityManager _cityManager;
        private readonly IDistrictManager _districtManager;
        private readonly IAddressManager _addressManager;
        private readonly IEmailSenderManager _emailSenderManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly INeighborhoodManager _neighborhoodManager;

        public HomeController(ILogger<HomeController> logger, ICityManager cityManager, IDistrictManager districtManager, IAddressManager addressManager, IEmailSenderManager emailSenderManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, INeighborhoodManager neighborhoodManager)
        {
            _logger = logger;
            _cityManager = cityManager;
            _districtManager = districtManager;
            _addressManager = addressManager;
            _emailSenderManager = emailSenderManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _neighborhoodManager = neighborhoodManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Member")]
        public IActionResult AddAddress()
        {
            try
            {
                var user = _userManager.FindByNameAsync
             (HttpContext.User.Identity?.Name).Result;
                ViewBag.UserId = user.Id;
                ViewBag.Cities =
                    _cityManager.GetAll(x => !x.IsDeleted).Data.OrderBy(x => x.Name);
            
                ViewBag.AddressList =
                    _addressManager.GetAll(x => !x.IsDeleted && x.UserId == user.Id).Data;
          
                foreach (AddressVM item in ViewBag.AddressList)
                {
                    item.Neighborhood.District =
                        _districtManager.GetById(item.Neighborhood.DistrictId).Data;

                    item.Neighborhood.District.City =
                    _cityManager.GetById(item.Neighborhood.District.CityId).Data;
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Cities = new List<CityVM>();
                return View();

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public JsonResult GetCityDistricts(int id)
        {
            try
            {
                var data = _districtManager
                    .GetAll(x => !x.IsDeleted && x.CityId == id).Data;
                if (data.Count == 0)
                {
                    return Json(new { issuccess = false, message = $"{id} idli ile bağlı ilçeler bulunamadı!", data });

                }
                return Json(new { issuccess = true, message = "İlçeler geldi", data });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public JsonResult SaveAddress([FromBody] AddressVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { issuccess = false, message = "Verileri eksizsiz girdiğinize emin olunuz!" });
                }

                model.CreatedDate = DateTime.Now;
                model.IsDeleted = false;
                var result = _addressManager.Add(model);
                if (result.IsSuccess)
                {
                    result.Data.Neighborhood =
                        _neighborhoodManager.GetById(model.NeighborhoodId).Data;
                    result.Data.Neighborhood.District =
                        _districtManager.GetById(result.Data.Neighborhood.DistrictId).Data;
                    result.Data.Neighborhood.District.City =
                        _cityManager.GetById(result.Data.Neighborhood.District.CityId).Data;
                    return Json(new { issuccess = true, message = "Adres eklendi", data=result.Data });
                }
                else
                {
                    return Json(new { issuccess = false, message = "DİKKAT! Adres EKLENEMEDİ!" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = ex.Message });

            }
        }


        [HttpGet]
        public JsonResult GetDistrictNeighborhoods(int id)
        {
            try
            {
                var data = _neighborhoodManager
                    .GetAll(x => !x.IsDeleted && x.DistrictId == id).Data;
                if (data.Count == 0)
                {
                    return Json(new { issuccess = false, message = $"{id} idli ilçeye bağlı mahalleler bulunamadı!", data });

                }
                return Json(new { issuccess = true, message = "Mahalleler geldi", data });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = ex.Message });
            }
        }

        public JsonResult GetNeighborhoodPostalCode(int cityid, int districtid, int neighborhoodid)
        {
            try
            {
                var district = _districtManager.GetById(Convert.ToInt16(districtid)).Data;
                var neighborhood = _neighborhoodManager.GetById(neighborhoodid).Data;

                string url = "https://api.ubilisim.com/postakodu/il/" + cityid;
                using (WebClient client = new WebClient())
                {
                    var response = client.DownloadString(url);
                    var dataAll = JsonConvert.
                        DeserializeObject<UBilisimApiModel>(response);

                    var data = dataAll.postakodu.
                        FirstOrDefault(x => x.ilce.ToLowerInvariant() == district.Name.ToLower() && x.mahalle.ToLowerInvariant() == neighborhood.Name.Trim().ToLowerInvariant());
                    if (data != null)
                    {
                        return Json(new { issuccess = true, message = "Mahalleler geldi", data = data.pk });
                    }
                    return Json(new { issuccess = false, message = "Mahalle yok" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = ex.Message });
            }
        }


        public JsonResult GetPiedonutChartData()
        {
            try
            {
                //user 
                //gider address tablosundan kaç tane addresini olduğunu bulurum ve sayarım
                var user = _userManager.FindByNameAsync
           (HttpContext.User.Identity?.Name).Result;
                List<PieDonutChartModel> list = new List<PieDonutChartModel>();
                if (user==null)
                {
                    return Json(new { issuccess = false, message = "Kullanıcı bulunamadı!" });
                }
                var address = _addressManager.
                    GetAll(x => !x.IsDeleted && x.UserId == user.Id).Data;
                foreach (var item in address)
                {
                    var district =
                        _districtManager.
                        GetById(item.Neighborhood.DistrictId).Data;

                    var city = _cityManager.GetById(district.CityId).Data;

                    if (list.Count(x=> x.CityName==city.Name)==0)
                    {
                        PieDonutChartModel m = new PieDonutChartModel()
                        {
                            CityName=city.Name
                        };
                        m.AddressCount = 1;
                        list.Add(m);
                    }
                    else
                    {
                        list.FirstOrDefault(x => x.CityName == city.Name).AddressCount++;
                    }
                }
                return Json(new { issuccess = true, message = "Data geldi", data=list });

            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = ex.Message });

            }
        }
    }
}