using System.Collections.Generic;
using System.Windows;
using Infragistics.Windows.Editors;
using MantesisVerIusCommonObjects.Model;


namespace Mantesis2015
{
    /// <summary>
    /// Lógica de interacción para BitacoraWin.xaml
    /// </summary>
    public partial class BitacoraWin : Window
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

            Style wrapstyle = new Style(typeof(XamTextEditor));
            wrapstyle.Setters.Add(new Setter(XamTextEditor.TextWrappingProperty, TextWrapping.Wrap));
            xamBitacora.FieldSettings.EditorStyle = wrapstyle;

            xamBitacora.DataSource = bitacora.GetBitacoraCambios(ius);

            if(!permisos.Contains(16))
                xamBitacora.Records.FieldLayout.Fields[4].Visibility = System.Windows.Visibility.Collapsed;

        }

        
    }
}
