using System.Text.Json;
using Tenray.ZoneTree.Serializers;

namespace ZoneTreeSample.Serialization
{
    public sealed class ComplexTypeSerializer<TEntry> : ISerializer<TEntry> where TEntry : class
    {
        /// <summary>
        /// Deserialize the stream to <see cref="TEntry"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public TEntry Deserialize(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            return JsonSerializer.Deserialize(stream, typeof(TEntry)) as TEntry;
        }

        /// <summary>
        /// Serialize the given type <paramref name="entry"/> to byte array
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public byte[] Serialize(in TEntry entry)
        {
            if (entry == null)
                return null;

            return JsonSerializer.SerializeToUtf8Bytes(entry);
        }
    }
}
