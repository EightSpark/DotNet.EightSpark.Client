namespace EightSpark.Client
{
    public interface ICache
    {
        void Set(string url, RuleValue deserializedResult);
        string Get(string url);
        void SetCacheTime(int minutes);
    }
}