using System;
using System.ComponentModel;
using System.Threading;
using System.Xml.Serialization;

namespace Jeon.ViewModelFramework.BaseNotify
{
	/// <summary>
	/// DataContext Change Event Argument
	/// </summary>
	public class SourceEventArgs : EventArgs
	{
		public object Source { get; set; }
		public SourceEventArgs(object source)
			: base()
		{
			this.Source = source;
		}
	}

	/// <summary>
	/// PropertyChanged Delegate
	/// SetValue 사용 시 전달 된 Action 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	public delegate void RaisePropertyChangedDelegate<T>(T oldValue, T newValue);

	/// <summary>
	/// Dispose 추상객체
	/// </summary>
	public abstract class Disposable : ObservableObject
	{
		bool disposed = false;
		public Disposable()
		{
			InitializeCommands();
		}
		public bool Disposed { get { return disposed; } }
		/// <summary>
		/// 객체 Dispose 이후 이벤트
		/// </summary>
		public event EventHandler AfterDispose;
		/// <summary>
		/// Command 초기화
		/// </summary>
		protected virtual void InitializeCommands() { }
		/// <summary>
		/// 객체 Dispose
		/// </summary>
		protected virtual void DisposeManaged() { }
		protected virtual void DisposeUnmanaged() { }
		protected override void DoDispose(bool disposing)
		{
			if (Disposed) return;
			disposed = true;
			if (disposed)
				DisposeManaged();
			DisposeUnmanaged();
			base.DoDispose(disposing);
			RaiseAfterDispose();
		}
		void RaiseAfterDispose()
		{
			if (AfterDispose != null)
				AfterDispose(this, EventArgs.Empty);
			AfterDispose = null;
		}
	}
	/// <summary>
	/// ViewModel 사용시 PropertyChanged 를 간편하게 사용 할 수 있도록 해주는 추상객체
	/// </summary>
	public abstract class BindableAndDisposable : Disposable
	{
		/// <summary>
		/// The ThreadBarrier's captured SynchronizationContext
		/// </summary>
		private readonly SynchronizationContext _syncContext = AsyncOperationManager.SynchronizationContext;

		bool disposeSignal;
		[XmlIgnore]
		public bool DisposeSignal
		{
			get { return disposeSignal; }
			private set { SetValue<bool>("DisposeSignal", ref disposeSignal, value); }
		}
		/// <summary>
		/// 속성값 Binding 설정
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="field"></param>
		/// <param name="newValue"></param>
		protected void SetValue<T>(string propertyName, ref T field, T newValue)
		{
			SetValue<T>(propertyName, ref field, newValue, false, null);
		}
		/// <summary>
		/// 속성값 Binding 설정(PropertyChanged Delegate 전달)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="field"></param>
		/// <param name="newValue"></param>
		/// <param name="raiseChangedDelegate">PropertyChanged Delegate</param>
		protected void SetValue<T>(string propertyName, ref T field, T newValue, RaisePropertyChangedDelegate<T> raiseChangedDelegate)
		{
			SetValue<T>(propertyName, ref field, newValue, false, raiseChangedDelegate);
		}
		/// <summary>
		/// 속성값 Binding 설정(동일값 PropertyChanged 발생유무)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="field"></param>
		/// <param name="newValue"></param>
		/// <param name="disposeOldValue">oldValue Dispose 여부</param>
		/// <param name="isEquals">oldValue = newValue 동일한 값 PropertyChanged 이벤트 발생 여부</param>
		/// <param name="raiseChangedDelegate">PropertyChanged Delegate</param>
		protected void SetValue<T>(string propertyName, ref T field, T newValue, bool disposeOldValue, bool isEquals, RaisePropertyChangedDelegate<T> raiseChangedDelegate = null)
		{
			SetValue<T>(propertyName, ref field, newValue, disposeOldValue, raiseChangedDelegate, isEquals);
		}

		/// <summary>
		/// 현재값과 발생값이 동일한 경우 Changed Event 가 발생하지 않는다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="field"></param>
		/// <param name="newValue"></param>
		/// <param name="disposeOldValue">oldValue Dispose 여부</param>
		/// <param name="raiseChangedDelegate">PropertyChanged Delegate</param>
		private void SetValue<T>(string propertyName, ref T field, T newValue, bool disposeOldValue, RaisePropertyChangedDelegate<T> raiseChangedDelegate)
		{
			if (Equals(field, newValue)) return;
			T oldValue = field;
			field = newValue;

			if (_syncContext == null)
				PostCallback<T>(propertyName, newValue, disposeOldValue, raiseChangedDelegate, oldValue);
			else
			{
				_syncContext.Send(delegate {
					PostCallback<T>(propertyName, newValue, disposeOldValue, raiseChangedDelegate, oldValue);
				}, null);
			}
		}

		/// <summary>
		/// 현재값과 들어온 값이 같은 경우에도 이벤트를 발생하게 한다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="field"></param>
		/// <param name="newValue"></param>
		/// <param name="disposeOldValue"></param>
		/// <param name="raiseChangedDelegate"></param>
		/// <param name="isEquals">OldValue.Equals(NewValue) = true 일 경우에도 raiseProperty 이벤트를 발생 시킨다.</param>
		private void SetValue<T>(string propertyName, ref T field, T newValue, bool disposeOldValue, RaisePropertyChangedDelegate<T> raiseChangedDelegate, bool isEquals)
		{
			if (!isEquals && Equals(field, newValue)) return;
			T oldValue = field;
			field = newValue;

			if (_syncContext == null)
				PostCallback<T>(propertyName, newValue, disposeOldValue, raiseChangedDelegate, oldValue);
			else
			{
				_syncContext.Send(delegate {
					PostCallback<T>(propertyName, newValue, disposeOldValue, raiseChangedDelegate, oldValue);
				}, null);
			}
		}

		/// <summary>
		/// SetValue Delegate CallBack 및 OldValue Dispose
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="newValue"></param>
		/// <param name="disposeOldValue"></param>
		/// <param name="raiseChangedDelegate"></param>
		/// <param name="oldValue"></param>
		private void PostCallback<T>(string propertyName, T newValue, bool disposeOldValue, RaisePropertyChangedDelegate<T> raiseChangedDelegate, T oldValue)
		{
			NotifyPropertyChanged(propertyName);
			if (raiseChangedDelegate != null)
				raiseChangedDelegate(oldValue, newValue);
			if (!disposeOldValue) return;
			IDisposable disposableOldValue = oldValue as IDisposable;
			if (disposableOldValue != null)
				disposableOldValue.Dispose();
		}
		protected override void DisposeManaged()
		{
			DisposeSignal = true;
			DisposeSignal = false;
			base.DisposeManaged();
		}
	}
}
