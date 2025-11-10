using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Unmanaged
{
    public class SafeComTest
    {
        #region function

        [Fact]
        public void Constructor_IShellLink_Test()
        {
            using var test = new SafeCom<IShellLink>((IShellLink)new ShellLinkObject());
            Assert.NotNull(test);
        }

        [Fact]
        public void Constructor_throw_null_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new SafeCom<object>(null!));
        }

        [Fact]
        public void Constructor_throw_com_Test()
        {
            var exception = Assert.Throws<ArgumentException>(() => new SafeCom<object>(new object()));
            Assert.Equal("comInstance", exception.ParamName);
            Assert.StartsWith("Marshal.IsComObject", exception.Message);
        }

        [Fact]
        public void CastTest()
        {
            using var test = new SafeCom<IShellLink>((IShellLink)new ShellLinkObject());
            using var actual1 = test.Cast<IPersistFile>();
            Assert.NotNull(actual1);

            using var actual2 = test.Cast<IPersistFile>();
            Assert.NotNull(actual2);

            Assert.NotEqual(actual1, actual2);
        }

        [Fact]
        public void Cast_throw_Test()
        {
            using var test = new SafeCom<IShellLink>((IShellLink)new ShellLinkObject());
            Assert.Throws<InvalidCastException>(() => test.Cast<IImageList>());
        }

        [Fact]
        public void Dispose_throw_instance_Test()
        {
            using var test = new SafeCom<IShellLink>((IShellLink)new ShellLinkObject());
            var po = new PrivateObject(test);
            var prevInstance = po.GetField("_instance");
            po.SetField("_instance", null);
            Assert.Throws<InvalidOperationException>(() => { _ = test.Instance; });

            po.SetField("_instance", prevInstance);
        }

        #endregion
    }

    public class Com_Wrapper_Test
    {
        #region function

        [Fact]
        public void CreateTest()
        {
            using var actual = Com.Create((IShellLink)new ShellLinkObject());
            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_throw_null_Test()
        {
            Assert.Throws<ArgumentNullException>(() => Com.Create<object>(null!));
        }

        [Fact]
        public void Create_throw_com_Test()
        {
            var exception = Assert.Throws<ArgumentException>(() => Com.Create(new object()));
            Assert.Equal("comInstance", exception.ParamName);
            Assert.StartsWith("Marshal.IsComObject", exception.Message);
        }

        #endregion
    }
}
