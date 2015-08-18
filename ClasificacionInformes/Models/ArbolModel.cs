using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasificacionInformes.Models
{
    public class ArbolModel
    {
        public List<TreeViewItem> GetArbolClasificación(int seleccionado,int tpoTesis)
        {
            List<TreeViewItem> arbolClasif = new List<TreeViewItem>();

            List<ClasificacionDto> temas = new ClasificacionModel().GetClasif(0, seleccionado,tpoTesis);

            foreach (ClasificacionDto tema in temas)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = tema.Descripcion;
                
                item.Tag = tema;

                item = GeneraHijoRecursivo(item, tema.IdClasif, seleccionado,tpoTesis);
                item.IsExpanded = true;
                arbolClasif.Add(item);
            }

            return arbolClasif;
        }

        private TreeViewItem GeneraHijoRecursivo(TreeViewItem item, int idPadre, int seleccionado,int tpoTesis)
        {
            List<ClasificacionDto> childs = new ClasificacionModel().GetClasif(idPadre, seleccionado,tpoTesis);

            foreach (ClasificacionDto hijo in childs)
            {
                TreeViewItem child = new TreeViewItem();

                try
                {
                    child.Header = hijo.Descripcion;
                    child.Tag = hijo;
                    child = GeneraHijoRecursivo(child, hijo.IdClasif, seleccionado,tpoTesis);
                    child.IsExpanded = true;
                    child.IsSelected = hijo.IsSelected;
                }
                catch (Exception)
                {
                }
                item.Items.Add(child);
            }
            return item;
        }
    }
    }
}
