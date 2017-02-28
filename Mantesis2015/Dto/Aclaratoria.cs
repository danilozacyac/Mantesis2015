using System;
using System.Linq;

namespace Mantesis2015.Dto
{
    public class Aclaratoria
    {
        private int idNota;
        private int parteInicia;
        private int parteFin;
        private int materia;
        private string nota;

        public int IdNota
        {
            get
            {
                return this.idNota;
            }
            set
            {
                this.idNota = value;
            }
        }

        public int ParteInicia
        {
            get
            {
                return this.parteInicia;
            }
            set
            {
                this.parteInicia = value;
            }
        }

        public int ParteFin
        {
            get
            {
                return this.parteFin;
            }
            set
            {
                this.parteFin = value;
            }
        }

        public int Materia
        {
            get
            {
                return this.materia;
            }
            set
            {
                this.materia = value;
            }
        }

        public string Nota
        {
            get
            {
                return this.nota;
            }
            set
            {
                this.nota = value;
            }
        }
    }
}
