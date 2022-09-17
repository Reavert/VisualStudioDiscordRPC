using System.Windows;
using VisualStudioDiscordRPC.Shared.ViewModels;

namespace VisualStudioDiscordRPC.Shared
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsViewModel ViewModel => (SettingsViewModel)DataContext;
        public SettingsWindow(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            Closing += SettingsWindow_Closing;
        }

        private void SettingsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.Wrapper.Update();
            Settings.Default.Save();
        }

        private void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DiscordAppId = (string) Settings.Default.Properties["ApplicationID"].DefaultValue;
        }
    }
}
