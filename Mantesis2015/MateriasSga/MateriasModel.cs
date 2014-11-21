using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Mantesis2015.MateriasSga
{
    public class MateriasModel : INotifyPropertyChanged
    {
        #region Data

        bool? isChecked = false;
        MateriasModel parent;

        #endregion // Data

        bool isReadOnly = false;
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                isReadOnly = value;
            }
        }

        #region CreateMateriasTree

        public static List<MateriasModel> CreateMateriasTree(bool isReadOnly)
        {
            List<MateriasModel> listaMaterias = MateriasViewModel.GetEstructuraNivel(-1,isReadOnly);

            MateriasModel root = new MateriasModel("Todas")
            {
                IsInitiallySelected = true,
                Children = listaMaterias
            };

            root.Initialize();
            return new List<MateriasModel> { root };
        }

        public MateriasModel(string name)
        {
            this.Name = name;

            this.Children = new List<MateriasModel>();
        }

        public MateriasModel(string name, List<MateriasModel> hijos, int id)
        {
            this.Name = name;
            this.Children = hijos;
            this.Id = id;
        }

        void Initialize()
        {
            foreach (MateriasModel child in this.Children)
            {
                child.parent = this;

                child.Initialize();
            }
        }

        #endregion

        #region Properties

        public List<MateriasModel> Children { get; private set; }

        public bool IsInitiallySelected { get; private set; }

        public string Name { get; private set; }

        public int Id { get; private set; }

        #region IsChecked

        /// <summary>
        /// Gets/sets the state of the associated UI toggle (ex. CheckBox).
        /// The return value is calculated based on the check state of all
        /// child FooViewModels.  Setting this property to true or false
        /// will set all children to the same check state, and setting it 
        /// to any value will cause the parent to verify its check state.
        /// </summary>
        public bool? IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                this.SetIsChecked(value, true, true);
            }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == isChecked)
                return;

            isChecked = value;

            if (updateChildren && isChecked.HasValue)
                this.Children.ForEach(c => c.SetIsChecked(isChecked, true, false));

            if (updateParent && parent != null)
                parent.VerifyCheckState();

            this.OnPropertyChanged("IsChecked");
        }

        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; ++i)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

        #endregion // IsChecked

        #endregion // Properties

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
