using Tenray.ZoneTree.Options;

namespace ZoneTree.Configurations
{
    public static class ZoneTreeConfig
    {
        public const bool RecreateDatabases = true;
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
        public const bool EnableIncrementalBackup = true;

        /// <summary>
        /// Configures the disk segment compression. Default is true.
        /// </summary>
        public const bool EnableDiskSegmentCompression = true;
        public const int WALCompressionBlockSize = 32768;

        /// <summary>
        /// The disk segment compression block size.
        /// Default: 10 MB
        /// Minimum: 8KB, Maximum: 1 GB
        /// </summary>
        public const int DiskCompressionBlockSize = 32768;
        public const int DiskSegmentMaximumCachedBlockCount = 1000;
        public const int MinimumSparseArrayLength = 1_000_000;
        public const int CompressionLevel = 0;

        public const int ItemCount = 1_000_000;
        public static WriteAheadLogMode WALMode { get; set; } = WriteAheadLogMode.None;
        public const string StorageAccountConnStr = "DefaultEndpointsProtocol=https;AccountName=testfrstorage1;AccountKey=cAiJHQyPQbkJHMnVFiFNxZKY+sIdFOP+2KIX5/i/5CCcJAQrjV2erpx8Cx4+oF3XRs6cL1Lh0tDh/Unje/ZJrw==;EndpointSuffix=core.windows.net";
        public const string ContainerName = "zone-tree-test";
    }
}
