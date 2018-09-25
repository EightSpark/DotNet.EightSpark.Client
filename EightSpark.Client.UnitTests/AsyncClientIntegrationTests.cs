using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace EightSpark.Client.UnitTests
{
    [TestFixture]
    public class AsyncClientIntegrationTests
    {
        [Test]
        public async Task CallStringRule()
        {
            var client = new EightSparkClient("bfbbce5a-15c1-4d6d-85b9-42ab90e3abbf");

            var result = await client.GetRuleAsync("lightsaber-color", "black");
            Assert.AreEqual("green", result);

            var filterResult = await client.GetRuleAsync("lightsaber-color", "black", "Han-Solo");
            Assert.AreEqual("blaster", filterResult);

            var cachedResult = await client.GetRuleAsync("lightsaber-color", "black");
            Assert.AreEqual("green", cachedResult);

            var filterResultCached = await client.GetRuleAsync("lightsaber-color", "black", "Han-Solo");
            Assert.AreEqual("blaster", filterResultCached);
        }

        [Test]
        public async Task CallIntRule()
        {
            var client = new EightSparkClient("bfbbce5a-15c1-4d6d-85b9-42ab90e3abbf");

            var result = await client.GetRuleAsync("x-wing-squad-size", 99);
            Assert.AreEqual(8, result);

            var filterResult = await client.GetRuleAsync("x-wing-squad-size", 99, "gold");
            Assert.AreEqual(12, filterResult);

            var cachedResult = await client.GetRuleAsync("x-wing-squad-size", 99);
            Assert.AreEqual(8, cachedResult);

            var filterResultCached = await client.GetRuleAsync("x-wing-squad-size", 99, "gold");
            Assert.AreEqual(12, filterResultCached);
        }

        [Test]
        public async Task CallBoolRule()
        {
            var client = new EightSparkClient("bfbbce5a-15c1-4d6d-85b9-42ab90e3abbf");

            var result = await client.GetRuleAsync("mf-hyperdrive", false);
            Assert.AreEqual(true, result);

            var filterResult = await client.GetRuleAsync("mf-hyperdrive", true, "in-times-of-great-need");
            Assert.AreEqual(false, filterResult);

            var cachedResult = await client.GetRuleAsync("mf-hyperdrive", false);
            Assert.AreEqual(true, cachedResult);

            var filterResultCached = await client.GetRuleAsync("mf-hyperdrive", true, "in-times-of-great-need");
            Assert.AreEqual(false, filterResultCached);
        }
    }
}
