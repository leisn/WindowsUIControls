using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;

using Windows.Storage;
using Windows.UI.Xaml;

namespace Demo.ViewModel
{
    public class AppSettings : BaseViewModel
    {
        private static AppSettings _Current;

        public static AppSettings Current
        {
            get
            {
                if (_Current != null)
                    return _Current;
                LoadSettings();
                return _Current;
            }
        }

        private ElementTheme _Theme;
        public ElementTheme Theme
        {
            get => _Theme;
            set
            {
                if ((int)value == -1)
                    return;
                SetProperty(ref _Theme, value, onChanged: SaveSettings);
            }
        }

        private NavigationViewPaneDisplayMode _NavigationPosition;
        public NavigationViewPaneDisplayMode NavigationPosition
        {
            get => _NavigationPosition;
            set
            {
                if ((int)value == -1)
                    return;
                SetProperty(ref _NavigationPosition, value, onChanged: SaveSettings);
            }
        }

        static void LoadSettings()
        {
            _Current = new AppSettings();
            _Current._Theme = ElementTheme.Default;
            _Current._NavigationPosition = NavigationViewPaneDisplayMode.Left;
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey("ThemeMode"))
                _Current._Theme = (ElementTheme)localSettings.Values["ThemeMode"];
            if (localSettings.Values.ContainsKey("NavigationPosition"))
                _Current._NavigationPosition =
                     (NavigationViewPaneDisplayMode)localSettings.Values["NavigationPosition"];
        }

        static void SaveSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["ThemeMode"] = (int)Current.Theme;
            localSettings.Values["NavigationPosition"] = (int)Current.NavigationPosition;
        }
    }
}
