using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class FileWatcherTest
    {
        #region property

        private ProjectPath IOProjectPath { get; } = ProjectPath.Factory.CreateIO();

        #endregion

        #region function

        [Fact]
        public void WatchTest()
        {
            var methodPath = IOProjectPath.CreateMethodDirectory(this);
            var filePath = methodPath.CreateEmptyFile("a.txt");
            var time = TimeSpan.FromMicroseconds(1);

            var called = new HashSet<int>();

            using var test = new FileWatcher(new FileWatchParameter {
                File = filePath.File,
                DelayTime = time,
            }, NullLoggerFactory.Instance);
            test.Start();

            int step = 0;
            using var ev = new AutoResetEvent(false);

            test.FileContentChanged += (sender, e) => {
                switch(step) {
                    case 1: {
                            using var r = e.File.OpenText();
                            var data = r.ReadToEnd();
                            AssertEx.EqualMultiLineTextIgnoreNewline("abc", data);
                            called.Add(step);
                        }
                        break;

                    case 2: {
                            using var r = e.File.OpenText();
                            var data = r.ReadToEnd();
                            AssertEx.EqualMultiLineTextIgnoreNewline("abc\ndef", data);
                            called.Add(step);
                            ev.Reset();
                        }
                        break;

                    case 3:
                        Assert.Fail();
                        break;

                    case 4: {
                            using var r = e.File.OpenText();
                            var data = r.ReadToEnd();
                            AssertEx.EqualMultiLineTextIgnoreNewline("abc\ndef\rghi\r\njkl", data);
                            called.Add(step);
                            ev.Reset();
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }

                ev.Set();
            };

            step += 1;
            using(var stream = filePath.AppendText()) {
                stream.WriteLine("abc");
            }
            ev.WaitOne();
            Assert.Contains(1, called);

            step += 1;
            using(var stream = filePath.AppendText()) {
                stream.WriteLine("def");
            }
            ev.WaitOne();
            Assert.Contains(2, called);

            test.Stop();
            step += 1;
            using(var stream = filePath.AppendText()) {
                stream.WriteLine("ghi");
            }
            Assert.DoesNotContain(3, called);

            test.Start();
            step += 1;
            using(var stream = filePath.AppendText()) {
                stream.WriteLine("jkl");
            }
            ev.WaitOne();
            Assert.Contains(4, called);
        }

        #endregion
    }
}
