namespace APISCLC.Models
{
    public class Computadora
    {
        public int idComputadora { get; set; }
        public int lab_id { get; set; }
        public Laboratorio Laboratorio { get; set; }
    }
}
