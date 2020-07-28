using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyDemo
{
    public class MyTileView : ViewBase
    {
        private DataTemplate _ItemTemplate;

        public DataTemplate ItemTemplate
        {
            get { return _ItemTemplate; }
            set { _ItemTemplate = value; }
        }

        protected override object DefaultStyleKey => new ComponentResourceKey(GetType(), "TileView");
        protected override object ItemContainerDefaultStyleKey => new ComponentResourceKey(GetType(), "TileViewItem");
    }
}
