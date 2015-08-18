using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace InformesSalas.Dto
{
    public class ClasificacionDto : INotifyPropertyChanged
    {
        private bool? isChecked = false;
        private bool isSelected;
        private int idClasif;
        private string descripcion;
        private int nivel;
        private int padre;
        private bool estado;
        private ObservableCollection<ClasificacionDto> hijos;
        private ClasificacionDto parentItem;
        private bool reentrancyCheck = false;

        public ClasificacionDto() { }

        public ClasificacionDto(ClasificacionDto parent)
        {
            this.parentItem = parent;
        }

        public bool? IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                if (this.isChecked != value)
                {
                    if (reentrancyCheck)
                        return;
                    this.reentrancyCheck = true;
                    this.isChecked = value;
                    this.UpdateCheckState();
                    OnPropertyChanged("IsChecked");
                    this.reentrancyCheck = false;
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
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

        public string Descripcion
        {
            get
            {
                return this.descripcion;
            }
            set
            {
                this.descripcion = value;
            }
        }

        public int Nivel
        {
            get
            {
                return this.nivel;
            }
            set
            {
                this.nivel = value;
            }
        }

        public int Padre
        {
            get
            {
                return this.padre;
            }
            set
            {
                this.padre = value;
            }
        }

        public bool Estado
        {
            get
            {
                return this.estado;
            }
            set
            {
                this.estado = value;
            }
        }

        public ObservableCollection<ClasificacionDto> Hijos
        {
            get
            {
                if (this.hijos == null)
                    this.hijos = new ObservableCollection<ClasificacionDto>();

                return this.hijos;
            }
            set
            {
                this.hijos = value;
            }
        }

        public ClasificacionDto ParentItem
        {
            get
            {
                return this.parentItem;
            }
            set
            {
                this.parentItem = value;
            }
        }

        public bool ReentrancyCheck
        {
            get
            {
                return this.reentrancyCheck;
            }
            set
            {
                this.reentrancyCheck = value;
            }
        }


        private void UpdateCheckState()
        {
            // update all children:
            if (this.Hijos.Count != 0)
            {
                this.UpdateChildrenCheckState();
            }
            //update parent item
            if (this.parentItem != null)
            {
                bool? parentIsChecked = this.parentItem.DetermineCheckState();
                this.parentItem.IsChecked = parentIsChecked;

            }
        }

        private void UpdateChildrenCheckState()
        {
            foreach (var item in this.Hijos)
            {
                if (this.IsChecked != null)
                {
                    item.IsChecked = this.IsChecked;
                }
            }
        }

        private bool? DetermineCheckState()
        {
            bool allChildrenChecked = this.Hijos.Count(x => x.IsChecked == true) == this.Hijos.Count;
            if (allChildrenChecked)
            {
                return true;
            }

            bool allChildrenUnchecked = this.Hijos.Count(x => x.IsChecked == false) == this.Hijos.Count;
            if (allChildrenUnchecked)
            {
                return false;
            }

            return null;
        }

        #region INotifyPropertyChanged Members

        void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
