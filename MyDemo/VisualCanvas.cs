using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyDemo
{
    public class VisualCanvas : Canvas
    {
        private List<Visual> Visuals { get; set; } = new List<Visual>();

        protected override int VisualChildrenCount => Visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            return Visuals[index];
        }

        public void AddVisual(Visual visual)
        {
            Visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void RemoveVisual(Visual visual)
        {
            Visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }
    }
}
