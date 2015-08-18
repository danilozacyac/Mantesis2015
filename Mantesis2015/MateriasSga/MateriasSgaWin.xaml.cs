using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Mantesis2015.MateriasSga
{
    /// <summary>
    /// Lógica de interacción para MateriasSgaWin.xaml
    /// </summary>
    public partial class MateriasSgaWin : Window
    {
        private List<int> idMaterias;
        List<MateriasModel> listaMaterias;

        private readonly int ius;
        private readonly int volumen;
        private readonly bool isUpdatable;

        public MateriasSgaWin(int ius, int volumen, bool isUpdatable)
        {
            InitializeComponent();
            this.ius = ius;
            this.volumen = volumen;
            this.isUpdatable = isUpdatable;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.idMaterias = new List<int>();

            listaMaterias = MateriasModel.CreateMateriasTree(isUpdatable);
            tree.DataContext = listaMaterias;
            SetSeleccionados(listaMaterias, MateriasViewModel.GetMateriasRelacionadas(ius, volumen));

            List<int> permisos = (List<int>)this.Tag;

            if (!isUpdatable || !permisos.Contains(4))
            {
                BtnQuitar.Visibility = Visibility.Hidden;
                BtnSalvar.Visibility = Visibility.Hidden;
                BtnSalir.Content = "Cerrar";
            }
        }
        
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (isUpdatable)
            {
                GetSeleccionados(((MateriasModel)tree.Items[0]).Children);

                if (idMaterias.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un tema con el cual relacionar la tesis, de los contrario oprima cancelar");
                    return;
                }
                else
                {
                    MateriasViewModel.SetRelacionMateriasIus(ius, idMaterias, volumen);
                    MessageBox.Show("Esta tesis fue relacionada con " + idMaterias.Count.ToString() + ((idMaterias.Count == 1) ? " tema" : " temas"));
                    idMaterias.Clear();

                    DialogResult = true;
                    this.Close();
                }
            }
        }

        private void GetSeleccionados(List<MateriasModel> items)
        {
            foreach (MateriasModel item in items)
            {
                if (item.IsChecked == true)
                    idMaterias.Add(item.Id);

                GetSeleccionados(item.Children);
            }
        }

        private void SetSeleccionados(List<MateriasModel> items, List<int> matSeleccionadas)
        {
            foreach (MateriasModel item in items)
            {
                if (matSeleccionadas.Contains(item.Id))
                    item.IsChecked = true;

                SetSeleccionados(item.Children, matSeleccionadas);
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}