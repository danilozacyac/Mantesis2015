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
            xamBitacora.FieldSettings.AllowEdit = false;
            xamBitacora.FieldSettings.AllowResize = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BitacoraModel bitacora = new BitacoraModel();

            Style wrapstyle = new Style(typeof(XamTextEditor));
            wrapstyle.Setters.Add(new Setter(XamTextEditor.TextWrappingProperty, TextWrapping.Wrap));

            xamBitacora.FieldSettings.EditorStyle = wrapstyle;


            xamBitacora.DataSource = bitacora.GetBitacoraCambios(ius);

            xamBitacora.Records.FieldLayout.Fields[0].Width = new Infragistics.Windows.DataPresenter.FieldLength(80);
            xamBitacora.Records.FieldLayout.Fields[1].Width = new Infragistics.Windows.DataPresenter.FieldLength(80);
            xamBitacora.Records.FieldLayout.Fields[2].Width = new Infragistics.Windows.DataPresenter.FieldLength(450);

            xamBitacora.Records.FieldLayout.Fields[0].Height = new Infragistics.Windows.DataPresenter.FieldLength(80);
            xamBitacora.Records.FieldLayout.Fields[1].Height = new Infragistics.Windows.DataPresenter.FieldLength(80);
            xamBitacora.Records.FieldLayout.Fields[2].Height = new Infragistics.Windows.DataPresenter.FieldLength(80);
        }

        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
