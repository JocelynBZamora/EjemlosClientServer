using Ejercicio2Chat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows;

namespace Ejercicio2Chat.Services
{
    public class ChatServer
    {
        TcpListener server = null!;
        List<TcpClient> clients = new List<TcpClient>();
       //public Dictionary<IPEndPoint, string> Usuarios { get; set; } = new();
       public event EventHandler<MensajeDTO> MensajeRecibido;
        public void Iniciar()
        {
            server = new(new IPEndPoint(IPAddress.Any,9000));
            server.Start();
            new Thread(Escuchar) { IsBackground = true }.Start();

        }

        void Escuchar()
        {
            while (server.Server.IsBound)
            {
               var tcpClient=  server.AcceptTcpClient();
                clients.Add(tcpClient);

                Thread t = new(() =>
                {
                    RecibirMensajes(tcpClient);
                });
                t.IsBackground = true;  
                t.Start();
            }
        }
        void RecibirMensajes(TcpClient cliente)
        {
            while (cliente.Connected)
            {
                var ns = cliente.GetStream();
                while(cliente.Available== 0)
                {
                    Thread.Sleep(500);
                }
                byte[] buffer = new byte[cliente.Available];
                ns.Read(buffer,0, buffer.Length);
                string json = Encoding.UTF8.GetString(buffer);

                var mensaje = JsonSerializer.Deserialize<MensajeDTO>(json);

                if (mensaje != null)
                {

                    RelayMensaje(cliente, buffer);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MensajeRecibido?.Invoke(this, mensaje);

                    });

                }
            }
            clients.Remove(cliente);
        }
        void RelayMensaje(TcpClient cliente, byte[] mensaje)
        {
            foreach (var item in clients)
            {
                if(item != cliente)//Enviar a todos menos al origen
                {
                    var ns = item.GetStream();
                    ns.Write(mensaje,0, mensaje.Length);
                    ns.Flush();

                }
            }
        }
        public void Detener()
        {
            if(server != null) 
            { 
                server.Stop();
                foreach (var item in clients)
                {
                    item.Close();
                }
            }
        }
    }
}













//var ipe = (IPEndPoint)cliente.Client.RemoteEndPoint ?? new IPEndPoint(IPAddress.Loopback, 8001);
//if (mensaje.Mensaje == "**HEllO")
//{

//    if (Usuarios.ContainsKey(ipe))
//    {
//        Usuarios.Add(ipe, mensaje.Origen);
//    }
//}
//else if (mensaje.Mensaje == "**BYE")
//{

//}
//else
//{

//}