﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mantesis2015.UserControls.Controller;
using MantesisCommonObjects.Dto;
using MantesisCommonObjects.MantUtilities;
using ScjnUtilities;
using Telerik.Windows.Controls;

namespace Mantesis2015.UserControls
{
    /// <summary>
    /// Lógica de interacción para ListaDeTesis.xaml
    /// </summary>
    public partial class ListaDeTesis : UserControl
    {
        ListaTesisController controller;
        

        /// <summary>
        /// Indica la época, ápendice o informe seleccionado y que sirve para llenar el combo de volumenes
        /// </summary>
        private DatosComp selectedEpoca;

        /// <summary>
        /// Despliega las tesis que pertenecen a la ápoca y volumen seleccionados. El objeto puede ser de Tipo DatosComp
        /// o de tipo volumen de acuerdo con la época seleccionada
        /// </summary>
        private object volumeSelect;

        


        public ListaDeTesis()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            controller = new ListaTesisController(this);

            controller.GetTesisImportadas();
            //permisosCon = new PermisosController(this);
            //permisosCon.LoadListaTesisControlAuth();


        }

       

        private void CbxMaterias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbxMaterias.SelectedItem is DatosComp)
            {
                DatosComp comp = CbxMaterias.SelectedItem as DatosComp;
                controller.GetTesisFiltradas(comp.IdDato);
            }
            else if (CbxMaterias.SelectedItem is Volumen)
            {
                Volumen volumen = CbxMaterias.SelectedItem as Volumen;
                ValuesMant.Volumen = volumen.Volumenes;
                controller.GetTesisFiltradas(volumen.Volumenes);
            }
        }

        private void TxtIus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void TxtIus_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void BtnVerIus_Click(object sender, RoutedEventArgs e)
        {
            controller.GetTesisByVerIus(TxtVerIus.Text);
        }

        private void TxtVerIus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                controller.GetTesisByVerIus(TxtVerIus.Text);
        }




        #region Metodos Externos

        /// <summary>
        /// Muestra el detalle de la tesis seleccionada
        /// </summary>
        /// <param name="materiasSgaEstado">Indica si las materias SGA pueden ser actualizadas</param>
        /// <param name="isTesisUpdatable">Indica si la tesis que se va a mostrar puede sufrir modificaciones</param>
        /// <param name="mainWindow">Ventana dueña de la que esta por ser lanzada</param>
        public void MostrarTesis(byte materiasSgaEstado, bool isTesisUpdatable,MainWindow mainWindow)
        {
            controller.MostrarTesis(materiasSgaEstado, isTesisUpdatable,mainWindow);
        }

        public void ExportaInformacionTesis(RadRibbonButton action)
        {
            controller.ExportarOptions(action.Name);
        }

        #endregion

        private void GTesis_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            controller.TesisSeleccionada();
        }

        private void GTesis_Sorting(object sender, GridViewSortingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
