using DiscordRPC;
using EnvDTE;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using Button = DiscordRPC.Button;

namespace VisualStudioDiscordRPC.Shared
{
    public class RichPresenceWrapper
    {
        public enum TimerMode
        {
            Disabled,
            File,
            Project,
            Solution
        }

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

        private TimerMode _workTimerMode;

        public TimerMode WorkTimerMode
        {
            get => _workTimerMode;
            set
            {
                _workTimerMode = value;
                _presence.Timestamps = _workTimerMode == TimerMode.Disabled ? null : Timestamps.Now;
            }
        }

        private bool _gitLinkVisible;
        public bool GitLinkVisible
        {
            get => _gitLinkVisible;
            set
            {
                _gitLinkVisible = value;
                _presence.Buttons = _gitLinkVisible ? new[] { _repositoryButton } : null;
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

        private readonly Button _repositoryButton = new Button
        {
            Label = "Repository"
        };

        private ExtensionAsset _documentAsset;
        private Document _document;
        public Document Document
        {
            get => _document;
            set
            {
                if (_document == value)
                {
                    return;
                }

                if (WasTimerWorkSpaceChanged(value))
                {
                    _presence.Timestamps = Timestamps.Now;
                }

                _solutionName = Path.GetFileNameWithoutExtension(_dte.Solution.FullName);

                if (value != default)
                {
                    string extension = Path.GetExtension(value.Name).ToLower();

                    _documentAsset = 
                        ExtensionAssets.GetAsset(asset => asset.Extensions.Contains(extension)) 
                        ?? ExtensionAsset.Default;

                    if (!_gitLinkVisible)
                    {
                        _presence.Buttons = null;
                    }

                    if (_document?.DTE.Solution != value.DTE.Solution)
                    {
                        string solutionPath = _dte.Solution.FullName;

                        if (!string.IsNullOrEmpty(solutionPath))
                        {
                            string repositoryName = Path.GetDirectoryName(solutionPath);
                            if (Repository.IsValid(repositoryName))
                            {
                                _repositoryButton.Url = new Repository(repositoryName).Network.Remotes.First().Url;
                            }
                            else
                            {
                                _presence.Buttons = null;
                            }
                        }
                    }
                }

                _document = value;
            }
        }

        public LocalizationFile Localization { get; set; }
        public IAssetMap<ExtensionAsset> ExtensionAssets { get; set; }

        private readonly DiscordRpcClient _client;
        private readonly RichPresence _presence;

        private readonly Dictionary<string, string> _versions = new Dictionary<string, string>
        {
            { "16", "2019" },
            { "17", "2022" }
        };

        public RichPresenceWrapper(DiscordRpcClient discordRpcClient)
        {
            _client = discordRpcClient;
            _presence = new RichPresence
            {
                Assets = new Assets()
            };

            TitleText = Text.FileName;
            SubTitleText = Text.SolutionName;

            WorkTimerMode = TimerMode.Solution;

            LargeIcon = Icon.FileExtension;
            SmallIcon = Icon.VisualStudioVersion;
        }

        private bool WasTimerWorkSpaceChanged(Document newDocument)
        {
            if (newDocument == null)
            {
                return false;
            }

            switch (WorkTimerMode)
            {
                case TimerMode.Disabled:
                    return false;
                case TimerMode.File:
                    return _document != newDocument;
                case TimerMode.Project:
                    return _dte.ActiveWindow?.Project != newDocument.ActiveWindow?.Project;
                case TimerMode.Solution:
                    return false;
            }

            return false;
        }

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
