using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EightSpark.Client.UnitTests
{
    [TestFixture]
    public class ClientIntegrationTests
    {
        [Test]
        public void CallStringRule()
        {
            var client = new EightSparkClient("bfbbce5a-15c1-4d6d-85b9-42ab90e3abbf");

            var result = client.GetRule("lightsaber-color", "black");
            Assert.AreEqual("green", result);

            var filterResult = client.GetRule("lightsaber-color", "black", "Han-Solo");
            Assert.AreEqual("blaster", filterResult);

            var cachedResult = client.GetRule("lightsaber-color", "black");
            Assert.AreEqual("green", cachedResult);

            var filterResultCached = client.GetRule("lightsaber-color", "black", "Han-Solo");
            Assert.AreEqual("blaster", filterResultCached);
        }

        [Test]
        public void CallIntRule()
        {
            var client = new EightSparkClient("bfbbce5a-15c1-4d6d-85b9-42ab90e3abbf");

            var result =  client.GetRule("x-wing-squad-size", 99);
            Assert.AreEqual(8, result);

            var filterResult =  client.GetRule("x-wing-squad-size", 99, "gold");
            Assert.AreEqual(12, filterResult);

            var cachedResult =  client.GetRule("x-wing-squad-size", 99);
            Assert.AreEqual(8, cachedResult);

            var filterResultCached =  client.GetRule("x-wing-squad-size", 99, "gold");
            Assert.AreEqual(12, filterResultCached);
        }

        [Test]
        public void CallBoolRule()
        {
            var client = new EightSparkClient("bfbbce5a-15c1-4d6d-85b9-42ab90e3abbf");

            var result = client.GetRule("mf-hyperdrive", false);
            Assert.AreEqual(true, result);

            var filterResult = client.GetRule("mf-hyperdrive", true, "in-times-of-great-need");
            Assert.AreEqual(false, filterResult);

            var cachedResult = client.GetRule("mf-hyperdrive", false);
            Assert.AreEqual(true, cachedResult);

            var filterResultCached = client.GetRule("mf-hyperdrive", true, "in-times-of-great-need");
            Assert.AreEqual(false, filterResultCached);
        }

        [Test]
        public void ErrorCases()
        {
            var client = new EightSparkClient("bfbbce5a-15c1-4d6d-85b9-42ab90e3abbf");

            var result = client.GetRule("does-not-exist", true);
            Assert.AreEqual(true, result);

            client.SetThrowExceptions(true);
            Assert.Throws<Exception>(() => client.GetRule("does-not-exist", true));
        }
    }
}