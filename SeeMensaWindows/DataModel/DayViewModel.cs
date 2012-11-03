using SeeMensaWindows.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SeeMensaWindows.DataModel
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class DayViewModel : ViewModelDataCommon
    {
        public DayViewModel(string uniqueId, DateTime day)
            : base(uniqueId)
        {
            this._day = day;
        }

        private DateTime _day;
        public DateTime Day
        {
            get
            {
                return _day;
            }
        }

        public string DayText
        {
            get
            {
                return _day.ToString("dddd");
            }
        }

        public string DateText
        {
            get
            {
                return _day.ToString("d/M/yyyy");
            }
        }

        private ObservableCollection<MealViewModel> _meals = new ObservableCollection<MealViewModel>();
        public ObservableCollection<MealViewModel> Meals
        {
            get { return this._meals; }
        }

        private bool _isValid;
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
        }

        public static DayViewModel CreateFromXml(XElement xmlDay)
        {
            string timestamp = xmlDay.Attribute("timestamp").Value;
            var day = Converter.TimestampToDate(timestamp);

            var dayViewModel = new DayViewModel(day.GetHashCode().ToString(), day);

            // Do not process items of the past.
            // These days will be deleted after parsing the xml file.
            if (day.Date >= DateTime.Now.Date)
            {
                // Do not process items of the past.
                // These days will be deleted after parsing the xml file.
                dayViewModel.Meals.Clear();

                foreach (XElement xmlMeal in xmlDay.Elements("item"))
                {
                    dayViewModel.Meals.Add(MealViewModel.CreateFromXml(xmlMeal));
                }

                dayViewModel._isValid = true;
            }
            else
            {
                dayViewModel._isValid = false;
            }

            return dayViewModel;
        }
    }
}
