using Tenray.ZoneTree.Options;

namespace ZoneTree.Blazor.Common.Domain.Models
{
    public static class ZoneTreeConfig
    {
        /// <summary>
        /// Starts merge operation when records count
        /// in read-only segments exceeds this value.
        /// Default value is 2M.
        /// </summary>
        public const int ThresholdForMergeOperationStart = 2_000_000;

        /// <summary>
        /// Mutable segment maximumum key-value pair count.
        /// When the maximum count is reached 
        /// MoveMutableSegmentForward is called and current mutable segment is enqueued to
        /// the ReadOnlySegments layer.
        /// </summary>
        public const int MutableSegmentMaxItemCount = 1_000_000;

        /// <summary>
        /// Disk segment maximumum key-value pair count.
        /// When the maximum count is reached
        /// The disk segment is enqueued into to the bottom segments layer.
        /// </summary>
        public const int DiskSegmentMaxItemCount = 20_000_000;

        /// <summary>
        /// Incremental backup is a WAL feature which moves
        /// all WAL data to another incremental log file when it is compacted.
        /// It is required to compact WAL in memory without data loss in 
        /// persistent device. Used by Optimistic Transactional ZoneTree for
        /// transaction log compaction. Enabling backup will make transactions 
        /// slower.
        /// Default value is false.
        /// </summary>
        public const bool EnableIncrementalBackup = true; // used by transactional zone tree only

        /// <summary>
        /// Configures the disk segment compression. Default is true.
        /// </summary>
        public const bool EnableDiskSegmentCompression = true;

        /// <summary>
        /// WAL compressin block size. New WALs will be created 
        /// based on this setting. Default value = 256 KB
        /// </summary>
        public const int WALCompressionBlockSize = 32768;

        /// <summary>
        /// The disk segment compression block size.
        /// Default: 10 MB
        /// Minimum: 8KB, Maximum: 1 GB
        /// </summary>
        public const int DiskCompressionBlockSize = 32768;

        /// <summary>
        /// The disk segment block cache limit.
        /// A disk segment cannot have more cache blocks than the limit.
        /// Total memory space that block cache can take is
        /// = CompressionBlockSize X BlockCacheLimit
        /// Default: 1024 * 1024 * 10 * 32 = 320 MB
        /// </summary>
        public const int DiskSegmentMaximumCachedBlockCount = 1000;

        /// <summary>
        /// Minimum sparse array length when a new disk segment is created.
        /// Default value is 0.
        /// </summary>
        public const int MinimumSparseArrayLength = 1_000_000;

        /// <summary>
        /// The compression level of the selected compression method.
        /// Default is <see cref="CompressionLevels.LZ4Fastest"/>.
        /// </summary>
        public const int CompressionLevel = CompressionLevels.LZ4Fastest;

        /// <summary>
        /// The item count
        /// </summary>
        public const int ItemCount = 1_000_000;

        /// <summary>
        /// Write Ahead Log Options. The options will be used
        /// for creation of new Write Ahead Logs.
        /// Existing WALs will be created with their existing options.
        /// Default value is AsyncCompressed.
        /// </summary>
        public static WriteAheadLogMode WALMode { get; set; } = WriteAheadLogMode.None;
    }
}
