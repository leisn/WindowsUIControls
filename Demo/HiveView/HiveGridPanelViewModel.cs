using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo.ViewModel;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Demo.HiveView
{
    public class HiveGridPanelViewModel : BaseViewModel
    {
        public Array LineCaps { get; }
        public Array LineJoins { get; }

        public HiveGridPanelViewModel()
        {
            LineJoins = Enum.GetValues(typeof(PenLineJoin));
            LineCaps = Enum.GetValues(typeof(PenLineCap));
        }

        #region hivegrid
        bool autocell = false;
        public bool Autocell
        {
            get { return autocell; }
            set { SetProperty(ref autocell, value); }
        }

        double panelSpacing = 4;
        public double PanelSpacing
        {
            get { return panelSpacing; }
            set { SetProperty(ref panelSpacing, value); }
        }

        double panelFixedEdge = 0;
        public double PanelFixedEdge
        {
            get { return panelFixedEdge; }
            set { SetProperty(ref panelFixedEdge, value); }
        }

        int panelRowCont = 4;
        public int PanelRowCont
        {
            get { return panelRowCont; }
            set { SetProperty(ref panelRowCont, value); }
        }

        int panelColCont = 4;
        public int PanelColCont
        {
            get { return panelColCont; }
            set { SetProperty(ref panelColCont, value); }
        }
        #endregion

        #region strokes

        private double strokeThickness = 5;
        public double StrokeThickness
        {
            get { return strokeThickness; }
            set { SetProperty(ref strokeThickness, value); }
        }

        private double strokeMiterLimit = 10;
        public double StrokeMiterLimit
        {
            get { return strokeMiterLimit; }
            set { SetProperty(ref strokeMiterLimit, value); }
        }

        private double strokeDashOffset = 0;
        public double StrokeDashOffset
        {
            get { return strokeDashOffset; }
            set { SetProperty(ref strokeDashOffset, value); }
        }

        private DoubleCollection strokeDashArray = new DoubleCollection { 0, 0 };
        public DoubleCollection StrokeDashArray
        {
            get { return strokeDashArray; }
            set { SetProperty(ref strokeDashArray, value); }
        }

        private PenLineCap strokeStartLineCap = PenLineCap.Flat;
        public PenLineCap StrokeStartLineCap
        {
            get { return strokeStartLineCap; }
            set { SetProperty(ref strokeStartLineCap, value); }
        }

        private PenLineCap strokeEndLineCap = PenLineCap.Flat;
        public PenLineCap StrokeEndLineCap
        {
            get { return strokeEndLineCap; }
            set { SetProperty(ref strokeEndLineCap, value); }
        }

        private PenLineCap strokeDashCap = PenLineCap.Round;
        public PenLineCap StrokeDashCap
        {
            get { return strokeDashCap; }
            set { SetProperty(ref strokeDashCap, value); }
        }

        private PenLineJoin strokeLineJoin = PenLineJoin.Miter;
        public PenLineJoin StrokeLineJoin
        {
            get { return strokeLineJoin; }
            set { SetProperty(ref strokeLineJoin, value); }
        }
        #endregion
    }
}
