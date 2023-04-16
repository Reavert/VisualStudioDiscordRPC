using System.Windows;
using VisualStudioDiscordRPC.Shared.ViewModels;

namespace VisualStudioDiscordRPC.Shared
{
    /// <summary>
    /// Логика взаимодействия для ListedSettingEditorWindow.xaml
    /// </summary>
    public partial class CustomSlotsEditorWindow : Window
    {
        public CustomSlotsEditorViewModel ViewModel => (CustomSlotsEditorViewModel)DataContext;
        public CustomSlotsEditorWindow(CustomSlotsEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}