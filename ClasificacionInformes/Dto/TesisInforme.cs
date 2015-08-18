using System;
using System.Linq;
using MantesisVerIusCommonObjects.Dto;

namespace ClasificacionInformes.Dto
{
    public class TesisInforme : TesisDto
    {
        private bool isCheck;
        private int idProyecto;
        private int idClasif;
        private int idClasifAnterior;
        private long numericOrder;

        public bool IsCheck
        {
            get
            {
                return this.isCheck;
            }
            set
            {
                this.isCheck = value;
            }
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

        public int IdClasifAnterior
        {
            get
            {
                return this.idClasifAnterior;
            }
            set
            {
                this.idClasifAnterior = value;
            }
        }

        public long NumericOrder
        {
            get
            {
                return this.numericOrder;
            }
            set
            {
                this.numericOrder = value;
            }
        }
    }
}
