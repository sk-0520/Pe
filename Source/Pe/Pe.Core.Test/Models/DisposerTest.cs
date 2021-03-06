using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class ActionDisposerTest
    {
        [TestMethod]
        public void UsingTest()
        {
            using(var dispoer = new ActionDisposer(disposing => {
                Assert.IsTrue(disposing);
            })) {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void FinalizeTest()
        {
            var dispoer = new ActionDisposer(disposing => {
                Assert.IsFalse(disposing);
            });
        }
    }
}
