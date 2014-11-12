using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Model;
using MantesisVerIusCommonObjects.Utilities;

namespace Mantesis2015.MotivosFolder
{
    /// <summary>
    /// Lógica de interacción para MModificacion.xaml
    /// </summary>
    public partial class MModificacion : Window
    {
        private MotivosViewModel motivosViewModel;
        private string binaryMotivos;
        private char[] binaryArray;

        private long lMotivoModif;

        public MModificacion()
        {
            InitializeComponent();

            

        }

        public MModificacion(long pMotivoModif)
            : this()
        {
            this.lMotivoModif = pMotivoModif;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MantesisVerIusCommonObjects.Utilities.ValuesMant.SelectedMotiv = false;
            binaryMotivos = MiscFunct.ToBinaryInvert(lMotivoModif);
            binaryArray = binaryMotivos.ToCharArray();

            MotivosModel motivosModifModel = new MotivosModel();
            List<Motivos> lstMotivos = new List<Motivos>();

            //motivosModifModel.DbConnectionOpen();
            lstMotivos = motivosModifModel.CargaMotivos(2);
            //motivosModifModel.DbConnectionClose();

            List<UnMotivoViewModel> members =
               (from p in lstMotivos
                select new UnMotivoViewModel(p, binaryArray))
               .ToList();

            base.DataContext = new MotivosViewModel(members);

        }

        bool SelectedRecords()
        {
            bool isChecked;
            char[] binaryArray;

            this.motivosViewModel = base.DataContext as MotivosViewModel;
            isChecked = this.motivosViewModel.AnyMembersAreChecked();

            binaryArray = this.motivosViewModel.BinaryVal.ToCharArray();
            Array.Reverse(binaryArray);
            ValuesMant.BinaryVal = new string(binaryArray);

            return isChecked;
        }

        //
        public class UnMotivoViewModel : INotifyPropertyChanged
        {
            private Motivos motivosDto;
            bool isChecked;

            public UnMotivoViewModel(Motivos motivosDto, char[] pBinaryArray)
            {
                this.motivosDto = motivosDto;
                if (this.motivosDto.ClaveMovimiento - 1 < pBinaryArray.Length)
                {
                    if (pBinaryArray[this.motivosDto.ClaveMovimiento - 1] == '1')
                    {
                        isChecked = false;
                        //_isChecked = true; //Cuando quiero que muestre seleccionados los cambios que se hicieron anteriormente
                    }
                }

            }

            public bool IsChecked
            {
                get { return isChecked; }
                set
                {
                    if (value == isChecked)
                        return;

                    isChecked = value;

                    this.OnPropertyChanged("IsChecked");
                }
            }


            public int TipoMotivo { get { return motivosDto.TipoMovimiento; } }
            public int CveMotivo { get { return motivosDto.ClaveMovimiento; } }
            public string DescMotivo { get { return motivosDto.Descripcion; } }

            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged(string prop)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }


        //
        public class MotivosViewModel : INotifyPropertyChanged
        {
            public List<UnMotivoViewModel> Members { get; set; }
            public string BinaryVal;

            public MotivosViewModel(List<UnMotivoViewModel> members)
            {
                this.Members = members;

                foreach (UnMotivoViewModel uno in members)
                    uno.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName == "IsChecked")
                        {

                            this.OnPropertyChanged("AllMembersAreChecked");
                        }
                    };
            }


            public bool? AllMembersAreChecked
            {
                get
                {
                    // Determine if all members have the same 
                    // value for the IsChecked property.

                    bool? value = null;
                    for (int i = 0; i < this.Members.Count; ++i)
                    {
                        if (i == 0)
                        {
                            value = this.Members[0].IsChecked;
                        }
                        else if (value != this.Members[i].IsChecked)
                        {
                            value = null;
                            break;
                        }
                    }

                    return value;


                }
                set
                {
                    if (value == null)
                        return;

                    foreach (UnMotivoViewModel member in this.Members)
                        member.IsChecked = value.Value;
                }
            }

            public bool AnyMembersAreChecked()
            {
                bool value = false;
                string sBinary = "";
                for (int i = 0; i < this.Members.Count; ++i)
                {
                    if (this.Members[i].IsChecked)
                    {
                        value = true;
                        //break;
                        sBinary = sBinary + "1";
                    }
                    else
                    {
                        sBinary = sBinary + "0";
                    }
                }
                BinaryVal = sBinary;
                return value;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged(string prop)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRecords())
            {
                ValuesMant.SelectedMotiv = true;
                Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar almenos un punto de la lista");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}
