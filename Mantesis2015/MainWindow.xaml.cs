using System;
using System.Linq;
using System.Windows;
using AuthManager.PermisosSecciones;
using AuthManager.PermisosVolumen;
using Mantesis2015.Model;
using MantesisCommonObjects.Dto;
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
        




        /**
         * Controles de usuario
         * */
        public MainSeccionesPanel PermisosSecciones;
        public RadPane SeccionesPane;
        public MainVolumenPanel PermisosVolumenes;
        public RadPane VolumenesPane;

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
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

            //permisosCon = new PermisosController(this);
            
            //permisosCon.LoadPermission();

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
                
               
                

                
                case "RbtnSalir": Application.Current.Shutdown();
                    break;


                   
            }

        }

        private void ExportarGroupClick(object sender, RoutedEventArgs e)
        {
            RadRibbonButton action = sender as RadRibbonButton;

            MantesisPane.ExportaInformacionTesis(action);

        }

       
        

        

        

    }
}
