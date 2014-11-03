using System;
using System.Linq;

namespace Mantesis2015.Dto
{
    public class TesisReferencia
    {

        public TesisReferencia()
        {

        }

        public TesisReferencia(long pIus, Int16 pMotivo, long pRegistro)
        {
            this.ius = pIus;
            this.motivo = pMotivo;
            this.registro = pRegistro;
        }

        private long ius;
        public long Ius
        {
            get { return ius; }
            set { ius = value; }
        }

        private Int16 motivo;
        public Int16 Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }

        private long registro;
        public long Registro
        {
            get { return registro; }
            set { registro = value; }
        }
    }
}
