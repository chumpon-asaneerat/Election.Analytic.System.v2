#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;

using PPRP.Models;
using System.Windows.Threading;

#endregion

namespace PPRP.Controls
{
    public class VisualHost : UIElement
    {
        private DispatcherTimer timer = null;

        public VisualHost()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        ~VisualHost()
        {
            if (null != timer)
            {
                timer.Tick -= Timer_Tick;
                timer.Stop();
            }
            timer = null;
        }

        public Canvas Canvas { get; set; }
        public ShapeDrawingVisual Visual { get; set; }

        protected override int VisualChildrenCount
        {
            get { return Visual != null ? 1 : 0; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return Visual;
        }


        private bool _trackResize = false;
        private Size _trackSize = new Size(0, 0);
        private Size _canvasSize = new Size(0, 0);

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (null == Canvas)
                return;

            // last size not same as actual size
            if (_canvasSize.Width != Canvas.ActualWidth || _canvasSize.Height != Canvas.ActualHeight)
            {
                _trackResize = true; // start tracking
            }

            if (_trackResize)
            {
                // on tracking and still on resize so track size should not same as actual size
                if (_trackSize.Width != Canvas.ActualWidth && _trackSize.Height != Canvas.ActualHeight)
                {
                    // update value
                    _trackSize.Width = Canvas.ActualWidth;
                    _trackSize.Height = Canvas.ActualHeight;
                    return;
                }
                // if tracking size is equal to actual size then reset flag
                if (_canvasSize.Width != _trackSize.Width || _canvasSize.Height != _trackSize.Height)
                {
                    _trackResize = false;

                    RefreshTransforms();
                }
            }
        }

        public void RefreshTransforms()
        {
            if (null == Canvas)
                return;

            if (_canvasSize.Width == Canvas.ActualWidth && _canvasSize.Height == Canvas.ActualHeight)
                return;

            // keep last size
            _canvasSize.Width = Canvas.ActualWidth;
            _canvasSize.Height = Canvas.ActualHeight;

            // reset transform.
            Visual.ShapeTransform = null;
            // call to invaludate
            this.InvalidateVisual();
        }

        private GeometryGroup geometry;
        private bool onRender = false;

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (onRender)
                return;
            onRender = true;

            if (null == geometry)
                geometry = Visual.GetStreamGeometryGroup();
            if (null == Visual.ShapeTransform)
                Visual.InitTransforms(Canvas);
            geometry.Transform = Visual.ShapeTransform;
            //geometry.Freeze();

            drawingContext.DrawGeometry(Brushes.Gray, new Pen(Brushes.Salmon, 0.5), geometry);

