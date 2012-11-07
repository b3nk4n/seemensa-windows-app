using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeMensaWindows.Common.DataModel
{
    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class MainViewModel
    {
        private static MainViewModel _mainViewModel = new MainViewModel();

        public static MainViewModel GetInstance
        {
            get
            {
                return _mainViewModel;
            }
        }

        private ObservableCollection<MensaItemViewModel> _allMensas = new ObservableCollection<MensaItemViewModel>();
        public ObservableCollection<MensaItemViewModel> AllMensas
        {
            get { return _allMensas; }
        }

        public static IEnumerable<MensaItemViewModel> GetMensas(string uniqueId)
        {
            if (!uniqueId.Equals("AllMensas")) throw new ArgumentException("Only 'AllMensas' is supported as a collection of mensas");

            return _mainViewModel.AllMensas;
        }

        public static MensaItemViewModel GetMensa(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _mainViewModel.AllMensas.Where((mensa) => mensa.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static DayViewModel GetDay(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _mainViewModel.AllMensas.SelectMany(mensa => mensa.Days).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public MainViewModel()
        {
            var mensa1 = new MensaItemViewModel("unikn",
                        "Menseria Universität Konstanz",
                        new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_giessberg/speiseplan.xml", UriKind.Absolute),
                        "Universitätsstraße 10",
                        "78464 Konstanz",
                        "Assets/unikn.png");
            var mensa2 = new MensaItemViewModel("htwgkn",
                    "Mensa HTWG Konstanz",
                    new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_htwg/speiseplan.xml", UriKind.Absolute),
                    "Alfred-Wachtel-Straße 12",
                    "78462 Konstanz",
                    "Assets/htwgkn.png");
            var mensa3 = new MensaItemViewModel("phwg",
                    "Mensa HS/PH Weingarten",
                    new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_weingarten/speiseplan.xml", UriKind.Absolute),
                    "Doggenriedstraße 28",
                    "88250 Weingarten",
                    "Assets/phwg.png");
            var mensa4 = new MensaItemViewModel("unifn",
                    "Mensa Friedrichshafen",
                    new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_friedrichshafen/speiseplan.xml", UriKind.Absolute),
                    "Fallenbrunnen 2",
                    "88045 Friedrichshafen",
                    "Assets/unifn.png");

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var day1 = new DayViewModel("day1",
                    DateTime.Now);
                var meal1 = new MealViewModel("meal01",
                    "Stamm",
                    "Nudeln mit Pommes",
                    "Description?",
                    "V",
                    "Pommes, Gemüse",
                    "2.65",
                    "5.50",
                    "6.50",
                    "7.50",
                    "€",
                    "V,R");
                var meal2 = new MealViewModel("meal02",
                    "Wahl 1",
                    "Schweinshaxe mit Kartoffelsalat",
                    "Description?",
                    "S",
                    "Nudeln, Gemüse",
                    "2.75",
                    "5.60",
                    "6.60",
                    "7.60",
                    "€",
                    "X");
                day1.Meals.Add(meal1);
                day1.Meals.Add(meal2);
                mensa1.Days.Add(day1);

                var day2 = new DayViewModel("day2",
                    DateTime.Now.AddDays(1));
                var meal3 = new MealViewModel("meal03",
                    "Stamm",
                    "Nudeln mit Schinken",
                    "Description?",
                    "V",
                    "Pommes, Gemüse",
                    "2.65",
                    "5.50",
                    "6.50",
                    "7.50",
                    "€",
                    "V,G");
                var meal4 = new MealViewModel("meal04",
                    "Wahl 1",
                    "chweinsknödelmit Kartoffelpüree",
                    "Desctiption?",
                    "S",
                    "Nudeln, Gemüse",
                    "2.75",
                    "5.60",
                    "6.60",
                    "7.60",
                    "€",
                    "P,S");
                day2.Meals.Add(meal3);
                day2.Meals.Add(meal4);
                mensa1.Days.Add(day2);
            }

            this.AllMensas.Add(mensa1);
            this.AllMensas.Add(mensa2);
            this.AllMensas.Add(mensa3);
            this.AllMensas.Add(mensa4);
        }

        private static string _selectedMensaId = string.Empty;

        public static string SelectedMensaId
        {
            get
            {
                return _selectedMensaId;
            }
        }

        public static bool IsMensaSelected
        {
            get
            {
                return !string.IsNullOrEmpty(_selectedMensaId);
            }
        }

        public static void SelectMensa(string mensaId)
        {
            _selectedMensaId = mensaId;
        }

        public static void ResetSelectedMensa()
        {
            _selectedMensaId = string.Empty;
        }

        private static bool _hasLiveTile;

        public static bool HasLiveTile
        {
            get
            {
                return _hasLiveTile;
            }
            set
            {
                _hasLiveTile = true;
            }
        }
    }
}
