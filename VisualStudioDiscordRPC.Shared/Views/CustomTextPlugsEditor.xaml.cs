using System.Windows;
using VisualStudioDiscordRPC.Shared.ViewModels;

namespace VisualStudioDiscordRPC.Shared
{
    /// <summary>
    /// Логика взаимодействия для CustomTextPlugsEditor.xaml
    /// </summary>
    public partial class CustomTextPlugsEditor : Window
    {
        public CustomTextPlugsEditorViewModel ViewModel => (CustomTextPlugsEditorViewModel)DataContext;
        public CustomTextPlugsEditor(CustomTextPlugsEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}