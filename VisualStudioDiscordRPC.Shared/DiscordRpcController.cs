using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Slots;
using VisualStudioDiscordRPC.Shared.Updaters;

namespace VisualStudioDiscordRPC.Shared
{
    public class DiscordRpcController
    {
        private DiscordRpcClient _discordRpcClient;

        private LargeIconUpdater _largeIconUpdater;
        public AssetSlot LargeIconSlot
        {
            get => (AssetSlot) _largeIconUpdater.Slot;
            set => SetSlot(_largeIconUpdater, value);
        }

        private SmallIconUpdater _smallIconUpdater;
        public AssetSlot SmallIconSlot
        {
            get => (AssetSlot)_smallIconUpdater.Slot;
            set => SetSlot(_smallIconUpdater, value);
        }

        private DetailsUpdater _detailsUpdater;
        public TextSlot DetailsSlot
        {
            get => (TextSlot) _detailsUpdater.Slot;
            set => SetSlot(_detailsUpdater, value);
        }

        private StateUpdater _stateUpdater;
        public TextSlot StateSlot
        {
            get => (TextSlot) _stateUpdater.Slot;
            set => SetSlot(_stateUpdater, value);
        }

        public DiscordRpcController() 
        {
            _discordRpcClient = new DiscordRpcClient(Settings.Default.ApplicationID);

            _largeIconUpdater = new LargeIconUpdater(_discordRpcClient);
            _smallIconUpdater = new SmallIconUpdater(_discordRpcClient);

            _detailsUpdater = new DetailsUpdater(_discordRpcClient);
            _stateUpdater = new StateUpdater(_discordRpcClient);
        }

        public void InitilizeRpcClient()
        {
            _discordRpcClient.Initialize();
        }

        public void DisposeRpcClient()
        {
            _discordRpcClient.Dispose();
        }

        private void SetSlot<T>(BaseUpdater<T> updater, AbstractSlot<T> slot)
        {
            updater.Slot = slot;

            if (_discordRpcClient.IsInitialized)
            {
                slot.UpdateWithLastData();
            }
        }
    }
}
