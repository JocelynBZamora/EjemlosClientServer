// See https://aka.ms/new-console-template for more information

//CLIENTE

using System.Net;
using System.Net.Sockets;
using System.Text;

UdpClient cliente = new();
cliente.EnableBroadcast = true;
Console.WriteLine("Escribe el mendaje a enviar: ");
string s = Console.ReadLine()?? "";

IPEndPoint endPoint = new(IPAddress.Broadcast, 5001);

byte[] buffer = Encoding.UTF8.GetBytes(s);
cliente.Send(buffer, buffer.Length, endPoint);