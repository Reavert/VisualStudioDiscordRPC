using DiscordRPC;
using System.Threading;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots;
using VisualStudioDiscordRPC.Shared.Slots.AssetSlots;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;
using VisualStudioDiscordRPC.Shared.Slots.TimerSlots;
using VisualStudioDiscordRPC.Shared.Updaters;
using VisualStudioDiscordRPC.Shared.Updaters.Base;

namespace VisualStudioDiscordRPC.Shared
{
    public class DiscordRpcController
    {
        private DiscordRpcClient _discordRpcClient;

        private LocalizationService<LocalizationFile> _localizationService;

        private RichPresence _sharedRichPresence;
        public static object _richPresenceSync = new object();

        private Thread _sendingRichPresenceDataThread;
        private bool _sendingThreadCancellation;
        private int _sendDataMillisecondsTimeout;

        private bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;

                _largeIconUpdater.Enabled = value;
                _smallIconUpdater.Enabled = value;
                _detailsUpdater.Enabled = value;
                _stateUpdater.Enabled = value;

                if (_discordRpcClient.IsInitialized)
                {
                    if (_enabled) 
                    {
                        UpdateAllUpdaters();
                    }
                    else
                    {
                        _discordRpcClient.ClearPresence();
                    }
                }
            }
        }

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

        private TimerUpdater _timerUpdater;
        public TimerSlot TimerSlot
        {
            get => (TimerSlot) _timerUpdater.Slot;
            set => SetSlot(_timerUpdater, value);
        }

        public DiscordRpcController(int updateMillisecondsTimeout) 
        {
            _discordRpcClient = new DiscordRpcClient(Settings.Default.ApplicationID);
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();

            _sharedRichPresence = new RichPresence();
            _sharedRichPresence.Assets = new Assets();

            _largeIconUpdater = new LargeIconUpdater(_sharedRichPresence);
            _smallIconUpdater = new SmallIconUpdater(_sharedRichPresence);

            _detailsUpdater = new DetailsUpdater(_sharedRichPresence);
            _stateUpdater = new StateUpdater(_sharedRichPresence);

            _timerUpdater = new TimerUpdater(_sharedRichPresence);

            _sendingRichPresenceDataThread = new Thread(SendRichPresenceData);
            _sendDataMillisecondsTimeout = updateMillisecondsTimeout;
        }

        public void Initialize()
        {
            _localizationService.LocalizationChanged += OnLocalizationChanged;

            _sendingRichPresenceDataThread.Start();

            lock (_richPresenceSync)
            {
                _discordRpcClient.Initialize();
            }
        }

        public void Dispose()
        {
            _localizationService.LocalizationChanged -= OnLocalizationChanged;

            lock (_richPresenceSync)
            {
                _sendingThreadCancellation = true;
                _discordRpcClient.Dispose();
            }
        }

        private void OnLocalizationChanged()
        {
            UpdateAllUpdaters();
        }

        private void UpdateAllUpdaters()
        {
            _largeIconUpdater.Slot?.Update();
            _smallIconUpdater.Slot?.Update();
            _stateUpdater.Slot?.Update();
            _detailsUpdater.Slot?.Update();
        }

        private void SetSlot<T>(BaseUpdater<T> updater, AbstractSlot<T> slot)
        {
            updater.Slot = slot;

            if (_enabled)
            {
                updater.Enabled = true;
            }

            if (_discordRpcClient.IsInitialized)
            {
                slot.Update();
            }
        }

        private void SendRichPresenceData()
        {
            while (true)
            {
                Thread.Sleep(_sendDataMillisecondsTimeout);

                lock (_richPresenceSync)
                {
                    if (_sendingThreadCancellation)
                    {
                        return;
                    }

                    if (_enabled)
                    {
                        _discordRpcClient.SetPresence(_sharedRichPresence);
                    }
                }
            }
        }
    }
}
