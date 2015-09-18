using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Infragistics.Windows.DataPresenter;
using Mantesis2015.Model;
using Mantesis2015.Reportes;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Model;
using MantesisVerIusCommonObjects.Singletons;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;
using UtilsMantesis;

namespace Mantesis2015.UserControls.Controller
{
    public class ListaTesisController
    {
        readonly ListaDeTesis listaTesisWindow;

        private DatosComp selectedEpoca;

        /// <summary>
        /// Listado de tesis que se muestran en el listado principal
        /// </summary>
        private List<AddTesis> listaTesis;

        /// <summary>
        /// Registro digital de la tesis seleccionada
        /// </summary>
        private int selectedIus;

        /// <summary>
        /// Número de fila de la tesis seleccionada
        /// </summary>
        private int selectedRowIndex;

        private const string RegLoc = "Registros localizados : ";


        public ListaTesisController(ListaDeTesis listaTesisWindow)
        {
            this.listaTesisWindow = listaTesisWindow;
        }


        /// <summary>
        /// Rellena el Combo Box de las épocas de acuerdo a lo seleccionado
        /// </summary>
        /// <param name="tipoEpoca">5. Épocas -- 6. Informes -- 7. Apéndices</param>
        public void SetEpocasComboValues(int tipoEpoca)
        {
            listaTesisWindow.CbxVolumen.DataContext = null;
            switch (tipoEpoca)
            {
                case 5: listaTesisWindow.CbEpoca.DataContext = from n in DatosCompartidosSingleton.Epocas
                                                               where n.IdDato != 6 && n.IdDato != 7
                                                               select n;
                    
                    break;
                case 6: listaTesisWindow.CbEpoca.DataContext = from n in DatosCompartidosSingleton.Epocas
                                              where n.IdDato == 6
                                              select n;
                    break;
                case 7: listaTesisWindow.CbEpoca.DataContext = DatosCompartidosSingleton.Apendices;
                    break;

            }

            
        }

        /// <summary>
        /// Despliega los volumenes contenidos en la época, informe o apéndice seleccionado
        /// </summary>
        /// <param name="selectedEpoca">Epoca, Informe o Apéndice seleccionado</param>
        public void SeleccionaEpoca(DatosComp selectedEpoca)
        {
            listaTesisWindow.CbxVolumen.DataContext = null;
            this.selectedEpoca = selectedEpoca;

            if (selectedEpoca != null)
            {
                if (listaTesisWindow.RRadApendices.IsChecked == true) //Muestra materias apendices
                {
                    ValuesMant.ApendicYear = selectedEpoca.IdDato;
                    listaTesisWindow.CbxVolumen.ItemsSource = Utils.GetMateriasApendice(selectedEpoca.IdDato);
                    listaTesisWindow.CbxVolumen.DisplayMemberPath = "Descripcion";
                    listaTesisWindow.CbxVolumen.SelectedValuePath = "IdDato";
                }
                else //Muestra Volumen epocas
                {
                    listaTesisWindow.CbxVolumen.IsEnabled = true;
                    listaTesisWindow.CbxVolumen.ItemsSource = Utils.GetVolumenesEpoca(selectedEpoca.IdDato);
                    listaTesisWindow.CbxVolumen.DisplayMemberPath = "VolumenTxt";
                    listaTesisWindow.CbxVolumen.SelectedValuePath = "Volumenes";
                }
            }
        }

        /// <summary>
        /// Obtiene las tesis que pertenecen a la época y volumen seleccionados
        /// </summary>
        /// <param name="qMateria">Materia o tomo por el que se filtrarán las tesis</param>
        /// <param name="volumenSelect">Da información del Volumen seleccionado. si el objeto es DatosComp se trata de Apéndice, si es Volumen se trata de épocas o de informes</param>
        /// <param name="epoca">Indica si las tesis solicitadas son de épocas, apéndices o informes</param>
        public void GetTesisPorEpocaVolumen(int qMateria, object volumenSelect, DatosComp epoca)
        {
            int permitido = 0;
            

            if (volumenSelect is Volumen)
            {
                Volumen selectedVolume = volumenSelect as Volumen;
                ValuesMant.Volumen = selectedVolume.Volumenes;
                permitido = (from n in AccesoUsuarioModel.VolumenesPermitidos
                             where n.Volumenes == selectedVolume.Volumenes
                             select n).ToList().Count;
                ValuesMant.Epoca = epoca.IdDato;
            }
            else if (volumenSelect is DatosComp)// Si es apéndice
            {
                DatosComp selectedVolume = volumenSelect as DatosComp;
                this.SetSelectedApendice();

                ValuesMant.Parte = Utils.GetParte(selectedVolume.IdDato, ValuesMant.ApendicNom);
                Utils.GetVolumenesParte(ValuesMant.Parte);
                ValuesMant.Volumen = selectedVolume.IdDato;
                permitido = (from n in AccesoUsuarioModel.VolumenesPermitidos
                             where n.Volumenes >= ValuesMant.MinVolumen && n.Volumenes <= ValuesMant.MaxVolumen
                             select n.Volumenes).ToList().Count;
                ValuesMant.Epoca = 7;
            }

            if (permitido == 0)
            {
                MessageBox.Show("No tiene permiso para revisar la información relacionada con este volumen", "  Aviso  ");
                listaTesisWindow.XamDataGridTesis.DataSource = new List<AddTesis>();
                return;
            }

            ListaTesisModel listaTesisModel = new ListaTesisModel();
            listaTesis = listaTesisModel.CargaTesisMantesisSql(qMateria);

            listaTesisWindow.XamDataGridTesis.DataSource = listaTesis;
            listaTesisWindow.XamDataGridTesis.Focus();
            if (listaTesisWindow.XamDataGridTesis.Records.Count == 0)
            {
                MessageBox.Show("No existe el registro");
            }
            else
            {
                listaTesisWindow.XamDataGridTesis.ActiveRecord = listaTesisWindow.XamDataGridTesis.Records[0];
                listaTesisWindow.TxtRegs.Content = RegLoc + listaTesis.Count;
            }

            this.SetFiltroPorMateriaTomo();


           // listaTesisWindow.GrExport.IsEnabled = true;
        }

