using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.KeyAction
{
    [TestClass]
    public class KeyOptionConverterTest
    {
        #region define

        class KeyOptionConverter: KeyOptionConverterBase
        {
            public new KeyActionOptionAttribute GetAttribute<TEnum>(TEnum value)
                where TEnum : struct, Enum
            {
                return base.GetAttribute(value);
            }
        }

        enum Option
        {
            A,
            [KeyActionOption(typeof(int), "NAME")]
            B,
        }

        #endregion

        #region function

        [TestMethod]
        public void GetAttributeTest()
        {
            var koc = new KeyOptionConverter();
            Assert.ThrowsException<InvalidOperationException>(() => koc.GetAttribute(Option.A));
            Assert.ThrowsException<InvalidOperationException>(() => koc.GetAttribute((Option)(-1)));
            var actual = koc.GetAttribute(Option.B);
            Assert.AreEqual("NAME", actual.OptionName);
            Assert.AreEqual(typeof(int), actual.ToType);
        }

        #endregion
    }


    [TestClass]
    public class DisableOptionConverterTest
    {
        [TestMethod]
        [DataRow(true, "true")]
        [DataRow(true, "TRUE")]
        [DataRow(false, "false")]
        [DataRow(false, "FALSE")]
        public void ToForeverTest(bool expected, string input)
        {
            var doc = new DisableOptionConverter();
            var map = new Dictionary<string, string>() {
                [nameof(KeyActionDisableOption.Forever)] = input,
            };
            var actual = doc.ToForever(map);
            Assert.AreEqual(expected, actual);
        }
    }
}
