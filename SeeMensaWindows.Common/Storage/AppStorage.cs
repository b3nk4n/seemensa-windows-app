using SeeMensaWindows.Common.DataModel;
using SeeMensaWindows.Helpers;
using System;
using System.Threading.Tasks;

namespace SeeMensaWindows.Common.Storage
{
    public static class AppStorage
    {
         static MainViewModel _mainViewModel = MainViewModel.GetInstance;

        #region Load/Save

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public static async void Save()
        {
            EasyStorage.Save("selectedMensa", _mainViewModel.SelectedMensaId.ToString());
            EasyStorage.Save("pricetype", _mainViewModel.PriceType.ToString());

            foreach (var mensa in _mainViewModel.GetMensas("AllMensas"))
            {
                EasyStorage.Save(mensa.UniqueId + "update", mensa.LastUpdate.ToString());
                await EasyStorage.SaveLarge(mensa.UniqueId + "xml", mensa.Xml);
            }

            
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public static async Task Load()
        {
            _mainViewModel.SelectMensa(EasyStorage.Load("selectedMensa"));

            var pricetype = EasyStorage.Load("pricetype");
            if (!string.IsNullOrEmpty(pricetype))
            {
                _mainViewModel.PriceType = (PriceType)Enum.Parse(typeof(PriceType), pricetype, true);
            }


            foreach (var mensa in _mainViewModel.GetMensas("AllMensas"))
            {
                DateTime dt;
                if (DateTime.TryParse(EasyStorage.Load(mensa.UniqueId + "update"), out dt))
                {
                    mensa.LastUpdate = dt;
                }

                var xml = await EasyStorage.LoadLarge<string>(mensa.UniqueId + "xml");
                if (!string.IsNullOrEmpty(xml) && mensa.UniqueId.Equals(_mainViewModel.SelectedMensaId))
                {
                    try
                    {
                        mensa.ParseXml(xml);
                    }
                    catch (Exception)
                    {
                        // Reset last update flag, if loading the xml file produces errors.
                        mensa.LastUpdate = new DateTime();
                    }
                }
            } 
        }

        #endregion
    }
}
