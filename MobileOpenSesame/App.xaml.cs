using MobileOpenSesame.Views;

namespace MobileOpenSesame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            Routing.RegisterRoute(nameof(LoginDataPage), typeof(LoginDataPage));
            Routing.RegisterRoute(nameof(ShowDataPage), typeof(ShowDataPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}
