﻿using EnvDTE;
using EnvDTE80;
using System;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class DebugModeVariable : Variable
    {
        private readonly DTE2 _dte;

        private Debugger _debugger;
        private DebuggerEvents _debuggerEvents;

        public DebugModeVariable(DTE2 dte)
        {
            _dte = dte;   
        }

        public override void Initialize()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _debugger = _dte.Debugger;
            _debuggerEvents = _dte.Events.DebuggerEvents;

            _debuggerEvents.OnEnterBreakMode += OnEnterBreakMode;
            _debuggerEvents.OnEnterDesignMode += OnEnterDesignMode;
            _debuggerEvents.OnEnterRunMode += OnEnterRunMode;
            _debuggerEvents.OnContextChanged += OnContextChanged;
        }

        public override string GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            switch (_debugger.CurrentMode)
            {
                case dbgDebugMode.dbgRunMode: return $"Run Mode";
                case dbgDebugMode.dbgBreakMode: return $"Break Mode";
                case dbgDebugMode.dbgDesignMode: return "Design Mode";               
                default: throw new ArgumentOutOfRangeException(nameof(_debugger.CurrentMode));
            }
        }

        private void OnEnterBreakMode(dbgEventReason Reason, ref dbgExecutionAction ExecutionAction) => 
            RaiseChangedEvent();

        private void OnEnterDesignMode(dbgEventReason Reason) => 
            RaiseChangedEvent();

        private void OnEnterRunMode(dbgEventReason Reason) => 
            RaiseChangedEvent();

        private void OnContextChanged(Process NewProcess, Program NewProgram, Thread NewThread, StackFrame NewStackFrame) => 
            RaiseChangedEvent();
    }
}
