using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SeeMensaWindows.Controls
{
    public sealed partial class MealTypeControl : UserControl
    {
        #region Members
        /// <summary>
        /// The image base uri.
        /// </summary>
        private static readonly Uri URI_BASE = new Uri("ms-appx:///");

        /// <summary>
        /// The defined bindings between a sign key and an image.
        /// </summary>
        private static Dictionary<string, Uri> _bindings = new Dictionary<string, Uri>();

        /// <summary>
        /// The dependency property for the signs.
        /// </summary>
        public static readonly DependencyProperty SignsProperty =
            DependencyProperty.Register("Signs",
            typeof(string), typeof(MealTypeControl),
            new PropertyMetadata(
                string.Empty,
                new PropertyChangedCallback(SignsChanged)));



        public int Size
        {
            get { return (int)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(MealTypeControl), new PropertyMetadata(0));



        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of MealTypeControl.
        /// </summary>
        public MealTypeControl()
        {
            this.InitializeComponent();
            InitializeBindings();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Changed event handler which is called when the dependency property Signs has changed.
        /// </summary>
        /// <param name="source">The dependency source object.</param>
        /// <param name="e">The event args.</param>
        private static void SignsChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            MealTypeControl mtControl = source as MealTypeControl;
            string newValue = e.NewValue as string;

            if (source != null && newValue != null)
            {
                UpdateUI(mtControl, newValue); 
            }
        }

        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="mtControl">The MealTypeControl to update.</param>
        /// <param name="mealSigns">The new meal signs.</param>
        private static void UpdateUI(MealTypeControl mtControl, string mealSigns)
        {
            var signs = mealSigns.Split(',');

            mtControl.Meals.Items.Clear();

            foreach (var sign in signs)
            {
                if (_bindings.ContainsKey(sign))
                {
                    var imgSource = new BitmapImage(_bindings[sign]);
                    mtControl.Meals.Items.Add(
                        new Image {
                            Source = imgSource,
                            Width = mtControl.Size,
                            Height = mtControl.Size,
                            Margin = new Thickness(0, 0, 4, 0)});
                }
            }
        }

        /// <summary>
        /// Initializes the sing bindings.
        /// </summary>
        private static void InitializeBindings()
        {
            if (_bindings.Count == 0)
            {
                _bindings.Add("Veg", new Uri(URI_BASE, "Assets/veg.png"));
                _bindings.Add("S", new Uri(URI_BASE, "Assets/pork.png"));
                _bindings.Add("P", new Uri(URI_BASE, "Assets/pork.png"));
                _bindings.Add("R", new Uri(URI_BASE, "Assets/beef.png"));
                _bindings.Add("B", new Uri(URI_BASE, "Assets/beef.png"));
                _bindings.Add("F", new Uri(URI_BASE, "Assets/fish.png"));
                _bindings.Add("L", new Uri(URI_BASE, "Assets/lamb.png"));
                _bindings.Add("K", new Uri(URI_BASE, "Assets/veal.png"));
                _bindings.Add("V", new Uri(URI_BASE, "Assets/veal.png"));
                _bindings.Add("G", new Uri(URI_BASE, "Assets/poultry.png"));
                _bindings.Add("Po", new Uri(URI_BASE, "Assets/poultry.png"));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the meal signs.
        /// </summary>
        public string Signs
        {
            get { return (string)GetValue(SignsProperty); }
            set { SetValue(SignsProperty, value); }
        }

        #endregion
    }
}
