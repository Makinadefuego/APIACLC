namespace APISCLC.Models
{
    public class Cancelacion
    {
        public int idCancelaciones { get; set; }
        public string usuario { get; set; }
        public int compu { get; set; }
        public int reserva { get; set; }
        public Usuario Usuario { get; set; }
        public Computadora Computadora { get; set; }
        public Reservacion Reservacion { get; set; }
    }
}
