using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio2ChatCliente.Models.DTOs
{
    public class MensajeDTO
    {
        public string Mensaje { get; set; } = "";
        public string Origen { get; set; } = "";
        public DateTime Fecha { get; set; }
    }
}
