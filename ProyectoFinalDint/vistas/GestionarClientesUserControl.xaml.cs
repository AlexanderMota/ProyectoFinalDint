﻿using ProyectoFinalDint.vistamodelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoFinalDint.vistas
{
    /// <summary>
    /// Lógica de interacción para GestionarClientesUserControl.xaml
    /// </summary>
    public partial class GestionarClientesUserControl : UserControl
    {
        public GestionarClientesUserControl()
        {
            InitializeComponent();
            this.DataContext = new GestionarClientesVM();

        }
    }
}
