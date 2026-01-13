using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test.Linq
{
    public class CachedEnumerableTest
    {
        #region define

        /// <summary>
        /// テスト用の遅延列挙子。
        /// </summary>
        /// <remarks>Copilot が書いた、内容見てない、sleepTime を int から TimeSpan にしただけ。</remarks>
        private sealed class SlowEnumerator: IEnumerator<int>
        {
            private readonly int[] _items;
            private readonly TimeSpan _sleepTime;
            private int _index = -1;

            public SlowEnumerator(int[] items, TimeSpan sleepTime)
            {
                this._items = items ?? throw new ArgumentNullException(nameof(items));
                this._sleepTime = sleepTime;
            }

            public bool IsDisposed { get; private set; }

            public int Current => this._items[this._index];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                IsDisposed = true;
            }

            public bool MoveNext()
            {
                // 列挙中に遅延を入れることでスレッド間の競合状態を発生させやすくする
                Thread.Sleep(this._sleepTime);
                this._index++;
                return this._index < this._items.Length;
            }

            public void Reset() => throw new NotSupportedException();
        }

        #endregion

        #region function

        [Fact]
        public void SimpleTest()
        {
            int[] raw = [0, 1, 2, 3];

            using var test = CachedEnumerable.Create(raw);

            Assert.Equal(raw, test);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Cahced_N_Test(int count)
        {
            int[] raw = [0, 1, 2, 3];

            var test = CachedEnumerable.Create(raw);

            var i = 0;
            foreach(var item in test) {
                Assert.Equal(raw[i], item);
                if(count <= ++i) {
                    break;
                }
            }

            Assert.Equal(count, test.Cached.Count);
            for(var j = 0; j < count; j++) {
                Assert.Equal(raw[j], test.Cached[j]);
            }
        }

        [Fact]
        public void Cahced_Full_Test()
        {
            int[] raw = [0, 1, 2, 3];

            var test = CachedEnumerable.Create(raw);

            var i = 0;
            foreach(var item in test) {
                Assert.Equal(raw[i++], item);
            }

            Assert.Equal(raw, test.Cached);
        }

        // Copilot が書いた、内容見てない
        [Fact]
        public async Task MultiThreaded_Enumeration_AsyncTest()
        {
            // テストデータサイズとスレッド数
            const int itemCount = 200;
            const int threadCount = 8;

            var raw = Enumerable.Range(0, itemCount).ToArray();

            // 内部 enumerator を直接渡して、Dispose の呼び出しを検出できるようにする
            var slowEnumerator = new SlowEnumerator(raw, TimeSpan.FromMicroseconds(2));
            var cached = new CachedEnumerable<int>(slowEnumerator);

            // 各スレッドは全要素を列挙して自分の結果リストに格納する
            var tasks = new Task<List<int>>[threadCount];
            for(var t = 0; t < threadCount; t++) {
                tasks[t] = Task.Run(() => {
                    var results = new List<int>(itemCount);
                    foreach(var v in cached) {
                        results.Add(v);
                    }
                    return results;
                });
            }

            // 全タスク完了を待機
            var results = await Task.WhenAll(tasks);

            // 各タスクの結果が期待通りであることを確認
            foreach(var result in results) {
                Assert.Equal(itemCount, result.Count);
                Assert.Equal(raw, result);
            }

            // 全列挙が完了してキャッシュが完全になっていること
            Assert.True(cached.IsEnumerationCompleted);
            Assert.Equal(itemCount, cached.Cached.Count);
            Assert.Equal(raw, cached.Cached.ToArray());

            // 内部 enumerator が Dispose されていること（列挙完了時に破棄されるはず）
            Assert.True(slowEnumerator.IsDisposed);
        }

        #endregion
    }
}
