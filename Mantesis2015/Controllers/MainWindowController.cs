using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Infragistics.Windows.DataPresenter;
using Mantesis2015.Model;
using Mantesis2015.Reportes;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Singletons;
using MantesisVerIusCommonObjects.Utilities;
using MateriasSgaControl;
using UtilsMantesis;

namespace Mantesis2015.Controllers
{
    public class MainWindowController 
    {
        readonly MainWindow main;
        private List<AddTesis> listaTesis;
        //private List<AddTesis> listaTesisTemp;

        //Datos de la tesis seleccionada
        private int selectedIus;
        private int selectedRowIndex;

        private const string RegLoc = "Registros localizados : ";

        public MainWindowController(MainWindow main)
        {
            this.main = main;
        }

        /// <summary>
        /// Regresa las tesis del Volumen solicitado y posteriormente llama a setGRidResultados
        /// </summary>
        /// <param name="qMateria"></param>
        public void GetTesisPorEpocaVolumen(int qMateria, object volumenSelect, DatosComp epoca)
        {
            int permitido = 0;
            ValuesMant.Epoca = epoca.IdDato;

            if (volumenSelect is Volumen)
            {
                Volumen selectedVolume = volumenSelect as Volumen;
                ValuesMant.Volumen = selectedVolume.Volumenes;
                //permitido = (from n in AccesoUsuarioModel.VolumenesPermitidos
                //             where n.Volumen == selectedVolume.Volumenes
                //             select n).ToList().Count;
            }
            else if (volumenSelect is DatosComp)
            {
                DatosComp selectedVolume = volumenSelect as DatosComp;
                this.SetSelectedApendice();

                ValuesMant.Parte = Utils.GetParte(selectedVolume.IdDato, ValuesMant.ApendicNom);
                ValuesMant.Volumen = selectedVolume.IdDato;
                //permitido = (from n in AccesoUsuarioModel.VolumenesPermitidos
                //             where n.Volumen >= ValuesMant.MinVolumen && n.Volumen <= ValuesMant.MaxVolumen
                //             select n.Volumen).ToList().Count;
            }

            //if (permitido == 0)
            //{
            //    MessageBox.Show("No tiene permiso para revisar la información relacionada con este volumen", "  Aviso  ");
            //    return;
            //}

            ListaTesisModel listaTesisModel = new ListaTesisModel();
            listaTesis = listaTesisModel.CargaTesisMantesisSql(qMateria);

            main.XamDataGridTesis.DataSource = listaTesis;
            main.XamDataGridTesis.Focus();
            if (main.XamDataGridTesis.Records.Count == 0)
            {
                MessageBox.Show("No existe el registro");
            }
            else
            {
                main.XamDataGridTesis.ActiveRecord = main.XamDataGridTesis.Records[0];
                main.TxtRegs.Content = RegLoc + listaTesis.Count;
            }

            this.SetFiltroPorMateriaTomo();
        }

        public void GetTesisFiltradasPorMateria(int qMateria)
        {
            ListaTesisModel listaTesisModel = new ListaTesisModel();
            listaTesis = listaTesisModel.CargaTesisMantesisSql(qMateria);

            main.XamDataGridTesis.DataSource = listaTesis;
            main.XamDataGridTesis.Focus();
            if (main.XamDataGridTesis.Records.Count == 0)
            {
                MessageBox.Show("No existe el registro");
            }
            else
            {
                main.XamDataGridTesis.ActiveRecord = main.XamDataGridTesis.Records[0];
                main.TxtRegs.Content = RegLoc + listaTesis.Count;
            }
        }

        public void GetTesisFiltradasPorTomo(int volumen)
        {

            ListaTesisModel listaTesisModel = new ListaTesisModel();
            listaTesis = listaTesisModel.CargaTesisMantesisSql(volumen);

            main.XamDataGridTesis.DataSource = listaTesis;
            main.XamDataGridTesis.Focus();
            if (main.XamDataGridTesis.Records.Count == 0)
            {
                MessageBox.Show("No existen registros");
            }
            else
            {
                main.XamDataGridTesis.ActiveRecord = main.XamDataGridTesis.Records[0];
                main.TxtRegs.Content = RegLoc + listaTesis.Count;
            }
            
        }

