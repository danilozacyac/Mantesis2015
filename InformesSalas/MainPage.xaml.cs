using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InformesSalas.Dto;
using InformesSalas.Models;
using Telerik.Windows.Controls;

namespace InformesSalas
{
    /// <summary>
    /// Lógica de interacción para MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        private ProyectoDto selectedProyecto;
        private ClasificacionDto selectedFiltro;
        

        private int filtroClasif = 0;
        List<TesisInforme> listaTesis = null;
        TesisInforme tesisSeleccionada = null;
        List<TreeViewItem> items = null;

        //public MainPage(int proyectoSala)
        //{
        //    InitializeComponent();
        //    this.proyectoSala = proyectoSala;
        //}

        public MainPage() { InitializeComponent(); }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            CbxProyectos.DataContext = new ProyectoModel().GetProyectos();
            ClasificacionTreeView.DataContext = new ClasificacionModel().GetClasificacion(null, 1);
        }

        private void DgTesis_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (selectedFiltro == null || selectedFiltro.IdClasif == 0 || selectedFiltro.IdClasif == 1)
            {
                ClasificarTesis add = new ClasificarTesis(tesisSeleccionada, 1, selectedFiltro);
                add.ShowDialog();
            }
            else
            {
                ClasificarTesis update = new ClasificarTesis(tesisSeleccionada, 2, selectedFiltro);
                update.ShowDialog();
            }
        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void CbOrden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = cbOrden.SelectedItem as ComboBoxItem;

            if (filtroClasif == 1 || filtroClasif == 0)
                listaTesis = new TesisInformeModel().GetTesisSalasVolumen(selectedProyecto.Sala,Convert.ToInt16(item.Tag));
            else
                listaTesis = new TesisInformeModel().GetTesisInformeSalas(Convert.ToInt16(item.Tag), filtroClasif);

            dgTesis.DataContext = listaTesis;


            
        }

        private void BtnIr_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbxProyectos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProyecto = CbxProyectos.SelectedItem as ProyectoDto;
            ClasifVar.CurrentProject = selectedProyecto;


            listaTesis = new TesisInformeModel().GetTesisSalasVolumen(selectedProyecto.Sala,0);
            dgTesis.DataContext = listaTesis;

            //Solo durante las pruebas
            ActualizarVrias varias = new ActualizarVrias();
            varias.ShowDialog();
            //Hasta aqui
        }

        private void ClasificacionTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectedFiltro = ClasificacionTreeView.SelectedItem as ClasificacionDto;

            if (selectedFiltro == null || selectedFiltro.IdClasif == 1 || selectedFiltro.IdClasif == 0)
                listaTesis = new TesisInformeModel().GetTesisSalasVolumen(selectedProyecto.Sala, 0);
            else
                listaTesis = new TesisInformeModel().GetTesisInformeSalas(0, filtroClasif);

            dgTesis.DataContext = listaTesis;

        }

        private void dgTesis_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            tesisSeleccionada = dgTesis.SelectedItem as TesisInforme;
        }

        
    }
}