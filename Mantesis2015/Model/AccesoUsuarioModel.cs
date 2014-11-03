using System;
using System.Collections.ObjectModel;
using System.Linq;
using MantesisAdminUtil.Dto;
using PVolumenesControl.Dao;

namespace Mantesis2015.Model
{
    public static class AccesoUsuarioModel
    {

        private static string nombre;



        public static string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
            }
        }

        private static string usuario;
        public static string Usuario
        {
            get { return usuario; }
            set
            {
                usuario = value;
            }
        }

        private static string pwd;
        public static string Pwd
        {
            get { return pwd; }
            set
            {
                pwd = value;
            }
        }

        private static int llave;
        public static int Llave
        {
            get { return llave; }
            set
            {
                llave = value;
            }
        }

        private static int grupo;
        public static int Grupo
        {
            get { return grupo; }
            set
            {
                grupo = value;
            }
        }

        private static ObservableCollection<Secciones> permisos;
        public static ObservableCollection<Secciones> Permisos
        {
            get
            {
                return permisos;
            }
            set
            {
                permisos = value;
            }
        }

        private static ObservableCollection<VolumenesDao> volumenesPermitidos;
        public static ObservableCollection<VolumenesDao> VolumenesPermitidos
        {
            get
            {
                return volumenesPermitidos;
            }
            set
            {
                volumenesPermitidos = value;
            }
        }
    }
}
