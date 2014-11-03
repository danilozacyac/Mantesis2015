﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Infragistics.Windows.DataPresenter;
using Mantesis2015.Controllers;
using Mantesis2015.Model;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Singletons;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;

namespace Mantesis2015
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<RadioButton> apendices;
        private DatosComp epocaSelec;
        private object volumeSelect;
        private MainWindowController controller;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new AccesoModel().ObtenerUsuarioContraseña("lvega", "lvegaa");
            controller = new MainWindowController(this);
            apendices = new List<RadioButton>() { rbtAp17, rbtAp2000, rbtAp2001, rbtAp2002, rbtAp2011, rbtAp54 };
            
            CbEpoca.DataContext = DatosCompartidosSingleton.Epocas;
            SbxRubroClave.LabelText = "Buscar por rubro o clave de tesis";
        }

        private void CbEpoca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            epocaSelec = CbEpoca.SelectedItem as DatosComp;

            if (epocaSelec.IdDato == 7) //Muestra materias apendices
            {
                CbxVolumen.IsEnabled = false;
                this.SetApendicesState(true);
            }
            else //Muestra Volumen epocas
            {
                CbxVolumen.IsEnabled = true;
                this.SetApendicesState(false);
                CbxVolumen.ItemsSource = Utils.GetVolumenesEpoca(epocaSelec.IdDato);
                CbxVolumen.DisplayMemberPath = "VolumenTxt";
                CbxVolumen.SelectedValuePath = "Volumenes";
            }
        }

        private void SetApendicesState(bool state)
        {
            foreach (RadioButton radio in apendices)
            {
                radio.IsEnabled = state;
            }
        }

        private void RadioApendiceChecked(object sender, RoutedEventArgs e)
        {
            RadioButton apenSelect = sender as RadioButton;
            
            ValuesMant.ApendicYear = Convert.ToInt32(apenSelect.Tag);

            CbxVolumen.ItemsSource = Utils.GetMateriasApendice(Convert.ToInt32(apenSelect.Tag));
            CbxVolumen.DisplayMemberPath = "Descripcion";
            CbxVolumen.SelectedValuePath = "IdDato";

            CbxVolumen.IsEnabled = true;
        }

        private void CbxVolumen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbxVolumen.SelectedItem is Volumen)
                volumeSelect = CbxVolumen.SelectedItem as Volumen;
            else if (CbxVolumen.SelectedItem is DatosComp)
                volumeSelect = CbxVolumen.SelectedItem as DatosComp;

            controller.GetTesisPorEpocaVolumen(99, volumeSelect, epocaSelec);
        }

        #region  GridTesis

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

        private void XamDataGridTesis_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void XamDataGridTesis_RecordActivated(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        {
            controller.GetDatosTesisSeleccionada(e);
        }

        private void XamDataGridTesis_Sorting(object sender, Infragistics.Windows.DataPresenter.Events.SortingEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion GridTesis

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper();
            controller.GetSearchResult(tempString);

            
        }

        private void CbxMaterias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbxMaterias.SelectedItem is DatosComp)
            {
                DatosComp comp = CbxMaterias.SelectedItem as DatosComp;
                controller.GetTesisFiltradasPorMateria(comp.IdDato);
            }
            else if (CbxMaterias.SelectedItem is Volumen)
            {

            }
        }

        private void TxtIus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void BtnIusLista_Click(object sender, RoutedEventArgs e)
        {
            long ius = 0;
            Int64.TryParse(TxtIus.Text.Trim(), out ius);

            if (ius != 0)
                controller.MoveGridToIus(ius);
            else
                MessageBox.Show("Ingrese un número de registro IUS");
        }

        private void TxtIus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnIusLista_Click(null, null);
            }
        }

        private void BtnVerIus_Click(object sender, RoutedEventArgs e)
        {
            NumIusController verIus = new NumIusController();
            verIus.GetTesisByVerIus(TxtVerIus.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {




            controller.LaunchUnaTesis();
        }

        

        
    }
}