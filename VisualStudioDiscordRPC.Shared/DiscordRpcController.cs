using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Threading;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots;
using VisualStudioDiscordRPC.Shared.Updaters;
using VisualStudioDiscordRPC.Shared.Updaters.Base;

namespace VisualStudioDiscordRPC.Shared
{
    public class DiscordRpcController
    {
        private static readonly RichPresence HiddenRichPresence;

        private readonly DiscordRpcClient _discordRpcClient;
        
        private bool _isDirty;
        private readonly object _dirtyFlagSync = new object();

        private readonly LocalizationService<LocalizationFile> _localizationService;

        private readonly RichPresence _sharedRichPresence;
        private readonly object _richPresenceSync = new object();

        private readonly Thread _sendingRichPresenceDataThread;
        private readonly int _sendDataMillisecondsTimeout;
        private bool _sendingThreadCancellation;

        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;

                foreach (BaseUpdater updater in Updaters)
                {
                    updater.Enabled = value;
                }

                if (_discordRpcClient.IsInitialized)
                {
                    if (_enabled) 
                    {
                        RefreshAll();
                    }
                    else
                    {
                        _discordRpcClient.ClearPresence();
                    }
                }
            }
        }

        private bool _secret;
        public bool Secret
        {
            get => _secret;
            set
            {
                _secret = value;
                _isDirty = true;
            }
        }

        private Dictionary<Type, BaseUpdater> _updaters = new Dictionary<Type, BaseUpdater>();
        public IEnumerable<BaseUpdater> Updaters => _updaters.Values;

        static DiscordRpcController()
        {
            HiddenRichPresence = new RichPresence
            {
                Assets = new Assets()
                {
                    LargeImageKey = "secret",
                    LargeImageText = "This solution is hidden"
                },
                Details = "This solution is hidden"
            };
        }

        public DiscordRpcController(int updateMillisecondsTimeout) 
        {
            _discordRpcClient = new DiscordRpcClient(Settings.Default.ApplicationID)
            {
                SkipIdenticalPresence = false
            };

            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();

            _sharedRichPresence = new RichPresence
            {
                Assets = new Assets(),
                Buttons = new Button[]
                {
                    new Button(),
                    new Button()
                }
            };
            
            RegisterUpdater(new LargeIconUpdater(_sharedRichPresence));
            RegisterUpdater(new SmallIconUpdater(_sharedRichPresence));

            RegisterUpdater(new DetailsUpdater(_sharedRichPresence));
            RegisterUpdater(new StateUpdater(_sharedRichPresence));

            RegisterUpdater(new TimerUpdater(_sharedRichPresence));

            RegisterUpdater(new FirstButtonUpdater(_sharedRichPresence));
            RegisterUpdater(new SecondButtonUpdater(_sharedRichPresence));

            _sendingRichPresenceDataThread = new Thread(SendRichPresenceData);
            _sendDataMillisecondsTimeout = updateMillisecondsTimeout;
        }

        public void Initialize()
        {
            _localizationService.LocalizationChanged += OnLocalizationChanged;

            lock (_richPresenceSync)
            {
                _discordRpcClient.Initialize();
            }

            _sendingRichPresenceDataThread.Start();

            foreach (BaseUpdater updater in Updaters)
            {
                updater.Changed += OnUpdaterChanged;
            }
        }

        public void Dispose()
        {
            _localizationService.LocalizationChanged -= OnLocalizationChanged;

            foreach (BaseUpdater updater in Updaters)
            {
                updater.Changed -= OnUpdaterChanged;
            }

            lock (_richPresenceSync)
            {
                _sendingThreadCancellation = true;
                _discordRpcClient.Dispose();
            }
        }

        public void SetSlot<TUpdater>(BaseSlot slot) where TUpdater : BaseUpdater
        {
            if (slot == null)
            {
                return;
            }

            Type updaterType = typeof(TUpdater);
            BaseUpdater updater = _updaters[updaterType];

            updater.BaseSlot = slot;

            if (_enabled)
            {
                updater.Enabled = true;
            }

            if (_discordRpcClient.IsInitialized)
            {
                slot.Update();
            }
        }

        public BaseSlot GetSlotOfUpdater<TUpdater>()
        {
            BaseUpdater updater = _updaters[typeof(TUpdater)];
            return updater.BaseSlot;
        }

        private void RegisterUpdater(BaseUpdater updater)
        {
            _updaters.Add(updater.GetType(), updater);
        }

        private void OnLocalizationChanged()
        {
            RefreshAll();
        }

        private void OnUpdaterChanged()
        {
            lock (_dirtyFlagSync)
            {
                _isDirty = true;
            }
        }

        public void RefreshAll()
        {
            foreach (BaseUpdater updater in Updaters)
            {
                updater.BaseSlot?.Update();
            }

            lock (_dirtyFlagSync)
            {
                _isDirty = true;
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

                    lock (_dirtyFlagSync)
                    {
                        if (_enabled && _isDirty)
                        {
                            if (_secret)
                            {
                                _discordRpcClient.SetPresence(HiddenRichPresence);
                            }
                            else
                            {
                                _discordRpcClient.SetPresence(_sharedRichPresence);
                            }

                            _isDirty = false;
                        }
                    }
                }
            }
        }
    }
}
