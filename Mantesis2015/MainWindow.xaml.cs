using System;
using System.Linq;
using System.Windows;
using AuthManager.PermisosSecciones;
using AuthManager.PermisosVolumen;
using CatalogoSga;
using CheckPrecedentes;
using ClasifInformeSalas15;
using Mantesis2015.Controllers;
using Mantesis2015.Model;
using MantesisVerIusCommonObjects.Dto;
using Telerik.Windows.Controls;

namespace Mantesis2015
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DatosComp EpocaSelec;
        private object volumeSelect;
        private MainWindowController controller;
        private PermisosController permisosCon;
        
        public RadPane InformesPane;
        public VistaTesisPorClasif ClasifTesisInf;

        public RadPane MateriasSgaPane;
        public MantoClasifSga CatalogMateriasSga;

        public RadPane ChecaPrecPane;
        public ChecaPrecedentes PanelPrecede;

        /**
         * Controles de usuario
         * */
        public MainSeccionesPanel PermisosSecciones;
        public RadPane SeccionesPane;
        public MainVolumenPanel PermisosVolumenes;
        public RadPane VolumenesPane;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AccesoModel accesoModel = new AccesoModel();

            if (accesoModel.ObtenerUsuarioContraseña() == false)
            {
                MessageBox.Show("  No tienes permiso para acceder a la aplicación  ");
                this.Close();
                return;
            }

            controller = new MainWindowController(this);
            permisosCon = new PermisosController(this);
            
            permisosCon.LoadPermission();

        }


        private void ButtonTool_Click(object sender, RoutedEventArgs e)
        {
            RadRibbonButton boton = sender as RadRibbonButton;

            switch (boton.Name)
            {
                case "BtnNuevaTesis":
                    break;
                case "BtnVisualizaTesis": MantesisPane.MostrarTesis(1, false, this); 
                    break;
                case "BtnActualizaTesis": MantesisPane.MostrarTesis(2, true, this);
                    break;
                case "BtnEliminaTesis":
                    break;
                
                //Materias SGA
                case "BtnStructMateriasSga": 
                    break;

                case "BtnEditStructSga": controller.LaunchMateriasSgaPane();
                    break;
                case "BtnPrintSgaPdf": controller.MateriasEnPdf();
                    break;
                case "BtnPrintSgaWord": controller.MateriasEnWord();
                    break;
                case "BtnReasignaOrden":

                
                case "RbtnSalir": Application.Current.Shutdown();
                    break;


                    //Usuarios
                case "RBtnRefreshPermisos": controller.RefreshPermission(permisosCon);
                    break;

                    //Secciones
                case "RBtnPermisos": controller.LaunchSeccionesPane();
                    break;

                case "RBtnAuthSeccion": controller.AuthSeccion();
                    break;
                case "RBtnDenySeccion": controller.DenySeccion();
                    break;
                case "RBtnEditAuthSec": controller.ModificarPermisosSeccion();
                    break;

                    //Volumenes
                case "RBtnVolumenes": controller.LaunchVolumenesPane();
                    break;
                case "RBtnAuthVolumen": controller.AutorizarVolumen();
                    break;
                case "RBtnDenyVolumen": controller.DenegarVolumen();
                    break;
                case "RBtnAuthEpocas": controller.AutorizarEpoca();
                    break;
                case "RBtnDenyEpocas": controller.DenegarEpoca();
                    break;

                    //Verificadores
                case "RBtnComparaBases": controller.LaunchVerificadorBases();
                    break;
                case "RBtnTesisTotales": controller.LaunchTesisTotales();
                    break;
            }

        }

        private void ExportarGroupClick(object sender, RoutedEventArgs e)
        {
            RadRibbonButton action = sender as RadRibbonButton;

            MantesisPane.ExportaInformacionTesis(action);

        }

        /// <summary>
        /// Maneja los eventos generados por lo botones del RibbonView pertenecientes a la parte de Informe 
        /// de las Salas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InformesButtonTools(object sender, RoutedEventArgs e)
        {

            RadRibbonButton boton = sender as RadRibbonButton;

            switch (boton.Name)
            {
                case "BtnMostrarProy": controller.LaunchInformesPane();
                    break;
                case "BtnEliminarClasif": controller.DeleteClasifTesis();
                    break;
                case "BtnActualizaVarias": controller.ActualizaVarias();
                    break;

            }

            controller.LaunchInformesPane();
        }

        

        

    }
}
