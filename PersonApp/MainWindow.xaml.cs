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
        private string _errorMessage;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); UpdateCanProceed(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); UpdateCanProceed(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); UpdateCanProceed(); }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(); UpdateCanProceed(); }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set { _isProcessing = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanProceed)); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public bool CanProceed => !IsProcessing &&
                               !string.IsNullOrWhiteSpace(FirstName) &&
                               !string.IsNullOrWhiteSpace(LastName) &&
                               !string.IsNullOrWhiteSpace(Email) &&
                               BirthDate != null;

        public Person Person
        {
            get => _person;
            set { _person = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsPersonCreated)); }
        }

        public bool IsPersonCreated => Person != null;

        public ICommand ProceedCommand => new RelayCommand(async _ => await ProceedAsync(), _ => CanProceed);

        private async Task ProceedAsync()
        {
            IsProcessing = true;
            ErrorMessage = string.Empty;

            try
            {
                await Task.Delay(1000); // Simulate processing

                Person = new Person(FirstName, LastName, Email, BirthDate.Value);

                if (Person.IsBirthday)
                {
                    MessageBox.Show($"З днем народження, {FirstName}!",
                                  "Вітання",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
            catch (FutureBirthDateException)
            {
                ErrorMessage = "Дата народження не може бути в майбутньому";
            }
            catch (DistantPastBirthDateException)
            {
                ErrorMessage = "Дата народження занадто давня (максимум 150 років)";
            }
            catch (InvalidEmailException)
            {
                ErrorMessage = "Невірний формат електронної пошти";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Помилка: {ex.Message}";
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private void UpdateCanProceed()
        {
            OnPropertyChanged(nameof(CanProceed));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}