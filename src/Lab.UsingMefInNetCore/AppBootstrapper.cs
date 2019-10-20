using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Lab.UsingMefInNetCore.Interfaces;

namespace Lab.UsingMefInNetCore {
	public class AppBootstrapper : BootstrapperBase {

		private IEnumerable<IPlugin> _plugins;

		public AppBootstrapper() : base(true) { }

		protected override void Configure() {
			base.Configure();

			var conventions = new ConventionBuilder();
			conventions
				.ForTypesDerivedFrom<IPlugin>()
				.Export<IPlugin>()
				.Shared();

			//			var assemblies = new[] { typeof(MyPlugin).Assembly };
			//			var configuration = new ContainerConfiguration().WithAssemblies(assemblies, conventions);

			var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var configuration = new ContainerConfiguration().WithAssembliesInPath(path, conventions);

			using (var container = configuration.CreateContainer()) {
				_plugins = container.GetExports<IPlugin>();
			}

			foreach (var plugin in _plugins) {
				Debug.WriteLine($"{plugin.Name}");
			}
		}

	}

}
