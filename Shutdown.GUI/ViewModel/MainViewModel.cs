using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Shutdown.GUI.Data;
using Shutdown.GUI.Data.Unit;
using Shutdown.GUI.Util;

namespace Shutdown.GUI.ViewModel
{
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<TimeUnit> TimeTypes { get; }

        public ObservableCollection<ActionUnit> ActionTypes { get; }

        public TimeUnit SelectTimeUnit { get; set; }
        public ActionUnit SelectActionUnit { get; set; }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        public double? TimeValue { get; set; }

        public string Msg { get; set; }

        public bool IsForceCloseWithoutSave { get; set; }

        public MainViewModel(UnitUtil unitUtil)
        {
            StopCommand = new ActionCommand(o => ShutdownLib.stop());

            StartCommand = new DelegateCommand(
                o => TryCreateOption(out _),
                _ =>
                {
                    TryCreateOption(out var option);
                    ShutdownLib.exec(option);
                });
            
            TimeTypes = new ObservableCollection<TimeUnit>(unitUtil.GetAllPriorityValues<TimeUnit>());
            
            SelectTimeUnit = TimeTypes.First(d => ReferenceEquals(d, TimeUnit.Minutes));

            ActionTypes = new ObservableCollection<ActionUnit>(unitUtil.GetAllPriorityValues<ActionUnit>());
            
            SelectActionUnit = ActionTypes.First();

            TimeValue = 100;
        }

        private bool TryCreateOption(out ShutdownOption option)
        {
            option = default;
            if (!TimeValue.HasValue)
                return false;

            if (SelectActionUnit == null)
                return false;

            if (SelectTimeUnit == null)
                return false;

            var message = MessageModule.createNone();
            if (Msg != null)
            {
                var result = MessageModule.createMsg(Msg);
                if (result.IsError)
                    return false;

                message = result.ResultValue;
            }

            var time = SelectTimeUnit.AsTimeSpan(TimeValue.Value);

            option = new ShutdownOption(
                SelectActionUnit.Action,
                TimeInSecond.NewTime(time),
                IsForceCloseWithoutSave ? CloseType.CloseWindowsWithoutSave : CloseType.SoftWindowsClose, 
                message
            );

            return true;
        }
    }
}