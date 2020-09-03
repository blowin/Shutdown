namespace Shutdown.GUI.Data.Unit
{
    public sealed class ActionUnit : PriorityUnitBase<ActionUnit>
    {
        public static readonly ActionUnit Restart = new ActionUnit(2, "Перезагрузить", ShutdownAction.Restart);
        public static readonly ActionUnit Shutdown = new ActionUnit(1, "Выключить", ShutdownAction.Shutdown);

        public ShutdownAction Action { get; }

        private ActionUnit(int priority, string name, ShutdownAction action) 
            : base(name, priority)
        {
            Action = action;
        }
    }
}