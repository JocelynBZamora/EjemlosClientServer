using ServidorPostit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace ServidorPostit.Services
{
    public class ServerHttp
    {
        HttpListener _server = new();
        public event EventHandler<Nota>? NotaRecibida;
        public ServerHttp()
        {
            _server.Prefixes.Add("http://*:12345/notas/");
        }
        public void Iniciar()
        {
            if (!_server.IsListening)
            {
                _server.Start();
                new Thread(Escuchar)
                {
                    IsBackground = true
                }.Start();

            }
        }
        void Escuchar()
        {
            while (true)
            {
                var context = _server.GetContext(); // pausa hasta que reciba el request

                var pagina = File.ReadAllText("assets/index.html");
                var bufferPagina = Encoding.UTF8.GetBytes(pagina);

                if(context.Request.Url != null)
                {
                    if (context.Request.Url.LocalPath == "/notas/")
                    {
                        context.Response.ContentLength64 = bufferPagina.Length;
                        context.Response.OutputStream.Write(bufferPagina, 0, bufferPagina.Length);
                        context.Response.StatusCode = 200;
                        context.Response.Close();
                    }
                    else if (context.Request.HttpMethod =="POST" &&
                        context.Request.Url.LocalPath == "/notas/crear")// me mandan los datos del formulario
                    {
                        byte[] bufferDatos = new byte[context.Request.ContentLength64];
                        context.Request.InputStream.Read(bufferDatos, 0, bufferDatos.Length);
                        string datos = Encoding.UTF8.GetString(bufferDatos);
                        


                        var diccionario = HttpUtility.ParseQueryString(datos);

                        Nota nota = new()
                        {
                            Titulo = diccionario["titulo"] ?? "",
                            Contenido = diccionario["contenido"] ?? "",
                            X = int.Parse(diccionario["x"] ?? "0"),
                            Y = int.Parse(diccionario["y"] ?? "0"),
                            Remitente = context.Request.RemoteEndPoint.Address.ToString()

                        };

                        Application.Current.Dispatcher.Invoke(() =>
                        {

                            NotaRecibida?.Invoke(this, nota);
                        });
                        context.Response.StatusCode = 404;
                        context.Response.Close();

                    }
                        
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Close();
                    }

                
                
                
            }
        }


        public void Detener()
        {
            _server.Stop();
        }
    }
}
