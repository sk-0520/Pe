using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.CommonTest
{
    public sealed class TestRegex: Regex
    {
        public TestRegex(string pattern)
            : base(pattern, RegexOptions.None, DefaultMatchTimeout)
        { }

        public TestRegex(string pattern, RegexOptions options)
            : base(pattern, options, DefaultMatchTimeout)
        { }

        #region property

        public static TimeSpan DefaultMatchTimeout => TimeSpan.FromSeconds(1);

        #endregion

    }
}
