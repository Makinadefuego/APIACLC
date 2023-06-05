using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using APISCLC.Models;
using Microsoft.AspNetCore.Cors;
using System.Globalization;

namespace APISCLC.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]

    public class ReservacionController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ReservacionController(IConfiguration configuration)
        {
            cadenaSQL = configuration.GetConnectionString("ConexionSQL");
        }

        [HttpPost]
        [Route("ReservarLista")]
        public IActionResult ReservarLista([FromBody] List<int> computadoras, int boleta, DateTime fecha, int modulo, int lab)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    // Crear una tabla temporal para pasar la lista de computadoras como parámetro
                    var table = new DataTable();
                    table.Columns.Add("Value", typeof(int));
                    foreach (var item in computadoras)
                    {
                        table.Rows.Add(item);
                    }

                    // Crear el objeto SqlCommand y asignar los parámetros
                    using (SqlCommand cmd = new SqlCommand("[dbo].[ReservarComputadoras]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@computadoras", table);
                        cmd.Parameters.AddWithValue("@boleta", boleta);
                        cmd.Parameters.AddWithValue("@fecha", fecha);
                        cmd.Parameters.AddWithValue("@modulo", modulo);
                        cmd.Parameters.AddWithValue("@laboratorio", lab);

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpPost]
        [Route("ReservarModulo")]
        public IActionResult ReservarModulo(int usuario, int modulo, DateTime date, int lab)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("ReservarLaboratorio", connection))
                    {
                        command.Parameters.AddWithValue("@usuario", usuario);
                        command.Parameters.AddWithValue("@modulo", modulo);
                        command.Parameters.AddWithValue("@fecha", date);
                        command.Parameters.AddWithValue("@laboratorio", lab);

                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ReservacionesSemana")]
        public IActionResult ReservacionesLista(DateTime dateTime, int labo)
        {
            List<Reservacion> lista = new List<Reservacion>();
            try
            {
                using (SqlConnection connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("ObtenerReservasPorSemana", connection))
                    {
                        command.Parameters.AddWithValue("@fecha_inicio_semana", dateTime);
                        command.Parameters.AddWithValue("@laboratorio_id", labo);
                        command.CommandType = CommandType.StoredProcedure;


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Usuario usario = new Usuario()
                                {
                                    boleta = Convert.ToInt32(reader["usuario"]),
                                    password = "dummie"
                                };



                                Laboratorio laboratorio = new Laboratorio()
                                {
                                    idLaboratorio = Convert.ToInt32(reader["laboratorio"])
                                };

                                
                                Reservacion reservacion = new Reservacion();
                                reservacion.id = Convert.ToInt32(reader["id"]);
                                reservacion.fecha = (DateTime)reader["fecha"];
                                reservacion.modulo = Convert.ToInt32(reader["modulo"]);
                                reservacion.Usuario = usario;
                                reservacion.Laboratorio = laboratorio;


                                lista.Add(reservacion);


                            }
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });

            }
        }



        [HttpGet]
        [Route("ObtenerComputadorasReservadasModulo")]
        public IActionResult ObtenerComputadorasReservadasModulo(DateTime dateTime, int modulo, int lab)
        {
            List<Computadora> lista = new List<Computadora>();
            try
            {
                using (SqlConnection connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("ObtenerComputadorasReservadasModulo", connection))
                    {
                        command.Parameters.AddWithValue("@fecha", dateTime);
                        command.Parameters.AddWithValue("@modulo", modulo);
                        command.Parameters.AddWithValue("@labo", lab);
                        command.CommandType = CommandType.StoredProcedure;


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                

                                Computadora computadora = new Computadora()
                                {
                                    idComputadora = Convert.ToInt32(reader["computadora"]),
                                    Laboratorio = new Laboratorio() { idLaboratorio = 1},
                                    lab_id = lab
                                };
  
                                lista.Add(computadora);


                            }
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });

            }
        }
    }

}





