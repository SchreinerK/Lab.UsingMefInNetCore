using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Lab.UsingMefInNetCore {

	public class BootstrapperBase {

		private readonly bool _useApplication;
		private bool _isInitialized;
		private static readonly DependencyObject DependencyObject = new DependencyObject();

		/// <summary>The application.</summary>
		protected Application Application { get; set; }

		/// <summary>Creates an instance of the bootstrapper.</summary>
		/// <param name="useApplication">Set this to false when hosting Caliburn.Micro inside and Office or WinForms application. The default is true.</param>
		protected BootstrapperBase(bool useApplication = true) {
			_useApplication = useApplication;
			Initialize();//TODO workaround why? 
		}

		/// <summary>Initialize the framework.</summary>
		public void Initialize() {
			if (_isInitialized) return;
			_isInitialized = true;
//			PlatformProvider.Current = (IPlatformProvider)new XamlPlatformProvider();
//			Func<Assembly, IEnumerable<Type>> baseExtractTypes = AssemblySourceCache.ExtractTypes;
//			AssemblySourceCache.ExtractTypes = (Func<Assembly, IEnumerable<Type>>)(assembly => baseExtractTypes(assembly).Union<Type>(((IEnumerable<Type>)assembly.GetExportedTypes()).Where<Type>((Func<Type, bool>)(t => typeof(UIElement).IsAssignableFrom(t)))));
//			AssemblySource.Instance.Refresh();
			if (DesignerProperties.GetIsInDesignMode(DependencyObject)) {
				try { StartDesignTime(); }
				catch {
					_isInitialized = false;
					throw;
				}
			}
			else
				StartRuntime();
		}

		/// <summary>
		/// Called by the bootstrapper's constructor at design time to start the framework.
		/// </summary>
		protected virtual void StartDesignTime() {
//			AssemblySource.Instance.Clear();
//			AssemblySource.Instance.AddRange(this.SelectAssemblies());
			Configure();
//			IoC.GetInstance = new Func<Type, string, object>(this.GetInstance);
//			IoC.GetAllInstances = new Func<Type, IEnumerable<object>>(this.GetAllInstances);
//			IoC.BuildUp = new System.Action<object>(this.BuildUp);
		}

		/// <summary>
		/// Called by the bootstrapper's constructor at runtime to start the framework.
		/// </summary>
		protected virtual void StartRuntime() {
//			EventAggregator.HandlerResultProcessing = (Action<object, object>) ((target, result) => {
//				Task task = result as Task;
//				if (task != null)
//					result = (object) new IResult[1] {
//						(IResult) task.AsResult()
//					};
//				IEnumerable<IResult> results = result as IEnumerable<IResult>;
//				if (results == null)
//					return;
//				object view = (target as IViewAware)?.GetView((object) null);
//				CoroutineExecutionContext context = new CoroutineExecutionContext() {
//					Target = target,
//					View = view
//				};
//				Coroutine.BeginExecute(results.GetEnumerator(), context, (EventHandler<ResultCompletionEventArgs>) null);
//			});
//			AssemblySourceCache.Install();
//			AssemblySource.Instance.AddRange(SelectAssemblies());
			if (_useApplication) {
				Application = Application.Current;
				PrepareApplication();
			}

			Configure();
//			IoC.GetInstance = new Func<Type, string, object>(GetInstance);
//			IoC.GetAllInstances = new Func<Type, IEnumerable<object>>(GetAllInstances);
//			IoC.BuildUp = new Action<object>(BuildUp);
		}

		/// <summary>
		/// Provides an opportunity to hook into the application object.
		/// </summary>
		protected virtual void PrepareApplication() {
			Application.Startup += OnStartup;
			Application.DispatcherUnhandledException += OnUnhandledException;
			Application.Exit += OnExit;
		}

		/// <summary>
		/// Override to configure the framework and setup your IoC container.
		/// </summary>
		protected virtual void Configure() { }

		/// <summary>
		/// Override to tell the framework where to find assemblies to inspect for views, etc.
		/// </summary>
		/// <returns>A list of assemblies to inspect.</returns>
		protected virtual IEnumerable<Assembly> SelectAssemblies() {
			return (IEnumerable<Assembly>) new Assembly[1] {
				GetType().Assembly
			};
		}

		/// <summary>
		/// Override this to provide an IoC specific implementation.
		/// </summary>
		/// <param name="service">The service to locate.</param>
		/// <param name="key">The key to locate.</param>
		/// <returns>The located service.</returns>
		protected virtual object GetInstance(Type service, string key) {
//			if (service == typeof(IWindowManager)) service = typeof(WindowManager);
			return Activator.CreateInstance(service);
		}

		/// <summary>
		/// Override this to provide an IoC specific implementation
		/// </summary>
		/// <param name="service">The service to locate.</param>
		/// <returns>The located services.</returns>
		protected virtual IEnumerable<object> GetAllInstances(Type service) {
			return (IEnumerable<object>) new object[1] {
				Activator.CreateInstance(service)
			};
		}

		/// <summary>
		/// Override this to provide an IoC specific implementation.
		/// </summary>
		/// <param name="instance">The instance to perform injection on.</param>
		protected virtual void BuildUp(object instance) { }

		/// <summary>
		/// Override this to add custom behavior to execute after the application starts.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The args.</param>
		protected virtual void OnStartup(object sender, StartupEventArgs e) { }

		/// <summary>Override this to add custom behavior on exit.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event args.</param>
		protected virtual void OnExit(object sender, EventArgs e) { }

		/// <summary>
		/// Override this to add custom behavior for unhandled exceptions.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event args.</param>
		protected virtual void OnUnhandledException(
			object sender,
			DispatcherUnhandledExceptionEventArgs e) { }

		/// <summary>
		/// Locates the view model, locates the associate view, binds them and shows it as the root view.
		/// </summary>
		/// <param name="viewModelType">The view model type.</param>
		/// <param name="settings">The optional window settings.</param>
		protected void DisplayRootViewFor(Type viewModelType, IDictionary<string, object> settings = null) {
//			IoC.Get<IWindowManager>((string) null).ShowWindow(IoC.GetInstance(viewModelType, (string) null), (object) null, settings);
		}

		/// <summary>
		/// Locates the view model, locates the associate view, binds them and shows it as the root view.
		/// </summary>
		/// <typeparam name="TViewModel">The view model type.</typeparam>
		/// <param name="settings">The optional window settings.</param>
		protected void DisplayRootViewFor<TViewModel>(IDictionary<string, object> settings = null) {
			DisplayRootViewFor(typeof(TViewModel), settings);
		}

	}

}
