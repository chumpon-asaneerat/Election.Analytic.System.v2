#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;

using PPRP.Models;

#endregion

namespace PPRP.Controls
{
    public class VisualHost : UIElement
    {
        public Visual Visual { get; set; }

        protected override int VisualChildrenCount
        {
            get { return Visual != null ? 1 : 0; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return Visual;
        }
    }

    #region ShapeDrawingVisual (abstract)

    /// <summary>
    /// The ShapeDrawingVisual class (abstract).
    /// </summary>
    public abstract class ShapeDrawingVisual : FrameworkElement
    {
        #region Internal Variables

        private ScaleTransform _scaletransform = null;
        private TransformGroup _ShapeTransformGroup = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShapeDrawingVisual() : base()
        {
            Children = new VisualCollection(this);
            // Add the event handler for MouseLeftButtonUp.
            /*
            this.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(MyVisualHost_MouseLeftButtonUp);
            */
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ShapeDrawingVisual()
        {
            if (null != Children)
            {
                Children.Clear();
            }
            Children = null;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets collection of child visual objects.
        /// </summary>
        protected VisualCollection Children { get; private set; }

        protected TransformGroup ShapeTransform { get; private set; }

        protected void InitTransforms(RectangleD shapeBound)
        {
            // Bounding box for the shape.
            double xmin = shapeBound.Left;
            double xmax = shapeBound.Right;
            double ymin = shapeBound.Top;
            double ymax = shapeBound.Bottom;

            // Width and height of the bounding box.
            double shapeWd = Math.Abs(xmax - xmin);
            double shapeHt = Math.Abs(ymax - ymin);

            // Aspect ratio of the bounding box.
            double aspectRatio = shapeWd / shapeHt;

            // Aspect ratio of the current viewbox i.e canvas.
            double viewBoxRatio = this.ActualWidth / this.ActualHeight;

            // Compute a scale factor so that the shapefile geometry
            // will maximize the space used on the canvas while still
            // maintaining its aspect ratio.
            double scaleFactor;
            if (aspectRatio < viewBoxRatio)
                scaleFactor = this.ActualHeight / shapeHt;
            else
                scaleFactor = this.ActualWidth / shapeWd;

            // Compute the scale transformation. Note that we flip
            // the Y-values because the lon/lat grid is like a cartesian
            // coordinate system where Y-values increase upwards.
            ScaleTransform xformScale = new ScaleTransform(scaleFactor, -scaleFactor);

            // Compute the translate transformation so that the shapefile
            // geometry will be centered on the canvas.
            TranslateTransform xformTranslate = new TranslateTransform()
            {
                X = (this.ActualWidth - (xmin + xmax) * scaleFactor) / 2,
                Y = (this.ActualHeight + (ymin + ymax) * scaleFactor) / 2
            };

            // create new transform group.
            ShapeTransform = new TransformGroup();
            // add scale transform
            ShapeTransform.Children.Add(xformScale);
            // add translate transform
            ShapeTransform.Children.Add(xformTranslate);
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Provide a required override for the VisualChildrenCount property.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return null != Children ? Children.Count : 0; }
        }
        /// <summary>
        /// Provide a required override for the GetVisualChild method.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Returns Visual instance on specificed index.</returns>
        protected override Visual GetVisualChild(int index)
        {
            if (null == Children)
            {
                throw new NullReferenceException();
            }
            if (index < 0 || index >= Children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return Children[index];
        }

        #endregion

        #region Public Methods

        public void InvalidateLayout()
        {

        }

        #endregion
    }

    #endregion

    #region ThailandDrawingVisual

    /// <summary>
    /// The Thailand DrawingVisual class.
    /// Create a host visual derived from the FrameworkElement class.
    /// This class provides layout, event handling, and container support for
    /// the child visual objects.
    /// </summary>
    public class ThailandDrawingVisual : ShapeDrawingVisual
    {
        #region Internal Variables

        private Geometry _geometry = null;

        #endregion

        #region Constructor

        public ThailandDrawingVisual() : base()
        {
            var visual = Create();
            _ = Children.Add(visual);

            // Add the event handler for MouseLeftButtonUp.
            /*
            this.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(MyVisualHost_MouseLeftButtonUp);
            */
        }

        #endregion

        #region Override Methods

        #endregion

        #region Private Methods

        private Visual Create()
        {
            Geometry geometry = CreateStreamGeometry();
            geometry.Freeze();

            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            drawingContext.DrawGeometry(Brushes.Gray, new Pen(Brushes.Salmon, 0.5), geometry);

            // Persist the drawing content.
            drawingContext.Close();

            return drawingVisual;
        }

        public LADM0 ADM { get; set; }
        public List<LADM0Point> LADMPoints { get; set; }

        private Geometry CreateStreamGeometry()
        {
            if (null == ADM) ADM = LADM0.Get().Value();
            if (null == ADM) return null;

            // Call to recalc current transform.
            InitTransforms(new RectangleD() 
            { 
                Left = ADM.LF, 
                Top = ADM.TP, 
                Right = ADM.RT, 
                Bottom = ADM.BT 
            });

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

                        Point pt = ShapeTransform.Transform(new Point(admPt.X, admPt.Y));

                        if (iPt == 0)
                            ctx.BeginFigure(pt, true, false);
                        else
                            ctx.LineTo(pt, isStroked, true);

                        iPt++;
                    }
                }

                /*
                var parts = LADM0Part.Gets(adm.ADM0Code).Value();
                if (null != parts)
                {
                    int iPart = 1;
                    foreach (var part in parts)
                    {
                        // Draw figures.
                        DateTime dt = DateTime.Now;

                        var points = LADM0Point.Gets(adm.ADM0Code, part.RecordId, part.PartId).Value();

                        TimeSpan ts = DateTime.Now - dt;
                        Console.WriteLine("Part: {0:n0} load time: {1:n3} ms.", iPart, ts.TotalMilliseconds);

                        if (null != points)
                        {
                            int maxPts = points.Count;
                            for (int i = 0; i < maxPts; ++i)
                            {
                                var point = points[i];
                                Point pt = new Point(point.X, point.Y);

                                if (i == 0)
                                    ctx.BeginFigure(pt, true, false);
                                else
                                    ctx.LineTo(pt, isStroked, true);

                                iCnt++; // count all points
                            }
                        }

                        iPart++;
                    }
                }
                */
            }
            // add shape geometry
            combine.Children.Add(geometry);

            // Return the created stream geometry.
            return combine;
        }

        #endregion
    }

    #endregion
}
