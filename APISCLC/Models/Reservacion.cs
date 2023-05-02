namespace APISCLC.Models
{
    public class Reservacion
    {
        public int idReserva { get; set; }
        public DateTime fechahora_reserva { get; set; }
        public int modulo_sreservacion { get; set; }
        public Usuario Usuario { get; set; }
        public Computadora Computadora { get; set; }

        public Laboratorio Laboratorio { get; set; }
    }
}
