using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MensajeServer.Services
{
    public class DiscoveryService
    {
        UdpClient _server = new()
        {
            EnableBroadcast = true
        };
        IPEndPoint destino = new IPEndPoint(IPAddress.Broadcast, 7000);
        
        byte[] buffer;
        public DiscoveryService()
        {
            var usuario = Environment.UserName;
            buffer = Encoding.UTF8.GetBytes(usuario);
            Saludar(); //cuando lo creamos lo mandamos a la red
            new Thread(RecibirSaludo) { IsBackground = true }.Start();//para esperar a que nos saluden
            new Thread(StillAlive) { IsBackground = true}.Start();//para informar que seguimos vivos

        }
        public void Saludar()
        {
            _server.Send(buffer,buffer.Length,destino);
        }
        public void RecibirSaludo()
        {
            UdpClient udp2 = new UdpClient(7001) ;
            while (true)
            {
                IPEndPoint remoto = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = udp2.Receive(ref remoto);
                _server.Send(buffer, buffer.Length, remoto);
            }
        }
        private void StillAlive()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(30));
                Saludar();
            }
        }
    }
}
