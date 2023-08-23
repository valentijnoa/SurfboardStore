using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SurfboardStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String succesMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        { 
           clientInfo.name = Request.Form["name"];
           clientInfo.email = Request.Form["email"];
           clientInfo.phone = Request.Form["phone"];
           clientInfo.address = Request.Form["address"];
           clientInfo.width = Request.Form["width"];
           clientInfo.length = Request.Form["length"];

            if (clientInfo.name.Length == 0  || clientInfo.phone.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0 ||
                clientInfo.width.Length == 0 || clientInfo.length.Length == 0) 
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

            //save the data into the database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=surfdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " +
                                 "(name, email, phone, address, width, length) VALUES " +
                                 "(@name, @email, @phone, @address, @width, @length);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@width", clientInfo.width);
                        command.Parameters.AddWithValue("@length", clientInfo.length);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = ""; clientInfo.width = ""; clientInfo.length = "";
            succesMessage = "New job added";

            Response.Redirect("/Clients/Index");
        }
    }
}
