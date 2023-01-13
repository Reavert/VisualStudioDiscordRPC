using System.Collections.Generic;
using System;
using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Data;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseButtonUpdater : BaseDiscordRpcUpdater<ButtonInfo>
    {
        private readonly static Button _firstButton = new Button();
        private readonly static Button _secondButton = new Button();

        private readonly static List<Button> _buttons = new List<Button>();

        protected BaseButtonUpdater(RichPresence richPresence) : base(richPresence)
        { }

        protected void SetButton(int index, ButtonInfo buttonInfo)
        {
            UpdateButtonWithInfo(index, buttonInfo);
            RichPresence.Buttons = (Button[]) _buttons.ToArray().Clone();
        }

        private void UpdateButtonWithInfo(int index, ButtonInfo buttonInfo)
        {
            Button button;
            switch (index)
            {
                case 0: button = _firstButton; break;
                case 1: button = _secondButton; break;
                default: throw new ArgumentOutOfRangeException($"{nameof(index)} must be 0 or 1");
            }

            if (string.IsNullOrEmpty(buttonInfo.Label) || string.IsNullOrEmpty(buttonInfo.Url))
            {
                _buttons.Remove(button);
                return;
            }

            button.Label = buttonInfo.Label;
            button.Url = buttonInfo.Url;

            if (!_buttons.Contains(button))
            {
                _buttons.Add(button);
            }
        }
    }
}
