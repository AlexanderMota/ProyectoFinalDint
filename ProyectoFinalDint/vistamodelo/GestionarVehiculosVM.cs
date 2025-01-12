﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using ProyectoFinalDint.modelo;
using ProyectoFinalDint.servicios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProyectoFinalDint.mensajes.Mensajes;
using static ProyectoFinalDint.servicios.MessageService;

namespace ProyectoFinalDint.vistamodelo
{
    class GestionarVehiculosVM : ObservableObject
    {
        /*private Estacionamientos plazaActual;
        public Estacionamientos PlazaActual
        {
            get => plazaActual;
            private set => plazaActual = value;
        }
        private Clientes clienteActual;
        public Clientes ClienteActual
        {
            get => clienteActual;
            private set => SetProperty(ref clienteActual, value);
        }*/
        private Vehiculos vehiculoActual;
        public Vehiculos VehiculoActual
        {
            get => vehiculoActual;
            set
            {
                SetProperty(ref vehiculoActual, value);
                /*this.ClienteActual = new SQLiteRepositoryClientes().FindById(this.VehiculoActual.Id_cliente);
                this.PlazaActual = new SQLiteRepositoryEstacionamientos().FindByMatricula(this.VehiculoActual.Matricula);*/
            }
        }
        private ObservableCollection<Vehiculos> listaVehiculos;
        public ObservableCollection<Vehiculos> ListaVehiculos
        {
            get => listaVehiculos;
            set => SetProperty(ref listaVehiculos, value);
        }

        public RelayCommand AñadirVehiculoCommand { get; }

        public RelayCommand EditarVehiculoCommand { get; }

        public RelayCommand BorrarVehiculoCommand { get; }

        SQLiteRepositoryVehiculos ServicioSQLVehiculos;

        NavigationService navigation;
        public GestionarVehiculosVM()
        {
            navigation = new NavigationService();
            ServicioSQLVehiculos = new SQLiteRepositoryVehiculos();
            WeakReferenceMessenger.Default.Register<VehiculoMessage>(this, (r, m) => { VehiculoActual = m.Value; });
            this.ListaVehiculos = ServicioSQLVehiculos.FindAll();
            AñadirVehiculoCommand = new RelayCommand(AñadirVehiculo);
            EditarVehiculoCommand = new RelayCommand(EditarVehiculo);
            BorrarVehiculoCommand = new RelayCommand(BorrarVehiculo);
        }

        private void BorrarVehiculo()
        {
            if (new SQLiteRepositoryEstacionamientos().FindByMatricula(this.VehiculoActual.Matricula) == null)
            {
                new SQLiteRepositoryVehiculos().DeleteVehiculo(this.VehiculoActual);
            }
            ListaVehiculos = ServicioSQLVehiculos.FindAll();
        }

        private void EditarVehiculo()
        {
            if (new SQLiteRepositoryEstacionamientos().FindByMatricula(this.VehiculoActual.Matricula) == null)
            {
                new SQLiteRepositoryVehiculos().UpdateVehiculo(this.VehiculoActual);
            }
            ListaVehiculos = ServicioSQLVehiculos.FindAll();
        }

        private void AñadirVehiculo()
        {
            bool? dialogResult = navigation.AbrirDialogoNuevoVehiculo();
            if(dialogResult == true)
            {

                if (new SQLiteRepositoryEstacionamientos().FindByMatricula(this.VehiculoActual.Matricula) == null)
                {
                    new SQLiteRepositoryVehiculos().InsertaVehiculo(this.VehiculoActual);
                }
                ListaVehiculos = ServicioSQLVehiculos.FindAll();
            }
            
        }
    }
}