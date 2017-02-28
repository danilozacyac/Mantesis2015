using Mantesis2015.Dto;
using Mantesis2015.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Mantesis2015
{
    /// <summary>
    /// Interaction logic for SelectNotas.xaml
    /// </summary>
    public partial class SelectNotas
    {

        private TesisQuinta tesis;
        private Aclaratoria selectedNota;

        public SelectNotas(ref TesisQuinta tesis)
        {
            InitializeComponent();
            this.tesis = tesis;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<Aclaratoria> notas = (from n in AclaratoriaSingleton.Aclaratorias
                         where (n.ParteInicia <= tesis.Parte) && n.Materia == tesis.Materia1 //>= n.ParteInicia && tesis.Parte <= n.ParteFin
                                    orderby n.IdNota
                                    select n).ToList();

            CbxNotas.ItemsSource = notas;
        }

        private void CbxNotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNota = CbxNotas.SelectedItem as Aclaratoria;

            TxtDescripcion.DataContext = selectedNota.Nota;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (selectedNota == null)
            {
                MessageBox.Show("Selecciona la nota pertinente de los contrario presiona Cancelar", "Atención,", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                tesis.NotaAclaratoria = selectedNota.IdNota;
                this.Close();
            }

        }
    }
}
