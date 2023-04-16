using VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom;

namespace VisualStudioDiscordRPC.Shared.ViewModels.CustomSlots
{
    public class CustomTextSlotViewModel : ViewModelBase, ICustomSlotViewModel
    {
        private string _customText;
        public string CustomText
        {
            get => _customText;
            set => SetProperty(ref _customText, value);
        }

        public object GetSlotSetting()
        {
            return new CustomizableTextSlotSettings
            {
                CustomText = CustomText
            };
        }

        public void ClearView()
        {
            CustomText = string.Empty;
        }
    }
}
