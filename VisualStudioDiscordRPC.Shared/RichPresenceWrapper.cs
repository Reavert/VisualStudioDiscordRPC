using DiscordRPC;
using EnvDTE;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.Localization.Models;

namespace VisualStudioDiscordRPC.Shared
{
    public class RichPresenceWrapper
    {
        public enum Icon
        {
            None,
            VisualStudioVersion,
            FileExtension
        }

        public enum Text
        {
            None,
            VisualStudioVersion,
            FileExtension,
            ProjectName,
            SolutionName,
            FileName
        }

        public Text TitleText { get; set; }
        public Text SubTitleText { get; set; }

        private bool _workTimerVisible;
        public bool WorkTimerVisible
        {
            get => _workTimerVisible;
            set
            {
                if (_workTimerVisible ^ value)
                {
                    switch (value)
                    {
                        case true: _presence.Timestamps = Timestamps.Now; break;
                        case false: _presence.Timestamps = null; break;
                    }
                }
                _workTimerVisible = value;
            }
        }

        public Icon LargeIcon { get; set; }
        public Icon SmallIcon { get; set; }

        private string _version;
        private string _solutionName;
        private DTE _dte;
        public DTE Dte
        {
            get => _dte;
            set
            {
                _dte = value;
                _version = GetVersion(_dte);
            }
        }

        private ExtensionAsset _documentAsset;
        private Document _document;
        public Document Document
        {
            get => _document;
            set
            {
                _document = value;
                _solutionName = Path.GetFileNameWithoutExtension(Path.GetFileName(_dte.Solution.FullName));

                if (WorkTimerVisible)
                {
                    _presence.Timestamps = Timestamps.Now;
                }

                if (_document == null)
                {
                    return;
                }

                var comparer = new ExtensionAssetComparer
                {
                    RequiredExtension = Path.GetExtension(_document.Name)
                };
                _documentAsset = ExtensionAssets.GetAsset(comparer) ?? ExtensionAsset.Default;
            }
        }

        public LocalizationFile Localization { get; set; }
        public IAssetMap<ExtensionAsset> ExtensionAssets { get; set; }

        private readonly DiscordRpcClient _client;
        private readonly RichPresence _presence;

        public RichPresenceWrapper(DiscordRpcClient discordRpcClient)
        {
            _client = discordRpcClient;
            _presence = new RichPresence
            {
                Assets = new Assets()
            };

            TitleText = Text.FileName;
            SubTitleText = Text.ProjectName;

            WorkTimerVisible = true;

            LargeIcon = Icon.FileExtension;
            SmallIcon = Icon.VisualStudioVersion;
        }

        private readonly Dictionary<string, string> _versions = new Dictionary<string, string>
        {
            { "16", "2019" },
            { "17", "2022" }
        };

        private string GetVersion(DTE dte)
        {
            string versionMajor = dte.Version.Split('.')[0];
            return _versions[versionMajor];
        }

        private string GetText(Text text)
        {
            switch (text)
            {
                case Text.None:
                    return string.Empty;

                case Text.ProjectName:
                    if (Document != null)
                    {
                        return string.Format(
                            ConstantStrings.ActiveProjectFormat,
                            Localization.Project,
                            Document.ActiveWindow?.Project.Name);
                    }
                    return Localization.NoActiveProject;
                    
                case Text.FileName:
                    if (Document != null)
                    {
                        return string.Format(
                            ConstantStrings.ActiveFileFormat,
                            Localization.File,
                            Document.Name);
                    }
                    return Localization.NoActiveFile;

                case Text.SolutionName:
                    if (Document != null)
                    {
                        return string.Format(
                            ConstantStrings.ActiveProjectFormat,
                            Localization.Project,
                            _solutionName);
                    }

                    return Localization.NoActiveProject;
                case Text.FileExtension: 
                    return _documentAsset?.Name;

                case Text.VisualStudioVersion: 
                    return string.Format(ConstantStrings.VisualStudioVersion, _version);

                default: 
                    return string.Empty;
            }
        }

        private string GetAssetKey(Icon icon)
        {
            switch (icon)
            {
                case Icon.None: 
                    return string.Empty;

                case Icon.VisualStudioVersion: 
                    return string.Format(ConstantStrings.VisualStudioVersionAssetKey, _version);

                case Icon.FileExtension:
                    return _documentAsset?.Key;

                default:
                    return string.Empty;
            }
        }

        private string GetAssetText(Icon icon)
        {
            switch (icon)
            {
                case Icon.None: 
                    return string.Empty;

                case Icon.VisualStudioVersion:
                    return string.Format(ConstantStrings.VisualStudioVersion, _version);

                case Icon.FileExtension:
                    return _documentAsset?.Name;

                default: 
                    return string.Empty;
            }
        }

        public void Update()
        {
            _presence.Details = GetText(TitleText);
            _presence.State = GetText(SubTitleText);

            Icon largeIcon = LargeIcon;
            Icon smallIcon = SmallIcon;

            if (Document == null)
            {
                largeIcon = Icon.VisualStudioVersion;
                smallIcon = Icon.None;
            }

            _presence.Assets.LargeImageKey = GetAssetKey(largeIcon);
            _presence.Assets.SmallImageKey = GetAssetKey(smallIcon);

            _presence.Assets.LargeImageText = GetAssetText(largeIcon);
            _presence.Assets.SmallImageText = GetAssetText(smallIcon);

            _client.SetPresence(_presence);
        }
    }
}
