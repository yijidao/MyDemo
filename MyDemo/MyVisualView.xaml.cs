using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace MyDemo
{
    /// <summary>
    /// MyVisualView.xaml 的交互逻辑
    /// 一个画板并且支持拖拉、删除、新增、多选
    /// </summary>
    public partial class MyVisualView : UserControl
    {
        public MyVisualView()
        {
            InitializeComponent();
        }

        private void VisualCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pointClicked = e.GetPosition(CanvasPanel);
            if (rbAdd.IsChecked == true)
            {
                var visual = new DrawingVisual();
                DrawSquare(visual, pointClicked, false);
                CanvasPanel.AddVisual(visual);
            }
            else if (rbDelete.IsChecked == true)
            {
                var visual = GetVisual(pointClicked);
                if (visual != null) CanvasPanel.RemoveVisual(visual);
            }
            else if (rbSelected.IsChecked == true)
            {
                var visual = GetVisual(pointClicked);
                if(visual != null)
                {
                    var topLeftCorner = new Point(visual.ContentBounds.TopLeft.X + DrawingPen.Thickness / 2,
                                                visual.ContentBounds.TopLeft.Y + DrawingPen.Thickness / 2);
                    DrawSquare(visual, topLeftCorner, true); // 重绘为黄色
                    ClickOffet = topLeftCorner - pointClicked;
                    IsDragging = true;
                    if(SelectedVisual != null && SelectedVisual != visual)
                    {
                        ClearSelection(); // 将原来为黄色的重绘为淡蓝
                    }
                    SelectedVisual = visual;
                }
            }
            else if (rbSelectMultiple.IsChecked == true)
            {
                SelectionSquare = new DrawingVisual();
                CanvasPanel.AddVisual(SelectionSquare);
                SelectionSquareTopLeft = pointClicked;
                IsMultiSelecting = true;
                CanvasPanel.CaptureMouse();
            }
        }

        public void ClearSelection()
        {
            var topLeftCorner = new Point(SelectedVisual.ContentBounds.TopLeft.X + DrawingPen.Thickness / 2, 
                                SelectedVisual.ContentBounds.TopLeft.Y + DrawingPen.Thickness / 2);
            DrawSquare(SelectedVisual, topLeftCorner, false);
            SelectedVisual = null;
        }

        public DrawingVisual SelectionSquare { get; set; }
        public bool IsMultiSelecting { get; set; }
        public Point SelectionSquareTopLeft { get; set; }
        public bool IsDragging { get; set; }
        public Vector ClickOffet { get; set; }
        public Brush DrawingBrush { get; set; } = Brushes.AliceBlue;
        public Brush SelectedDrawingBrush { get; set; } = Brushes.LightGoldenrodYellow;
        public Pen DrawingPen { get; set; } = new Pen(Brushes.SteelBlue, 3);
        public Size SquareSize { get; set; } = new Size(30, 30);
        public DrawingVisual SelectedVisual { get; set; }

        /// <summary>
        /// 将visual绘制正方形，这个方法只是绘制，不会添加到画板中
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="topLeftCorner"></param>
        /// <param name="isSelected"></param>
        private void DrawSquare(DrawingVisual visual, Point topLeftCorner, bool isSelected)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                var brush = DrawingBrush;
                // 切换颜色，选中的时候用淡黄色绘制
                if (isSelected) brush = SelectedDrawingBrush;
                dc.DrawRectangle(brush, DrawingPen, new Rect(topLeftCorner, SquareSize));
            }
        }

        private DrawingVisual GetVisual(Point point)
        {
            var result = VisualTreeHelper.HitTest(CanvasPanel, point);
            return result.VisualHit as DrawingVisual;
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                var pointDragged = e.GetPosition(CanvasPanel) + ClickOffet;
                DrawSquare(SelectedVisual, pointDragged, true);
            }
            else if (IsMultiSelecting)
            {
                var pointDragged = e.GetPosition(CanvasPanel);
                DrawSelectionSquare(SelectionSquareTopLeft, pointDragged);
            }
        }

        private void CanvasPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMultiSelecting)
            {
                var geometry = new RectangleGeometry(new Rect(SelectionSquareTopLeft, e.GetPosition(CanvasPanel)));
                var visualInRegion = CanvasPanel.GetVisuals(geometry);

                MessageBox.Show($"选中数量：{visualInRegion.Count}");
                IsMultiSelecting = false;
                CanvasPanel.RemoveVisual(SelectionSquare);
                CanvasPanel.ReleaseMouseCapture();
            }
            else
            {
                IsDragging = false;
            }
        }

       

        public Brush SelectionSquareBrush { get; set; } = Brushes.Transparent;
        public Pen SelectionSquarePen { get; set; } = new Pen(Brushes.Black, 2);

        private void DrawSelectionSquare(Point point1, Point point2)
        {
            SelectionSquarePen.DashStyle = DashStyles.Dash;

            using (var dc = SelectionSquare.RenderOpen())
            {
                dc.DrawRectangle(SelectionSquareBrush, SelectionSquarePen, new Rect(point1, point2));
            }
        }

    }
}
