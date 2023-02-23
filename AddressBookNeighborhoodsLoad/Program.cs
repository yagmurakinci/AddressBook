using ClosedXML.Excel;
using System.Collections;
using System.Data;

namespace AddressBookNeighborhoodsLoad
{
    internal class Program
    {
        //Excel dosyası masaüstündeyse Enviroment.GetSpecialFolder ile bu yolu masaüstünden de çekebilirsiniz
        const string EXCELPATH = @"C:\Users\grung\OneDrive\Masaüstü\AddressBook-master\AddressBookPL\wwwroot\Excels";
        static MyPocketADO adoManager = new MyPocketADO();
        static void Main(string[] args)
        {
            //Biz burada 70 bin data mahalleyi ekleyebiliriz.
            // Console app ile aspnetmvc core aynı solution altındalar
            //Burada amacımız aynı solutionda birden çok projeyi çalıştırabileceğimizi öğrenmektir.

            try
            {

                CreateAllNeighborhoods();

            }
            catch (Exception ex)
            {
                //log

            }


        }

        private static void CreateAllNeighborhoods()
        {
            try
            {
                adoManager.SQLConnectionString = "Data Source=YAGMUR\\MSSQLSERVER02;Initial Catalog=AddressBookDB;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
                //adoManager.SQLConnectionString = "workstation id = AddressBook302DB.mssql.somee.com; packet size = 4096; user id = oguzhanmavi_SQLLogin_1; pwd = 7m4lxp7nuz; data source = AddressBook302DB.mssql.somee.com; persist security info = False; initial catalog = AddressBook302DB; TrustServerCertificate = True";

                var neighborhoodList =
                    adoManager.GetData("Neighborhoods", "*", "IsDeleted=0");

                string path = EXCELPATH;
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

                            var city =
                                adoManager.ReadData("Cities", new string[] { "Id" }, $"IsDeleted=0 and Name='{cityname.Trim()}'");
                            /*cityManager.GetByConditions(x => x.Name == cityname && !x.IsDeleted).Data;*/

                            var district = adoManager.ReadData("Districts", new string[] { "Id" }, $"IsDeleted=0 and Name='{districtname.Trim()}' and CityId=" + city["Id"]);
                            //GetByConditions(x => x.Name == districtname && !x.IsDeleted && x.CityId == city.Id).Data;

                            bool isExist = false;
                            foreach (DataRow rowitem in neighborhoodList.Rows)
                            {
                                if (rowitem["Name"].ToString().Trim().ToLower() == neighborhoodname.Trim().ToLower()
                                                                  &&
                          Convert.ToInt16(rowitem["DistrictId"])

                           == Convert.ToInt16(district["Id"]))
                                {
                                    isExist = true; break;
                                }

                            }
                            if (!isExist) //mahalleden yoksa eklenecek
                            {
                                Hashtable ht = new Hashtable();
                                ht.Add("CreatedDate", $"'{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}'");
                                ht.Add("Name", $"'{neighborhoodname.Trim()}'");
                                ht.Add("IsDeleted", '0');
                                ht.Add("DistrictId", district["Id"]);
                                adoManager.InsertData("Neighborhoods", ht);
                                Console.WriteLine("Eklendi");
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
    }
}