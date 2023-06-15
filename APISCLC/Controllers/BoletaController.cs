using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APISCLC.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]


    public class BoletaController : Controller
    {
        private readonly string cadenaSQL;

        public BoletaController(IConfiguration configuration)
        {
            cadenaSQL = configuration.GetConnectionString("ConexionSQL");
        }

        [HttpGet]
        [Route("ObtenerBoletaValida")]

        public IActionResult ObtenerBoletaValida(string boleta)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("[dbo].[VerificarBoleta]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@boleta", boleta);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
                            }

                            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "No encontrada" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }   
    }
}
