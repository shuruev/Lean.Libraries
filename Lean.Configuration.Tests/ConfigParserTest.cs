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
		public void Can_Detect_Empty_Values()
		{
			var reader = new FakeConfigReader();

			reader.IsEmpty(null).Should().BeTrue();
			reader.IsEmpty(String.Empty).Should().BeFalse();
			reader.IsEmpty("   ").Should().BeFalse();
			reader.IsEmpty("name").Should().BeFalse();
		}

		[TestMethod]
		public void Cannot_Get_Empty_Values()
		{
			Action action;
			var reader = new NullConfigReader();

			action = () => reader.Get<string>("name");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'name' is not found.");

			action = () => reader.Get<int>("name");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'name' is not found.");
		}

		[TestMethod]
		public void Can_Get_Empty_Values_With_Default()
		{
			var reader = new NullConfigReader();

			reader.Get<string>("name", null).Should().BeNull();
			reader.Get("name", String.Empty).Should().BeEmpty();
			reader.Get("name", "default").Should().Be("default");

			reader.Get("name", 0).Should().Be(0);
			reader.Get("name", 1).Should().Be(1);
		}

		[TestMethod]
		public void Cannot_Parse_Values_Of_Unkown_Type()
		{
			Action action;
			var reader = new FakeConfigReader();

			action = () => reader.Get<NullConfigReader>("name");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Cannot read configuration value of unknown type NullConfigReader.");
		}

		#endregion

		#region Parsing values of different types

		[TestMethod]
		public void Can_Parse_String_Values()
		{
			var reader = new FakeConfigReader();

			reader.Get<string>("text").Should().Be("text");
			reader.Get<string>(String.Empty).Should().BeEmpty();
		}

		[TestMethod]
		public void Can_Parse_Boolean_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<bool>("true").Should().BeTrue();
			reader.Get<bool>("True").Should().BeTrue();
			reader.Get<bool>("TRUE").Should().BeTrue();
			reader.Get<bool>("TrUe").Should().BeTrue();

			reader.Get<bool>("false").Should().BeFalse();
			reader.Get<bool>("False").Should().BeFalse();
			reader.Get<bool>("FALSE").Should().BeFalse();
			reader.Get<bool>("FaLsE").Should().BeFalse();

			action = () => reader.Get<bool>("yes");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'yes' should contain value of type Boolean.");
		}

		[TestMethod]
		public void Can_Parse_Byte_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<byte>("25").Should().Be(25);

			action = () => reader.Get<byte>("-38");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '-38' should contain value of type Byte.");

			action = () => reader.Get<byte>("1000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '1000' should contain value of type Byte.");

			action = () => reader.Get<byte>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Byte.");
		}

		[TestMethod]
		public void Can_Parse_Short_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<short>("25").Should().Be(25);
			reader.Get<short>("-38").Should().Be(-38);
			reader.Get<short>("1000").Should().Be(1000);

			action = () => reader.Get<short>("100000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '100000' should contain value of type Int16.");

			action = () => reader.Get<short>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Int16.");
		}

		[TestMethod]
		public void Can_Parse_Integer_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<int>("25").Should().Be(25);
			reader.Get<int>("-38").Should().Be(-38);
			reader.Get<int>("1000").Should().Be(1000);
			reader.Get<int>("100000").Should().Be(100000);

			action = () => reader.Get<int>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Int32.");

			action = () => reader.Get<int>("10000000000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '10000000000' should contain value of type Int32.");
		}

		[TestMethod]
		public void Can_Parse_Long_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<long>("25").Should().Be(25);
			reader.Get<long>("-38").Should().Be(-38);
			reader.Get<long>("1000").Should().Be(1000);
			reader.Get<long>("100000").Should().Be(100000);
			reader.Get<long>("10000000000").Should().Be(10000000000);

			action = () => reader.Get<long>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Int64.");

			action = () => reader.Get<long>("10000000000000000000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '10000000000000000000' should contain value of type Int64.");
		}

		[TestMethod]
		public void Can_Parse_Float_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<float>("2.5").Should().Be(2.5f);
			reader.Get<float>("-0.2").Should().Be(-0.2f);
			reader.Get<float>("1000").Should().Be(1000f);
			reader.Get<float>("3.4e-12").Should().Be(3.4e-12f);

			reader.Get<float>("NaN").Should().Be(Single.NaN);
			reader.Get<float>("-Infinity").Should().Be(Single.NegativeInfinity);
			reader.Get<float>("Infinity").Should().Be(Single.PositiveInfinity);

			action = () => reader.Get<float>("+Infinity");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '+Infinity' should contain value of type Single.");

			reader.Get<float>("0").Should().Be(0);
			reader.Get<float>("+0").Should().Be(0);
			reader.Get<float>("-0").Should().Be(0);

			action = () => reader.Get<float>("1e39");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '1e39' should contain value of type Single.");
		}

		[TestMethod]
		public void Can_Parse_Double_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<double>("2.5").Should().Be(2.5d);
			reader.Get<double>("-0.2").Should().Be(-0.2d);
			reader.Get<double>("1000").Should().Be(1000d);
			reader.Get<double>("3.4e-12").Should().Be(3.4e-12d);

			reader.Get<double>("NaN").Should().Be(Double.NaN);
			reader.Get<double>("-Infinity").Should().Be(Double.NegativeInfinity);
			reader.Get<double>("Infinity").Should().Be(Double.PositiveInfinity);

			action = () => reader.Get<double>("+Infinity");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '+Infinity' should contain value of type Double.");

			reader.Get<double>("0").Should().Be(0);
			reader.Get<double>("+0").Should().Be(0);
			reader.Get<double>("-0").Should().Be(0);

			action = () => reader.Get<double>("1e309");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '1e309' should contain value of type Double.");
		}

		[TestMethod]
		public void Can_Parse_Decimal_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<decimal>("2.5").Should().Be(2.5m);
			reader.Get<decimal>("-0.2").Should().Be(-0.2m);
			reader.Get<decimal>("1000").Should().Be(1000m);
			reader.Get<decimal>("0.12345678901234567890").Should().Be(0.12345678901234567890m);

			action = () => reader.Get<decimal>("NaN");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'NaN' should contain value of type Decimal.");

			action = () => reader.Get<decimal>("Infinity");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'Infinity' should contain value of type Decimal.");

			reader.Get<decimal>("0").Should().Be(0);
			reader.Get<decimal>("+0").Should().Be(0);
			reader.Get<decimal>("-0").Should().Be(0);

			action = () => reader.Get<decimal>("100000000000000000000000000000");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter '100000000000000000000000000000' should contain value of type Decimal.");
		}

		[TestMethod]
		public void Can_Parse_TimeSpan_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<TimeSpan>("15:20:57").Should().Be(TimeSpan.Parse("15:20:57"));
			reader.Get<TimeSpan>("25:20:57").Should().Be(TimeSpan.Parse("25:20:57"));
			reader.Get<TimeSpan>("1:15:20:57").Should().Be(TimeSpan.Parse("1:15:20:57"));

			action = () => reader.Get<TimeSpan>("unknown");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'unknown' should contain value of type TimeSpan.");
		}

		[TestMethod]
		public void Can_Parse_DateTime_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<DateTime>("1983-05-25").Should().Be(new DateTime(1983, 05, 25));
			reader.Get<DateTime>("2000-11-21 15:20:57").Should().Be(new DateTime(2000, 11, 21, 15, 20, 57));
			reader.Get<DateTime>("2000/11/21 15:20:57").Should().Be(new DateTime(2000, 11, 21, 15, 20, 57));

			action = () => reader.Get<DateTime>("unknown");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'unknown' should contain value of type DateTime.");
		}

		[TestMethod]
		public void Can_Parse_Guid_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			Guid id = new Guid("2DC0B86E-3E16-445A-88B8-C39EFF611331");
			reader.Get<Guid>("{2DC0B86E-3E16-445A-88B8-C39EFF611331}").Should().Be(id);
			reader.Get<Guid>("2DC0B86E-3E16-445A-88B8-C39EFF611331").Should().Be(id);
			reader.Get<Guid>("2dc0b86e-3e16-445a-88b8-c39eff611331").Should().Be(id);
			reader.Get<Guid>("2dc0b86e3e16445a88b8c39eff611331").Should().Be(id);

			action = () => reader.Get<Guid>("unknown");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'unknown' should contain value of type Guid.");
		}

		[TestMethod]
		public void Can_Parse_Enum_Values()
		{
			Action action;
			var reader = new FakeConfigReader();

			reader.Get<FakeEnum>("Zero").Should().Be(FakeEnum.Zero);
			reader.Get<FakeEnum>("one").Should().Be(FakeEnum.One);
			reader.Get<FakeEnum>("TWO").Should().Be(FakeEnum.Two);
			reader.Get<FakeEnum>("0").Should().Be(FakeEnum.Zero);
			reader.Get<FakeEnum>("1").Should().Be(FakeEnum.One);
			reader.Get<FakeEnum>("2").Should().Be(FakeEnum.Two);
			reader.Get<FakeEnum>("3").Should().NotBe(FakeEnum.Zero)
				.And.Should().NotBe(FakeEnum.One)
				.And.Should().NotBe(FakeEnum.Two);

			action = () => reader.Get<FakeEnum>("unknown");
			action.ShouldThrow<ConfigurationException>()
				.WithMessage("Configuration parameter 'unknown' should contain value of type FakeEnum.");
		}

		#endregion
	}
}
