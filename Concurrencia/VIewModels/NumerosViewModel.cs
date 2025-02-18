using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Concurrencia.VIewModels
{
    public class NumerosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public NumerosViewModel()
        {
            SumarCommand = new RelayCommand(SumarParallel
                );
        }

        private void SumarSincrono()
        {
            long suma = 0;
            for (long i = 0; i < 1000000000; i++)
            {
                suma += i;
            }
            Suma = suma;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));
        }
        private async void SumarAsync()
        {

            //_= es descarte , para no estar esperando
            _= Task.Run(() =>
            {
                long suma = 0;
                for (long i = 0; i < 1000000000; i++)
                {
                    suma += i;
                }
                Suma = suma;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));
            });
            
           
        }
        private void SumarThread()
        {
            
            Thread hilo = new Thread(() =>
            {
                Stopwatch s = new();
                long suma = 0;
                for (long i = 0; i < 1000000000; i++)
                {
                    suma += i;
                }
                Suma = suma;
                s.Stop();
                Tiempo = s.Elapsed.ToString(); ;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            });
            hilo.IsBackground = true; //para que no tenga una interfaz
            hilo.Start();
        }

        private async void SumarParallel()
        {
            await Task.Run(() =>
            {
                Stopwatch s = new();
                long suma = 0;
                Parallel.For(1, 10, (x) =>
                {
                    long rango = 1000000000 / 10;
                    long inicial = rango * (x - 1) + 1;
                    for (long i = inicial; i < inicial+rango; i++)
                    {
                        suma += i;
                    }
                });
                Suma = suma;
                s.Stop();
                Tiempo = s.Elapsed.ToString(); ;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            });
            
        }

        public ICommand SumarCommand { get; set; }  
        public long Suma { get; private set; }
        public string Tiempo { get; set; }




    }
}
