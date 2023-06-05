namespace APISCLC.Models
{
    public class Reservacion
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int modulo { get; set; }
        public Usuario Usuario { get; set; }
        public Laboratorio Laboratorio { get; set; }
    }
}
