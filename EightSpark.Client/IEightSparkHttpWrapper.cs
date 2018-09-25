using System.Threading.Tasks;

namespace EightSpark.Client
{
    public interface IEightSparkHttpWrapper
    {
        RuleValue CallEightSparkApi(string url);
        Task<RuleValue> CallEightSparkApiAsync(string url);
    }
}