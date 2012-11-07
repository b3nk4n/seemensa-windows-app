﻿using System;
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

            // Replace tasg in title (= meals description)
            if (!string.IsNullOrEmpty(kennzeichnungen))
            {
                title = title.Replace(string.Format(" ({0})", kennzeichnungen), "");
            }

            var signs = ReadSigns(ref title);

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

        public string DisplayPrice
        {
            get
            {
                return string.Format("{0} {1}", Preis1, Einheit);
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