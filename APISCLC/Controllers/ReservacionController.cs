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
        [Route("ReservarModulo")]
        public IActionResult ReservarModulo(int usuario, int modulo, DateTime date, int lab)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("ReservarComputadorasPorLaboratorio", connection))
                    {
                        command.Parameters.AddWithValue("@usuario", usuario);
                        command.Parameters.AddWithValue("@modulo", modulo);
                        command.Parameters.AddWithValue("@fecha", date);
                        command.Parameters.AddWithValue("@lab", lab);

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
        public IActionResult ReservacionesLista(DateTime dateTime)
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
                                    idLaboratorio = Convert.ToInt32(reader["lab"])
                                };

                                Computadora computadora = new Computadora()
                                {
                                    idComputadora = Convert.ToInt32(reader["compu"]),
                                    Laboratorio = laboratorio,

                                    lab_id = laboratorio.idLaboratorio
                                };
                                Reservacion reservacion = new Reservacion();
                                reservacion.idReserva = Convert.ToInt32(reader["idReserva"]);
                                reservacion.fechahora_reserva = (DateTime)reader["fechahora_reserva"];
                                reservacion.modulo_sreservacion = Convert.ToInt32(reader["modulo_sreservacion"]);
                                reservacion.Usuario = usario;
                                reservacion.Computadora = computadora;
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
        [Route("ReservacionesModulo")]
        public IActionResult ReservacionesModulo(DateTime dateTime, int modulo, int lab)
        {
            List<Reservacion> lista = new List<Reservacion>();
            try
            {
                using (SqlConnection connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("ObtenerReservacionesModulo", connection))
                    {
                        command.Parameters.AddWithValue("@fecha", dateTime);
                        command.Parameters.AddWithValue("@modulo", modulo);
                        command.Parameters.AddWithValue("@labo", lab);
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
                                    idLaboratorio = Convert.ToInt32(reader["lab"])
                                };

                                Computadora computadora = new Computadora()
                                {
                                    idComputadora = Convert.ToInt32(reader["compu"]),
                                    Laboratorio = laboratorio,

                                    lab_id = laboratorio.idLaboratorio
                                };
                                Reservacion reservacion = new Reservacion();
                                reservacion.idReserva = Convert.ToInt32(reader["idReserva"]);
                                reservacion.fechahora_reserva = (DateTime)reader["fechahora_reserva"];
                                reservacion.modulo_sreservacion = Convert.ToInt32(reader["modulo_sreservacion"]);
                                reservacion.Usuario = usario;
                                reservacion.Computadora = computadora;
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
    }

}





