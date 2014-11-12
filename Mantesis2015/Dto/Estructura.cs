using System;
using System.Linq;

namespace Mantesis2015.Dto
{
    public class Estructura
    {
        private long id;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        private long nivel;

        public long Nivel
        {
            get { return nivel; }
            set { nivel = value; }
        }

        private long padre;

        public long Padre
        {
            get { return padre; }
            set { padre = value; }
        }

        private int historica;

        public int Historica
        {
            get { return historica; }
            set { historica = value; }
        }

        private string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private int hoja;

        public int Hoja
        {
            get { return hoja; }
            set { hoja = value; }
        }

        private long ius4;

        public long Ius4
        {
            get { return ius4; }
            set { ius4 = value; }
        }

        private string fecha;

        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private string llaveUsuario;

        public string LlaveUsuario
        {
            get { return llaveUsuario; }
            set { llaveUsuario = value; }
        }

        public Estructura(long id, string descripcion)
        {
            this.id = id;
            this.descripcion = descripcion;
        }


        public Estructura(long ius4, string fecha, string llave)
        {
            this.ius4 = ius4;
            this.fecha = fecha;
            this.llaveUsuario = llave;
        }

        public Estructura(long id, long nivel, long padre, string descripcion, int historica, int hoja)
        {
            this.id = id;
            this.nivel = nivel;
            this.padre = padre;
            this.descripcion = descripcion;
            this.historica = historica;
            this.hoja = hoja;
        }


    }
}
