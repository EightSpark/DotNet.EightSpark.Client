using System;
using System.Threading.Tasks;

namespace EightSpark.Client
{
    public class EightSparkClient : IEightSparkClient
    {
        private readonly string _apiKey;
        private string _baseUrl;
        private bool _throwException;
        private string _domain;
        private readonly ICache _cache;
        private IEightSparkHttpWrapper _httpWrapper;

        public EightSparkClient(string apiKey)
        {
            _apiKey = apiKey;
            _throwException = false;
            _baseUrl = string.Format("https://rules.eightspark.com/{0}/rules", apiKey);
            _cache = new InMemoryCache(true, 1);
            _httpWrapper = new EightSparkHttpWrapper();
        }

        public EightSparkClient(string apiKey, ICache cache, IEightSparkHttpWrapper eightSparkHttpWrapper)
        {
            _apiKey = apiKey;
            _throwException = false;
            _baseUrl = string.Format("https://rules.eightspark.com/{0}/rules", apiKey);
            _cache = cache;
            _httpWrapper = eightSparkHttpWrapper;
        }

        public void SetCacheTime(int minutes)
        {
            _cache.SetCacheTime(minutes);
        }

        public void SetThrowExceptions(bool throwExceptions)
        {
            _throwException = true;
        }

        public void SetDomain(string url)
        {
            // TODO validate this
            _domain = url;
            _baseUrl = string.Format("{0}/{1}/rules", url, _apiKey);
        }

        public bool GetRule(string tag, bool defaultValue, string filter = null)
        {
            try
            {
                return bool.Parse(ActuallyGetTheRule(tag, filter));
            }
            catch (Exception)
            {
                if (_throwException)
                    throw;

                return defaultValue;
            }
        }

        public int GetRule(string tag, int defaultValue, string filter = null)
        {
            try
            {
                return int.Parse(ActuallyGetTheRule(tag, filter));
            }
            catch (Exception)
            {
                if (_throwException)
                    throw;

                return defaultValue;
            }
        }

        public string GetRule(string tag, string defaultValue, string filter = null)
        {
            try
            {
                return ActuallyGetTheRule(tag, filter);
            }
            catch (Exception)
            {
                if (_throwException)
                    throw;

                return defaultValue;
            }
        }

        public async Task<string> GetRuleAsync(string tag, string defaultValue, string filter = null)
        {
            try
            {
                return await ActuallyGetTheRuleAsync(tag, filter);
            }
            catch (Exception)
            {
                if (_throwException)
                    throw;

                return defaultValue;
            }
        }

        public async Task<bool> GetRuleAsync(string tag, bool defaultValue, string filter = null)
        {
            try
            {
                return bool.Parse(await ActuallyGetTheRuleAsync(tag, filter));
            }
            catch (Exception)
            {
                if (_throwException)
                    throw;

                return defaultValue;
            }
        }

        public async Task<int> GetRuleAsync(string tag, int defaultValue, string filter = null)
        {
            try
            {
                return int.Parse(await ActuallyGetTheRuleAsync(tag, filter));
            }
            catch (Exception)
            {
                if (_throwException)
                    throw;

                return defaultValue;
            }
        }

        private string ActuallyGetTheRule(string tag, string filter)
        {
            var url = ConstructUrl(tag, filter);

            var cachedResult = _cache.Get(url);

            if (cachedResult != null)
                return cachedResult;

            var ruleResult = _httpWrapper.CallEightSparkApi(url);

            _cache.Set(url, ruleResult);

            return ruleResult.Result;
        }

        private async Task<string> ActuallyGetTheRuleAsync(string tag, string filter)
        {
            var url = ConstructUrl(tag, filter);

            var cachedResult = _cache.Get(url);

            if (cachedResult != null)
                return cachedResult;

            var ruleResult = await _httpWrapper.CallEightSparkApiAsync(url);

            _cache.Set(url, ruleResult);

            return ruleResult.Result;
        }

        private string ConstructUrl(string tag, string filter)
        {
            var url = string.Format("{0}/{1}", _baseUrl, tag);
            if (!string.IsNullOrEmpty(filter))
                url = url + "/filter/" + filter;
            return url;
        }
    }
}
