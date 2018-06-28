using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace UICalculator.CalculatorUtils
{
    public class DelegateCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action execute) : this(execute, null)
        {

        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
