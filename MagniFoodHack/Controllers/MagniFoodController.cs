using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MagniFoodHack.Models;

namespace MagniFoodHack.Controllers
{
    [RoutePrefix("api/magnifood")]
    public class MagniFoodController : ApiController
    {
        List<Items> items = new List<Items>();
        List<Users> users = new List<Users>();

        public MagniFoodController() { }

        public MagniFoodController(List<Items> items)
        {
            this.items = items;
        }

        private static SqlConnection GetDBConnection(string datasource, string database, string username, string password)
        {
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }

        private static SqlConnection GetDBConnection()
        {
            string datasource = @"AHARIHARAN";

            string database = "MAGNIFOODDB";
            string username = "sa";
            string password = "test123$";

            return GetDBConnection(datasource, database, username, password);
        }

        private IEnumerable<Items> GetAllItems()
        {
            using (SqlConnection conn = GetDBConnection())
            {
                string dbItemsQuery = "Select * from MenuItems";
                SqlCommand cmd = new SqlCommand(dbItemsQuery, conn);
                conn.Open();
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Items dbItem = new Items();
                        dbItem.Name = oReader["Name"].ToString();
                        dbItem.Type = oReader["Type"].ToString();
                        dbItem.Price = Convert.ToDecimal(oReader["Price"].ToString());
                        items.Add(dbItem);
                    }
                }
            }
            return items;
        }
       
        private IEnumerable<Users> GetUsers()
        {
            using (SqlConnection conn = GetDBConnection())
            {
                string dbItemsQuery = "Select * from UserTable";
                SqlCommand cmd = new SqlCommand(dbItemsQuery, conn);
                conn.Open();
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Users dbUser = new Users();
                        dbUser.UserName = oReader["UserName"].ToString();
                        dbUser.Password = oReader["Password"].ToString();
                        users.Add(dbUser);
                    }
                }
            }
            return users;
        }

        private bool CheckCredentials(string username, string password)
        {
            var users = GetUsers();
            foreach (var user in users)
            {
                if (username.Equals(user.UserName) && password.Equals(user.Password))
                    return true;
            }
            return false;
        }

        [Route("signup/{username}/{password}")]
        [HttpGet]
        public string SignUp(string username, string password)
        {
            if (CheckCredentials(username, password))
            {
                return "User with these credentials already exists";
            }
            else
            {
                using (SqlConnection conn = GetDBConnection())
                {
                    string signupQuery = "Insert into UserTable(UserName, Password)";
                    signupQuery += " values (@username, @password)";
                    SqlCommand cmd = new SqlCommand(signupQuery, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return "Sign up successful";
            }
        }

        [Route("login/{username}/{password}")]
        [HttpGet]
        public IEnumerable<Items> Login(string username, string password)
        {
            if(CheckCredentials(username, password))
            {
                items = (List<Items>)GetAllItems();
            }
            return items;
        }
        
        [HttpGet]
        [Route("getItem/{name}")]
        public Items GetItem(string name)
        {
            items = (List<Items>)GetAllItems();
            var item = items.FirstOrDefault((p) => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
            {
                return null;
            }
            return item;
        }
    }
}
