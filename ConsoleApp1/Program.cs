// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;

Console.WriteLine("Hello, World!");
try
{
    SqlConnection con = new SqlConnection("Server=DESKTOP-PN83M80;Initial Catalog=TUF_DB;Persist Security Info=False;User ID=daniel;Password=abcde123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
    con.Open();
    con.Close();
}
catch(Exception exc)
{

}