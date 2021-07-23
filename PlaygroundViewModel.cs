using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

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

        public bool CanSendToHistory { get; set; } = true;

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
            TwoWayTextBox = null;
        }

        /// <summary>
        /// Clears all textboxes and listview items
        /// </summary>
        public RelayCommand ClearButton { get; set; }
    }
}
