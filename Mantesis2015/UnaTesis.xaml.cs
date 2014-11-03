using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Mantesis2015.Controllers;
using Mantesis2015.Model;
using Microsoft.Windows.Controls.Ribbon;
using UtilsMantesis;

namespace Mantesis2015
{
    /// <summary>
    /// Lógica de interacción para UnaTesis.xaml
    /// </summary>
    public partial class UnaTesis : Window
    {

        private UnaTesisController controller;

        private UnaTesisModel unaTesisModel; 


        private long ius;
        private readonly byte accion;
        public List<AddTesis> ListaTesis;
        public int PosActual;
        private readonly bool isTesisUpdatable;



        public UnaTesis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ius">Número de registro IUS de la tesis a mostrar</param>
        /// <param name="accion">Como se abrira el registro.   1  Visualiza  ----  2  Actualiza  ---- 3 Nueva tesis</param>
        /// <param name="listaTesis">Lista de Tesis mostrada en la ventana principal</param>
        /// <param name="posActual">Posición de la tesis dentro del listado mostrado</param>
        /// <param name="isTesisUpdatable"></param>
        public UnaTesis(long ius, byte accion, List<AddTesis> listaTesis, int posActual, bool isTesisUpdatable)
        {
            InitializeComponent();
            this.ius = ius;
            this.accion = accion;
            this.PosActual = posActual;
            this.ListaTesis = listaTesis;
            this.isTesisUpdatable = isTesisUpdatable;
            controller = new UnaTesisController(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            controller.LoadTesisWindow(this.ius);
            
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton ribbon = sender as RibbonButton;

            switch (ribbon.Name)
            {
                case "RbtnInicio": controller.TesisStart();
                    break;
                case "RbtnPrevious": controller.TesisPrevious();
                    break;
                case "RbtnNext": controller.TesisNext();
                    break;
                case "RbtnFin": controller.TesisEnd();
                    break;
                case "RbtnClipboard": controller.TesisToClipboard();
                    break;
                case "RbtnLigas": controller.LaunchLigasPreview();
                    break;
                case "RbtnBitacora": controller.LaunchBitacora();
                    break;
            }
        }
    }
}
