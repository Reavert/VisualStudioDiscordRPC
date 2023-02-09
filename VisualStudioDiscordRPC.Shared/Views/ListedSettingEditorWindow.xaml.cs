using System.Windows;
using VisualStudioDiscordRPC.Shared.ViewModels;

namespace VisualStudioDiscordRPC.Shared
{
    /// <summary>
    /// Логика взаимодействия для ListedSettingEditorWindow.xaml
    /// </summary>
    public partial class ListedSettingEditorWindow : Window
    {
        public ListedSettingEditorViewModel ViewModel => (ListedSettingEditorViewModel)DataContext;
        public ListedSettingEditorWindow(ListedSettingEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            Title = $"Editing '{viewModel.SettingName}'";
        }
    }
}