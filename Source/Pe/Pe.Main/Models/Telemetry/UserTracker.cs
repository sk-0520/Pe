using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Telemetry
{
    /// <summary>
    /// ユーザー操作を追跡。
    /// </summary>
    public interface IUserTracker
    {
        #region function

        /// <inheritdoc cref="TrackAsync(string, TrackProperties, CancellationToken, string, string, int)"/>
        Task TrackAsync(string eventName, CancellationToken cancellationToken = default, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        /// <summary>
        /// ユーザーが操作したイベントを記録。
        /// </summary>
        /// <param name="eventName">イベント名。</param>
        /// <param name="properties">プロパティ。</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <param name="callerMemberName">[明示的指定は不要] メソッドの呼び出し元のメソッド名またはプロパティ名。<seealso cref="CallerMemberNameAttribute"/></param>
        /// <param name="callerFilePath">[明示的指定は不要] 呼び出し元を格納するソース ファイルの完全パス。<seealso cref="CallerFilePathAttribute"/></param>
        /// <param name="callerLineNumber">[明示的指定は不要] ソース ファイル内でメソッドが呼び出される位置の行番号。<seealso cref="CallerLineNumberAttribute"/></param>
        /// <returns></returns>
        Task TrackAsync(string eventName, TrackProperties properties, CancellationToken cancellationToken = default, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        #endregion
    }

    /// <inheritdoc cref="IUserTracker"/>
    internal class UserTracker: TelemeterBase, IUserTracker
    {
        public UserTracker(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region function

        private Task TrackCoreAsync(string eventName, TrackProperties properties, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            Logger.LogTrace("{0}: {1}, {2} {3}[{4}]", eventName, properties, callerMemberName, callerFilePath, callerLineNumber);
            return Task.CompletedTask;
        }

        #endregion

        #region IUserTracker

        /// <inheritdoc cref="IUserTracker.TrackAsync(string, CancellationToken, string, string, int)"/>
        public Task TrackAsync(string eventName, CancellationToken cancellationToken = default, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => TrackCoreAsync(eventName, TrackProperties.Empty, callerMemberName, callerFilePath, callerLineNumber);
        /// <inheritdoc cref="IUserTracker.TrackAsync(string, TrackProperties, CancellationToken, string, string, int)"/>
        public Task TrackAsync(string eventName, TrackProperties properties, CancellationToken cancellationToken = default, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => TrackCoreAsync(eventName, properties, callerMemberName, callerFilePath, callerLineNumber);

        #endregion
    }

    internal static class IUserTrackerExtensions
    {
        #region function

        /// <summary>
        /// 非同期を意識せずにユーザー操作記録。
        /// <inheritdoc cref="IUserTracker.TrackAsync(string, CancellationToken, string, string, int)"/>
        /// </summary>
        public static void Track(this IUserTracker tracker, string eventName, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            tracker.TrackAsync(eventName, CancellationToken.None, callerMemberName, callerFilePath, callerLineNumber);
        }

        /// <summary>
        /// 非同期を意識せずにユーザー操作記録。
        /// <inheritdoc cref="IUserTracker.TrackAsync(string, TrackProperties, CancellationToken, string, string, int)"/>
        /// </summary>
        public static void Track(this IUserTracker tracker, string eventName, TrackProperties properties, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            tracker.TrackAsync(eventName, properties, CancellationToken.None, callerMemberName, callerFilePath, callerLineNumber);
        }

        #endregion
    }

    public class UserTrackProperties: TrackProperties
    {
        public UserTrackProperties() { }

        #region function

        public static UserTrackProperties CreateExecuteLauncherItem(LauncherItemId launcherItemId, LauncherItemKind launcherItemKind, LauncherExecuteSource launcherExecuteSource)
        {
            var result = new UserTrackProperties() {
                [nameof(launcherItemId)] = launcherItemId.ToString(),
                [nameof(launcherItemKind)] = launcherItemKind.ToString(),
                [nameof(launcherExecuteSource)] = launcherExecuteSource.ToString()
            };
            return result;
        }

        #endregion
    }
}
