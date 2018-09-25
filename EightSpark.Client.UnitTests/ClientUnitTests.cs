using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace EightSpark.Client.UnitTests
{
    [TestFixture]
    public class ClientUnitTests
    {
        private string _apikey = "apikey";
        private string _tag = "tag";
        private string _ruleResult = "rule result";

        [Test]
        public void CallRuleWithCache()
        {
            var httpWrapperMock = new Mock<IEightSparkHttpWrapper>();
            var cacheMock = new Mock<ICache>();

            cacheMock.Setup(c => c.Get(string.Format("https://rules.eightspark.com/{0}/rules/{1}", _apikey, _tag)))
                .Returns(_ruleResult);

            var client = new EightSparkClient(_apikey, cacheMock.Object, httpWrapperMock.Object);

            var result = client.GetRule(_tag, "default value");

            Assert.AreEqual(_ruleResult, result);
            httpWrapperMock.Verify(r => r.CallEightSparkApi(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void CallRuleWithNoCache()
        {
            var httpWrapperMock = new Mock<IEightSparkHttpWrapper>();
            var cacheMock = new Mock<ICache>();

            var url = string.Format("https://rules.eightspark.com/{0}/rules/{1}", _apikey, _tag);
            cacheMock.Setup(c => c.Get(url)).Returns((string)null);
            httpWrapperMock.Setup(h => h.CallEightSparkApi(url)).Returns(new RuleValue() { Result = _ruleResult});

            var client = new EightSparkClient(_apikey, cacheMock.Object, httpWrapperMock.Object);

            var result = client.GetRule(_tag, "default value");

            Assert.AreEqual(_ruleResult, result);
            cacheMock.Verify(r => r.Set(url, It.Is<RuleValue>(e => e.Result == _ruleResult)), Times.Once);
        }

        [Test]
        public void CallRuleWithException()
        {
            var httpWrapperMock = new Mock<IEightSparkHttpWrapper>();
            var cacheMock = new Mock<ICache>();

            var url = string.Format("https://rules.eightspark.com/{0}/rules/{1}", _apikey, _tag);
            cacheMock.Setup(c => c.Get(url)).Returns((string)null);
            httpWrapperMock.Setup(h => h.CallEightSparkApi(url)).Throws(new Exception());

            var client = new EightSparkClient(_apikey, cacheMock.Object, httpWrapperMock.Object);

            var result = client.GetRule(_tag, "default value");

            Assert.AreEqual("default value", result);
        }
    }
}
