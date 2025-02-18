using CommunityToolkit.Mvvm.Input;
using Ejercicio2ChatCliente.Models.DTOs;
using Ejercicio2ChatCliente.Services;
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

namespace Ejercicio2ChatCliente.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        ChatClient client = new();
        public ICommand EnviarCommand { get; set; }
        public ObservableCollection<MensajeDTO> Mensajes { get; set; } = new ObservableCollection<MensajeDTO>();
        public string Mensaje { get; set; } = "";
        public string IP { get; set; } = "";
        
        public int NumeroMensaje { get; set; }
        public ICommand ConectarCommand { get; set; }
        public bool Conectado { get; set; }
        

        public ChatViewModel()
        {
            client.MensajeRecibido += Client_MensajeRecibido;
            EnviarCommand = new RelayCommand(Enviar);
            ConectarCommand = new RelayCommand(Conectar);
        }

        private void Conectar()
        {
            IPAddress.TryParse(IP, out IPAddress? ipAdress);
            if(ipAdress != null)
            {
                client.Conectar(ipAdress);
                Conectado = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Conectado)));
            }
        }

        private void Enviar()
        {
            if (!string.IsNullOrWhiteSpace(Mensaje))
            {
                client.EnviarMensaje(new MensajeDTO
                {
                    Fecha = DateTime.Now,
                    Origen = client.equipo,
                    Mensaje = Mensaje
                });
            }
        }

        private void Client_MensajeRecibido(object? sender, MensajeDTO e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (e.Mensaje == "**HELLO")
                {
                    e.Mensaje = $"{e.Origen} se ha conectado";
                }
                if (e.Mensaje == "**BYE")
                {
                    e.Mensaje = $"{e.Origen} se ha desconectado";
                }
                Mensajes.Add(e);
                NumeroMensaje = Mensajes.Count - 1;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumeroMensaje)));
            });

            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
