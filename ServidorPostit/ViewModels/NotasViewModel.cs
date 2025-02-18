using ServidorPostit.Models;
using ServidorPostit.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServidorPostit.ViewModels
{
    public class NotasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Nota> Notas { get; set; } = new();
        ServerHttp _server = new ServerHttp();
        public string IP
        {
            get
            {
                return string.Join(",",Dns.GetHostAddresses(Dns.GetHostName()).
                    Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).
                    Select(x => x.ToString()));
            }
        }
        public NotasViewModel()
        {
            _server.NotaRecibida += _server_NotaRecibida;
            _server.Iniciar();
        }
        Random r = new();
        private void _server_NotaRecibida(object? sender, Models.Nota e)
        {
            e.Angulo = r.Next(-5, 6);
            Notas.Add(e);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
