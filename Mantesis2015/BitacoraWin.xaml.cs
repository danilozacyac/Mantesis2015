using MantesisVerIusCommonObjects.Model;
using System.Collections.Generic;
using System.Windows;


namespace Mantesis2015
{
    /// <summary>
    /// Lógica de interacción para BitacoraWin.xaml
    /// </summary>
    public partial class BitacoraWin 
    {
        private long ius;

        public BitacoraWin(long ius)
        {
            InitializeComponent();
            this.ius = ius;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> permisos = (List<int>)this.Tag;

            BitacoraModel bitacora = new BitacoraModel();

            GBitacora.DataContext = bitacora.GetBitacoraCambios(ius);

            if (!permisos.Contains(16))
                GBitacora.Columns[4].IsVisible = false;

        }

        
    }
}
