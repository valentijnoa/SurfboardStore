using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace SurfboardStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String succesMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=surfdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.width = reader.GetString(5);
                                clientInfo.length = reader.GetString(6);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                errorMessage = ex.Message;
            }
        }
        public void OnPost() 
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            clientInfo.width = Request.Form["width"];
            clientInfo.length = Request.Form["length"];

            if (clientInfo.id.Length == 0 || clientInfo.name.Length == 0 ||
                clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 ||
                clientInfo.address.Length == 0 || clientInfo.width.Length == 0 || 
                clientInfo.length.Length == 0)
            {
                errorMessage = "Please fill in all the fields before proceeding";
                return;
            }

            if (int.TryParse(clientInfo.length, out int parsedLength) && int.TryParse(clientInfo.width, out int parsedWidth))
            {
                if ((parsedLength < 260 || parsedLength > 370) || (parsedWidth < 65 || parsedWidth > 85))
                {
                    errorMessage = "Length or Width is outside the specified range. Length between 260 and 370, Width between 65 and 85";
                    return;
                }
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=surfdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE clients " +
                                 "SET name=@name, email=@email, phone=@phone, address=@address, width=@width, length=@length " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@width", clientInfo.width);
                        command.Parameters.AddWithValue("@length", clientInfo.length);
                        command.Parameters.AddWithValue("@id", clientInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { 
                errorMessage=ex.Message; 
                return;
            }

            Response.Redirect("/Clients/Index");
        }
    }
}
