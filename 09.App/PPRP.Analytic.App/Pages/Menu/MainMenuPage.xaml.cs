#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Services;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainMenuPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void slideZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Zoom(slideZoom.Value);
        }

        private void Zoom(double factor)
        {
            var ctrl = lvItems;
            var scaler = ctrl.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                scaler = new ScaleTransform(factor, factor);
                ctrl.LayoutTransform = scaler;
            }
            else if (scaler.HasAnimatedProperties)
            {
                // Do nothing because the value is being changed by animation.
                // Setting scaler.ScaleX will cause infinite recursion due to the
                // binding specified in the XAML.
            }
            else
            {
                scaler.ScaleX = factor;
                scaler.ScaleY = factor;
            }
            /*
            if (scaler == null)
            {
                scaler = new ScaleTransform(1.0, 1.0);
                ctrl.LayoutTransform = scaler;
            }

            // We'll need a DoubleAnimation object to drive
            // the ScaleX and ScaleY properties.

            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(600)),
            };

            // Toggle the scale between 1.0 and 1.5.

            if (scaler.ScaleX == 1.0)
            {
                animator.To = 1.5;
            }
            else
            {
                animator.To = 1.0;
            }

            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
            */
        }

        #region Public Methods

        public void Setup() 
        {
            lvItems.ItemsSource = null;

            var items = new List<Data>();
            items.Add(new Data() { Age = 31, Name = "Example 1" });
            items.Add(new Data() { Age = 42, Name = "Charie" });
            items.Add(new Data() { Age = 23, Name = "John Doe" });
            items.Add(new Data() { Age = 19, Name = "Jame Smith" });
            items.Add(new Data() { Age = 27, Name = "Otto De Fransis" });
            items.Add(new Data() { Age = 39, Name = "Nicky Bround" });
            items.Add(new Data() { Age = 20, Name = "JJ O'Briun" });
            items.Add(new Data() { Age = 45, Name = "Smith Iron" });
            items.Add(new Data() { Age = 57, Name = "Bla Bla Bla" });
            items.Add(new Data() { Age = 28, Name = "Bla Bla Bla Bla Bla Bla" });
            items.Add(new Data() { Age = 31, Name = "Example 1" });
            items.Add(new Data() { Age = 42, Name = "Charie" });
            items.Add(new Data() { Age = 23, Name = "John Doe" });
            items.Add(new Data() { Age = 19, Name = "Jame Smith" });
            items.Add(new Data() { Age = 27, Name = "Otto De Fransis" });
            items.Add(new Data() { Age = 39, Name = "Nicky Bround" });
            items.Add(new Data() { Age = 20, Name = "JJ O'Briun" });
            items.Add(new Data() { Age = 45, Name = "Smith Iron" });
            items.Add(new Data() { Age = 57, Name = "Bla Bla Bla" });
            items.Add(new Data() { Age = 28, Name = "Bla Bla Bla Bla Bla Bla" });
            items.Add(new Data() { Age = 31, Name = "Example 1" });
            items.Add(new Data() { Age = 42, Name = "Charie" });
            items.Add(new Data() { Age = 23, Name = "John Doe" });
            items.Add(new Data() { Age = 19, Name = "Jame Smith" });
            items.Add(new Data() { Age = 27, Name = "Otto De Fransis" });
            items.Add(new Data() { Age = 39, Name = "Nicky Bround" });
            items.Add(new Data() { Age = 20, Name = "JJ O'Briun" });
            items.Add(new Data() { Age = 45, Name = "Smith Iron" });
            items.Add(new Data() { Age = 57, Name = "Bla Bla Bla" });
            items.Add(new Data() { Age = 28, Name = "Bla Bla Bla Bla Bla Bla" });
            items.Add(new Data() { Age = 31, Name = "Example 1" });
            items.Add(new Data() { Age = 42, Name = "Charie" });
            items.Add(new Data() { Age = 23, Name = "John Doe" });
            items.Add(new Data() { Age = 19, Name = "Jame Smith" });
            items.Add(new Data() { Age = 27, Name = "Otto De Fransis" });
            items.Add(new Data() { Age = 39, Name = "Nicky Bround" });
            items.Add(new Data() { Age = 20, Name = "JJ O'Briun" });
            items.Add(new Data() { Age = 45, Name = "Smith Iron" });
            items.Add(new Data() { Age = 57, Name = "Bla Bla Bla" });
            items.Add(new Data() { Age = 28, Name = "Bla Bla Bla Bla Bla Bla" });
            items.Add(new Data() { Age = 19, Name = "END OF ITEMS" });

            lvItems.ItemsSource = items;
        }

        #endregion

        public class Data
        {
            public int Age { get; set; }
            public string Name { get; set; }
        }
             
    }
}
