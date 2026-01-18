using System;
using ContentTypeTextNet.Pe.Mvvm.ViewModels;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.ViewModels
{
    public class ErrorsContainerTest
    {
        #region function

        [Fact]
        public void T_string_Test()
        {
            var test = new ErrorsContainer<string>(a => { });
            Assert.False(test.HasErrors);
        }


        [Fact]
        public void T_int_Test()
        {
            var test = new ErrorsContainer<int>(a => { });
            Assert.False(test.HasErrors);
        }

        [Fact]
        public void SetError_Enumerable_Test()
        {
            int count = 0;
            var test = new ErrorsContainer<string>(a => {
                Assert.Equal("TestProperty", a);
                count += 1;
            });

            test.SetError("TestProperty", new[] { "a", "b", "c" });
            Assert.True(test.HasErrors);
            Assert.Equal(1, count);
            Assert.Equal(["a", "b", "c"], test.GetError("TestProperty"));

            test.SetError("TestProperty", new[] { "A", "B" });
            Assert.True(test.HasErrors);
            Assert.Equal(2, count);
            Assert.Equal(["A", "B"], test.GetError("TestProperty"));
        }

        [Fact]
        public void SetError_Enumerable_Throw_Test()
        {
            var test = new ErrorsContainer<string>(a => { });

            var exception = Assert.Throws<ArgumentException>(() => test.SetError("TestProperty", []));
            Assert.StartsWith("empty", exception.Message);
            Assert.Equal("validationErrors", exception.ParamName);
        }

        [Fact]
        public void SetError_Single_Test()
        {
            int count = 0;
            var test = new ErrorsContainer<string>(a => {
                Assert.Equal("TestProperty", a);
                count += 1;
            });

            test.SetError("TestProperty", "a");
            Assert.True(test.HasErrors);
            Assert.Equal(1, count);
            Assert.Equal(["a"], test.GetError("TestProperty"));
        }

        [Fact]
        public void AddError_Enumerable_Test()
        {
            int count = 0;
            var test = new ErrorsContainer<string>(a => {
                Assert.Equal("TestProperty", a);
                count += 1;
            });

            test.AddError("TestProperty", new[] { "a", "b", "c" });
            Assert.True(test.HasErrors);
            Assert.Equal(1, count);
            Assert.Equal(["a", "b", "c"], test.GetError("TestProperty"));

            test.AddError("TestProperty", new[] { "A", "B" });
            Assert.True(test.HasErrors);
            Assert.Equal(2, count);
            Assert.Equal(["a", "b", "c", "A", "B"], test.GetError("TestProperty"));
        }

        [Fact]
        public void AddError_Enumerable_Throw_Test()
        {
            var test = new ErrorsContainer<string>(a => { });

            var exception = Assert.Throws<ArgumentException>(() => test.AddError("TestProperty", []));
            Assert.StartsWith("empty", exception.Message);
            Assert.Equal("validationErrors", exception.ParamName);
        }

        [Fact]
        public void AddError_Single_Test()
        {
            int count = 0;
            var test = new ErrorsContainer<string>(a => {
                Assert.Equal("TestProperty", a);
                count += 1;
            });

            test.AddError("TestProperty", "a");
            Assert.True(test.HasErrors);
            Assert.Equal(1, count);
            Assert.Equal(["a"], test.GetError("TestProperty"));
        }

        [Fact]
        public void ClearErrors_All_Test()
        {
            var test = new ErrorsContainer<string>(a => { });

            test.SetError("TestProperty1", new[] { "a", "b", "c" });
            test.SetError("TestProperty2", new[] { "A", "B", "C" });

            Assert.Equal(["a", "b", "c"], test.GetError("TestProperty1"));
            Assert.Equal(["A", "B", "C"], test.GetError("TestProperty2"));

            Assert.True(test.HasErrors);

            test.ClearErrors();

            Assert.False(test.HasErrors);
            Assert.Empty(test.GetError("TestProperty1"));
            Assert.Empty(test.GetError("TestProperty2"));
        }

        [Fact]
        public void GetErrors_Empty_Test()
        {
            var test = new ErrorsContainer<string>(a => { });
            var actual = test.GetErrors();
            Assert.Empty(actual);
        }


        [Fact]
        public void GetErrorsTest()
        {
            var test = new ErrorsContainer<string>(a => { });

            test.SetError("TestProperty1", new[] { "a", "b", "c" });
            test.SetError("TestProperty2", new[] { "A", "B", "C" });

            var actual1 = test.GetErrors();
            Assert.Equal(2, actual1.Count);
            Assert.Equal(["a", "b", "c"], actual1["TestProperty1"]);
            Assert.Equal(["A", "B", "C"], actual1["TestProperty2"]);

            test.ClearError("TestProperty1");

            var actual2 = test.GetErrors();
            Assert.Single(actual2);
            Assert.False(actual2.ContainsKey("TestProperty1"));
            Assert.Equal(["A", "B", "C"], actual1["TestProperty2"]);
        }

        #endregion
    }
}
