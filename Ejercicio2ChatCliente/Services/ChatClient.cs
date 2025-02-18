using Ejercicio2ChatCliente.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Ejercicio2ChatCliente.Services
{
    public class ChatClient
    {
        TcpClient client = null!;
       public string equipo { get; set; } = null!;
        public event EventHandler<MensajeDTO>? MensajeRecibido;
        public void Conectar(IPAddress ip)
        {
            try
            {
                IPEndPoint ipe = new IPEndPoint(ip, 9000);
                client = new();
                client.Connect(ipe);
                var equipo = Dns.GetHostName();
                var msg = new MensajeDTO
                {
                    Fecha = DateTime.Now,
                    Mensaje = "**HELLO",
                    Origen = equipo
                };
                EnviarMensaje(msg);
                RecibirMensaje();
            }
            catch (Exception ex)
            {

                //mostrar error
            }

        }
        public void Desconectar()
        {
            var msg = new MensajeDTO
            {
                Fecha = DateTime.Now,
                Mensaje = "**BYE",
                Origen = equipo
            };
            EnviarMensaje(msg);
            client.Close();
        }
        private void RecibirMensaje()
        {
            new Thread(() =>
            {
                try
                {
                    while (client.Connected)
                    {
                        if (client.Available > 0)
                        {
                            var ns = client.GetStream();
                            byte[] buffer = new byte[client.Available];
                            ns.Read(buffer, 0, buffer.Length);
                            var msg = JsonSerializer.Deserialize<MensajeDTO>(Encoding.UTF8.GetString(buffer));

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                if (msg != null)
                                {
                                    MensajeRecibido?.Invoke(this, msg);

                                }
                            });
                           
                        }
                    }
                }
                catch(Exception ex) { }
                
            })
            { IsBackground = true}.Start();
        }
        public void EnviarMensaje(MensajeDTO m)
        {
            if (!string.IsNullOrWhiteSpace(m.Mensaje))
            {
                var json = JsonSerializer.Serialize(m);
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                var ns = client.GetStream();
                ns.Write(buffer, 0, buffer.Length);

                ns.Flush();
            }
        }
    }
}
