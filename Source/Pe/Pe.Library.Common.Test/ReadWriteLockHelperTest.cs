using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class ReadWriteLockHelperTest
    {
        [Fact]
        public void BeginRead_Default_Read_Read_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginRead()) {
                Assert.Throws<LockRecursionException>(() => test.BeginRead());
            }
        }

        [Fact]
        public void BeginRead_Recursion_Read_Read_Test()
        {
            var test = new ReadWriteLockHelper(LockRecursionPolicy.SupportsRecursion);
            using(test.BeginRead()) {
                using(test.BeginRead()) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void BeginRead_Default_Read_Update_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginRead()) {
                Assert.Throws<LockRecursionException>(() => test.BeginUpdate());
            }
        }

        [Fact]
        public void BeginRead_Recursion_Read_Update_Test()
        {
            var test = new ReadWriteLockHelper(LockRecursionPolicy.SupportsRecursion);
            using(test.BeginRead()) {
                Assert.Throws<LockRecursionException>(() => test.BeginUpdate());
            }
        }

        [Fact]
        public void BeginRead_Default_Read_Write_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginRead()) {
                Assert.Throws<LockRecursionException>(() => test.BeginWrite());
            }
        }

        [Fact]
        public void BeginRead_Recursion_Read_Write_Test()
        {
            var test = new ReadWriteLockHelper(LockRecursionPolicy.SupportsRecursion);
            using(test.BeginRead()) {
                Assert.Throws<LockRecursionException>(() => test.BeginWrite());
            }
        }

        [Fact]
        public void BeginUpdate_Default_Update_Read_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginUpdate()) {
                using(test.BeginRead()) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void BeginUpdate_Default_Update_Update_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginUpdate()) {
                Assert.Throws<LockRecursionException>(() => test.BeginUpdate());
            }
        }

        [Fact]
        public void BeginUpdate_Recursion_Update_Update_Test()
        {
            var test = new ReadWriteLockHelper(LockRecursionPolicy.SupportsRecursion);
            using(test.BeginUpdate()) {
                using(test.BeginUpdate()) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void BeginUpdate_Default_Update_Write_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginUpdate()) {
                using(test.BeginWrite()) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void BeginUpdate_Recursiont_Update_Write_Test()
        {
            var test = new ReadWriteLockHelper(LockRecursionPolicy.SupportsRecursion);
            using(test.BeginUpdate()) {
                using(test.BeginWrite()) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void BeginWrite_Default_Write_Read_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginWrite()) {
                Assert.Throws<LockRecursionException>(() => test.BeginRead());
            }
        }

        [Fact]
        public void BeginWrite_Recursion_Write_Read_Test()
        {
            var test = new ReadWriteLockHelper(LockRecursionPolicy.SupportsRecursion);
            using(test.BeginWrite()) {
                using(test.BeginRead()) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void BeginWrite_Default_Write_Update_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginWrite()) {
                Assert.Throws<LockRecursionException>(() => test.BeginUpdate());
            }
        }

        [Fact]
        public void BeginWrite_Recursion_Write_Update_Test()
        {
            var test = new ReadWriteLockHelper(LockRecursionPolicy.SupportsRecursion);
            using(test.BeginWrite()) {
                using(test.BeginUpdate()) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void BeginWrite_Defualt_Write_Write_Test()
        {
            var test = new ReadWriteLockHelper();
            using(test.BeginWrite()) {
                Assert.Throws<LockRecursionException>(() => test.BeginWrite());
            }
        }

        [Fact]
        public void BeginWrite_Recursion_Write_Write_Test()
        {
            var test = new ReadWriteLockHelper(LockRecursionPolicy.SupportsRecursion);
            using(test.BeginWrite()) {
                using(test.BeginWrite()) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void BeginRead_Parallel_Test()
        {
            const int count = 1000;
            var test = new ReadWriteLockHelper();
            var bag = new ConcurrentBag<int>();
            Parallel.For(0, count, n => {
                using(test.BeginRead()) {
                    bag.Add(n);
                }
            });
            Assert.Equal(count, bag.Count);
        }

        [Fact]
        public async Task BeginRead_Thread_Read_Read_AsyncTest()
        {
            var test = new ReadWriteLockHelper();

            var selfActual = 0;
            var threadActual = 0;

            var task1 = new Task(() => {
                using(test.BeginRead()) {
                    selfActual = 1;
                }
            }, TestContext.Current.CancellationToken);

            var task2 = new Task(() => {
                using(test.BeginRead()) {
                    threadActual = 2;
                }
            }, TestContext.Current.CancellationToken);

            task1.Start();
            task2.Start();

            await Task.WhenAll(task1, task2);

            Assert.Equal(1, selfActual);
            Assert.Equal(2, threadActual);
        }


    }
}
