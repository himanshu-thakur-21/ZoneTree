using Tenray.ZoneTree;
using Tenray.ZoneTree.Options;
using Tenray.ZoneTree.Serializers;
using ZoneTree.Blazor.Common.Domain.Models;
using ZoneTree.Blazor.Common.Domain.Services;
using ZoneTree.Blazor.Common.Infrastructure.Factories;
using ZoneTree.Blazor.Common.Infrastructure.Serialization;

namespace ZoneTree.Blazor.Common.Services
{
    public class CacheService<TValue> : ICacheService<TValue> where TValue : class
    {
        private const string _dataPath = "\\\\LAPTOP-T1VMOFR2\\ZoneTreeCacheDb";

        private readonly ZoneTreeFactory<string, TValue> _zoneTreeFactory;

        public CacheService()
        {
            _zoneTreeFactory = ZoneTreeFactoryProvider<string, TValue>
                .GetZoneTreeFactory($"{_dataPath}",
                    new Utf8StringSerializer(),
                    new Utf8ByteSerializer<TValue>()
                );
        }

        public bool Exists(string key)
        {
            using var zoneTree = _zoneTreeFactory.OpenOrCreate();
            using var maintainer = ZoneTreeFactoryProvider<string, TValue>.GetMaintainer(zoneTree);

            return zoneTree.ContainsKey(key);
        }

        public TValue Get(string key)
        {
            using var zoneTree = _zoneTreeFactory.OpenOrCreate();
            using var maintainer = ZoneTreeFactoryProvider<string, TValue>.GetMaintainer(zoneTree);

            _ = zoneTree.TryGet(key, out var value);

            return value;
        }

        public void Set(string key, TValue value)
        {
            using var zoneTree = _zoneTreeFactory.OpenOrCreate();
            using var maintainer = ZoneTreeFactoryProvider<string, TValue>.GetMaintainer(zoneTree);

            zoneTree.Upsert(key, value);

            if (ZoneTreeConfig.WALMode == WriteAheadLogMode.None)
            {
                zoneTree.Maintenance.MoveMutableSegmentForward();
                zoneTree.Maintenance.StartMergeOperation()?.Join();
            }

            maintainer.CompleteRunningTasks();
        }
    }
}
