using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookNeighborhoodsLoad
{
    public class MyPocketADO
    {
        //Bu classta ADO.NET ile CRUD işlemleri yapılacaktır
        //Sadece burada olacak, Ado işlemi yapmak isteyen
        //UI projectleri (BLL) buradan referans alabilir.

        public  string SQLConnectionString { get; set; }
           
        private  SqlConnection sqlConnection = new SqlConnection();
        private  SqlCommand sqlCommand = new SqlCommand();

        private  void SetAdonetConnection()
        {
            sqlConnection.ConnectionString = SQLConnectionString;
            sqlCommand.Connection = sqlConnection;
        }
        #region CRUDIslemleri

        public  DataTable GetData(string tableName, string fieldName = "*", string condition = null)
        {
            try
            {
                SetAdonetConnection();
                DataTable result = new DataTable();
                string query = $"select {fieldName} from {tableName} ";
                if (condition != null)
                {
                    query += $"where {condition}";
                }
                sqlCommand.CommandText = query;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                sqlConnection.Open();
                adapter.Fill(result);
                sqlConnection.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public  string CreateInsertString(string tableName, Hashtable htData)
        {
            string query = string.Empty;
            string columns = string.Empty;
            string values = string.Empty;
            //insert into (col1,col2) values (değ1,değ2)
            foreach (var item in htData.Keys) // columns
            {
                string itemValue = htData[item].ToString();
                columns += item + ",";
                values += itemValue + ",";
            }

            columns = columns.TrimEnd(',');
            values = values.TrimEnd(',');

            query = $"insert into {tableName} ({columns}) values ({values})";

            return query;
        }


        public  bool InsertData(string tableName, Hashtable htData)
        {
            try
            {
                return ExecuteData(CreateInsertString(tableName, htData));

            }
            catch (Exception)
            {

                throw;
            }
        }

        public  string CreateUpdateString(string tableName, Hashtable htData,
            string condition = null)
        {
            string result = string.Empty;
            string sets = string.Empty;
            // update tabloismi set col1=değer1, col2=değer2 where id=3
            foreach (var item in htData.Keys)
            {
                string itemValue = htData[item].ToString();
                sets += $"{item}={itemValue},";
            }

            sets = sets.TrimEnd(',');
            result = $"update {tableName} set {sets} where {condition}";

            return result;
        }

        public  bool UpdateData(string tableName, Hashtable htData,
            string condition = null)
        {
            try
            {
                return ExecuteData(CreateUpdateString(tableName, htData, condition));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public  bool ExecuteData(string commandText)
        {
            try
            {
                SetAdonetConnection();
                sqlCommand.CommandText = commandText;
                sqlConnection.Open();
                int effectedRows = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return effectedRows > 0 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public  bool DeleteData(string tableName, string softDeleteset, string condition = null)
        {
            try
            {
                //soft delete AktifMi=0
                string query = $"update {tableName} set {softDeleteset} ";
                query += condition != null ? $"where {condition} " : "";
                return ExecuteData(query);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public  Hashtable ReadData(string tableName, string[] fields,
            string condition = null)
        {
            try
            {
                Hashtable result = new Hashtable();
                SetAdonetConnection();
                string fieldName = string.Empty;
                foreach (var item in fields)
                {
                    fieldName += $" {item} ,";
                }
                fieldName = fieldName.TrimEnd(',');

                string query = $"select {fieldName} from {tableName} ";
                if (condition != null)
                {
                    query += $" where {condition}";
                }
                sqlCommand.CommandText = query;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {

                            result.Add(fields[i], reader[fields[i].ToString()]);
                        }
                    }
                }
                reader.Close();
                sqlConnection.Close();
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
    }
}
