using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SeeMensaWindows.Helpers
{
    /// <summary>
    /// Helper class to simplify Windows.Storage.
    /// </summary>
    class EasyStorage
    {
        /// <summary>
        /// Saves a string.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string key, string value)
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values[key].ToString() != null)
                {
                    // do update
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] = value;
                }
            }
            else
            {
                // do create key and save value, first time only.
                Windows.Storage.ApplicationData.Current.LocalSettings.CreateContainer(key, ApplicationDataCreateDisposition.Always);
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] == null)
                {
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] = value;
                }
            }
        }

        public static string Load(string key)
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                return (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values[key];
            }

            return string.Empty;
        }

        public static async Task<bool> SaveLarge(string Key, object value)
        {
            var ms = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(value.GetType());
            serializer.WriteObject(ms, value);
            await ms.FlushAsync();

            ms.Seek(0, SeekOrigin.Begin);
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(Key, CreationCollisionOption.ReplaceExisting);
            using (Stream fileStream = await file.OpenStreamForWriteAsync())
            {
                await ms.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            return true;
        }

        // Necessary to pass back both the result and status from an async function since you  can't pass by ref
        internal class ReadResults
        {
            public bool Success { get; set; }
            public Object Result { get; set; }
        }

        public async static Task<T> LoadLarge<T>(string Key)
        {
            var rr = new ReadResults();

            try
            {
                var ms = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));

                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(Key);
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    var readRes = inStream.AsStreamForRead();
                    rr.Result = (T)serializer.ReadObject(readRes);
                }
                rr.Success = true;
            }
            catch (FileNotFoundException)
            {
                rr.Success = false;
            }
            catch (Exception)
            {
                rr.Success = false;
            }
            return (T)rr.Result;
        }
    }
}
