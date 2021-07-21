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
        public PlaygroundViewModel()
        {
            SimulateProgress = new RelayCommand(_ => DoSimulateProgress(), _ => CanRunProgressBarTask);
            GoButton = new RelayCommand(_ => DoAddStar(), _ => CanRunProgressBarTask);
        }


        private int _progress;
        private bool _canRunProgressBarTask = true;
        private RelayCommand _simulateProgress;
        private string _twoWayTextBox;
        private RelayCommand _goButton;

        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanRunProgressBarTask
        {
            get => _canRunProgressBarTask;
            set
            {
                _canRunProgressBarTask = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand SimulateProgress
        {
            get => _simulateProgress;
            set
            {
                _simulateProgress = value;
                NotifyPropertyChanged();
            }
        }

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

        public RelayCommand GoButton
        {
            get => _goButton;
            set
            {
                _goButton = value;
                NotifyPropertyChanged();
            }
        }

    }
}