        /// <summary>
        /// Asigna el valor interno del apéndice seleccionado de acuerdo al año en que se publico
        /// </summary>
        private void SetSelectedApendice()
        {
            if (selectedEpoca.IdDato == 1995)
            {
                ValuesMant.ApendicNom = 1;
            }
            else if (selectedEpoca.IdDato == 1988)
            {
                ValuesMant.ApendicNom = 2;
            }
            else if (selectedEpoca.IdDato == 2000)
            {
                ValuesMant.ApendicNom = 3;
            }
            else if (selectedEpoca.IdDato == 2001)
            {
                ValuesMant.ApendicNom = 4;
            }
            else if (selectedEpoca.IdDato == 2002)
            {
                ValuesMant.ApendicNom = 5;
            }
            else if (selectedEpoca.IdDato == 20011)
            {
                ValuesMant.ApendicNom = 6;
            }
        }

        /// <summary>
        /// Llena el comboBox que permite filtrar la lista de materias de determinada época y tomo/número
        /// de acuerdo a la materia o clasificación de las tesis
        /// </summary>
        private void SetFiltroPorMateriaTomo()
        {
            listaTesisWindow.CbxMaterias.Visibility = Visibility.Visible;
            listaTesisWindow.LblMaterias.Visibility = Visibility.Visible;
            if (ValuesMant.Epoca == ConstMant.Apendice)
            {
                if (ValuesMant.ApendicNom == 6)
                {
                    this.CargaVolumenesApendice2011();
                }
                else
                {
                    listaTesisWindow.CbxMaterias.Visibility = Visibility.Collapsed;
                    listaTesisWindow.LblMaterias.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                listaTesisWindow.CbxMaterias.ItemsSource = Utils.GetMateriasForFiltro();
                listaTesisWindow.CbxMaterias.DisplayMemberPath = "Descripcion";
                listaTesisWindow.CbxMaterias.SelectedValuePath = "IdDato";
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

            listaTesisWindow.CbxMaterias.ItemsSource = (from n in VolumenSingleton.Volumenes
                                                        where n.Volumenes == -1 || (n.Volumenes >= volInicial && n.Volumenes <= volFinal)
                                                        orderby n.Orden
                                                        select n).ToArray();
            listaTesisWindow.CbxMaterias.DisplayMemberPath = "VolumenTxt";
            listaTesisWindow.CbxMaterias.SelectedValuePath = "Volumenes";
        }

        /// <summary>
        /// Obtiene el número de registro digital y el número de fila de la tesis seleccionada
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// Devuelve una lista con las tesis que cumplen con el criterio de filtrado
        /// </summary>
        /// <param name="criterioFiltrado">Materia o volumen por el cual se solicita el filtro</param>
        public void GetTesisFiltradas(int criterioFiltrado)
        {
            ListaTesisModel listaTesisModel = new ListaTesisModel();
            listaTesis = listaTesisModel.CargaTesisMantesisSql(criterioFiltrado);

            listaTesisWindow.XamDataGridTesis.DataSource = listaTesis;
            listaTesisWindow.XamDataGridTesis.Focus();
            if (listaTesisWindow.XamDataGridTesis.Records.Count == 0)
            {
                MessageBox.Show("No existen registros");
            }
            else
            {
                listaTesisWindow.XamDataGridTesis.ActiveRecord = listaTesisWindow.XamDataGridTesis.Records[0];
                listaTesisWindow.TxtRegs.Content = RegLoc + listaTesis.Count;
            }
        }

        /// <summary>
        /// Realiza la búsqueda de una tesis por su número de registro digital y lanza una nueva ventana mostrando la tesis
        /// </summary>
        /// <param name="txtNumIus"></param>
        public void GetTesisByVerIus(string txtNumIus)
        {
            bool updatablePerm = ((List<int>)listaTesisWindow.VerIus.Tag).Contains(4);

            try
            {
                if (txtNumIus.Length < 8)
                {
                    NumIusModel numIusModel = new NumIusModel();

                    bool isTesisEliminated = numIusModel.GetCurrentTesisState(Convert.ToInt32(txtNumIus));
                    TesisDto tesis;

                    if (isTesisEliminated)//La tesis ya fue eliminada
                    {
                        tesis = numIusModel.BuscaTesisEliminadasPorRegistro(Convert.ToInt32(txtNumIus));

                        if (tesis != null)
                        {
                            tesis.IsEliminated = isTesisEliminated;

                            List<Volumen> volPerm = AccesoUsuarioModel.VolumenesPermitidos.Where(x => x.Volumenes == tesis.VolumenInt).ToList();

                            MessageBox.Show("Esta tesis fue eliminada");

                            UnaTesis unaTesis = new UnaTesis(tesis, updatablePerm && volPerm.Count > 0);
                            unaTesis.Tag = listaTesisWindow.VerIus.Tag;
                            unaTesis.ShowDialog();
                        }
                        else
                        {
                            tesis = numIusModel.BuscaTesis(Convert.ToInt32(txtNumIus));

                            List<Volumen> volPerm = AccesoUsuarioModel.VolumenesPermitidos.Where(x => x.Volumenes == tesis.VolumenInt).ToList();

                            if (tesis != null)
                            {
                                tesis.IsEliminated = isTesisEliminated;
                                MessageBox.Show("Esta tesis fue eliminada");

                                UnaTesis unaTesis = new UnaTesis(tesis, updatablePerm && volPerm.Count > 0);
                                unaTesis.Tag = listaTesisWindow.VerIus.Tag;
                                unaTesis.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Introduzca un número de registro valido");
                            }
                        }
                    }
                    else
                    {
                        tesis = numIusModel.BuscaTesis(Convert.ToInt32(txtNumIus));

                        List<Volumen> volPerm = AccesoUsuarioModel.VolumenesPermitidos.Where(x => x.Volumenes == tesis.VolumenInt).ToList();

                        if (tesis.Parte >= 100 && tesis.Parte <= 145)
                        {
                            ValuesMant.Epoca = 7;
                            ValuesMant.Volumen = tesis.VolumenInt;
                        }

                        if (tesis != null)
                        {
                            UnaTesis unaTesis = new UnaTesis(tesis, updatablePerm && volPerm.Count > 0);
                            unaTesis.Tag = listaTesisWindow.VerIus.Tag;
                            unaTesis.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Introduzca un número de registro valido");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Introduzca un número de registro valido");
                }
            }
            catch (NullReferenceException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message);//, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
        }


        /// <summary>
        /// Permite observar todos los detalles de la tesis, además de sus datos de publicación
        /// NO permite actulizar ningún dato de publicación
        /// </summary>
        /// <param name="materiasEstado">Indica si las materias SGA pueden ser actualizadas o no</param>
        /// <param name="isTesisUpdatable">Indica si la tesis puede ser actualizada o no</param>
        /// <param name="mainWindow">Ventana propietaria de la que esta por ser lanzada</param>
        public void MostrarTesis(byte materiasEstado, bool isTesisUpdatable,MainWindow mainWindow)
        {
            if (listaTesis != null && listaTesis.Count > 0)
            {
                ValuesMant.IusActualLstTesis = this.selectedIus;
                UnaTesis fUnaTesis = new UnaTesis(this.selectedIus, materiasEstado, listaTesis, this.selectedRowIndex, isTesisUpdatable);
                fUnaTesis.Owner = mainWindow;
                fUnaTesis.ShowDialog();

                //MoveGridToIus(ValuesMant.IusActualLstTesis);
            }
        }

        public void ExportarOptions(string id)
        {
            switch (id)
            {
                case "RBtnPdf":
                    MessageBoxResult result = MessageBox.Show("¿Desea generar el reporte con el detalle de las tesis? Si su respuesta es NO solo se generará el listado de tesis", "Atención:", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        new ListaTesisPdf().GeneraPdfConDetalleTesis(Convert.ToInt32(listaTesisWindow.CbxMaterias.SelectedValue));
                    else if (result == MessageBoxResult.No)
                        new ListaTesisPdf().GeneraPdfListaTesis(listaTesis, listaTesisWindow.CbEpoca.Text, listaTesisWindow.CbxVolumen.Text);
                    break;
                case "RBtnWord":
                    MessageBoxResult result2 = MessageBox.Show("¿Desea generar el reporte con el detalle de las tesis? Si su respuesta es NO solo se generará el listado de tesis", "Atención:", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result2 == MessageBoxResult.Yes)
                        new ListaTesisWord().GeneraWordConDetalleTesis(Convert.ToInt32(listaTesisWindow.CbxMaterias.SelectedValue));
                    else if (result2 == MessageBoxResult.No)
                        new ListaTesisWord().GeneraWordListaTesis(listaTesis, listaTesisWindow.CbEpoca.Text, listaTesisWindow.CbxVolumen.Text);
                    break;
                case "RBtnSga":
                    TablaSga tabla = new TablaSga();
                    tabla.GeneraReporte();
                    break;
            }
        }

    }
}
