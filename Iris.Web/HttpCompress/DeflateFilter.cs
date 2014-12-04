using System.IO;
using System.IO.Compression;

namespace Iris.Web.HttpCompress
{
    /// <summary>
    ///     Summary description for DeflateFilter.
    /// </summary>
    public class DeflateFilter : CompressingFilter
    {
        /// <summary>
        ///     compression stream member
        ///     has to be a member as we can only have one instance of the
        ///     actual filter class
        /// </summary>
        private readonly DeflateStream m_stream;

        /// <summary>
        ///     Basic constructor that uses the Normal compression level
        /// </summary>
        /// <param name="baseStream">The stream to wrap up with the deflate algorithm</param>
        public DeflateFilter(Stream baseStream)
            : this(baseStream, CompressionLevels.Normal)
        {
        }

        /// <summary>
        ///     Full constructor that allows you to set the wrapped stream and the level of compression
        /// </summary>
        /// <param name="baseStream">The stream to wrap up with the deflate algorithm</param>
        /// <param name="compressionLevel">The level of compression to use</param>
        public DeflateFilter(Stream baseStream, CompressionLevels compressionLevel)
            : base(baseStream, compressionLevel)
        {
            m_stream = new DeflateStream(baseStream, CompressionMode.Compress);
        }

        /// <summary>
        ///     Return the Http name for this encoding.  Here, deflate.
        /// </summary>
        public override string ContentEncoding
        {
            get { return "deflate"; }
        }

        /// <summary>
        ///     Write out bytes to the underlying stream after compressing them using deflate
        /// </summary>
        /// <param name="buffer">The array of bytes to write</param>
        /// <param name="offset">The offset into the supplied buffer to start</param>
        /// <param name="count">The number of bytes to write</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!HasWrittenHeaders) WriteHeaders();
            m_stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///     Closes this Filter and calls the base class implementation.
        /// </summary>
        public override void Close()
        {
            m_stream.Close();
        }

        /// <summary>
        ///     Flushes that the filter out to underlying storage
        /// </summary>
        public override void Flush()
        {
            m_stream.Flush();
        }
    }
}