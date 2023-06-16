using System.Diagnostics;
using Tenray.ZoneTree;
using Tenray.ZoneTree.Options;
using Tenray.ZoneTree.Serializers;
using ZoneTree.Configurations;
using ZoneTreeSample.Factories;

namespace ZoneTree.Caching
{
    public sealed class ZoneTreeStringTesting
    {
        private const string _dataPath = "../../../cache/data/string-string";

        private readonly ZoneTreeFactory<string, string> _zoneTreeStringFactory;

        public ZoneTreeStringTesting()
        {
            _zoneTreeStringFactory = ZoneTreeSampleFactory<string, string>
                .GetZoneTreeFactory(_dataPath, new Utf8StringSerializer(), new Utf8StringSerializer());
        }

        public long InsertStringData(bool enableParallelInserts = false)
        {
            var sw = Stopwatch.StartNew();

            using var zoneTree = _zoneTreeStringFactory.OpenOrCreate();
            using var maintainer = ZoneTreeSampleFactory<string, string>.GetMaintainer(zoneTree);

            if (enableParallelInserts)
            {
                Parallel.For(0, 1_000_000, (index) =>
                {
                    var str = $"Kernel_Cache_{index}";
                    zoneTree.Upsert(str, str);
                });
            }
            else
            {
                for (int index = 0; index < 1_000_000; index++)
                {
                    var str = $"Kernel_Cache_{index}";
                    zoneTree.Upsert(str, str);
                }
            }

            // if write ahead is not enabled then we can do it manually like this
            if (ZoneTreeConfig.WALMode == WriteAheadLogMode.None)
            {
                zoneTree.Maintenance.MoveMutableSegmentForward();
                zoneTree.Maintenance.StartMergeOperation()?.Join();
            }

            maintainer.CompleteRunningTasks();

            sw.Stop();

            return sw.ElapsedMilliseconds;
        }

        public long IterateStringData()
        {
            var sw = Stopwatch.StartNew();

            using var zoneTree = _zoneTreeStringFactory.OpenOrCreate();
            using var maintainer = ZoneTreeSampleFactory<string, string>.GetMaintainer(zoneTree);
            using var iterator = zoneTree.CreateIterator();

            var offset = 0;
            while (iterator.Next())
            {
                if (!string.Equals(iterator.CurrentKey, iterator.CurrentValue, StringComparison.OrdinalIgnoreCase))
                    Console.WriteLine("invalid key or value", ConsoleColor.Red);

                ++offset;
            }

            if (offset != 1_000_000)
                Console.WriteLine($"missing records. {offset} != {1_000_000}", ConsoleColor.Red);

            sw.Stop();

            return sw.ElapsedMilliseconds;
        }
    }
}
