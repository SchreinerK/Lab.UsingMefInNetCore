using System.Composition;
using Lab.UsingMefInNetCore.Interfaces;

namespace PluginA {

	[Export(typeof(IPlugin))]
	public class MyPlugin : IPlugin {

		public string Name => "PluginA";

	}
}
