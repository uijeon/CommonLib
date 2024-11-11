using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Jeon.ViewModelFramework.Interface;
using Jeon.ViewModelFramework.Managers;
using Jeon.ViewModelFramework.ViewModels;

namespace Jeon.ViewModelFramework.Views
{
	/// <summary>
	/// *.xaml 에서 View ViewModel 을 등록 할 수 있도록 하는 사용자 컨트롤
	/// MainWindow 에서 ModuleDescription 에 객체(BaseControlViewModel, Type)를 등록 할 수 있음
	/// </summary>
	[ContentProperty("Modules")]
	public class WindowRoot : Control, ISupportInitialize, IDisposable
	{
		#region Dependency Properties 
		public static readonly DependencyProperty MainModuleProperty;
		#endregion //Dependency Properties

		public IModulesManager ModulesManager { get; private set; }
		public ModuleDescriptionCollection Modules { get; private set; }
		public BaseMainControlViewModel MainModule { get { return (BaseMainControlViewModel)GetValue(MainModuleProperty); } private set { SetValue(MainModuleProperty, value); } }


		static WindowRoot()
		{
			Type ownerType = typeof(WindowRoot);
			MainModuleProperty = DependencyProperty.Register("MainModule", typeof(BaseMainControlViewModel), ownerType, new PropertyMetadata(null));
		}
		public WindowRoot()
		{
			DefaultStyleKey = typeof(WindowRoot);

			Modules = new ModuleDescriptionCollection();
			ModulesManager = new ModulesManager(new ViewsManager(Modules), Modules);
		}

		public void Dispose()
		{
			if (ModulesManager != null) ModulesManager = null;
			if (Modules != null) { Modules.Clear(); Modules = null; }
			if (MainModule != null) { MainModule.Dispose(); MainModule = null; }
		}


		public override void BeginInit()
		{
			base.BeginInit();
		}

		public override void EndInit()
		{
			base.EndInit();

			Type mainModuleType = Modules.GetMainModuleType();
			MainModule = mainModuleType == null ? null : (BaseMainControlViewModel)ModulesManager.CreateModule(mainModuleType, null, null);
		}
	}
}
