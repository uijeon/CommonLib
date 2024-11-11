using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jeon.ViewModelFramework.RelayCommands
{
	/// <summary>
	/// Action Command
	/// </summary>
	public class ActionCommandBase : ICommand
	{
		bool allowExecute = true;
		public ActionCommandBase(Action<object> action, object owner)
		{
			Action = action;
			Owner = owner;
		}
		public bool AllowExecute
		{
			get { return allowExecute; }
			protected set
			{
				allowExecute = value;
				RaiseAllowExecuteChanged();
			}
		}
		public Action<object> Action { get; private set; }
		protected object Owner { get; private set; }
		public event EventHandler CanExecuteChanged;
		public bool CanExecute(object parameter) { return AllowExecute; }
		public void Execute(object parameter)
		{
			if (Action != null)
				Action(parameter);
		}
		void RaiseAllowExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, EventArgs.Empty);
		}
	}

	/// <summary>
	/// Owner 의 PropertyChanged 를 발생 시켜 Content 를 Update 하는 Command
	/// </summary>
	public class ExtendedActionCommandBase : ActionCommandBase
	{
		string allowExecutePropertyName;
		PropertyInfo allowExecuteProperty;
		public ExtendedActionCommandBase(Action<object> action, INotifyPropertyChanged owner, string allowExecuteProperty)
			: base(action, owner)
		{
			this.allowExecutePropertyName = allowExecuteProperty;
			if (Owner != null)
			{
				this.allowExecuteProperty = Owner.GetType().GetProperty(this.allowExecutePropertyName, BindingFlags.Public | BindingFlags.Instance);
				if (this.allowExecuteProperty == null)
					throw new ArgumentOutOfRangeException("allowExecuteProperty");
				((INotifyPropertyChanged)Owner).PropertyChanged += OnOwnerPropertyChanged;
			}
		}
		protected virtual void UpdateAllowExecute()
		{
			AllowExecute = Owner == null ? true : (bool)this.allowExecuteProperty.GetValue(Owner, null);
		}
		void OnOwnerPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.allowExecutePropertyName)
				UpdateAllowExecute();
		}
	}

	/// <summary>
	/// MainViewModel 에서 ModelCollection 의 View(Content) 를 형상화 하는 Command
	/// </summary>
	public class ExtendedActionCommand : ExtendedActionCommandBase
	{
		Func<object, bool> allowExecuteCallback;
		object id;
		public ExtendedActionCommand(Action<object> action, INotifyPropertyChanged owner, string allowExecuteProperty, Func<object, bool> allowExecuteCallback, object id)
			: base(action, owner, allowExecuteProperty)
		{
			this.allowExecuteCallback = allowExecuteCallback;
			this.id = id;
			UpdateAllowExecute();
		}
		protected override void UpdateAllowExecute()
		{
			AllowExecute = this.allowExecuteCallback == null ? true : this.allowExecuteCallback(this.id);
		}
	}
}
