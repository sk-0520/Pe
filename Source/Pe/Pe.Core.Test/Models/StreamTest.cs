using System.IO;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class KeepStreamTest
    {
        #region define

        int TestLength { get; } = 32;

        #endregion

        #region function

        Stream CreateStream() => new MemoryStream(Enumerable.Repeat((byte)0, TestLength).ToArray());

        [TestMethod]
        public void KeepStream_PositionDefaultTrue_Test()
        {
            using var mem = CreateStream();

            using(var stream = new KeepStream(mem)) {
                Assert.AreEqual(stream.Position, 0);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17 });
                Assert.AreEqual(stream.Position, 8);
            }

            Assert.AreEqual(mem.Position, 0);

            using(var stream = new KeepStream(mem)) {
                Assert.AreEqual(stream.Position, 0);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 });
                Assert.AreEqual(stream.Position, 9);
            }
            Assert.AreEqual(mem.Position, 0);
            Assert.AreEqual(mem.Length, TestLength);
        }

        [TestMethod]
        public void KeepStream_PositionTrue_Test()
        {
            using var mem = CreateStream();

            using(var stream = new KeepStream(mem, true)) {
                Assert.AreEqual(stream.Position, 0);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17 });
                Assert.AreEqual(stream.Position, 8);
            }

            Assert.AreEqual(mem.Position, 0);

            using(var stream = new KeepStream(mem, true)) {
                Assert.AreEqual(stream.Position, 0);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 });
                Assert.AreEqual(stream.Position, 9);
            }
            Assert.AreEqual(mem.Position, 0);
            Assert.AreEqual(mem.Length, TestLength);
        }

        [TestMethod]
        public void KeepStream_PositionFalse_Test()
        {
            using var mem = CreateStream();

            using(var stream = new KeepStream(mem, false)) {
                Assert.AreEqual(stream.Position, 0);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17 });
                Assert.AreEqual(stream.Position, 8);
            }

            Assert.AreEqual(mem.Position, 8);

            using(var stream = new KeepStream(mem, false)) {
                Assert.AreEqual(stream.Position, 8);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 });
                Assert.AreEqual(stream.Position, 17);
            }
            Assert.AreEqual(mem.Position, 17);
            Assert.AreEqual(mem.Length, TestLength);
        }

        #endregion
    }
}
