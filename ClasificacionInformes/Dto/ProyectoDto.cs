using System;
using System.Linq;

namespace ClasificacionInformes.Dto
{
    public class ProyectoDto
    {
        private int idProyecto;
        private string descripcion;
        private int sala;
        private int volInicio;
        private int volFinal;
        private string condicionProyecto;
        private string condicionVolumen;
        
        public ProyectoDto(int idProyecto, string descripcion, int sala, int volInicio, int volFinal, string condicionProyecto, string condicionVolumen)
        {
            this.idProyecto = idProyecto;
            this.descripcion = descripcion;
            this.sala = sala;
            this.volInicio = volInicio;
            this.volFinal = volFinal;
            this.condicionProyecto = condicionProyecto;
            this.condicionVolumen = condicionVolumen;
        }

        public int IdProyecto
        {
            get
            {
                return this.idProyecto;
            }
            set
            {
                this.idProyecto = value;
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

        public int Sala
        {
            get
            {
                return this.sala;
            }
            set
            {
                this.sala = value;
            }
        }

        public int VolInicio
        {
            get
            {
                return this.volInicio;
            }
            set
            {
                this.volInicio = value;
            }
        }

        public int VolFinal
        {
            get
            {
                return this.volFinal;
            }
            set
            {
                this.volFinal = value;
            }
        }

        public string CondicionProyecto
        {
            get
            {
                return this.condicionProyecto;
            }
            set
            {
                this.condicionProyecto = value;
            }
        }

        public string CondicionVolumen
        {
            get
            {
                return this.condicionVolumen;
            }
            set
            {
                this.condicionVolumen = value;
            }
        }
    }
}
