#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

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

        private DrawingVisual Create()
        {
            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            // Create a rectangle and draw it in the DrawingContext.
            Rect rect = new Rect(new Point(160, 100), new Size(320, 80));
            
            drawingContext.DrawRectangle(Brushes.LightBlue, (Pen)null, rect);

            // Persist the drawing content.
            drawingContext.Close();

            return drawingVisual;
        }

        #endregion
    }

    #endregion
}
