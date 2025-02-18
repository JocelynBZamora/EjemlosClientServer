using Ejercicio1ClienteTalleres.Services;
using Ejercicio1ServidorTalleres.Models.DTOs;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ejercicio1ClienteTalleres.ViewModels
{
    public class InscripcionesViewModel : INotifyPropertyChanged
    {
        public InscripcionDTO Datos { get; set; } = new();
        private InscripcionesCliente ClienteUDP = new();
        public string IP { get; set; } = "0.0.0.0";

        public ICommand InscribirCommand { get; set; }
        public InscripcionesViewModel()
        {
            InscribirCommand = new RelayCommand(Inscribir);
        }

        private void Inscribir()
        {
            ClienteUDP.Servidor = IP;
            ClienteUDP.EnviarInscripcion(Datos);
        }

        


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
