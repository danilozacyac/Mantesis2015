using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Infragistics.Windows.DataPresenter;
using Mantesis2015.UserControls.Controller;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;

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

        }

        private void RRadEpocas_Checked(object sender, RoutedEventArgs e)
        {
            controller.SetEpocasComboValues(5);
        }

        private void RRadApendices_Checked(object sender, RoutedEventArgs e)
        {
            controller.SetEpocasComboValues(7);
        }

        private void RRadInformes_Checked(object sender, RoutedEventArgs e)
        {
            controller.SetEpocasComboValues(6);
        }

        private void CbEpoca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEpoca = CbEpoca.SelectedItem as DatosComp;
            controller.SeleccionaEpoca(selectedEpoca);
        }

        private void CbxVolumen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            controller.GetTesisPorEpocaVolumen(99, CbxVolumen.SelectedItem, selectedEpoca);
        }

        private void XamDataGridTesis_Loaded(object sender, RoutedEventArgs e)
        {
            Style s = (Style)this.FindResource("MyConvertRow");

            FieldLayoutSettings field = new FieldLayoutSettings();
            field.DataRecordPresenterStyle = s;
            XamDataGridTesis.FieldLayoutSettings = field;
        }

        private void XamDataGridTesis_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            if (source == null)
                return;

            DataRecordPresenter drp = Infragistics.Windows.Utilities.GetAncestorFromType(source,
                typeof(DataRecordPresenter), true) as DataRecordPresenter;
            if (drp == null)
                return;

            if (drp.Record != null)
            {
                //BtnVisualizar_Click(sender, null);
            }
        }

        private void XamDataGridTesis_RecordActivated(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        {
            controller.GetDatosTesisSeleccionada(e);
        }

        private void XamDataGridTesis_Sorting(object sender, Infragistics.Windows.DataPresenter.Events.SortingEventArgs e)
        {
            e.Cancel = true;
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
    }
}
