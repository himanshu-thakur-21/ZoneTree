using System.Diagnostics;
using Tenray.ZoneTree;
using Tenray.ZoneTree.Options;
using Tenray.ZoneTree.Serializers;
using ZoneTree.Configurations;
using ZoneTreeSample.Factories;

namespace ZoneTree.Caching
{
    public sealed class ZoneTreeIntTesting
    {
        private const string _dataPath = "../../../cache/data/int-int";

        private readonly ZoneTreeFactory<int, int> _zoneTreeIntFactory;

        public ZoneTreeIntTesting()
        {
            _zoneTreeIntFactory = ZoneTreeSampleFactory<int, int>
                .GetZoneTreeFactory(_dataPath, new Int32Serializer(), new Int32Serializer());
        }

        public long InsertIntData(bool enableParallelInserts = false)
        {
            var sw = Stopwatch.StartNew();

            using var zoneTree = _zoneTreeIntFactory.OpenOrCreate();
            using var maintainer = ZoneTreeSampleFactory<int, int>.GetMaintainer(zoneTree);

            if (enableParallelInserts)
            {
                Parallel.For(0, 1_000_000, (index) =>
                {
                    zoneTree.Upsert(index, index + index);
                });
            }
            else
            {
                for (int index = 0; index < 1_000_000; index++)
                {
                    zoneTree.Upsert(index, 2 * index);
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

        public long IterateIntData()
        {
            var sw = Stopwatch.StartNew();

            using var zoneTree = _zoneTreeIntFactory.OpenOrCreate();
            using var maintainer = ZoneTreeSampleFactory<int, int>.GetMaintainer(zoneTree);
            using var iterator = zoneTree.CreateIterator();

            var offset = 0;
            while (iterator.Next())
            {
                if (iterator.CurrentKey * 2 != iterator.CurrentValue)
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
