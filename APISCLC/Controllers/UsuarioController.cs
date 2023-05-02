using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using APISCLC.Models;
using Microsoft.AspNetCore.Cors;

namespace APISCLC.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly string cadenaSQL;

        public UsuarioController(IConfiguration configuration)
        {
            cadenaSQL = configuration.GetConnectionString("ConexionSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("obtener_usuarios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                boleta = Convert.ToInt32(rd["boleta"]),
                                password = rd["contra"].ToString()
                            }); 
                        }
                    }

                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista});

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message});
            }
        }

        [HttpPost]
        [Route("Registrar")]
        public IActionResult Resgistrar([FromBody] Usuario objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("registrar_usuario", conexion);
                    cmd.Parameters.AddWithValue("boleta", objeto.boleta);
                    cmd.Parameters.AddWithValue ("contra", objeto.password);
                    cmd.CommandType= CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        [HttpGet]
        [Route("Validar")]
        public IActionResult Validar(int boleta, string contrasenia)
        {
            List<Usuario> lista = new List<Usuario>();
            Usuario user = new Usuario();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("obtener_usuarios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                boleta = Convert.ToInt32(rd["boleta"]),
                                password = rd["contra"].ToString()
                            });
                        }
                    }

                }

                user = lista.Where(item => item.boleta == boleta && item.password == contrasenia).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = user });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = user });
            }
        }



    }
}
