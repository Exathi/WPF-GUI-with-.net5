using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
            SimulateProgress = new RelayCommand(_ => DoSimulateProgress(), _ => CanRunProgressBarTask);
            GoButton = new RelayCommand(_ => DoAddStar(), _ => true);
            SendButton = new RelayCommand(_ => DoAddHistory(), _ => CanSendToHistory);
            ClearButton = new RelayCommand(_ => DoClear(), _ => true);
            ActionList = new ObservableCollection<ActionHistory>();
            ListCopy = new RelayCommand(_ => DoCopyActionList(), _ => true);
        }

        /// <summary>
        /// Properties that require NotifyPropertyChanged(); to update view
        /// </summary>
        private int _progress;
        private string _twoWayTextBox;
        private string _historyTextBox;

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

        public ObservableCollection<ActionHistory> ActionList { get; set; }
        private void DoAddActionToList(DateTime dateTime, string description)
        {
            ActionList.Add(new ActionHistory(dateTime, description));
        }

        public RelayCommand ListCopy { get; set; }
        private void DoCopyActionList()
        {
            StringBuilder sb = new();
            foreach (ActionHistory actionHistory in ActionList)
            {
                _ = sb.AppendLine($"{actionHistory.DateTime}\t{actionHistory.Description}");
            }
            Clipboard.SetText(sb.ToString());
        }
        /// <summary>
        /// Check if task is able to run with progress bar
        /// </summary>
        public bool CanRunProgressBarTask { get; set; } = true;

        /// <summary>
        /// Creates an async task that uses the progress bar
        /// </summary>
        public RelayCommand SimulateProgress { get; set; }

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
        public RelayCommand GoButton { get; set; }
        public string TwoWayTextBox
        {
            get => _twoWayTextBox;
            set
            {
                _twoWayTextBox = value;
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
        public RelayCommand SendButton { get; set; }

        public bool CanSendToHistory { get; set; }

        public string HistoryTextBox
        {
            get => _historyTextBox;
            set
            {
                _historyTextBox = value;
                NotifyPropertyChanged();
            }
        }

        private void DoAddHistory()
        {
            HistoryTextBox += TwoWayTextBox;
            TwoWayTextBox = string.Empty;
        }

        /// <summary>
        /// Clears all textboxes and listview items
        /// </summary>
        public RelayCommand ClearButton { get; set; }

        private void DoClear()
        {
            HistoryTextBox = string.Empty;
            TwoWayTextBox = string.Empty;
            ActionList.Clear();
        }
    }
}
