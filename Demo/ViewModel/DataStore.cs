using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Demo.Models;

using Newtonsoft.Json;
using System.IO;

namespace Demo.ViewModel
{
    public class DataStore
    {
        private static IEnumerable<ControlCategorie> _Controls;

        public static async Task<IEnumerable<ControlCategorie>> GetControlCategoriesAsync()
        {
            if (_Controls != null)
                return _Controls;

            var stream = typeof(DataStore).Assembly.GetManifestResourceStream("Demo.Resources.controls.json");
            return await Task.Run(() =>
            {
                var str = "";
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                    str = sr.ReadToEnd();
                _Controls = JsonConvert.DeserializeObject<ControlCategorie[]>(str);
                return _Controls;
            }); ;
        }

        public static async Task<ControlCategorie> GetControlCategorieAsync(string categorie)
        {
            if (_Controls == null)
                await GetControlCategoriesAsync();

            foreach (var item in _Controls)
            {
                if (item.Title == categorie)
                    return item;
            }
            return null;
        }
    }
}
