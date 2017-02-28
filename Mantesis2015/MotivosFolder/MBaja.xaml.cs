using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Mantesis2015.Model;
using MantesisCommonObjects.MantUtilities;

namespace Mantesis2015.MotivosFolder
{
    /// <summary>
    /// Lógica de interacción para MBaja.xaml
    /// </summary>
    public partial class MBaja : Window
    {
        public MBaja()
        {
            InitializeComponent();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            bool bSelecMotiv = false;
            int nRegValido;

            ListaTesisModel listaTesisModel = new ListaTesisModel();

            if (rbtBaja01.IsChecked == true) { bSelecMotiv = true; ValuesMant.MotivoBaja = 1; }
            if (rbtBaja02.IsChecked == true) { bSelecMotiv = true; ValuesMant.MotivoBaja = 2; }
            if (rbtBaja03.IsChecked == true) { bSelecMotiv = true; ValuesMant.MotivoBaja = 4; }
            if (rbtBaja04.IsChecked == true) { bSelecMotiv = true; ValuesMant.MotivoBaja = 8; }
            if (rbtBaja05.IsChecked == true) { bSelecMotiv = true; ValuesMant.MotivoBaja = 16; }
            if (rbtBaja06.IsChecked == true) { bSelecMotiv = true; ValuesMant.MotivoBaja = 32; }
            if (rbtBaja07.IsChecked == true) { bSelecMotiv = true; ValuesMant.MotivoBaja = 64; }


            if (bSelecMotiv == false)
            {
                MessageBox.Show("Debe seleccionar una opción para dar de baja un registro");
            }


            if (ValuesMant.MotivoBaja != 32)
            {
                // No se valida
                nRegValido = 0;
            }
            else
            {

                if (txtRegIus.Text.Length == 0)
                {
                    MessageBox.Show("Debe proporcionar un número de registro IUS");
                }

                nRegValido = listaTesisModel.VerificaIus(Convert.ToInt32(txtRegIus.Text));


                if (nRegValido == 4)
                {
                    MessageBox.Show("El registro proporcionado se encuentra Eliminado");
                }

                if (nRegValido == 5)
                {
                    MessageBox.Show("El registro proporcionado no existe");
                }
            }

            if (bSelecMotiv == true && nRegValido == 0)
            {
                ValuesMant.RegIusporBaja = Convert.ToInt32(txtRegIus.Text);
                ValuesMant.SelectedMotiv = true;
                Close();
            }


        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            ValuesMant.SelectedMotiv = false;
            Close();
        }

        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }


        private static bool IsTextAllowed(string text)
        {
            // Regex NumEx = new Regex(@"^\d+(?:.\d{0,2})?$"); 
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text 
            return regex.IsMatch(text);
        }
    }
}
