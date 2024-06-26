using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class IsNotEmptyCollectionConverterTest
    {
        #region function

        [Fact]
        public void Convert_ICollectionView_empty_Test()
        {
            var test = new IsNotEmptyCollectionConverter();

            var value = new CollectionView(Array.Empty<int>());
            var actual = test.Convert(value, default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(false, actual);
        }

        [Fact]
        public void Convert_ICollectionView_notEmpty_Test()
        {
            var test = new IsNotEmptyCollectionConverter();

            var value = new CollectionView(new[] { 1, 2, 3 });
            var actual = test.Convert(value, default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(true, actual);
        }

        [Fact]
        public void Convert_IEnumerable_empty_Test()
        {
            var test = new IsNotEmptyCollectionConverter();

            var value = Array.Empty<int>();
            var actual = test.Convert(value, default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(false, actual);
        }

        [Fact]
        public void Convert_IEnumerable_notEmpty_Test()
        {
            var test = new IsNotEmptyCollectionConverter();

            var value = new[] { 1, 2, 3 };
            var actual = test.Convert(value, default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(true, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var test = new IsNotEmptyCollectionConverter();

            Assert.Throws<NotSupportedException>(() => test.ConvertBack(default!, default!, default!, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
