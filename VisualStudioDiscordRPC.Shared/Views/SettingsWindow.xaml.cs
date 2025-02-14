using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VisualStudioDiscordRPC.Shared.Services;
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

        private void ValidateIdleTimeTextBox(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
    }
}
