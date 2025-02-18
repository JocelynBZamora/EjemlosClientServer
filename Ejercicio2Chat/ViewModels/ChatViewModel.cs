using CommunityToolkit.Mvvm.Input;
using Ejercicio2Chat.Models.DTOs;
using Ejercicio2Chat.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Ejercicio2Chat.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public ChatServer Server { get; set; } = new();
        public ObservableCollection<string> Usuarios { get; set; } = new();
        public ICommand IniciarServerCommand { get; set; }
        public ICommand DetenerServerCommand { get; set; }
        public int NumeroMensaje { get; set; }
        public string IP { get; set; } = "0.0.0.0";
        public ObservableCollection<MensajeDTO> Mensajes { get; set; } = new();
        public ChatViewModel()
        {
            var direcciones = Dns.GetHostAddresses(Dns.GetHostName());
            if(direcciones != null)
            {
                 IP = string.Join(",",direcciones.Where(x=>x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(x=>x.ToString()));
            }
            IniciarServerCommand = new RelayCommand(IniciarServer);
            DetenerServerCommand = new RelayCommand(DetenerCommand);
            Server.MensajeRecibido += Server_MensajeRecibido;
        }

        private void Server_MensajeRecibido(object? sender, MensajeDTO e)
        {
            
                if (e.Mensaje == "**HELLO")
                {
                    e.Mensaje = $"{e.Origen} se ha conectado";
                    Usuarios.Add(e.Origen);
                }
                else if (e.Mensaje == "**BYE")
                {
                    e.Mensaje = $"{e.Origen} se ha desconectado";
                    Usuarios.Remove(e.Origen);
                }
                Mensajes.Add(e);

            //NumeroMensaje = Mensajes.Count - 1;
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumeroMensaje)));

        }

        public void DetenerCommand()
        {
            Mensajes.Clear();
            Usuarios.Clear();   
            Server.Detener();
        }
        public void IniciarServer()
        {
            Server.Iniciar();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
