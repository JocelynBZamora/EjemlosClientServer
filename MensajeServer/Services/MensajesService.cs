using MensajeServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MensajeServer.Services
{
    public class MensajesService
    {
        HttpListener _servidor = new();
        public event EventHandler<Mensaje>? MensajeRecibido;
        public MensajesService()
        {
            _servidor.Prefixes.Add("http://*:7002/mensajitos/");
            _servidor.Start();

            new Thread(RecibirPeticiones) { IsBackground = true }.Start();
            
        }

        void RecibirPeticiones()
        {
            while (true) 
            {
                var _context = _servidor.GetContext();
                if(_context!= null)
                {
                    if (_context.Request.QueryString["texto"]!=null)
                    {
                        Mensaje _mensaje = new Mensaje()
                        {
                            Texto = _context.Request.QueryString["texto"] ?? "",
                            ColorFondo = _context.Request.QueryString["colorfondo"] ?? "#000",
                            ColorLetra = _context.Request.QueryString["colorletra"] ?? "#fff"
                        };
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MensajeRecibido?.Invoke(this, _mensaje);
                        });
                        _context.Response.StatusCode = 200;
                        _context.Response.Close();
                    }
                    else
                    {
                        _context.Response.StatusCode = 400;
                        _context.Response.Close();
                    }
                }
            }
        }
    }
}
