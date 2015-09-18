using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AuthManager.Models;
using AuthManager.PermisosSecciones;
using AuthManager.PermisosVolumen;
using CatalogoSga;
using ClasifInformeSalas15;
using MantesisVerIusCommonObjects.Dto;
using Telerik.Windows.Controls;
using UtilsMantesis;

namespace Mantesis2015.Controllers
{
    public class MainWindowController
    {
        readonly MainWindow main;
        private List<AddTesis> listaTesis;

        public MainWindowController(MainWindow main)
        {
            this.main = main;
        }

        public void LaunchInformesPane()
        {
            if (main.InformesPane == null)
            {
                main.InformesPane = new RadPane();
                main.ClasifTesisInf = new VistaTesisPorClasif();
                
                main.InformesPane.Header = "Informe de Salas";
                main.InformesPane.Content = main.ClasifTesisInf;
                main.InformesPane.CanFloat = false;
                main.MainPanel.Items.Add(main.InformesPane);
            }
            else if(main.InformesPane.IsVisible == false)
            {
                main.InformesPane.IsHidden = false;
            }
            

        }

        public void LaunchSeccionesPane()
        {
            if (main.SeccionesPane == null)
            {
                main.SeccionesPane = new RadPane();
                main.PermisosSecciones = new MainSeccionesPanel();

                main.SeccionesPane.Header = "Permisos Secciones";
                main.SeccionesPane.Content = main.PermisosSecciones;
                main.SeccionesPane.CanFloat = false;
                main.MainPanel.Items.Add(main.SeccionesPane);
            }
            else if (main.SeccionesPane.IsVisible == false)
            {
                main.SeccionesPane.IsHidden = false;
            }
        }

        public void LaunchVolumenesPane()
        {
            if (main.VolumenesPane == null)
            {
                main.VolumenesPane = new RadPane();
                main.PermisosVolumenes = new MainVolumenPanel();

                main.VolumenesPane.Header = "Permisos Volumenes";
                main.VolumenesPane.Content = main.PermisosVolumenes;
                main.VolumenesPane.CanFloat = false;
                main.MainPanel.Items.Add(main.VolumenesPane);
            }
            else if (main.VolumenesPane.IsVisible == false)
            {
                main.VolumenesPane.IsHidden = false;
            }
        }

        public void DeleteClasifTesis()
        {
            main.ClasifTesisInf.EliminarRelacionTesis();
        }

        public void ActualizaVarias()
        {
            main.ClasifTesisInf.ActualizarVarias();
        }


        public void ExportarOptions(string id)
        {
            //switch (id)
            //{
            //    case "RBtnPdf":
            //        MessageBoxResult result = MessageBox.Show("¿Desea generar el reporte con el detalle de las tesis? Si su respuesta es NO solo se generará el listado de tesis", "Atención:", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //        if (result == MessageBoxResult.Yes)
            //            new ListaTesisPdf().GeneraPdfConDetalleTesis(Convert.ToInt32(main.CbxMaterias.SelectedValue));
            //        else if (result == MessageBoxResult.No)
            //            new ListaTesisPdf().GeneraPdfListaTesis(listaTesis, main.CbEpoca.Text, main.CbxVolumen.Text);
            //        break;
            //    case "RBtnWord":
            //        MessageBoxResult result2 = MessageBox.Show("¿Desea generar el reporte con el detalle de las tesis? Si su respuesta es NO solo se generará el listado de tesis", "Atención:", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //        if (result2 == MessageBoxResult.Yes)
            //            new ListaTesisWord().GeneraWordConDetalleTesis(Convert.ToInt32(main.CbxMaterias.SelectedValue));
            //        else if (result2 == MessageBoxResult.No)
            //            new ListaTesisWord().GeneraWordListaTesis(listaTesis, main.CbEpoca.Text, main.CbxVolumen.Text);
            //        break;
            //    case "RBtnSga":
            //        TablaSga tabla = new TablaSga();
            //        tabla.GeneraReporte();
            //        break;
            //}
        }


        #region Materias SGA

        public void LaunchMateriasSgaPane()
        {
            if (main.MateriasSgaPane == null)
            {
                main.MateriasSgaPane = new RadPane();
                main.CatalogMateriasSga = new MantoClasifSga();

                main.MateriasSgaPane.Header = "Materias SGA";
                main.MateriasSgaPane.Content = main.CatalogMateriasSga;
                main.MateriasSgaPane.CanFloat = false;
                main.MainPanel.Items.Add(main.MateriasSgaPane);
            }
            else if (main.MateriasSgaPane.IsVisible == false)
            {
                main.MateriasSgaPane.IsHidden = false;
            }
        }

        public void MateriasEnPdf()
        {
            main.CatalogMateriasSga.ImprimeEstructuraPdf();
        }

        public void MateriasEnWord()
        {
            main.CatalogMateriasSga.ImprimeEstructuraWord();
        }

        public void ReasignaConsecutivo()
        {
            main.CatalogMateriasSga.ReasignarConsecutivo();
        }

        #endregion


        #region Herramientas Administrativas


        public void RefreshPermission(PermisosController permContr)
        {
            AccesoUsuarioModel.Permisos = new PermisosSeccionModel().GetSeccionesByUsuario(AccesoUsuarioModel.Llave);
            AccesoUsuarioModel.VolumenesPermitidos = new VolumenesViewModel().GetVolumenesPorUser(AccesoUsuarioModel.Llave);
            permContr.LoadPermission();
            
        }

        public void AuthSeccion()
        {
            if (main.PermisosSecciones == null)
            {
                MessageBox.Show("Primer debes mostrar el listado y seleccionar un usuario");
                return;
            }
            main.PermisosSecciones.AuthSeccion();
        }

        public void DenySeccion()
        {
            if (main.PermisosSecciones == null)
            {
                MessageBox.Show("Primer debes mostrar el listado y seleccionar un usuario");
                return;
            }
            main.PermisosSecciones.DenegarPermiso();
        }

        public void ModificarPermisosSeccion()
        {
            if (main.PermisosSecciones == null)
            {
                MessageBox.Show("Primer debes mostrar el listado y seleccionar un usuario");
                return;
            }
            main.PermisosSecciones.ModificaPermisosDeSeccion();
        }

        public void AutorizarVolumen()
        {
            if (main.PermisosVolumenes == null)
            {
                MessageBox.Show("Primer debes mostrar el listado y seleccionar un usuario");
                return;
            }
            main.PermisosVolumenes.AutorizarVolumen();
        }

        public void DenegarVolumen()
        {
            if (main.PermisosVolumenes == null)
            {
                MessageBox.Show("Primer debes mostrar el listado y seleccionar un usuario");
                return;
            }
            main.PermisosVolumenes.DenegarVolumen();
        }

        public void AutorizarEpoca()
        {
            if (main.PermisosVolumenes == null)
            {
                MessageBox.Show("Primer debes mostrar el listado y seleccionar un usuario");
                return;
            }
            main.PermisosVolumenes.AutorizarEpoca();
        }

        public void DenegarEpoca()
        {
            if (main.PermisosVolumenes == null)
            {
                MessageBox.Show("Primer debes mostrar el listado y seleccionar un usuario");
                return;
            }
            main.PermisosVolumenes.DenegarEpoca();
        }

        #endregion
    }
}