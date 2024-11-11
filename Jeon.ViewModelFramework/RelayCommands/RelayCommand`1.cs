using System;
using System.Windows.Input;

namespace Jeon.ViewModelFramework.RelayCommands
{
	public class RelayCommand<T> : ICommand
	{
		private readonly Predicate<T> _canExecute;

		private readonly Action<T> _execute;

		public RelayCommand(Action<T> execute) : this(execute, null)
		{
			this._execute = execute;
		}

		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			this._execute = execute;
			this._canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return this._canExecute == null || this._canExecute((T)parameter);
		}

		public void Execute(object parameter)
		{
			this._execute((T)parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}
	}
}
