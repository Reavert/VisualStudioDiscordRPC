﻿using System;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class StaticTextSource : IObservableString
    {
        public string Text => _text;

        public event Action Changed;

        private readonly string _text;

        public StaticTextSource(string text)
        {
            _text = text;
            Changed?.Invoke();
        }
    }
}
