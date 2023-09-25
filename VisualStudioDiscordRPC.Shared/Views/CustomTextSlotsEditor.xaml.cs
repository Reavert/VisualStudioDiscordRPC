using System.Windows;
using VisualStudioDiscordRPC.Shared.ViewModels;

namespace VisualStudioDiscordRPC.Shared
{
    /// <summary>
    /// Логика взаимодействия для CustomTextSlotsEditor.xaml
    /// </summary>
    public partial class CustomTextSlotsEditor : Window
    {
        public CustomTextSlotsEditorViewModel ViewModel => (CustomTextSlotsEditorViewModel)DataContext;
        public CustomTextSlotsEditor(CustomTextSlotsEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}