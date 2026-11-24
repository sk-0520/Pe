using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class ReferencePoolTest
    {
        #region define

        private sealed class TestValue: IDisposable
        {
            public int DisposeCount { get; private set; }

            public void Dispose()
            {
                DisposeCount++;
            }
        }

        private static ReferencePool<string, TestValue> CreatePool(TimeSpan timeout, bool manage)
        {
            return new ReferencePool<string, TestValue>(TimeSpan.FromDays(1), timeout, manage, TimeProvider.System, NullLoggerFactory.Instance);
        }

        #endregion

        #region function

        [Fact]
        public void ReferenceItem_Alive_TimeoutBoundary_Test()
        {
            var timeProvider = new FakeTimeProvider();
            var value = new TestValue();
            var item = new ReferenceItem<TestValue>(value, TimeSpan.FromSeconds(10), true, timeProvider);

            Assert.Same(value, item.Value);
            Assert.True(item.IsManage);
            Assert.True(item.Alive);
            timeProvider.Advance(TimeSpan.FromSeconds(10) - TimeSpan.FromTicks(1));
            Assert.True(item.Alive);
            timeProvider.Advance(TimeSpan.FromTicks(1));
            Assert.False(item.Alive);
        }

        [Fact]
        public void Get_Recycle_ExtendsLifetime_Test()
        {
            var timeProvider = new FakeTimeProvider();
            using var pool = new ReferencePool<string, TestValue>(TimeSpan.FromDays(1), TimeSpan.FromSeconds(10), true, timeProvider, NullLoggerFactory.Instance);
            var calls = 0;
            TestValue Create(string key)
            {
                calls++;
                return new TestValue();
            }

            var first = pool.Get("key", Create);
            timeProvider.Advance(TimeSpan.FromSeconds(9));
            Assert.Same(first, pool.Get("key", Create));
            timeProvider.Advance(TimeSpan.FromSeconds(9));
            Assert.Same(first, pool.Get("key", Create));
            Assert.Equal(1, calls);
            Assert.Equal(0, first.DisposeCount);

            timeProvider.Advance(TimeSpan.FromSeconds(10));
            var replacement = pool.Get("key", Create);
            Assert.NotSame(first, replacement);
            Assert.Equal(2, calls);
            Assert.Equal(1, first.DisposeCount);
            Assert.Equal(0, replacement.DisposeCount);
        }

        [Fact]
        public void Refresh_UsesTimeProvider_Test()
        {
            var timeProvider = new FakeTimeProvider();
            using var pool = new ReferencePool<string, TestValue>(TimeSpan.FromDays(1), TimeSpan.FromSeconds(10), true, timeProvider, NullLoggerFactory.Instance);
            var value = pool.Get("key", _ => new TestValue());

            timeProvider.Advance(TimeSpan.FromSeconds(10) - TimeSpan.FromTicks(1));
            pool.Refresh();
            Assert.Equal(0, value.DisposeCount);
            timeProvider.Advance(TimeSpan.FromTicks(1));
            pool.Refresh();
            Assert.Equal(1, value.DisposeCount);
        }

        [Fact]
        public void Get_ReuseByKey_Test()
        {
            using var pool = CreatePool(TimeSpan.FromDays(1), false);
            var keys = new List<string>();
            TestValue Create(string key)
            {
                keys.Add(key);
                return new TestValue();
            }

            var first = pool.Get("first", Create);
            var second = pool.Get("second", Create);
            var again = pool.Get("first", Create);

            Assert.Same(first, again);
            Assert.NotSame(first, second);
            Assert.Equal(new[] { "first", "second" }, keys);
            Assert.Equal(TimeSpan.FromDays(1), pool.DefaultTimeout);
            Assert.False(pool.DefaultManage);
        }

        [Fact]
        public void Get_CreatorThrows_Retry_Test()
        {
            using var pool = CreatePool(TimeSpan.FromDays(1), false);
            var expected = new TestValue();
            var calls = 0;

            Assert.Throws<InvalidOperationException>(() => pool.Get("key", _ => {
                calls++;
                throw new InvalidOperationException();
            }));
            var actual = pool.Get("key", _ => {
                calls++;
                return expected;
            });

            Assert.Same(expected, actual);
            Assert.Equal(2, calls);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void Get_Defaults_Expired_Test(bool manage, int expectedDisposeCount)
        {
            using var pool = CreatePool(TimeSpan.Zero, manage);
            var created = new List<TestValue>();
            TestValue Create(string key)
            {
                var value = new TestValue();
                created.Add(value);
                return value;
            }

            var actual = pool.Get("key", Create);

            // 現状、即期限切れの場合は初回取得時にも生成し直す。
            Assert.Equal(2, created.Count);
            Assert.Same(created[1], actual);
            Assert.Equal(expectedDisposeCount, created[0].DisposeCount);
            Assert.Equal(0, actual.DisposeCount);
        }

        [Fact]
        public void Get_TimeoutOverride_Test()
        {
            using var pool = CreatePool(TimeSpan.FromDays(1), true);
            var created = new List<TestValue>();
            TestValue Create(string key)
            {
                var value = new TestValue();
                created.Add(value);
                return value;
            }

            var actual = pool.Get("key", TimeSpan.Zero, Create);

            Assert.Equal(2, created.Count);
            Assert.Equal(1, created[0].DisposeCount);
            Assert.Same(created[1], actual);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void Get_ManageOverride_Test(bool manage, int expectedDisposeCount)
        {
            using var pool = CreatePool(TimeSpan.Zero, !manage);
            var created = new List<TestValue>();
            TestValue Create(string key)
            {
                var value = new TestValue();
                created.Add(value);
                return value;
            }

            var actual = pool.Get("key", manage, Create);

            Assert.Equal(2, created.Count);
            Assert.Equal(expectedDisposeCount, created[0].DisposeCount);
            Assert.Same(created[1], actual);
        }

        [Fact]
        public void Get_TimeoutAndManageOverride_Test()
        {
            using var pool = CreatePool(TimeSpan.FromDays(1), false);
            var created = new List<TestValue>();
            TestValue Create(string key)
            {
                var value = new TestValue();
                created.Add(value);
                return value;
            }

            var actual = pool.Get("key", TimeSpan.Zero, true, Create);

            Assert.Equal(2, created.Count);
            Assert.Equal(1, created[0].DisposeCount);
            Assert.Same(created[1], actual);
        }

        [Fact]
        public void Get_ExistingItemKeepsOriginalOptions_Test()
        {
            using var pool = CreatePool(TimeSpan.FromDays(1), false);
            var calls = 0;
            TestValue Create(string key)
            {
                calls++;
                return new TestValue();
            }

            var first = pool.Get("key", TimeSpan.FromDays(1), false, Create);
            var again = pool.Get("key", TimeSpan.Zero, true, Create);
            pool.Refresh();

            Assert.Same(first, again);
            Assert.Equal(1, calls);
            Assert.Equal(0, first.DisposeCount);
        }

        [Fact]
        public void Refresh_ExpiredAndLiveItems_Test()
        {
            using var pool = CreatePool(TimeSpan.FromDays(1), true);
            var live = pool.Get("live", _ => new TestValue());
            var expired = pool.Get("expired", TimeSpan.Zero, true, _ => new TestValue());
            var unmanaged = pool.Get("unmanaged", TimeSpan.Zero, false, _ => new TestValue());

            pool.Refresh();

            Assert.Equal(0, live.DisposeCount);
            Assert.Equal(1, expired.DisposeCount);
            Assert.Equal(0, unmanaged.DisposeCount);
        }

        [Fact]
        public void Refresh_ExpiredItemIsNotRemoved_Test()
        {
            using var pool = CreatePool(TimeSpan.Zero, true);
            var expired = pool.Get("key", _ => new TestValue());

            // 現状の Refresh は除去せず、同じ値を繰り返し破棄する。
            pool.Refresh();
            pool.Refresh();
            Assert.Equal(2, expired.DisposeCount);

            var replacement = pool.Get("key", _ => new TestValue());
            Assert.NotSame(expired, replacement);
            Assert.Equal(3, expired.DisposeCount);
            Assert.Equal(0, replacement.DisposeCount);
        }

        [Fact]
        public void Refresh_NonDisposableValue_Test()
        {
            using var pool = new ReferencePool<string, object>(TimeSpan.FromDays(1), TimeSpan.Zero, true, TimeProvider.System, NullLoggerFactory.Instance);
            var expired = pool.Get("key", _ => new object());

            pool.Refresh();
            var replacement = pool.Get("key", _ => new object());

            Assert.NotSame(expired, replacement);
        }

        [Fact]
        public void Dispose_OnlyExpiredManagedItems_Test()
        {
            var pool = CreatePool(TimeSpan.FromDays(1), true);
            var live = pool.Get("live", _ => new TestValue());
            var expired = pool.Get("expired", TimeSpan.Zero, true, _ => new TestValue());
            var unmanaged = pool.Get("unmanaged", TimeSpan.Zero, false, _ => new TestValue());

            pool.Dispose();
            pool.Dispose();

            Assert.True(pool.IsDisposed);
            Assert.Equal(0, live.DisposeCount);
            Assert.Equal(1, expired.DisposeCount);
            Assert.Equal(0, unmanaged.DisposeCount);
        }

        #endregion
    }
}
