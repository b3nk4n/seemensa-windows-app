using SeeMensaWindows.DataModel;
using SeeMensaWindows.Helpers;
using System;

namespace SeeMensaWindows.Storage
{
    public static class AppStorage
    {
        #region Load/Save

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public static async void Save()
        {
            EasyStorage.Save("selectedMensa", MainViewModel.SelectedMensaId.ToString());

            foreach (var mensa in MainViewModel.GetMensas("AllMensas"))
            {
                EasyStorage.Save(mensa.UniqueId + "update", mensa.LastUpdate.ToString());
                await EasyStorage.SaveLarge(mensa.UniqueId + "xml", mensa.Xml);
            }

            
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public static async void Load()
        {
            MainViewModel.SelectMensa(EasyStorage.Load("selectedMensa"));

            foreach (var mensa in MainViewModel.GetMensas("AllMensas"))
            {
                DateTime dt;
                if (DateTime.TryParse(EasyStorage.Load(mensa.UniqueId + "update"), out dt))
                {
                    mensa.LastUpdate = dt;
                }

                var xml = await EasyStorage.LoadLarge<string>(mensa.UniqueId + "xml");
                if (!string.IsNullOrEmpty(xml))
                {
                    mensa.ParseXml(xml);
                }
            } 
        }

        #endregion
    }
}
