using Tenray.ZoneTree;
using Tenray.ZoneTree.Options;
using Tenray.ZoneTree.Serializers;
using ZoneTree.Configurations;
using ZoneTreeSample.FileStreamProviders;

namespace ZoneTreeSample.Factories
{
    public class ZoneTreeSampleFactory<TKey, TValue>
    {
        public static ZoneTreeFactory<TKey, TValue> GetZoneTreeFactory(
            string dataDirectory,
            ISerializer<TKey> keySerializer,
            ISerializer<TValue> valueSerializer)
        {
            if (dataDirectory is null)
                throw new ArgumentNullException(nameof(dataDirectory));

            if (keySerializer is null)
                throw new ArgumentNullException(nameof(keySerializer));

            if (valueSerializer is null)
                throw new ArgumentNullException(nameof(valueSerializer));

            return new ZoneTreeFactory<TKey, TValue>()
                .SetMutableSegmentMaxItemCount(ZoneTreeConfig.MutableSegmentMaxItemCount)
                .SetDiskSegmentMaxItemCount(ZoneTreeConfig.DiskSegmentMaxItemCount)
                .SetDiskSegmentCompression(ZoneTreeConfig.EnableDiskSegmentCompression)
                .SetDiskSegmentCompressionBlockSize(ZoneTreeConfig.DiskCompressionBlockSize)
                .SetDiskSegmentMaximumCachedBlockCount(ZoneTreeConfig.DiskSegmentMaximumCachedBlockCount)
                .SetDataDirectory(dataDirectory)
                .SetKeySerializer(keySerializer)
                .SetValueSerializer(valueSerializer)
                .SetInitialSparseArrayLength(ZoneTreeConfig.MinimumSparseArrayLength)
                .ConfigureDiskSegmentOptions(x =>
                {
                    x.DiskSegmentMode = DiskSegmentMode.MultiPartDiskSegment;
                    x.CompressionMethod = CompressionMethod.LZ4;
                    x.CompressionLevel = ZoneTreeConfig.CompressionLevel;
                })
                .ConfigureWriteAheadLogOptions(x =>
                {
                    x.CompressionMethod = CompressionMethod.LZ4;
                    x.CompressionLevel = ZoneTreeConfig.CompressionLevel;
                    x.CompressionBlockSize = ZoneTreeConfig.WALCompressionBlockSize;
                    x.WriteAheadLogMode = ZoneTreeConfig.WALMode;
                    x.EnableIncrementalBackup = ZoneTreeConfig.EnableIncrementalBackup;
                });
        }

        /// <summary>
        /// Get zone tree maintainer
        /// </summary>
        /// <param name="zoneTree">the zone tree</param>
        /// <returns></returns>
        public static IMaintainer GetMaintainer(IZoneTree<TKey, TValue> zoneTree)
        {
            var maintainer = zoneTree.CreateMaintainer();
            maintainer.ThresholdForMergeOperationStart = ZoneTreeConfig.ThresholdForMergeOperationStart;
            maintainer.MinimumSparseArrayLength = ZoneTreeConfig.MinimumSparseArrayLength;

            return maintainer;
        }
    }
}
