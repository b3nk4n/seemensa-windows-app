using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SeeMensaWindows.Common.DataModel
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class MealViewModel : ViewModelDataCommon
    {
        static MainViewModel _mainViewModel = MainViewModel.GetInstance;

        public MealViewModel(string uniqueId, string category, string title, string description, string kennzeichnung,
                             string beilagen, string preis1, string preis2, string preis3, string preis4, string einheit,
                             string signs)
            : base(uniqueId)
        {
            _category = category;
            _title = title;
            _description = description;
            _kennzeichnungen = kennzeichnung;
            _beilagen = beilagen;
            _preis1 = preis1;
            _preis2 = preis2;
            _preis3 = preis3;
            _preis4 = preis4;
            _einheit = einheit;
            _signs = signs;
        }

        public static MealViewModel CreateFromXml(XElement xmlMeal)
        {
            var category = xmlMeal.Element("category").Value;
            var title = xmlMeal.Element("title").Value;
            var description = xmlMeal.Element("description").Value;
            var kennzeichnungen = xmlMeal.Element("kennzeichnungen").Value;
            var beilagen = xmlMeal.Element("beilagen").Value;
            var preis1 = xmlMeal.Element("preis1").Value;
            var preis2 = xmlMeal.Element("preis2").Value;
            var preis3 = xmlMeal.Element("preis3").Value;
            var preis4 = xmlMeal.Element("preis4").Value;
            var einheit = xmlMeal.Element("einheit").Value;

            // Replace tags in title (= meals description)
            if (!string.IsNullOrEmpty(kennzeichnungen))
            {
                title = title.Replace(string.Format(" ({0})", kennzeichnungen), "");
            }

            var signs = ReadSigns(ref title);

            // Check title and pricing
            title = title.Replace("&quot;", "\"");
            if (preis1.Equals("0,00"))
            {
                string end = title.Substring(title.Length - 4);
                float res;
                if (float.TryParse(end, out res))
                {
                    preis1 = preis2 = preis3 = preis4 = end;
                }

                title = title.Substring(0, title.Length - 4);
            }
            title = title.TrimEnd();

            // complete "Eing" to "Eingang"
            if (title.EndsWith("Uni-Eing"))
            {
                title += "ang";
            }

            return new MealViewModel(category.GetHashCode().ToString(),
                category,
                title,
                description,
                kennzeichnungen,
                beilagen,
                preis1,
                preis2,
                preis3,
                preis4,
                einheit,
                signs);
        }

        /// <summary>
        /// Reads the signs in the title/meal description.
        /// </summary>
        /// <param name="title">The meal description</param>
        /// <returns> The comma seperated signs as string.</returns>
        private static string ReadSigns(ref string title)
        {
            List<string> signList = new List<string>();

            // Vegetarian
            if (title.Contains(" Veg "))
            {
                title = title.Replace(" Veg ", " ");
                signList.Add("V");
            }
            else if (title.Contains(" (Veg)"))
            {
                title = title.Replace(" (Veg)", "");
                signList.Add("V");
            }
            else if (title.Contains(" ( Veg)"))
            {
                title = title.Replace(" ( Veg)", "");
                signList.Add("V");
            }
            else if (title.EndsWith(" Veg"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("V");
            }

            // Pig
            if (title.Contains(" Sch "))
            {
                title = title.Replace(" Sch ", " ");
                signList.Add("S");
            }
            else if (title.Contains(" (Sch)"))
            {
                title = title.Replace(" (Sch)", "");
                signList.Add("S");
            }
            else if (title.EndsWith(" Sch"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("S");
            }
            else if (title.Contains(" P "))
            {
                title = title.Replace(" P ", " ");
                signList.Add("P");
            }
            else if (title.Contains(" (P)"))
            {
                title = title.Replace(" (P)", "");
                signList.Add("P");
            }
            else if (title.EndsWith(" P"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("P");
            }

            // Beef
            if (title.Contains(" R "))
            {
                title = title.Replace(" R ", " ");
                signList.Add("R");
            }
            else if (title.Contains(" (R)"))
            {
                title = title.Replace(" (R)", "");
                signList.Add("R");
            }
            else if (title.EndsWith(" R"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("R");
            }
            else if (title.Contains(" B "))
            {
                title = title.Replace(" B ", " ");
                signList.Add("B");
            }
            else if (title.Contains(" (B)"))
            {
                title = title.Replace(" (B)", "");
                signList.Add("B");
            }
            else if (title.EndsWith(" B"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("B");
            }
            // Fish
            if (title.Contains(" F "))
            {
                title = title.Replace(" F ", " ");
                signList.Add("F");
            }
            else if (title.Contains(" (F)"))
            {
                title = title.Replace(" (F)", "");
                signList.Add("F");
            }
            else if (title.EndsWith(" F"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("F");
            }
            // Lamb
            if (title.Contains(" L "))
            {
                title = title.Replace(" L ", " ");
                signList.Add("L");
            }
            else if (title.Contains(" (L)"))
            {
                title = title.Replace(" (L)", "");
                signList.Add("L");
            }
            else if (title.EndsWith(" L"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("L");
            }
            // Veal
            if (title.Contains(" K "))
            {
                title = title.Replace(" K ", " ");
                signList.Add("K");
            }
            else if (title.Contains(" (K)"))
            {
                title = title.Replace(" (K)", "");
                signList.Add("K");
            }
            else if (title.EndsWith(" K"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("K");
            }
            else if (title.Contains(" V "))
            {
                title = title.Replace(" V ", " ");
                signList.Add("V");
            }
            else if (title.Contains(" (V)"))
            {
                title = title.Replace(" (V)", "");
                signList.Add("V");
            }
            else if (title.EndsWith(" V"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("V");
            }
            // Poultry
            if (title.Contains(" G "))
            {
                title = title.Replace(" G ", " ");
                signList.Add("G");
            }
            else if (title.Contains(" (G)"))
            {
                title = title.Replace(" (G)", "");
                signList.Add("G");
            }
            else if (title.EndsWith(" G"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("G");
            }
            else if (title.Contains(" Po "))
            {
                title = title.Replace(" Po ", " ");
                signList.Add("Po");
            }
            else if (title.Contains(" (Po)"))
            {
                title = title.Replace(" (Po)", "");
                signList.Add("Po");
            }
            else if (title.EndsWith(" Po"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("Po");
            }

            // Pig/Beef
            if (title.Contains(" Sch/R "))
            {
                title = title.Replace(" (Sch/R) ", " ");
                signList.Add("S");
                signList.Add("R");
            }
            else if (title.Contains(" (Sch/R)"))
            {
                title = title.Replace(" (Sch/R)", "");
                signList.Add("S");
                signList.Add("R");
            }
            else if (title.EndsWith(" Sch/R"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("S");
                signList.Add("R");
            }
            else if (title.Contains(" P/B "))
            {
                title = title.Replace(" P/B ", " ");
                signList.Add("P");
                signList.Add("B");
            }
            else if (title.Contains(" (P/B)"))
            {
                title = title.Replace(" (P/B)", "");
                signList.Add("P");
                signList.Add("B");
            }
            else if (title.EndsWith(" P/B"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("P");
                signList.Add("B");
            }

            // Pig/Veg
            if (title.Contains(" Sch/Veg "))
            {
                title = title.Replace(" (Sch/Veg) ", " ");
                signList.Add("S");
                signList.Add("V");
            }
            else if (title.Contains(" (Sch/Veg)"))
            {
                title = title.Replace(" (Sch/Veg)", "");
                signList.Add("S");
                signList.Add("V");
            }
            else if (title.EndsWith(" Sch/Veg"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("S");
                signList.Add("V");
            }
            else if (title.Contains(" P/Veg "))
            {
                title = title.Replace(" P/Veg ", " ");
                signList.Add("P");
                signList.Add("V");
            }
            else if (title.Contains(" (P/Veg)"))
            {
                title = title.Replace(" (P/Veg)", "");
                signList.Add("P");
                signList.Add("V");
            }
            else if (title.EndsWith(" P/Veg"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("P");
                signList.Add("V");
            }

            // Veg/Pig
            if (title.Contains(" Veg/Sch "))
            {
                title = title.Replace(" (Veg/Sch) ", " ");
                signList.Add("V");
                signList.Add("S");
            }
            else if (title.Contains(" (Veg/Sch)"))
            {
                title = title.Replace(" (Veg/Sch)", "");
                signList.Add("V");
                signList.Add("S");
            }
            else if (title.EndsWith(" Veg/Sch"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("V");
                signList.Add("S");
            }
            else if (title.Contains(" Veg/P "))
            {
                title = title.Replace(" Veg/P ", " ");
                signList.Add("V");
                signList.Add("P");
            }
            else if (title.Contains(" (Veg/P)"))
            {
                title = title.Replace(" (Veg/P)", "");
                signList.Add("V");
                signList.Add("P");
            }
            else if (title.EndsWith(" Veg/P"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("V");
                signList.Add("P");
            }

            // Po/F
            if (title.Contains(" G/F "))
            {
                title = title.Replace(" (G/F) ", " ");
                signList.Add("G");
                signList.Add("F");
            }
            else if (title.Contains(" (G/F)"))
            {
                title = title.Replace(" (G/F)", "");
                signList.Add("G");
                signList.Add("F");
            }
            else if (title.EndsWith(" G/F"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("G");
                signList.Add("F");
            }
            else if (title.Contains(" Po/F "))
            {
                title = title.Replace(" Po/F ", " ");
                signList.Add("Po");
                signList.Add("F");
            }
            else if (title.Contains(" (Po/F)"))
            {
                title = title.Replace(" (Po/F)", "");
                signList.Add("Po");
                signList.Add("F");
            }
            else if (title.EndsWith(" Po/F"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("Po");
                signList.Add("F");
            }

            // Beef/Pig
            if (title.Contains(" R/Sch "))
            {
                title = title.Replace(" (R/Sch) ", " ");
                signList.Add("R");
                signList.Add("S");
            }
            else if (title.Contains(" (R/Sch)"))
            {
                title = title.Replace(" (R/Sch)", "");
                signList.Add("R");
                signList.Add("S");
            }
            else if (title.EndsWith(" R/Sch"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("R");
                signList.Add("S");
            }
            else if (title.Contains(" B/P "))
            {
                title = title.Replace(" R/P ", " ");
                signList.Add("B");
                signList.Add("P");
            }
            else if (title.Contains(" (B/P)"))
            {
                title = title.Replace(" (B/P)", "");
                signList.Add("B");
                signList.Add("P");
            }
            else if (title.EndsWith(" B/P"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("B");
                signList.Add("P");
            }

            var sb = new StringBuilder();

            for (int i = 0; i < signList.Count; ++i)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }

                sb.Append(signList[i]);
            }

            return sb.ToString();
        }

        private string _category;
        public string Category
        {
            get
            {
                return _category;
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
        }

        private string _kennzeichnungen;
        public string Kennzeichnung
        {
            get
            {
                return _kennzeichnungen;
            }
        }

        private string _beilagen;
        public string Beilagen
        {
            get
            {
                return _beilagen;
            }
        }

        private string _preis1;
        public string Preis1
        {
            get
            {
                return _preis1;
            }
        }

        private string _preis2;
        public string Preis2
        {
            get
            {
                return _preis2;
            }
        }

        private string _preis3;
        public string Preis3
        {
            get
            {
                return _preis3;
            }
        }

        private string _preis4;
        public string Preis4
        {
            get
            {
                return _preis4;
            }
        }

        /// <summary>
        /// Gets the price by the selected type.
        /// </summary>
        public string DisplayPrice
        {
            get
            {
                string priceToReturn = "";

                switch (_mainViewModel.PriceType)
                {
                    case PriceType.Guest:
                        priceToReturn = _preis3;
                        break;

                    case PriceType.Employee:
                        priceToReturn = _preis2;
                        break;

                    case PriceType.Pupil:
                        priceToReturn = _preis4;
                        break;

                    default:
                        priceToReturn = _preis1;
                        break;
                }

                if (priceToReturn.Equals("0,00") || priceToReturn.Equals("0.00"))
                    return string.Empty;
                else
                    return priceToReturn + " €";
            }
        }

        private string _einheit;
        public string Einheit
        {
            get
            {
                return _einheit;
            }
        }

        private string _signs;
        public string Signs
        {
            get
            {
                return _signs;
            }
        }
    }
}
