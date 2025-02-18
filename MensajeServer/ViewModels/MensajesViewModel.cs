using CommunityToolkit.Mvvm.ComponentModel;
using MensajeServer.Models;
using MensajeServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajeServer.ViewModels
{
    public partial class MensajesViewModel:ObservableObject
    {
        [ObservableProperty]
        private Mensaje? mensaje = new();


        MensajesService mensajesService = new();
        DiscoveryService discoveryService = new DiscoveryService();

        public MensajesViewModel()
        {
            mensajesService.MensajeRecibido += MensajesService_MensajeRecibido;
        }

        private void MensajesService_MensajeRecibido(object? sender, Mensaje e)
        {
            if(Mensaje != null)
            {
                Mensaje = e;
            }
        }

        
    }
}
