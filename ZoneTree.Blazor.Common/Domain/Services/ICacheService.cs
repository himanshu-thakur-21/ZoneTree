namespace ZoneTree.Blazor.Common.Domain.Services
{
    public interface ICacheService<TValue>
    {
        bool Exists(string key);
        TValue Get(string key);
        void Set(string key, TValue value);
    }
}
