using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationInformationCollector : PlatformInformationCollector
    {
        public ApplicationInformationCollector(EnvironmentParameters environmentParameters)
        {
            EnvironmentParameters = environmentParameters;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }

        public string Header { get; set; } = "____________" + Environment.NewLine;
        public string SubjectFormat { get; set; } = "{0} =================";
        public string Indent { get; set; } = "    ";

        #endregion

        #region function

        public virtual IList<PlatformInformationItem> GetApplication()
        {
            return new[] {
                new PlatformInformationItem(nameof(BuildStatus.Name), BuildStatus.Name),
                new PlatformInformationItem(nameof(BuildStatus.BuildType), BuildStatus.BuildType),
                new PlatformInformationItem(nameof(BuildStatus.Version), BuildStatus.Version),
                new PlatformInformationItem(nameof(BuildStatus.Revision), BuildStatus.Revision),
                new PlatformInformationItem(nameof(BuildStatus.Copyright), BuildStatus.Copyright),
            }.ToList();
        }

        public virtual IList<PlatformInformationItem> GetEnvironmentParameter()
        {
            var result = new List<PlatformInformationItem>();

            var dump = new ObjectDumper();
            var dumpItems = dump.Dump(EnvironmentParameters);
            void AddResult(string parent, IReadOnlyList<ObjectDumpItem> items)
            {
                foreach(var item in items) {
                    var key = string.IsNullOrWhiteSpace(item.MemberInfo.Name) ? parent : parent + "." + item.MemberInfo.Name;
                    if(item.Children.Any()) {
                        AddResult(key, item.Children);
                    } else {
                        var pi = new PlatformInformationItem(key, item.Value);
                        result.Add(pi);
                    }
                }
            }
            AddResult(nameof(EnvironmentParameters), dumpItems);

            return result;
        }

        public string GetShortInformation()
        {
            var items = new[] {
                new PlatformInformationItem("Software", BuildStatus.Name),
                new PlatformInformationItem("Version", BuildStatus.Version + "-" + BuildStatus.Revision),
                new PlatformInformationItem("BuildType", BuildStatus.BuildType),
                new PlatformInformationItem("Process", Environment.Is64BitProcess ? "64" : "32"),
                new PlatformInformationItem("Platform", Environment.Is64BitOperatingSystem ? "64" : "32"),
                new PlatformInformationItem("OS", Environment.OSVersion),
                new PlatformInformationItem("CLR", System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion()),
            };

            var sb = new StringBuilder(255);
            sb.Append(Header);
            foreach(var item in items) {
                sb.Append(Indent);
                sb.Append(item.Key);
                sb.Append(": ");
                sb.Append(item.Value);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string GetLongInformation()
        {
            var infos = new (string name, Func<IList<PlatformInformationItem>> func)[] {
                ("Application", GetApplication),
                ("EnvironmentParameter", GetEnvironmentParameter),
                ("CPU", GetCPU),
                ("OS", GetOS),
                ("Environment", GetEnvironment),
                ("EnvironmentVariables", GetEnvironmentVariables),
                ("Screen", GetScreen),
            };

            var sb = new StringBuilder(4 * 1024);
            sb.AppendLine(Header);
            foreach(var info in infos) {
                sb.Append(Indent);
                sb.AppendFormat(SubjectFormat, info.name);
                sb.AppendLine();

                var items = info.func();
                foreach(var item in items) {
                    sb.Append(Indent);
                    sb.Append(item.Key);
                    sb.Append(": ");
                    sb.Append(item.Value);
                    sb.AppendLine();
                }

                sb.Append(Indent);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion
    }
}