            onRender = false;
        }
    }


    public abstract class ShapeDrawingVisual : DrawingVisual
    {
        public TransformGroup ShapeTransform { get; set; }

        public abstract RectangleD Bound { get; }

        public void InitTransforms(FrameworkElement parent)
        {
            if (null == parent || null == Bound)
                return;
            if (null != ShapeTransform)
                return;

            // Bounding box for the shape.
            double xmin = Bound.Left;
            double xmax = Bound.Right;
            double ymin = Bound.Top;
            double ymax = Bound.Bottom;

            // Width and height of the bounding box.
            double shapeWd = Math.Abs(xmax - xmin);
            double shapeHt = Math.Abs(ymax - ymin);

            // Aspect ratio of the bounding box.
            double aspectRatio = shapeWd / shapeHt;

            // Aspect ratio of the current viewbox i.e canvas.
            double viewBoxRatio = parent.ActualWidth / parent.ActualHeight;

            // Compute a scale factor so that the shapefile geometry
            // will maximize the space used on the canvas while still
            // maintaining its aspect ratio.
            double scaleFactor;
            if (aspectRatio < viewBoxRatio)
                scaleFactor = parent.ActualHeight / shapeHt;
            else
                scaleFactor = parent.ActualWidth / shapeWd;

            // Compute the scale transformation. Note that we flip
            // the Y-values because the lon/lat grid is like a cartesian
            // coordinate system where Y-values increase upwards.
            ScaleTransform xformScale = new ScaleTransform(scaleFactor, -scaleFactor);

            // Compute the translate transformation so that the shapefile
            // geometry will be centered on the canvas.
            TranslateTransform xformTranslate = new TranslateTransform()
            {
                X = (parent.ActualWidth - (xmin + xmax) * scaleFactor) / 2,
                Y = (parent.ActualHeight + (ymin + ymax) * scaleFactor) / 2
            };

            // create new transform group.
            ShapeTransform = new TransformGroup();
            // add scale transform
            ShapeTransform.Children.Add(xformScale);
            // add translate transform
            ShapeTransform.Children.Add(xformTranslate);
        }

        public abstract GeometryGroup GetStreamGeometryGroup();
    }

    public class ThailandDrawingVisual : ShapeDrawingVisual
    {
        public LADM0 ADM { get; set; }
        public List<LADM0Point> LADMPoints { get; set; }

        public override RectangleD Bound 
        {
            get
            {
                RectangleD ret = null;
                if (null != ADM)
                {
                    ret = new RectangleD()
                    {
                        Left = ADM.LF,
                        Top = ADM.TP,
                        Right = ADM.RT,
                        Bottom = ADM.BT
                    };

                }
                return ret;
            }
        }

        public override GeometryGroup GetStreamGeometryGroup()
        {
            if (null == ADM) ADM = LADM0.Get().Value();
            if (null == ADM) return null;

            // Decide if the line segments are stroked or not. For the PolyLine type it must be stroked.
            bool isStroked = true;

            GeometryGroup combine = new GeometryGroup();
            // Create a new stream geometry.
            StreamGeometry geometry = new StreamGeometry();
            // Obtain the stream geometry context for drawing each part.
            using (StreamGeometryContext ctx = geometry.Open())
            {
                if (LADMPoints == null)
                {
                    DateTime dt = DateTime.Now;
                    LADMPoints = LADM0Point.Gets(ADM.ADM0Code).Value();
                    TimeSpan ts = DateTime.Now - dt;
                    Console.WriteLine("load time: {0:n3} ms.", ts.TotalMilliseconds);
                }

                if (null != LADMPoints)
                {
                    int iRecNo = 0;
                    int iPart = 0;
                    int iPt = 0;
                    foreach (var admPt in LADMPoints)
                    {
                        if (iRecNo != admPt.RecordId)
                        {
                            iRecNo = admPt.RecordId; // record changed.
                            iPart = 0;
                        }
                        if (iPart != admPt.PartId)
                        {
                            iPart = admPt.PartId; // part changed
                            iPt = 0;
                        }

                        Point pt = new Point(admPt.X, admPt.Y);

                        if (iPt == 0)
                            ctx.BeginFigure(pt, true, false);
                        else
                            ctx.LineTo(pt, isStroked, true);

                        iPt++;
                    }
                }
            }
            // add shape geometry
            combine.Children.Add(geometry);

            // Return the created stream geometry.
            return combine;
        }
    }
}


namespace PPRP.Controls.v2
{
    public class ADMShapePoint
    {
        public int RecordId { get; set; }
        public int PartId { get; set; }
        public int PointId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class ADMShapePart
    {
        public ADMShapePart() : base()
        {
            this.Points = new List<ADMShapePoint>();
        }

        public int RecordId { get; set; }
        public int PartId { get; set; }        
        public List<ADMShapePoint> Points { get; private set; }
    }

    // need class to load data into GeomertyGroup
    public class ADMShape
    {
        public ADMShape() : base()
        {
            Parts = new List<ADMShapePart>();
        }


        public string ADMCode { get; set; }
        public RectangleD Bound { get; set; }
        public List<ADMShapePart> Parts { get; private set; }
    }
}

