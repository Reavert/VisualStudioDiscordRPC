using EnvDTE;
using System;

namespace VisualStudioDiscordRPC.Shared.Macros
{
    public class DebugMacro : Macro
    {
        public enum Scope
        {
            Function,
            Program
        }

        private readonly Debugger _debugger;
        private readonly dbgDebugMode _debugMode;
        private readonly Scope _scope;

        public DebugMacro(Debugger debugger, DebuggerEvents events, Scope scope = Scope.Function)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _debugger = debugger;
            events.OnEnterBreakMode += OnEnterBreakMode;
            events.OnEnterDesignMode += OnEnterDesignMode;
            events.OnEnterRunMode += OnEnterRunMode;
            events.OnContextChanged += OnContextChanged;

            _debugMode = debugger.CurrentMode;
            _scope = scope;
        }

        public override string GetData()
        {
            switch (_debugMode)
            {
                case dbgDebugMode.dbgRunMode: return $"Debugging: {GetScopeInfo()}";
                case dbgDebugMode.dbgDesignMode: return "Not debugging";
                case dbgDebugMode.dbgBreakMode: return $"Stay on breakpoint: {GetScopeInfo()}";
                default: throw new ArgumentOutOfRangeException(nameof(_debugMode));
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

        private string GetScopeInfo()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            switch (_scope)
            {
                case Scope.Program: return _debugger.CurrentProgram.Name;
                case Scope.Function: return _debugger.CurrentStackFrame.FunctionName;
                default: throw new ArgumentOutOfRangeException(nameof(_scope));
            }
        }
    }
}
