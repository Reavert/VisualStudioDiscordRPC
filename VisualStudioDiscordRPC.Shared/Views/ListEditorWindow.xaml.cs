using System.Windows;
using VisualStudioDiscordRPC.Shared.ViewModels;

namespace VisualStudioDiscordRPC.Shared
{
    /// <summary>
    /// Логика взаимодействия для ListEditorWindow.xaml
    /// </summary>
    public partial class ListEditorWindow : Window
    {
        public ListEditorViewModel ViewModel => (ListEditorViewModel)DataContext;
        public ListEditorWindow(ListEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}