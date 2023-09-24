using System.ComponentModel;
using System.Windows;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.ViewModels;

namespace VisualStudioDiscordRPC.Shared
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsViewModel ViewModel => (SettingsViewModel)DataContext;

        private SettingsService _settingsService;

        public SettingsWindow(SettingsViewModel viewModel)
        {
            _settingsService = ServiceRepository.Default.GetService<SettingsService>();

            InitializeComponent();
            DataContext = viewModel;
            
            Closing += SettingsWindow_Closing;
        }

        private void SettingsWindow_Closing(object sender, CancelEventArgs e)
        {
            _settingsService.Save();
        }

        private void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DiscordAppId = DiscordRpcController.DefaultApplicationId;
        }
    }
}
