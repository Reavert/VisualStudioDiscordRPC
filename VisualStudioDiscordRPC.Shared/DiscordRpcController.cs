using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                foreach (BaseUpdater updater in Updaters)
                {
                    updater.Enabled = value;
                }

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

        private Dictionary<Type, BaseUpdater> _updaters = new Dictionary<Type, BaseUpdater>();
        public IEnumerable<BaseUpdater> Updaters => _updaters.Values;

        public DiscordRpcController(int updateMillisecondsTimeout) 
        {
            _discordRpcClient = new DiscordRpcClient(Settings.Default.ApplicationID);
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
            UpdateAllUpdaters();
        }

        private void UpdateAllUpdaters()
        {
            foreach (BaseUpdater updater in Updaters)
            {
                updater.BaseSlot?.Update();
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
