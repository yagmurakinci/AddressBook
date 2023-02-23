using AddresBookEL.AllEnums;
using AddresBookEL.IdentityEntities;
using AddresBookEL.ViewModels;
using AddressBookBL.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;

namespace AddressBookPL.CreateDefaultDatas
{
    public static class CreateData
    {
        //private static Microsoft.AspNetCore.Hosting.IHostingEnvironment environment;

        public static IApplicationBuilder PrepareData(this IApplicationBuilder app)
        {
            //Managerları burada kullanabilmek için EXTENSION metot oluşturdum.

            using var scopedServices =
                app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            var cityManager = serviceProvider
                .GetRequiredService<ICityManager>();

            var districtManager = serviceProvider
                .GetRequiredService<IDistrictManager>();


            var neighborhoodManager = serviceProvider
               .GetRequiredService<INeighborhoodManager>();

            //CreateAllCities(cityManager);
           
           // CreateAllDistricts(districtManager);
           //CreateAllNeighborhoods(neighborhoodManager, cityManager, districtManager);

            // Tüm rolleri oluşturcak static classı çağıralım
            CreateAllRoles(roleManager);

            return app;
        }

        private static void CreateAllNeighborhoods(INeighborhoodManager neighborhoodManager, ICityManager cityManager, IDistrictManager districtManager)
        {
            try
            {
               // return; //NOT:
                        //BU METODU YAZMAK BEST PRACTICE DEĞİLDİR! 70BİN DATASI OLAN EXCELİ  PROJEYİ YORMAMAK İÇİN INSERT INTO İLE EKLEMEK DAHA MANTIKLIDIR!
                        //ya da Console applicatiın ile ekleyebiliriz.

                var neighborhoodList = neighborhoodManager
                   .GetAll(x =>
                       !x.IsDeleted).Data;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excels");
                string fileName =
                   Path.GetFileName("NeighborhoodPostalCode.xlsx");
                string filePath = Path.Combine(path, fileName);
                using (var excelBook = new XLWorkbook(filePath))
                {

                    var rows = excelBook.Worksheet(1)
                        .RowsUsed();
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1 &&
                            item.RowNumber() <= rows.Count()
                            )
                        {
                            string cityname = item.Cell(1).Value.ToString().Trim();
                            string districtname = item.Cell(2).Value.ToString().Trim();
                            string neighborhoodname = item.Cell(3).Value.ToString().Trim();

                            var city = cityManager.GetByConditions(x => x.Name == cityname && !x.IsDeleted).Data;

                            var district = districtManager.
                                GetByConditions(x => x.Name == districtname && !x.IsDeleted && x.CityId == city.Id).Data;

                            if (neighborhoodList.Count(x =>
                            x.Name.ToLower() == neighborhoodname.ToLower() && x.DistrictId == district.Id) == 0)
                            {
                                NeighborhoodVM n = new() //c# bilmem kaçla gelmiş yeni özellik
                                {
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    Name = neighborhoodname,
                                    DistrictId = district.Id
                                };
                                neighborhoodManager.Add(n);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //ex log
            }
        }

        private static void CreateAllDistricts(IDistrictManager districtManager)
        {
            try
            {
                var districts = districtManager.
                    GetAll(x => !x.IsDeleted).Data;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excels");
                string fileName =
                   Path.GetFileName("Districts.xlsx");
                string filePath = Path.Combine(path, fileName);
                using (var excelBook = new XLWorkbook(filePath))
                {
                    var rows = excelBook.
                    Worksheet(1).RowsUsed();
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1)
                        {
                            var districtName =
                                item.Cell(1).Value.ToString();
                            //Beşiktaş
                            var cityId = Convert.ToSByte(item.Cell(2).Value); //id=34 

                            if (districts.Count(x => x.Name.ToLower() == districtName.ToLower()) == 0)
                            {
                                DistrictVM d = new DistrictVM()
                                {
                                    CreatedDate = DateTime.Now,
                                    CityId = cityId,
                                    IsDeleted = false,
                                    Name = districtName
                                };
                                districtManager.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //loglanacak
            }
        }

        private static void CreateAllCities(ICityManager cityManager)
        {
            try
            {
                var cityList = cityManager
                    .GetAll(x =>
                        !x.IsDeleted).Data;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excels");
                //string path = Path.Combine(environment.WebRootPath,"Excels");
                string fileName =
                   Path.GetFileName("Cities.xlsx");
                //C://betul/AddreessBook/PL/wwwroot/Cities.xlsx
                string filePath = Path.Combine(path, fileName);
                using (var excelBook = new XLWorkbook(filePath))
                {

                    var rows = excelBook.Worksheet(1)
                        .RowsUsed();
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1 &&
                            item.RowNumber() <= rows.Count()
                            )
                        {
                            string cityname =
                                item.Cell(1).Value.ToString(); //Adana
                            //var platecode = item.Cell(2).Value; //01 //bizde yok kullanmayacağız

                            //Eğer cityname bizim tabloda 
                            //yoksa EKLESİN
                            if (cityList.Count(x => x.Name.ToLower() == cityname.ToLower()) == 0)
                            {
                                //Adanayı ekleyelim
                                CityVM c = new CityVM()
                                {
                                    CreatedDate = DateTime.Now,
                                    Name = cityname,
                                    IsDeleted = false
                                };

                                cityManager.Add(c);
                            }


                        }
                    }


                }


            }
            catch (Exception ex)
            {
                //loglanacak
            }
        }

        public static void CreateAllRoles(RoleManager<AppRole> roleManager)
        {
            try
            {
                //AllRoles enum içindeki değerlerin sadece string bölümünü getiriyor
                string[] allRoles = Enum.GetNames(typeof(AllRoles));

                foreach (var item in allRoles)
                {
                    if (!roleManager.RoleExistsAsync(item).Result) //Rolden yoksa
                    {
                        //rolu ekle
                        AppRole r = new AppRole()
                        {
                            CreatedDate = DateTime.Now,
                            Name = item,
                            IsDeleted = false,
                            Description = $"Sistem tatafından {item} isimli rol eklendi."
                        };
                        var result = roleManager.CreateAsync(r).Result;

                    }
                }
            }
            catch (Exception ex)
            {
                //ACİL log
            }
        }





    }

    public class Deneme
    {
        public string Metot1()
        {
            return "";
        }
    }
}
