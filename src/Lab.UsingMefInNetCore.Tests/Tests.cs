using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Lab.UsingMefInNetCore.Tests {
	public class Tests {

		private CompositionHost _container;

		[SetUp]
		public void Setup() {
			var assembly = typeof(Class1).GetTypeInfo().Assembly;

			var rules = new ConventionBuilder();
			rules.ForType<Class2Shared>().Shared();

			var config = new ContainerConfiguration()
				.WithAssemblies(new[] { assembly }, rules);

			_container = config.CreateContainer();
		}

		[TearDown]
		public void TearDown() {
			_container.Dispose();
			_container = null;

		}

		[Test]
		public void Class1Test() {
			var class2A = _container.GetExports<Class1>().First();
			var class2B = _container.GetExports<Class1>().First();
			Assert.That(class2A, Is.Not.SameAs(class2B));
		}

		[Test]
		public void Class2SharedTest() {
			var class2A = _container.GetExports<Class2Shared>().First();
			var class2B = _container.GetExports<Class2Shared>().First();
			Assert.That(class2A, Is.SameAs(class2B));

			var c=new Class1Container();
			_container.SatisfyImports(c);
		}

		[Test]
		public void Class3Test() {
			var class2A = _container.GetExports<Class3>().First();
			var class2B = _container.GetExports<Class3>().First();
			Assert.That(class2A, Is.Not.SameAs(class2B));
		}
	}

	class Class1Container {

		[ImportMany]
		public IEnumerable<Class1> Class1Instances { get; set; }
	}
}