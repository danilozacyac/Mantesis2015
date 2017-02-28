using System;
using System.Linq;
using MantesisCommonObjects.Dto;

namespace Mantesis2015.Dto
{
    public class TesisQuinta : TesisDto
    {
        private DateTime? fechaPublicado;
        private DateTime? fechaConocimietno;
        private string obsRubro;
        private string obsTexto;
        private string obsPrecedentes;
        private string notasSugeridas;
        private int notaAclaratoria;


        private string soportePrincipal;


        

        public DateTime? FechaPublicado
        {
            get
            {
                return this.fechaPublicado;
            }
            set
            {
                this.fechaPublicado = value;
            }
        }

        public DateTime? FechaConocimietno
        {
            get
            {
                return this.fechaConocimietno;
            }
            set
            {
                this.fechaConocimietno = value;
            }
        }

        public string ObsRubro
        {
            get
            {
                return this.obsRubro;
            }
            set
            {
                this.obsRubro = value;
                this.OnPropertyChanged("ObsRubro");
            }
        }

        public string ObsTexto
        {
            get
            {
                return this.obsTexto;
            }
            set
            {
                this.obsTexto = value;
                this.OnPropertyChanged("ObsTexto");
            }
        }

        public string ObsPrecedentes
        {
            get
            {
                return this.obsPrecedentes;
            }
            set
            {
                this.obsPrecedentes = value;
                this.OnPropertyChanged("ObsPrecedentes");
            }
        }

        public string NotasSugeridas
        {
            get
            {
                return this.notasSugeridas;
            }
            set
            {
                this.notasSugeridas = value;
                this.OnPropertyChanged("NotasSugeridas");
            }
        }


        public string SoportePrincipal
        {
            get
            {
                return this.soportePrincipal;
            }
            set
            {
                this.soportePrincipal = value;
                this.OnPropertyChanged("SoportePrincipal");
            }
        }

        public int NotaAclaratoria
        {
            get
            {
                return this.notaAclaratoria;
            }
            set
            {
                this.notaAclaratoria = value;
                this.OnPropertyChanged("NotaAclaratoria");
            }
        }
        
    }
}
