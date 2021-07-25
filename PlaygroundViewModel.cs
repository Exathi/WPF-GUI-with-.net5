using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace GUI_Playground
{
    class PlaygroundViewModel : ViewModelBase
    {
        /// <summary>
        /// Create GUI welcome message and button commands on new
        /// </summary>
        public PlaygroundViewModel()
        {
            WelcomeMessage = "Work In Progress";
            ActionList = new ObservableCollection<ActionHistory>();
            ListCopy = new RelayCommand<object>((commandparameter) => DoCopyActionList(commandparameter), _ => true);
            ListRemove = new RelayCommand<object>((commandparameter) => DoRemoveAction(commandparameter), _ => true);
            SimulateProgress = new RelayCommand<object>(_ => DoSimulateProgress(), _ => CanRunProgressBarTask);
            GoButton = new RelayCommand<object>(_ => DoAddStar(), _ => true);
            SendButton = new RelayCommand<object>(_ => DoAddHistory(), _ => CanSendToHistory);
            ClearButton = new RelayCommand<object>(_ => DoClear(), _ => true);
        }

        /// <summary>
        /// Properties that require NotifyPropertyChanged(); to update view
        /// </summary>
        private int _progress;
        private readonly StringBuilder _twoWayTextBox = new();
        private readonly StringBuilder _historyTextBox = new();

        public string WelcomeMessage { get; set; }

        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Log actions to display on view
        /// </summary>
        public ObservableCollection<ActionHistory> ActionList { get; set; }
        private void DoAddActionToList(DateTime dateTime, string description)
        {
            ActionList.Add(new ActionHistory(dateTime, description));
        }

        /// <summary>
        /// Copy listview items by casting IList to param because System.Windows.Controls.SelectedItemCollection implements IList
        /// </summary>
        public RelayCommand<object> ListCopy { get; set; }
        private static void DoCopyActionList(object param)
        {
            StringBuilder sb = new();

            IList tmpIList = (IList)param;
            List<ActionHistory> tmpList = tmpIList.Cast<ActionHistory>().ToList();
            IOrderedEnumerable<ActionHistory> orderedActionHistory = tmpList.OrderBy(_ => _.Time);

             _ = sb.AppendLine($"TimeStamp\tLog");
            foreach (ActionHistory actionHistory in orderedActionHistory)
            {
                _ = sb.AppendLine($"{actionHistory.Time}\t{actionHistory.Description}");
            }
            Clipboard.SetText(sb.ToString());
        }

        /// <summary>
        /// Remove selected listview items
        /// </summary>
        public RelayCommand<object> ListRemove { get; set; }

        private void DoRemoveAction(object param)
        {
            List<ActionHistory> actionHistories = new();
            foreach (ActionHistory actionHistory in (IList)param)
            {
                actionHistories.Add(actionHistory);
            }
            foreach (ActionHistory actionHistory in actionHistories)
            {
                _ = ActionList.Remove(actionHistory);
            }
        }

        /// <summary>
        /// Check if task is able to run with progress bar
        /// </summary>
        public bool CanRunProgressBarTask { get; set; } = true;

        /// <summary>
        /// Creates an async task that uses the progress bar
        /// </summary>
        public RelayCommand<object> SimulateProgress { get; set; }

        private async void DoSimulateProgress()
        {
            CanRunProgressBarTask = false;
            SimulateProgress.RaiseCanExecuteChanged();

            DoAddActionToList(DateTime.Now, "Start DoSimulateProgress");

            Random randomNumber = new();
            while (Progress < 100)
            {
                int number = randomNumber.Next(1, 25);

                if ((Progress + number) > 100)
                {
                    Progress = 100;
                }
                else
                {
                    Progress += number;
                }

                await Task.Delay(500);
            }

            Progress = 0;
            CanRunProgressBarTask = true;
            SimulateProgress.RaiseCanExecuteChanged();
            DoAddActionToList(DateTime.Now, "End DoSimulateProgress");
            //CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Adds * to a string
        /// </summary>
        public RelayCommand<object> GoButton { get; set; }
        public string TwoWayTextBox
        {
            get => _twoWayTextBox.ToString();
            set
            {
                int len = _twoWayTextBox.Length;
                _ = _twoWayTextBox.Remove(0, len);
                _ = _twoWayTextBox.Append(value);
                NotifyPropertyChanged();
                if (_twoWayTextBox.Length == 0)
                {
                    CanSendToHistory = false;
                    SendButton.RaiseCanExecuteChanged();
                }
                else
                {
                    CanSendToHistory = true;
                    SendButton.RaiseCanExecuteChanged();
                }
            }
        }
        private void DoAddStar()
        {
            TwoWayTextBox += '*';
        }

        /// <summary>
        /// Moves text from TwoWayTextBox to HistoryTextBox
        /// </summary>
        public RelayCommand<object> SendButton { get; set; }

        public bool CanSendToHistory { get; set; }

        public string HistoryTextBox
        {
            get => _historyTextBox.ToString();
            set
            {
                _ = _historyTextBox.Append(value);
                NotifyPropertyChanged();
            }
        }

        private void DoAddHistory()
        {
            HistoryTextBox = TwoWayTextBox;
            TwoWayTextBox = string.Empty;
            DoAddActionToList(DateTime.Now, "Sent to history");
        }

        /// <summary>
        /// Clears all textboxes and listview items
        /// </summary>
        public RelayCommand<object> ClearButton { get; set; }

        private void DoClear()
        {
            //HistoryTextBox = string.Empty;
            _ = _historyTextBox.Clear();
            NotifyPropertyChanged(HistoryTextBox);
            //TwoWayTextBox = string.Empty;
            _ = _twoWayTextBox.Clear();
            NotifyPropertyChanged(TwoWayTextBox);
            ActionList.Clear();
        }
    }
}
