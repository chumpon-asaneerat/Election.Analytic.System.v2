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

        private Visual CreateShape()
        {
            Geometry geometry = CreateStreamGeometry();

            // Create a new WPF Path.
            Path path = new Path();


            // Assign the geometry to the path and set its name.
            path.Data = geometry;
            //path.Name = shapeName;
            // Set path properties.
            path.StrokeThickness = 0.5;

            path.Stroke = Brushes.Gray;
            path.Fill = new SolidColorBrush(Colors.Green);

            // Return the created WPF shape.
            return path;
        }

        private Geometry CreateStreamGeometry()
        {
            // Decide if the line segments are stroked or not. For the PolyLine type it must be stroked.
            bool isStroked = true;

            var adm = LADM0.Get().Value();

            GeometryGroup combine = new GeometryGroup();
            // Create a new stream geometry.
            StreamGeometry geometry = new StreamGeometry();
            // Obtain the stream geometry context for drawing each part.
            using (StreamGeometryContext ctx = geometry.Open())
            {
                DateTime dt = DateTime.Now;
                var admPts = LADM0Point.Gets(adm.ADM0Code).Value();
                TimeSpan ts = DateTime.Now - dt;
                Console.WriteLine("load time: {0:n3} ms.", ts.TotalMilliseconds);

                if (null != admPts)
                {
                    int iRecNo = 0;
                    int iPart = 0;
                    int iPt = 0;
                    foreach (var admPt in admPts)
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
