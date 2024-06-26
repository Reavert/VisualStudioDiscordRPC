﻿using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Threading;
using VisualStudioDiscordRPC.Shared.Services;
using VisualStudioDiscordRPC.Shared.Plugs;
using VisualStudioDiscordRPC.Shared.Nests;
using VisualStudioDiscordRPC.Shared.Nests.Base;

namespace VisualStudioDiscordRPC.Shared
{
    public class DiscordRpcController
    {
        public const string DefaultApplicationId = "914622396630175855";

        private static readonly RichPresence HiddenRichPresence;

        private readonly DiscordRpcClient _discordRpcClient;
        
        private bool _isDirty;
        private readonly object _dirtyFlagSync = new object();

        private readonly LocalizationService _localizationService;

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

                foreach (BaseNest nest in Nests)
                {
                    nest.Enabled = value;
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

        private readonly Dictionary<Type, BaseNest> _nests = new Dictionary<Type, BaseNest>();
        public IEnumerable<BaseNest> Nests => _nests.Values;

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
            var settingsService = ServiceRepository.Default.GetService<SettingsService>();
            var applicationId = settingsService.Read<string>(SettingsKeys.ApplicationID);

            _discordRpcClient = new DiscordRpcClient(applicationId)
            {
                SkipIdenticalPresence = false
            };

            _localizationService = ServiceRepository.Default.GetService<LocalizationService>();

            _sharedRichPresence = new RichPresence
            {
                Assets = new Assets(),
                Buttons = new Button[]
                {
                    new Button(),
                    new Button()
                }
            };
            
            RegisterNest(new LargeIconNest(_sharedRichPresence));
            RegisterNest(new SmallIconNest(_sharedRichPresence));

            RegisterNest(new DetailsNest(_sharedRichPresence));
            RegisterNest(new StateNest(_sharedRichPresence));

            RegisterNest(new TimerNest(_sharedRichPresence));

            RegisterNest(new FirstButtonNest(_sharedRichPresence));
            RegisterNest(new SecondButtonNest(_sharedRichPresence));

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

            foreach (BaseNest nest in Nests)
            {
                nest.Changed += OnNestChanged;
            }
        }

        public void Dispose()
        {
            _localizationService.LocalizationChanged -= OnLocalizationChanged;

            foreach (BaseNest nest in Nests)
            {
                nest.Changed -= OnNestChanged;
            }

            lock (_richPresenceSync)
            {
                _sendingThreadCancellation = true;
                _discordRpcClient.Dispose();
            }
        }

        public void SetPlug<TNest>(BasePlug plug) where TNest : BaseNest
        {
            if (plug == null)
            {
                return;
            }

            Type nestType = typeof(TNest);
            BaseNest nest = _nests[nestType];

            nest.BasePlug = plug;

            if (_enabled)
            {
                nest.Enabled = true;
            }

            if (_discordRpcClient.IsInitialized)
            {
                plug.Update();
            }
        }

        public BasePlug GetPlugOfNest<TNest>()
        {
            BaseNest nest = _nests[typeof(TNest)];
            return nest.BasePlug;
        }

        private void RegisterNest(BaseNest nest)
        {
            _nests.Add(nest.GetType(), nest);
        }

        private void OnLocalizationChanged()
        {
            RefreshAll();
        }

        private void OnNestChanged()
        {
            lock (_dirtyFlagSync)
            {
                _isDirty = true;
            }
        }

        public void RefreshAll()
        {
            foreach (BaseNest nest in Nests)
            {
                nest.BasePlug?.Update();
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
