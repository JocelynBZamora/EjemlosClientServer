//HTTPListener

using System.Net;
using System.Text;
using System.Text.Encodings.Web;

HttpListener _server = new();
_server.Prefixes.Add("http://localhost:10000/");
var ip = Dns.GetHostAddresses(Dns.GetHostName()).Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault();
_server.Prefixes.Add($"http://{ip}:10000/");
string response = "";
_server.Start();
Console.WriteLine("escuchando en la ip " + ip);
while (true)
{
    HttpListenerContext _context = _server.GetContext();
    var nombre = _context.Request.QueryString["nombre"];
    if (nombre != null)
    {
        Console.WriteLine(nombre+ "ha hecho una peticion");
        response = $"<html><body><h1>Saludos {nombre}r</h1></body></html>";
    }
    else
    {

        response = $"<html><body><h1>Respuesta desde el servidor</h1></body></html>";
        
    }
    byte[] buffer = Encoding.UTF8.GetBytes(response);

    _context.Response.ContentLength64 = buffer.Length;
    var ns = _context.Response.OutputStream;
    ns.Write(buffer, 0, buffer.Length);
    _context.Response.StatusCode = 200;
    //_context.Response.ContentLength64 = buffer.Length;
    _context.Response.Close();
}
