using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace EightSpark.Client.UnitTests
{
    [TestFixture]
    public class AsyncClientUnitTests
    {
        private string _apikey = "apikey";
        private string _tag = "tag";
        private string _ruleResult = "rule result";

        [Test]
        public async Task CallRuleWithCache()
        {
            var httpWrapperMock = new Mock<IEightSparkHttpWrapper>();
            var cacheMock = new Mock<ICache>();

            cacheMock.Setup(c => c.Get(string.Format("https://rules.eightspark.com/{0}/rules/{1}", _apikey, _tag)))
                .Returns(_ruleResult);

            var client = new EightSparkClient(_apikey, cacheMock.Object, httpWrapperMock.Object);

            var result = await client.GetRuleAsync(_tag, "default value");

            Assert.AreEqual(_ruleResult, result);
            httpWrapperMock.Verify(r => r.CallEightSparkApiAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task CallRuleWithNoCache()
        {
            var httpWrapperMock = new Mock<IEightSparkHttpWrapper>();
            var cacheMock = new Mock<ICache>();

            var url = string.Format("https://rules.eightspark.com/{0}/rules/{1}", _apikey, _tag);
            cacheMock.Setup(c => c.Get(url)).Returns((string)null);
            httpWrapperMock.Setup(h => h.CallEightSparkApiAsync(url)).Returns(Task.FromResult(new RuleValue() { Result = _ruleResult }));

            var client = new EightSparkClient(_apikey, cacheMock.Object, httpWrapperMock.Object);

            var result = await client.GetRuleAsync(_tag, "default value");

            Assert.AreEqual(_ruleResult, result);
            cacheMock.Verify(r => r.Set(url, It.Is<RuleValue>(e => e.Result == _ruleResult)), Times.Once);
        }

        [Test]
        public async Task CallRuleWithException()
        {
            var httpWrapperMock = new Mock<IEightSparkHttpWrapper>();
            var cacheMock = new Mock<ICache>();

            var url = string.Format("https://rules.eightspark.com/{0}/rules/{1}", _apikey, _tag);
            cacheMock.Setup(c => c.Get(url)).Returns((string)null);
            httpWrapperMock.Setup(h => h.CallEightSparkApiAsync(url)).Throws(new Exception());

            var client = new EightSparkClient(_apikey, cacheMock.Object, httpWrapperMock.Object);

            var result = await client.GetRuleAsync(_tag, "default value");

            Assert.AreEqual("default value", result);
        }
    }
}