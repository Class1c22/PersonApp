using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PersonApp.Models;

namespace PersonApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime? _birthDate;
        private Person _person;
        private bool _isProcessing;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanProceed)); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanProceed)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanProceed)); }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set { _birthDate = value?.Date; OnPropertyChanged(); OnPropertyChanged(nameof(CanProceed)); }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            private set { _isProcessing = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanProceed)); }
        }

        public bool CanProceed =>
            !IsProcessing &&
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(Email) &&
            BirthDate != null;

        public Person Person
        {
            get => _person;
            private set { _person = value; OnPropertyChanged(); }
        }

        public ICommand ProceedCommand => new RelayCommand(async _ => await ProceedAsync(), _ => CanProceed);

        private async Task ProceedAsync()
        {
            IsProcessing = true;
            await Task.Delay(4000);

            if (!Email.Contains("@") || BirthDate > DateTime.Now || BirthDate < new DateTime(1900, 1, 1))
            {
                MessageBox.Show("Некоректна дата народження. Будь ласка, введіть правильну дату.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                IsProcessing = false;
                return;
            }

            Person = new Person(FirstName, LastName, Email, BirthDate.Value);
            IsProcessing = false;

            if (Person.IsBirthday)
            {
                MessageBox.Show($"З днем народження, {FirstName}! Бажаємо вам щастя та здоров'я!", "Вітання", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}