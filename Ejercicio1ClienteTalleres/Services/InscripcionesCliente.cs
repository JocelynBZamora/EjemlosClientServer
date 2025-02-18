using Ejercicio1ServidorTalleres.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ejercicio1ClienteTalleres.Services
{
    public class InscripcionesCliente
    {
        private UdpClient cliente = new();
        public string Servidor { get; set; } = "0.0.0.0";
        public void EnviarInscripcion(InscripcionDTO dto)
        {
            var ipe = new IPEndPoint(IPAddress.Parse(Servidor),5001);
            var json = JsonSerializer.Serialize(dto);
            

            byte[] buffer = Encoding.UTF8.GetBytes(json);


            cliente.Send(buffer, buffer.Length, ipe);
        }
    }
}
