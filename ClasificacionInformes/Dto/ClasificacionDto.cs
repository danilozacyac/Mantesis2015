using System;
using System.Linq;

namespace ClasificacionInformes.Dto
{
    public class ClasificacionDto
    {
        private bool isSelected;
        private int idClasif;
        private string descripcion;
        private int nivel;
        private int padre;
        private bool estado;

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
            }
        }

        public int IdClasif
        {
            get
            {
                return this.idClasif;
            }
            set
            {
                this.idClasif = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return this.descripcion;
            }
            set
            {
                this.descripcion = value;
            }
        }

        public int Nivel
        {
            get
            {
                return this.nivel;
            }
            set
            {
                this.nivel = value;
            }
        }

        public int Padre
        {
            get
            {
                return this.padre;
            }
            set
            {
                this.padre = value;
            }
        }

        public bool Estado
        {
            get
            {
                return this.estado;
            }
            set
            {
                this.estado = value;
            }
        }
    }
}
