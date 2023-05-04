using System.ComponentModel.DataAnnotations;

namespace TSK.Models
{
    public class Query
    {
        public int id { get; set; }

        public int? valor1 { get; set; }
        public int? valor2 { get; set; }
        public int? valor3 { get; set; }
        public string texto1 { get; set; }
        public string texto2 { get; set; }
        public string texto3 { get; set; }
        public string texto4 { get; set; }
        public string texto5 { get; set; }
        public string texto6 { get; set; }
        public string texto7 { get; set; }
        public string texto8 { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? fecha1 { get; set; }
        public bool? habilitado { get; set; }

        public bool? creado { get; set; }

    }
}
