using System.Diagnostics;
using Tenray.ZoneTree;
using Tenray.ZoneTree.Options;
using Tenray.ZoneTree.Serializers;
using ZoneTree.Configurations;
using ZoneTreeSample.Factories;
using ZoneTreeSample.Models;
using ZoneTreeSample.Serialization;

namespace ZoneTreeSample.Caching
{
    public sealed class ZoneTreeComplexTesting
    {
        private const string _dataPath = "\\\\LAPTOP-T1VMOFR2\\ZoneTreeCacheDb";

        private readonly ZoneTreeFactory<string, UserResponse> _zoneTreeStringFactory;

        public ZoneTreeComplexTesting()
        {
            _zoneTreeStringFactory = ZoneTreeSampleFactory<string, UserResponse>
                .GetZoneTreeFactory(_dataPath, new Utf8StringSerializer(), new ComplexTypeSerializer<UserResponse>());
        }

        public long InserComplexData(bool enableParallelInserts = false)
        {
            var sw = Stopwatch.StartNew();

            using var zoneTree = _zoneTreeStringFactory.OpenOrCreate();
            using var maintainer = ZoneTreeSampleFactory<string, UserResponse>.GetMaintainer(zoneTree);

            if (enableParallelInserts)
            {
                //Parallel.For(0, 1_000_000, (index) =>
                Parallel.For(0, ZoneTreeConfig.ItemCount, (index) =>
                {
                    var key = $"User_{index}";

                    var user = new UserResponse
                    {
                        Id = index,
                        Name = $"User_{index}",
                        Email = $"test{index}@example.com",
                        Password = "Password",
                    };

                    zoneTree.Upsert(key, user);
                });
            }
            else
            {
                // for (int index = 0; index < 1_000_000; index++)
                for (int index = 0; index < ZoneTreeConfig.ItemCount; index++)
                {
                    var key = $"User_{index}";

                    var user = new UserResponse
                    {
                        Id = index,
                        Name = $"User_{index}",
                        Email = $"test{index}@example.com",
                        Password = "Password",
                    };

                    zoneTree.Upsert(key, user);
                }
            }
            
            if (ZoneTreeConfig.WALMode == WriteAheadLogMode.None)
            {
                zoneTree.Maintenance.MoveMutableSegmentForward();
                zoneTree.Maintenance.StartMergeOperation()?.Join();
            }

            maintainer.CompleteRunningTasks();

            sw.Stop();

            return sw.ElapsedMilliseconds;
        }

        public long IterateComplexData()
        {
            var sw = Stopwatch.StartNew();

            using var zoneTree = _zoneTreeStringFactory.OpenOrCreate();
            using var maintainer = ZoneTreeSampleFactory<string, UserResponse>.GetMaintainer(zoneTree);
            using var iterator = zoneTree.CreateIterator();

            var offset = 0;
            while (iterator.Next())
            {
                if (!string.Equals(iterator.CurrentKey, iterator.CurrentValue.Name, StringComparison.OrdinalIgnoreCase))
                    Console.WriteLine("invalid key or value", ConsoleColor.Red);

                ++offset;
            }

            //if (offset != 1_000_000)
            //    Console.WriteLine($"missing records. {offset} != {1_000_000}", ConsoleColor.Red);
            if (offset != ZoneTreeConfig.ItemCount)
                Console.WriteLine($"missing records. {offset} != {1_000_000}", ConsoleColor.Red);

            sw.Stop();

            return sw.ElapsedMilliseconds;
        }
    }
}
