using System.Threading.Tasks;

namespace EightSpark.Client
{
    public interface IEightSparkClient
    {
        void SetCacheTime(int minutes);
        void SetThrowExceptions(bool throwExceptions);
        void SetDomain(string url);
        bool GetRule(string tag, bool defaultValue, string filter = null);
        int GetRule(string tag, int defaultValue, string filter = null);
        string GetRule(string tag, string defaultValue, string filter = null);
        Task<string> GetRuleAsync(string tag, string defaultValue, string filter = null);
        Task<bool> GetRuleAsync(string tag, bool defaultValue, string filter = null);
        Task<int> GetRuleAsync(string tag, int defaultValue, string filter = null);
    }
}