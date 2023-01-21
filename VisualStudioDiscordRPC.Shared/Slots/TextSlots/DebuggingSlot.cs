using EnvDTE;
using EnvDTE80;
using System.Linq;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class DebuggingSlot : TextSlot
    {
        private enum DebuggingMode
        {
            None,
            StayOnBreakpoint,
            Debugging
        }

        private readonly DTE2 _dte;
        private readonly DebuggerEvents _debuggerEvents;

        private DebuggingMode _debuggingMode;
        private string _functionName;

        public DebuggingSlot(DTE2 dte) 
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _dte = dte;
            _debuggerEvents = _dte.Events.DebuggerEvents;
        }

        public override void Enable()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _debuggerEvents.OnEnterRunMode += OnEnterRunMode;
            _debuggerEvents.OnEnterDesignMode += OnEnterDesignMode;
            _debuggerEvents.OnEnterBreakMode += OnEnterBreakMode;

            _debuggerEvents.OnContextChanged += OnContextChanged;
        }

        public override void Disable()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _debuggerEvents.OnEnterRunMode -= OnEnterRunMode;
            _debuggerEvents.OnEnterDesignMode -= OnEnterDesignMode;
            _debuggerEvents.OnEnterBreakMode -= OnEnterBreakMode;

            _debuggerEvents.OnContextChanged -= OnContextChanged;
        }

        private void OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
        {
            _debuggingMode = DebuggingMode.StayOnBreakpoint;

            Update();
        }

        private void OnEnterDesignMode(dbgEventReason reason)
        {
            _debuggingMode = DebuggingMode.None;

            Update();
        }

        private void OnEnterRunMode(dbgEventReason reason)
        {
            _debuggingMode = DebuggingMode.Debugging;

            Update();
        }

        private void OnContextChanged(Process newProcess, Program newProgram, Thread newThread, StackFrame newStackFrame)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (newStackFrame == null)
            {
                return;
            }

            // Get only function name (without assembly).
            _functionName = newStackFrame.FunctionName.Split('.').Last();
            
            Update();
        }

        protected override string GetData()
        {
            switch (_debuggingMode)
            {
                case DebuggingMode.None: return "Not debugging";
                case DebuggingMode.StayOnBreakpoint: return $"Debugging {_functionName}";
                case DebuggingMode.Debugging: return $"Debugging...";
                default: return string.Empty;
            }
        }
    }
}
