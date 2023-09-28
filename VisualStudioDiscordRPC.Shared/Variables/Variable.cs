﻿using System;

namespace VisualStudioDiscordRPC.Shared.Macros
{
    public abstract class Variable
    {
        public abstract string GetData();
        public event EventHandler<string> Changed;

        protected void RaiseChangedEvent()
        {
            Changed?.Invoke(this, GetData());
        }
    }
}