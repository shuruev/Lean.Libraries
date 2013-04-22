using System;
using System.Configuration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lean.Configuration.Tests
{
	[TestClass]
	public class ConfigParserTest
	{
		#region General tests

		[TestMethod]
		public void Cannot_Be_Created_With_Invalid_Arguments()
		{
			Action action;

			action = () => new ConfigParser(null);
			action.ShouldThrow<ArgumentNullException>()
				.And.ParamName.Should().Be("reader");

			action = () => new ConfigParser(new FakeConfigReader());
			action.ShouldNotThrow();
		}

		[TestMethod]
		public void Can_Detect_Empty_Values()
		{
			var parser = new ConfigParser(new FakeConfigReader());

			parser.IsEmpty(null).Should().BeTrue();
			parser.IsEmpty(String.Empty).Should().BeFalse();
			parser.IsEmpty("   ").Should().BeFalse();
			parser.IsEmpty("name").Should().BeFalse();
		}

		[TestMethod]
		public void Cannot_Get_Empty_Values()
		{
			Action action;
			var parser = new ConfigParser(new NullConfigReader());

			action = () => parser.Get<string>("name");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'name' is not found.");

			action = () => parser.Get<int>("name");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'name' is not found.");
		}

		[TestMethod]
		public void Can_Get_Empty_Values_With_Default()
		{
			var parser = new ConfigParser(new NullConfigReader());

			parser.Get<string>("name", null).Should().BeNull();
			parser.Get("name", String.Empty).Should().BeEmpty();
			parser.Get("name", "default").Should().Be("default");

			parser.Get("name", 0).Should().Be(0);
			parser.Get("name", 1).Should().Be(1);
		}

		[TestMethod]
		public void Cannot_Parse_Values_Of_Unkown_Type()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			action = () => parser.Get<ConfigParser>("name");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Cannot read configuration value of unknown type ConfigParser.");
		}

		#endregion

		#region Parsing values of different types

		[TestMethod]
		public void Can_Parse_String_Values()
		{
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<string>("text").Should().Be("text");
			parser.Get<string>(String.Empty).Should().BeEmpty();
		}

		[TestMethod]
		public void Can_Parse_Boolean_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<bool>("true").Should().BeTrue();
			parser.Get<bool>("True").Should().BeTrue();
			parser.Get<bool>("TRUE").Should().BeTrue();
			parser.Get<bool>("TrUe").Should().BeTrue();

			parser.Get<bool>("false").Should().BeFalse();
			parser.Get<bool>("False").Should().BeFalse();
			parser.Get<bool>("FALSE").Should().BeFalse();
			parser.Get<bool>("FaLsE").Should().BeFalse();

			action = () => parser.Get<bool>("yes");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'yes' should contain value of type Boolean.");
		}

		[TestMethod]
		public void Can_Parse_Byte_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<byte>("25").Should().Be(25);

			action = () => parser.Get<byte>("-38");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '-38' should contain value of type Byte.");

			action = () => parser.Get<byte>("1000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '1000' should contain value of type Byte.");

			action = () => parser.Get<byte>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Byte.");
		}

		[TestMethod]
		public void Can_Parse_Short_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<short>("25").Should().Be(25);
			parser.Get<short>("-38").Should().Be(-38);
			parser.Get<short>("1000").Should().Be(1000);

			action = () => parser.Get<short>("100000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '100000' should contain value of type Int16.");

			action = () => parser.Get<short>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Int16.");
		}

		[TestMethod]
		public void Can_Parse_Integer_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<int>("25").Should().Be(25);
			parser.Get<int>("-38").Should().Be(-38);
			parser.Get<int>("1000").Should().Be(1000);
			parser.Get<int>("100000").Should().Be(100000);

			action = () => parser.Get<int>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Int32.");

			action = () => parser.Get<int>("10000000000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '10000000000' should contain value of type Int32.");
		}

		[TestMethod]
		public void Can_Parse_Long_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<long>("25").Should().Be(25);
			parser.Get<long>("-38").Should().Be(-38);
			parser.Get<long>("1000").Should().Be(1000);
			parser.Get<long>("100000").Should().Be(100000);
			parser.Get<long>("10000000000").Should().Be(10000000000);

			action = () => parser.Get<long>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Int64.");

			action = () => parser.Get<long>("10000000000000000000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '10000000000000000000' should contain value of type Int64.");
		}

		[TestMethod]
		public void Can_Parse_TimeSpan_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<TimeSpan>("15:20:57").Should().Be(TimeSpan.Parse("15:20:57"));
			parser.Get<TimeSpan>("25:20:57").Should().Be(TimeSpan.Parse("25:20:57"));
			parser.Get<TimeSpan>("1:15:20:57").Should().Be(TimeSpan.Parse("1:15:20:57"));

			action = () => parser.Get<TimeSpan>("unknown");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'unknown' should contain value of type TimeSpan.");
		}

		[TestMethod]
		public void Can_Parse_DateTime_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<DateTime>("1983-05-25").Should().Be(new DateTime(1983, 05, 25));
			parser.Get<DateTime>("2000-11-21 15:20:57").Should().Be(new DateTime(2000, 11, 21, 15, 20, 57));
			parser.Get<DateTime>("2000/11/21 15:20:57").Should().Be(new DateTime(2000, 11, 21, 15, 20, 57));

			action = () => parser.Get<DateTime>("unknown");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'unknown' should contain value of type DateTime.");
		}

		[TestMethod]
		public void Can_Parse_Guid_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			Guid id = new Guid("2DC0B86E-3E16-445A-88B8-C39EFF611331");
			parser.Get<Guid>("{2DC0B86E-3E16-445A-88B8-C39EFF611331}").Should().Be(id);
			parser.Get<Guid>("2DC0B86E-3E16-445A-88B8-C39EFF611331").Should().Be(id);
			parser.Get<Guid>("2dc0b86e-3e16-445a-88b8-c39eff611331").Should().Be(id);
			parser.Get<Guid>("2dc0b86e3e16445a88b8c39eff611331").Should().Be(id);

			action = () => parser.Get<Guid>("unknown");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'unknown' should contain value of type Guid.");
		}

		[TestMethod]
		public void Can_Parse_Enum_Values()
		{
			Action action;
			var parser = new ConfigParser(new FakeConfigReader());

			parser.Get<FakeEnum>("Zero").Should().Be(FakeEnum.Zero);
			parser.Get<FakeEnum>("one").Should().Be(FakeEnum.One);
			parser.Get<FakeEnum>("TWO").Should().Be(FakeEnum.Two);
			parser.Get<FakeEnum>("0").Should().Be(FakeEnum.Zero);
			parser.Get<FakeEnum>("1").Should().Be(FakeEnum.One);
			parser.Get<FakeEnum>("2").Should().Be(FakeEnum.Two);
			parser.Get<FakeEnum>("3").Should().NotBe(FakeEnum.Zero)
				.And.Should().NotBe(FakeEnum.One)
				.And.Should().NotBe(FakeEnum.Two);

			action = () => parser.Get<FakeEnum>("unknown");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'unknown' should contain value of type FakeEnum.");
		}

		#endregion
	}
}