        /// <summary>
        /// Llena el comboBox que permite filtrar la lista de materias de determinada época y tomo/número
        /// de acuerdo a la materia o clasificación de las tesis
        /// </summary>
        private void SetFiltroPorMateriaTomo()
        {
            main.CbxMaterias.Visibility = Visibility.Visible;
            main.LblMaterias.Visibility = Visibility.Visible;
            if (ValuesMant.Epoca == ConstMant.Apendice)
            {
                if (ValuesMant.ApendicNom == 6)
                {
                    this.CargaVolumenesApendice2011();
                }
                else
                {
                    main.CbxMaterias.Visibility = Visibility.Collapsed;
                    main.LblMaterias.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                main.CbxMaterias.ItemsSource = Utils.GetMateriasForFiltro();
                main.CbxMaterias.DisplayMemberPath = "Descripcion";
                main.CbxMaterias.SelectedValuePath = "IdDato";
            }
        }

        /// <summary>
        /// Devuelve el rango de volumenes para cada uno de los tomos del apéndice de 2011
        /// </summary>
        private void CargaVolumenesApendice2011()
        {
            int volInicial = 0;
            int volFinal = 0;
            switch (MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen)
            {
                case 1:
                    volInicial = 14000;//Constitucional
                    volFinal = 14052;
                    break;
                case 2:
                    volInicial = 14119; //Penal
                    volFinal = 14127;
                    break;
                case 3:
                    volInicial = 14128; //Administrativa
                    volFinal = 14139;
                    break;
                case 4:
                    volInicial = 14140; //Civil
                    volFinal = 14162;
                    break;
                case 5:
                    volInicial = 14163; //Laboral
                    volFinal = 14179;
                    break;
                case 10:
                    volInicial = 14194; //Electoral
                    volFinal = 14195;
                    break;
                case 50:
                    volInicial = 14053; // Procesal Constitucional
                    volFinal = 14118;
                    break;
                case 51:
                    volInicial = 14180; // Conflictos Competenciales
                    volFinal = 14193;
                    break;
            }

            main.CbxMaterias.ItemsSource = (from n in VolumenSingleton.Volumenes
                                            where n.Volumenes == -1 || (n.Volumenes >= volInicial && n.Volumenes <= volFinal)
                                            orderby n.Orden
                                            select n).ToArray();
            main.CbxMaterias.DisplayMemberPath = "VolumenTxt";
            main.CbxMaterias.SelectedValuePath = "Volumenes";
        }

        private void SetSelectedApendice()
        {
            if (main.rbtAp17.IsChecked == true)
            {
                ValuesMant.ApendicNom = 1;
            }
            else if (main.rbtAp54.IsChecked == true)
            {
                ValuesMant.ApendicNom = 2;
            }
            else if (main.rbtAp2000.IsChecked == true)
            {
                ValuesMant.ApendicNom = 3;
            }
            else if (main.rbtAp2001.IsChecked == true)
            {
                ValuesMant.ApendicNom = 4;
            }
            else if (main.rbtAp2002.IsChecked == true)
            {
                ValuesMant.ApendicNom = 5;
            }
            else if (main.rbtAp2011.IsChecked == true)
            {
                ValuesMant.ApendicNom = 6;
            }
        }
        
        //public void GetSearchResult(string search)
        //{

        //    if (!String.IsNullOrEmpty(search))
        //    {
        //        listaTesisTemp = (from n in listaTesis
        //                          where n.Rubro.Contains(search) || n.Tesis.ToUpper().Contains(search)
        //                          select n).ToList();
        //        main.XamDataGridTesis.DataSource = listaTesisTemp;
        //        main.TxtRegs.Content = RegLoc + listaTesisTemp.Count;
        //    }
        //    else
        //    {
        //        main.XamDataGridTesis.DataSource = listaTesis;
        //        main.TxtRegs.Content = RegLoc + listaTesis.Count;
        //    }
        //}

        public void MoveGridToIus(long nIus)
        {
            bool existe = false;
            foreach (DataRecord item in main.XamDataGridTesis.Records)
            {
                if (nIus == Convert.ToInt32(item.Cells[1].Value))
                {
                    item.IsSelected = true;
                    main.XamDataGridTesis.ActiveRecord = item;
                    existe = true;
                    break;
                }
            }

            if (!existe)
                MessageBox.Show("No existe el registro solicitado");
        }

        public void GetDatosTesisSeleccionada(Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        {
            if (e.Record is DataRecord)
            {
                // Cast the record passed in as a DataRecord
                DataRecord myRecord = (DataRecord)e.Record;
                // Display the selected Records values in the appropriate 
                // editor

                this.selectedIus = Convert.ToInt32(myRecord.Cells[1].Value);
                this.selectedRowIndex = myRecord.Index;
            }
        }

        public void LaunchUnaTesis()
        {
            UnaTesis unaTesis = new UnaTesis(selectedIus, 1, listaTesis, selectedRowIndex, false);
            unaTesis.ShowDialog();
        }

        #region AccionesTesis

        /// <summary>
        /// Permite observar todos los detalles de la tesis, además de sus datos de publicación
        /// NO permite actulizar ningún dato 
        /// </summary>
        /// <param name="materiasEstado">Indica si las materias SGA pueden ser actualizadas o no</param>
        /// <param name="isTesisUpdatable">Indica si la tesis puede ser actualizada o no</param>
        public void MostrarTesis(byte materiasEstado, bool isTesisUpdatable)
        {
            RequestData.Estado = materiasEstado;
            ValuesMant.IusActualLstTesis = this.selectedIus;
            UnaTesis fUnaTesis = new UnaTesis(this.selectedIus, materiasEstado, listaTesis, this.selectedRowIndex, isTesisUpdatable);

            fUnaTesis.ShowDialog();

            MoveGridToIus(ValuesMant.IusActualLstTesis);
        }

        #endregion 

        public void ExportarOptions(string id)
        {
            switch (id)
            {
                case "RBtnPdf":
                    MessageBoxResult result = MessageBox.Show("¿Desea generar el reporte con el detalle de las tesis? Si su respuesta es NO solo se generará el listado de tesis" , "Atención:", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        new ListaTesisPdf().GeneraPdfConDetalleTesis(Convert.ToInt32(main.CbxMaterias.SelectedValue));
                    else if (result == MessageBoxResult.No)
                        new ListaTesisPdf().GeneraPdfListaTesis(listaTesis, main.CbEpoca.Text, main.CbxVolumen.Text);
                    break;
                case "RBtnWord": new ListaTesisWord().GeneraPdfListaTesis(listaTesis, main.CbEpoca.Text, main.CbxVolumen.Text);
                    break;
                case "RBtnSga":
                    TablaSga tabla = new TablaSga();
                    tabla.GeneraReporte();
                    break;
            }
        }
    }
}