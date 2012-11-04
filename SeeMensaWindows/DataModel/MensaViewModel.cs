using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SeeMensaWindows.DataModel
{
    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class MensaItemViewModel : ViewModelDataCommon
    {
        public MensaItemViewModel(string uniqueId, string name, Uri interfaceUriDe, string address1, string address2,
                                  string imagePath)
            : base(uniqueId)
        {
            Days.CollectionChanged += ItemsCollectionChanged;
            _name = name;
            _interfaceUriDe = interfaceUriDe;
            _address1 = address1;
            _address2 = address2;
            //_imagePath = imagePath;
            SetImage(imagePath);
        }

        public void ParseXml(string xml)
        {
            _xml = xml;

            XDocument xmlDoc = XDocument.Parse(xml);

            this.Days.Clear();

            foreach (XElement xmlDay in xmlDoc.Elements("speiseplan").Elements("tag"))
            {
                var day = DayViewModel.CreateFromXml(xmlDay);

                if (day.IsValid)
                {
                    Days.Add(day);
                }
            }
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopDays.Insert(e.NewStartingIndex, Days[e.NewStartingIndex]);
                        if (TopDays.Count > 12)
                        {
                            TopDays.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopDays.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopDays.RemoveAt(e.OldStartingIndex);
                        TopDays.Add(Days[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopDays.Insert(e.NewStartingIndex, Days[e.NewStartingIndex]);
                        TopDays.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopDays.RemoveAt(e.OldStartingIndex);
                        if (Days.Count >= 12)
                        {
                            TopDays.Add(Days[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopDays[e.OldStartingIndex] = Days[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopDays.Clear();
                    while (TopDays.Count < Days.Count && TopDays.Count < 12)
                    {
                        TopDays.Add(Days[TopDays.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<DayViewModel> _days = new ObservableCollection<DayViewModel>();
        public ObservableCollection<DayViewModel> Days
        {
            get { return this._days; }
        }

        private ObservableCollection<DayViewModel> _topDay = new ObservableCollection<DayViewModel>();
        public ObservableCollection<DayViewModel> TopDays
        {
            get { return this._topDay; }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        private Uri _interfaceUriDe;
        public Uri InterfaceUriDe
        {
            get
            {
                return _interfaceUriDe;
            }
        }

        private string _address1;
        public string Address1
        {
            get
            {
                return _address1;
            }
        }

        private string _address2;
        public string Address2
        {
            get
            {
                return _address2;
            }
        }

        private static Uri _baseUri = new Uri("ms-appx:///");
        private ImageSource _image = null;
        private string _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(_baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(string path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        private string _xml = string.Empty;
        public string Xml
        {
            get
            {
                return _xml;
            }
        }

        private DateTime _lastUpdate;
        public DateTime LastUpdate
        {
            get
            {
                return _lastUpdate;
            }
            set
            {
                _lastUpdate = value;
            }
        }
    }
}
