using System;
using System.Collections.Generic;
using System.Linq;
using Mantesis2015.Dto;
using Mantesis2015.Model;

namespace Mantesis2015.Singleton
{
    public class AclaratoriaSingleton
    { 
        private static List<Aclaratoria> notas;

        private AclaratoriaSingleton()
        {
        }

        public static List<Aclaratoria> Aclaratorias
        {
            get
            {
                if (notas == null)
                    notas = new AclaratoriaModel().GetAclaratorias();

                return notas;
            }
        }
    }
}
