using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class ReadWriteLockHelper
    {
        [Fact]
        public void ReadLockTest()
        {
            var test = new Common.ReadWriteLockHelper();
            Parallel.For(0, 1000, n => {
                using(test.BeginRead()) {
                    Assert.True(true);
                    //Console.WriteLine(n);
                }
            });
        }

        [Fact]
        public void WriteLockTest()
        {
            var test = new Common.ReadWriteLockHelper();
            using(test.BeginWrite()) {
                try {
                    using(test.BeginRead()) {
                        Assert.True(false);
                    }
                } catch(LockRecursionException) {
                    Assert.True(true);
                }
            }
        }
    }
}
