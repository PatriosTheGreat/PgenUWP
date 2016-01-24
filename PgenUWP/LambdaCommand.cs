﻿using System;
using System.Windows.Input;

namespace PgenUWP
{
    public class LambdaCommand : ICommand
    {
        public LambdaCommand(Action<object> execute)
            : this(execute, _ => true)
        {
        }

        public LambdaCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged()
        {
            var canExecutedChanges = CanExecuteChanged;
            canExecutedChanges?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
    }
}
