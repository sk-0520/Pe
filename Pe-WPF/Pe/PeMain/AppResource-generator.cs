﻿namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	partial class AppResource
	{
		/*
		このソースは自動生成のため AppResource-generator.tt を編集すること。

		生成元フィールド数: 53
		*/
		#region Icon: Application

		/// <summary>
		/// [Icon] Applicationのリソースパスを取得。
		/// <para>/Resources/Icon/App.ico</para>
		/// </summary>
		public static string ApplicationPath
		{
			get { return application; }
		}

		/// <summary>
		/// [Icon] Applicationのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ApplicationPath: /Resources/Icon/App.ico</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetApplicationIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] Applicationのイメージソース(IconScale.Small)を取得。
		/// <para>内部的にはGetApplicationIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationPath: /Resources/Icon/App.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationIconSmall
		{
			get
			{
				return GetApplicationIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] Applicationのイメージソース(IconScale.Normal)を取得。
		/// <para>内部的にはGetApplicationIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationPath: /Resources/Icon/App.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationIconNormal
		{
			get
			{
				return GetApplicationIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] Applicationのイメージソース(IconScale.Big)を取得。
		/// <para>内部的にはGetApplicationIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationPath: /Resources/Icon/App.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationIconBig
		{
			get
			{
				return GetApplicationIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] Applicationのイメージソース(IconScale.Large)を取得。
		/// <para>内部的にはGetApplicationIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationPath: /Resources/Icon/App.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationIconLarge
		{
			get
			{
				return GetApplicationIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion Application
		#region Icon: NotFound

		/// <summary>
		/// [Icon] NotFoundのリソースパスを取得。
		/// <para>/Resources/Icon/NotFound.ico</para>
		/// </summary>
		public static string NotFoundPath
		{
			get { return notFound; }
		}

		/// <summary>
		/// [Icon] NotFoundのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>NotFoundPath: /Resources/Icon/NotFound.ico</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetNotFoundIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(NotFoundPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] NotFoundのイメージソース(IconScale.Small)を取得。
		/// <para>内部的にはGetNotFoundIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>NotFoundPath: /Resources/Icon/NotFound.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NotFoundIconSmall
		{
			get
			{
				return GetNotFoundIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] NotFoundのイメージソース(IconScale.Normal)を取得。
		/// <para>内部的にはGetNotFoundIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>NotFoundPath: /Resources/Icon/NotFound.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NotFoundIconNormal
		{
			get
			{
				return GetNotFoundIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] NotFoundのイメージソース(IconScale.Big)を取得。
		/// <para>内部的にはGetNotFoundIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>NotFoundPath: /Resources/Icon/NotFound.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NotFoundIconBig
		{
			get
			{
				return GetNotFoundIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] NotFoundのイメージソース(IconScale.Large)を取得。
		/// <para>内部的にはGetNotFoundIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>NotFoundPath: /Resources/Icon/NotFound.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NotFoundIconLarge
		{
			get
			{
				return GetNotFoundIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion NotFound
		#region Icon: LauncherToolbarMain

		/// <summary>
		/// [Icon] LauncherToolbarMainのリソースパスを取得。
		/// <para>/Resources/Icon/LauncherToolbarMain.ico</para>
		/// </summary>
		public static string LauncherToolbarMainPath
		{
			get { return launcherToolbarMain; }
		}

		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LauncherToolbarMainPath: /Resources/Icon/LauncherToolbarMain.ico</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetLauncherToolbarMainIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(LauncherToolbarMainPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース(IconScale.Small)を取得。
		/// <para>内部的にはGetLauncherToolbarMainIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>LauncherToolbarMainPath: /Resources/Icon/LauncherToolbarMain.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherToolbarMainIconSmall
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース(IconScale.Normal)を取得。
		/// <para>内部的にはGetLauncherToolbarMainIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>LauncherToolbarMainPath: /Resources/Icon/LauncherToolbarMain.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherToolbarMainIconNormal
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース(IconScale.Big)を取得。
		/// <para>内部的にはGetLauncherToolbarMainIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>LauncherToolbarMainPath: /Resources/Icon/LauncherToolbarMain.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherToolbarMainIconBig
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース(IconScale.Large)を取得。
		/// <para>内部的にはGetLauncherToolbarMainIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>LauncherToolbarMainPath: /Resources/Icon/LauncherToolbarMain.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherToolbarMainIconLarge
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion LauncherToolbarMain
		#region Icon: LauncherCommand

		/// <summary>
		/// [Icon] LauncherCommandのリソースパスを取得。
		/// <para>/Resources/Icon/LauncherCommand.ico</para>
		/// </summary>
		public static string LauncherCommandPath
		{
			get { return launcherCommand; }
		}

		/// <summary>
		/// [Icon] LauncherCommandのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LauncherCommandPath: /Resources/Icon/LauncherCommand.ico</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetLauncherCommandIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(LauncherCommandPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] LauncherCommandのイメージソース(IconScale.Small)を取得。
		/// <para>内部的にはGetLauncherCommandIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>LauncherCommandPath: /Resources/Icon/LauncherCommand.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherCommandIconSmall
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] LauncherCommandのイメージソース(IconScale.Normal)を取得。
		/// <para>内部的にはGetLauncherCommandIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>LauncherCommandPath: /Resources/Icon/LauncherCommand.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherCommandIconNormal
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] LauncherCommandのイメージソース(IconScale.Big)を取得。
		/// <para>内部的にはGetLauncherCommandIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>LauncherCommandPath: /Resources/Icon/LauncherCommand.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherCommandIconBig
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] LauncherCommandのイメージソース(IconScale.Large)を取得。
		/// <para>内部的にはGetLauncherCommandIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>LauncherCommandPath: /Resources/Icon/LauncherCommand.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherCommandIconLarge
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion LauncherCommand
		#region Icon: ApplicationTasktrayDebug

		/// <summary>
		/// [Icon] ApplicationTasktrayDebugのリソースパスを取得。
		/// <para>/Resources/Icon/Tasktray/App-debug.ico</para>
		/// </summary>
		public static string ApplicationTasktrayDebugPath
		{
			get { return applicationTasktrayDebug; }
		}

		/// <summary>
		/// [Icon] ApplicationTasktrayDebugのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ApplicationTasktrayDebugPath: /Resources/Icon/Tasktray/App-debug.ico</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetApplicationTasktrayDebugIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationTasktrayDebugPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] ApplicationTasktrayDebugのイメージソース(IconScale.Small)を取得。
		/// <para>内部的にはGetApplicationTasktrayDebugIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayDebugPath: /Resources/Icon/Tasktray/App-debug.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayDebugIconSmall
		{
			get
			{
				return GetApplicationTasktrayDebugIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayDebugのイメージソース(IconScale.Normal)を取得。
		/// <para>内部的にはGetApplicationTasktrayDebugIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayDebugPath: /Resources/Icon/Tasktray/App-debug.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayDebugIconNormal
		{
			get
			{
				return GetApplicationTasktrayDebugIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayDebugのイメージソース(IconScale.Big)を取得。
		/// <para>内部的にはGetApplicationTasktrayDebugIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayDebugPath: /Resources/Icon/Tasktray/App-debug.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayDebugIconBig
		{
			get
			{
				return GetApplicationTasktrayDebugIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayDebugのイメージソース(IconScale.Large)を取得。
		/// <para>内部的にはGetApplicationTasktrayDebugIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayDebugPath: /Resources/Icon/Tasktray/App-debug.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayDebugIconLarge
		{
			get
			{
				return GetApplicationTasktrayDebugIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion ApplicationTasktrayDebug
		#region Icon: ApplicationTasktrayBeta

		/// <summary>
		/// [Icon] ApplicationTasktrayBetaのリソースパスを取得。
		/// <para>/Resources/Icon/Tasktray/App-beta.ico</para>
		/// </summary>
		public static string ApplicationTasktrayBetaPath
		{
			get { return applicationTasktrayBeta; }
		}

		/// <summary>
		/// [Icon] ApplicationTasktrayBetaのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ApplicationTasktrayBetaPath: /Resources/Icon/Tasktray/App-beta.ico</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetApplicationTasktrayBetaIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationTasktrayBetaPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] ApplicationTasktrayBetaのイメージソース(IconScale.Small)を取得。
		/// <para>内部的にはGetApplicationTasktrayBetaIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayBetaPath: /Resources/Icon/Tasktray/App-beta.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayBetaIconSmall
		{
			get
			{
				return GetApplicationTasktrayBetaIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayBetaのイメージソース(IconScale.Normal)を取得。
		/// <para>内部的にはGetApplicationTasktrayBetaIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayBetaPath: /Resources/Icon/Tasktray/App-beta.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayBetaIconNormal
		{
			get
			{
				return GetApplicationTasktrayBetaIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayBetaのイメージソース(IconScale.Big)を取得。
		/// <para>内部的にはGetApplicationTasktrayBetaIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayBetaPath: /Resources/Icon/Tasktray/App-beta.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayBetaIconBig
		{
			get
			{
				return GetApplicationTasktrayBetaIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayBetaのイメージソース(IconScale.Large)を取得。
		/// <para>内部的にはGetApplicationTasktrayBetaIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayBetaPath: /Resources/Icon/Tasktray/App-beta.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayBetaIconLarge
		{
			get
			{
				return GetApplicationTasktrayBetaIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion ApplicationTasktrayBeta
		#region Icon: ApplicationTasktrayRelease

		/// <summary>
		/// [Icon] ApplicationTasktrayReleaseのリソースパスを取得。
		/// <para>/Resources/Icon/Tasktray/App-release.ico</para>
		/// </summary>
		public static string ApplicationTasktrayReleasePath
		{
			get { return applicationTasktrayRelease; }
		}

		/// <summary>
		/// [Icon] ApplicationTasktrayReleaseのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ApplicationTasktrayReleasePath: /Resources/Icon/Tasktray/App-release.ico</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetApplicationTasktrayReleaseIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationTasktrayReleasePath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] ApplicationTasktrayReleaseのイメージソース(IconScale.Small)を取得。
		/// <para>内部的にはGetApplicationTasktrayReleaseIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayReleasePath: /Resources/Icon/Tasktray/App-release.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayReleaseIconSmall
		{
			get
			{
				return GetApplicationTasktrayReleaseIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayReleaseのイメージソース(IconScale.Normal)を取得。
		/// <para>内部的にはGetApplicationTasktrayReleaseIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayReleasePath: /Resources/Icon/Tasktray/App-release.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayReleaseIconNormal
		{
			get
			{
				return GetApplicationTasktrayReleaseIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayReleaseのイメージソース(IconScale.Big)を取得。
		/// <para>内部的にはGetApplicationTasktrayReleaseIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayReleasePath: /Resources/Icon/Tasktray/App-release.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayReleaseIconBig
		{
			get
			{
				return GetApplicationTasktrayReleaseIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayReleaseのイメージソース(IconScale.Large)を取得。
		/// <para>内部的にはGetApplicationTasktrayReleaseIcon(IconScale, ILogger)の呼び出しを行う。</para>
		/// <para>ApplicationTasktrayReleasePath: /Resources/Icon/Tasktray/App-release.ico</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayReleaseIconLarge
		{
			get
			{
				return GetApplicationTasktrayReleaseIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion ApplicationTasktrayRelease
		#region Image: CommonFiltering

		/// <summary>
		/// [Image] CommonFilteringのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonFiltering.png</para>
		/// </summary>
		public static string CommonFilteringPath
		{
			get { return commonFiltering; }
		}

		/// <summary>
		/// [Image] CommonFilteringのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonFilteringPath: /Resources/Image/Common/CommonFiltering.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonFilteringImage
		{
			get { return GetImage(CommonFilteringPath); }
		}
		#endregion CommonFiltering
		#region Image: CommonCopy

		/// <summary>
		/// [Image] CommonCopyのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonCopy.png</para>
		/// </summary>
		public static string CommonCopyPath
		{
			get { return commonCopy; }
		}

		/// <summary>
		/// [Image] CommonCopyのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonCopyPath: /Resources/Image/Common/CommonCopy.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonCopyImage
		{
			get { return GetImage(CommonCopyPath); }
		}
		#endregion CommonCopy
		#region Image: CommonSend

		/// <summary>
		/// [Image] CommonSendのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonSend.png</para>
		/// </summary>
		public static string CommonSendPath
		{
			get { return commonSend; }
		}

		/// <summary>
		/// [Image] CommonSendのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonSendPath: /Resources/Image/Common/CommonSend.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonSendImage
		{
			get { return GetImage(CommonSendPath); }
		}
		#endregion CommonSend
		#region Image: CommonPin

		/// <summary>
		/// [Image] CommonPinのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonPin.png</para>
		/// </summary>
		public static string CommonPinPath
		{
			get { return commonPin; }
		}

		/// <summary>
		/// [Image] CommonPinのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonPinPath: /Resources/Image/Common/CommonPin.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonPinImage
		{
			get { return GetImage(CommonPinPath); }
		}
		#endregion CommonPin
		#region Image: CommonAdd

		/// <summary>
		/// [Image] CommonAddのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonAdd.png</para>
		/// </summary>
		public static string CommonAddPath
		{
			get { return commonAdd; }
		}

		/// <summary>
		/// [Image] CommonAddのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonAddPath: /Resources/Image/Common/CommonAdd.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonAddImage
		{
			get { return GetImage(CommonAddPath); }
		}
		#endregion CommonAdd
		#region Image: CommonRemove

		/// <summary>
		/// [Image] CommonRemoveのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonRemove.png</para>
		/// </summary>
		public static string CommonRemovePath
		{
			get { return commonRemove; }
		}

		/// <summary>
		/// [Image] CommonRemoveのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonRemovePath: /Resources/Image/Common/CommonRemove.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonRemoveImage
		{
			get { return GetImage(CommonRemovePath); }
		}
		#endregion CommonRemove
		#region Image: CommonSave

		/// <summary>
		/// [Image] CommonSaveのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonSave.png</para>
		/// </summary>
		public static string CommonSavePath
		{
			get { return commonSave; }
		}

		/// <summary>
		/// [Image] CommonSaveのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonSavePath: /Resources/Image/Common/CommonSave.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonSaveImage
		{
			get { return GetImage(CommonSavePath); }
		}
		#endregion CommonSave
		#region Image: CommonUsingClipboard

		/// <summary>
		/// [Image] CommonUsingClipboardのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonUsingClipboard.png</para>
		/// </summary>
		public static string CommonUsingClipboardPath
		{
			get { return commonUsingClipboard; }
		}

		/// <summary>
		/// [Image] CommonUsingClipboardのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonUsingClipboardPath: /Resources/Image/Common/CommonUsingClipboard.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonUsingClipboardImage
		{
			get { return GetImage(CommonUsingClipboardPath); }
		}
		#endregion CommonUsingClipboard
		#region Image: CommonUp

		/// <summary>
		/// [Image] CommonUpのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonUp.png</para>
		/// </summary>
		public static string CommonUpPath
		{
			get { return commonUp; }
		}

		/// <summary>
		/// [Image] CommonUpのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonUpPath: /Resources/Image/Common/CommonUp.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonUpImage
		{
			get { return GetImage(CommonUpPath); }
		}
		#endregion CommonUp
		#region Image: CommonDown

		/// <summary>
		/// [Image] CommonDownのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonDown.png</para>
		/// </summary>
		public static string CommonDownPath
		{
			get { return commonDown; }
		}

		/// <summary>
		/// [Image] CommonDownのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonDownPath: /Resources/Image/Common/CommonDown.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonDownImage
		{
			get { return GetImage(CommonDownPath); }
		}
		#endregion CommonDown
		#region Image: CommonFile

		/// <summary>
		/// [Image] CommonFileのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonFile.png</para>
		/// </summary>
		public static string CommonFilePath
		{
			get { return commonFile; }
		}

		/// <summary>
		/// [Image] CommonFileのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonFilePath: /Resources/Image/Common/CommonFile.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonFileImage
		{
			get { return GetImage(CommonFilePath); }
		}
		#endregion CommonFile
		#region Image: CommonFolder

		/// <summary>
		/// [Image] CommonFolderのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonFolder.png</para>
		/// </summary>
		public static string CommonFolderPath
		{
			get { return commonFolder; }
		}

		/// <summary>
		/// [Image] CommonFolderのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonFolderPath: /Resources/Image/Common/CommonFolder.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonFolderImage
		{
			get { return GetImage(CommonFolderPath); }
		}
		#endregion CommonFolder
		#region Image: CommonClear

		/// <summary>
		/// [Image] CommonClearのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonClear.png</para>
		/// </summary>
		public static string CommonClearPath
		{
			get { return commonClear; }
		}

		/// <summary>
		/// [Image] CommonClearのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonClearPath: /Resources/Image/Common/CommonClear.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonClearImage
		{
			get { return GetImage(CommonClearPath); }
		}
		#endregion CommonClear
		#region Image: CommonRefresh

		/// <summary>
		/// [Image] CommonRefreshのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonRefresh.png</para>
		/// </summary>
		public static string CommonRefreshPath
		{
			get { return commonRefresh; }
		}

		/// <summary>
		/// [Image] CommonRefreshのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonRefreshPath: /Resources/Image/Common/CommonRefresh.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonRefreshImage
		{
			get { return GetImage(CommonRefreshPath); }
		}
		#endregion CommonRefresh
		#region Image: CommonFontBold

		/// <summary>
		/// [Image] CommonFontBoldのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonFontBold.png</para>
		/// </summary>
		public static string CommonFontBoldPath
		{
			get { return commonFontBold; }
		}

		/// <summary>
		/// [Image] CommonFontBoldのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonFontBoldPath: /Resources/Image/Common/CommonFontBold.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonFontBoldImage
		{
			get { return GetImage(CommonFontBoldPath); }
		}
		#endregion CommonFontBold
		#region Image: CommonFontItalic

		/// <summary>
		/// [Image] CommonFontItalicのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonFontItalic.png</para>
		/// </summary>
		public static string CommonFontItalicPath
		{
			get { return commonFontItalic; }
		}

		/// <summary>
		/// [Image] CommonFontItalicのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonFontItalicPath: /Resources/Image/Common/CommonFontItalic.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonFontItalicImage
		{
			get { return GetImage(CommonFontItalicPath); }
		}
		#endregion CommonFontItalic
		#region Image: CommonRun

		/// <summary>
		/// [Image] CommonRunのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonRun.png</para>
		/// </summary>
		public static string CommonRunPath
		{
			get { return commonRun; }
		}

		/// <summary>
		/// [Image] CommonRunのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonRunPath: /Resources/Image/Common/CommonRun.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonRunImage
		{
			get { return GetImage(CommonRunPath); }
		}
		#endregion CommonRun
		#region Image: CommonCreate

		/// <summary>
		/// [Image] CommonCreateのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonCreate.png</para>
		/// </summary>
		public static string CommonCreatePath
		{
			get { return commonCreate; }
		}

		/// <summary>
		/// [Image] CommonCreateのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonCreatePath: /Resources/Image/Common/CommonCreate.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonCreateImage
		{
			get { return GetImage(CommonCreatePath); }
		}
		#endregion CommonCreate
		#region Image: CommonConfig

		/// <summary>
		/// [Image] CommonConfigのリソースパスを取得。
		/// <para>/Resources/Image/Common/CommonConfig.png</para>
		/// </summary>
		public static string CommonConfigPath
		{
			get { return commonConfig; }
		}

		/// <summary>
		/// [Image] CommonConfigのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>CommonConfigPath: /Resources/Image/Common/CommonConfig.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonConfigImage
		{
			get { return GetImage(CommonConfigPath); }
		}
		#endregion CommonConfig
		#region Image: ToolbarToolbar

		/// <summary>
		/// [Image] ToolbarToolbarのリソースパスを取得。
		/// <para>/Resources/Image/Toolbar/Toolbar.png</para>
		/// </summary>
		public static string ToolbarToolbarPath
		{
			get { return toolbarToolbar; }
		}

		/// <summary>
		/// [Image] ToolbarToolbarのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ToolbarToolbarPath: /Resources/Image/Toolbar/Toolbar.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ToolbarToolbarImage
		{
			get { return GetImage(ToolbarToolbarPath); }
		}
		#endregion ToolbarToolbar
		#region Image: StreamKill

		/// <summary>
		/// [Image] StreamKillのリソースパスを取得。
		/// <para>/Resources/Image/Stream/StreamKill.png</para>
		/// </summary>
		public static string StreamKillPath
		{
			get { return streamKill; }
		}

		/// <summary>
		/// [Image] StreamKillのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>StreamKillPath: /Resources/Image/Stream/StreamKill.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource StreamKillImage
		{
			get { return GetImage(StreamKillPath); }
		}
		#endregion StreamKill
		#region Image: NoteNote

		/// <summary>
		/// [Image] NoteNoteのリソースパスを取得。
		/// <para>/Resources/Image/Note/Note.png</para>
		/// </summary>
		public static string NoteNotePath
		{
			get { return noteNote; }
		}

		/// <summary>
		/// [Image] NoteNoteのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>NoteNotePath: /Resources/Image/Note/Note.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NoteNoteImage
		{
			get { return GetImage(NoteNotePath); }
		}
		#endregion NoteNote
		#region Image: TemplateTemplate

		/// <summary>
		/// [Image] TemplateTemplateのリソースパスを取得。
		/// <para>/Resources/Image/Template/Template.png</para>
		/// </summary>
		public static string TemplateTemplatePath
		{
			get { return templateTemplate; }
		}

		/// <summary>
		/// [Image] TemplateTemplateのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>TemplateTemplatePath: /Resources/Image/Template/Template.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource TemplateTemplateImage
		{
			get { return GetImage(TemplateTemplatePath); }
		}
		#endregion TemplateTemplate
		#region Image: TemplatePlain

		/// <summary>
		/// [Image] TemplatePlainのリソースパスを取得。
		/// <para>/Resources/Image/Template/TemplatePlain.png</para>
		/// </summary>
		public static string TemplatePlainPath
		{
			get { return templatePlain; }
		}

		/// <summary>
		/// [Image] TemplatePlainのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>TemplatePlainPath: /Resources/Image/Template/TemplatePlain.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource TemplatePlainImage
		{
			get { return GetImage(TemplatePlainPath); }
		}
		#endregion TemplatePlain
		#region Image: TemplateReplace

		/// <summary>
		/// [Image] TemplateReplaceのリソースパスを取得。
		/// <para>/Resources/Image/Template/TemplateReplace.png</para>
		/// </summary>
		public static string TemplateReplacePath
		{
			get { return templateReplace; }
		}

		/// <summary>
		/// [Image] TemplateReplaceのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>TemplateReplacePath: /Resources/Image/Template/TemplateReplace.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource TemplateReplaceImage
		{
			get { return GetImage(TemplateReplacePath); }
		}
		#endregion TemplateReplace
		#region Image: TemplateProgrammable

		/// <summary>
		/// [Image] TemplateProgrammableのリソースパスを取得。
		/// <para>/Resources/Image/Template/TemplateProgrammable.png</para>
		/// </summary>
		public static string TemplateProgrammablePath
		{
			get { return templateProgrammable; }
		}

		/// <summary>
		/// [Image] TemplateProgrammableのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>TemplateProgrammablePath: /Resources/Image/Template/TemplateProgrammable.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource TemplateProgrammableImage
		{
			get { return GetImage(TemplateProgrammablePath); }
		}
		#endregion TemplateProgrammable
		#region Image: ClipboardClipboard

		/// <summary>
		/// [Image] ClipboardClipboardのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/Clipboard.png</para>
		/// </summary>
		public static string ClipboardClipboardPath
		{
			get { return clipboardClipboard; }
		}

		/// <summary>
		/// [Image] ClipboardClipboardのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardClipboardPath: /Resources/Image/Clipboard/Clipboard.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardClipboardImage
		{
			get { return GetImage(ClipboardClipboardPath); }
		}
		#endregion ClipboardClipboard
		#region Image: ClipboardClear

		/// <summary>
		/// [Image] ClipboardClearのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/ClipboardClear.png</para>
		/// </summary>
		public static string ClipboardClearPath
		{
			get { return clipboardClear; }
		}

		/// <summary>
		/// [Image] ClipboardClearのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardClearPath: /Resources/Image/Clipboard/ClipboardClear.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardClearImage
		{
			get { return GetImage(ClipboardClearPath); }
		}
		#endregion ClipboardClear
		#region Image: ClipboardTextFormat

		/// <summary>
		/// [Image] ClipboardTextFormatのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/ClipboardText.png</para>
		/// </summary>
		public static string ClipboardTextFormatPath
		{
			get { return clipboardTextFormat; }
		}

		/// <summary>
		/// [Image] ClipboardTextFormatのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardTextFormatPath: /Resources/Image/Clipboard/ClipboardText.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardTextFormatImage
		{
			get { return GetImage(ClipboardTextFormatPath); }
		}
		#endregion ClipboardTextFormat
		#region Image: ClipboardHtml

		/// <summary>
		/// [Image] ClipboardHtmlのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/ClipboardHtml.png</para>
		/// </summary>
		public static string ClipboardHtmlPath
		{
			get { return clipboardHtml; }
		}

		/// <summary>
		/// [Image] ClipboardHtmlのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardHtmlPath: /Resources/Image/Clipboard/ClipboardHtml.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardHtmlImage
		{
			get { return GetImage(ClipboardHtmlPath); }
		}
		#endregion ClipboardHtml
		#region Image: ClipboardRichTextFormat

		/// <summary>
		/// [Image] ClipboardRichTextFormatのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/ClipboardRichTextFormat.png</para>
		/// </summary>
		public static string ClipboardRichTextFormatPath
		{
			get { return clipboardRichTextFormat; }
		}

		/// <summary>
		/// [Image] ClipboardRichTextFormatのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardRichTextFormatPath: /Resources/Image/Clipboard/ClipboardRichTextFormat.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardRichTextFormatImage
		{
			get { return GetImage(ClipboardRichTextFormatPath); }
		}
		#endregion ClipboardRichTextFormat
		#region Image: ClipboardImage

		/// <summary>
		/// [Image] ClipboardImageのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/ClipboardImage.png</para>
		/// </summary>
		public static string ClipboardImagePath
		{
			get { return clipboardImage; }
		}

		/// <summary>
		/// [Image] ClipboardImageのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardImagePath: /Resources/Image/Clipboard/ClipboardImage.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardImageImage
		{
			get { return GetImage(ClipboardImagePath); }
		}
		#endregion ClipboardImage
		#region Image: ClipboardFile

		/// <summary>
		/// [Image] ClipboardFileのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/ClipboardFile.png</para>
		/// </summary>
		public static string ClipboardFilePath
		{
			get { return clipboardFile; }
		}

		/// <summary>
		/// [Image] ClipboardFileのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardFilePath: /Resources/Image/Clipboard/ClipboardFile.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardFileImage
		{
			get { return GetImage(ClipboardFilePath); }
		}
		#endregion ClipboardFile
		#region Image: ClipboardImageFit

		/// <summary>
		/// [Image] ClipboardImageFitのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/ClipboardImageFit.png</para>
		/// </summary>
		public static string ClipboardImageFitPath
		{
			get { return clipboardImageFit; }
		}

		/// <summary>
		/// [Image] ClipboardImageFitのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardImageFitPath: /Resources/Image/Clipboard/ClipboardImageFit.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardImageFitImage
		{
			get { return GetImage(ClipboardImageFitPath); }
		}
		#endregion ClipboardImageFit
		#region Image: ClipboardImageRaw

		/// <summary>
		/// [Image] ClipboardImageRawのリソースパスを取得。
		/// <para>/Resources/Image/Clipboard/ClipboardImageRaw.png</para>
		/// </summary>
		public static string ClipboardImageRawPath
		{
			get { return clipboardImageRaw; }
		}

		/// <summary>
		/// [Image] ClipboardImageRawのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>ClipboardImageRawPath: /Resources/Image/Clipboard/ClipboardImageRaw.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardImageRawImage
		{
			get { return GetImage(ClipboardImageRawPath); }
		}
		#endregion ClipboardImageRaw
		#region Image: WindowList

		/// <summary>
		/// [Image] WindowListのリソースパスを取得。
		/// <para>/Resources/Image/Window/WindowList.png</para>
		/// </summary>
		public static string WindowListPath
		{
			get { return windowList; }
		}

		/// <summary>
		/// [Image] WindowListのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>WindowListPath: /Resources/Image/Window/WindowList.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource WindowListImage
		{
			get { return GetImage(WindowListPath); }
		}
		#endregion WindowList
		#region Image: WindowLoad

		/// <summary>
		/// [Image] WindowLoadのリソースパスを取得。
		/// <para>/Resources/Image/Window/WindowLoad.png</para>
		/// </summary>
		public static string WindowLoadPath
		{
			get { return windowLoad; }
		}

		/// <summary>
		/// [Image] WindowLoadのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>WindowLoadPath: /Resources/Image/Window/WindowLoad.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource WindowLoadImage
		{
			get { return GetImage(WindowLoadPath); }
		}
		#endregion WindowLoad
		#region Image: WindowSave

		/// <summary>
		/// [Image] WindowSaveのリソースパスを取得。
		/// <para>/Resources/Image/Window/WindowSave.png</para>
		/// </summary>
		public static string WindowSavePath
		{
			get { return windowSave; }
		}

		/// <summary>
		/// [Image] WindowSaveのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>WindowSavePath: /Resources/Image/Window/WindowSave.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource WindowSaveImage
		{
			get { return GetImage(WindowSavePath); }
		}
		#endregion WindowSave
		#region Image: LogLog

		/// <summary>
		/// [Image] LogLogのリソースパスを取得。
		/// <para>/Resources/Image/Log/Log.png</para>
		/// </summary>
		public static string LogLogPath
		{
			get { return logLog; }
		}

		/// <summary>
		/// [Image] LogLogのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LogLogPath: /Resources/Image/Log/Log.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LogLogImage
		{
			get { return GetImage(LogLogPath); }
		}
		#endregion LogLog
		#region Image: LogDebug

		/// <summary>
		/// [Image] LogDebugのリソースパスを取得。
		/// <para>/Resources/Image/Log/LogDebug.png</para>
		/// </summary>
		public static string LogDebugPath
		{
			get { return logDebug; }
		}

		/// <summary>
		/// [Image] LogDebugのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LogDebugPath: /Resources/Image/Log/LogDebug.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LogDebugImage
		{
			get { return GetImage(LogDebugPath); }
		}
		#endregion LogDebug
		#region Image: LogTrace

		/// <summary>
		/// [Image] LogTraceのリソースパスを取得。
		/// <para>/Resources/Image/Log/LogTrace.png</para>
		/// </summary>
		public static string LogTracePath
		{
			get { return logTrace; }
		}

		/// <summary>
		/// [Image] LogTraceのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LogTracePath: /Resources/Image/Log/LogTrace.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LogTraceImage
		{
			get { return GetImage(LogTracePath); }
		}
		#endregion LogTrace
		#region Image: LogInformation

		/// <summary>
		/// [Image] LogInformationのリソースパスを取得。
		/// <para>/Resources/Image/Log/LogInformation.png</para>
		/// </summary>
		public static string LogInformationPath
		{
			get { return logInformation; }
		}

		/// <summary>
		/// [Image] LogInformationのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LogInformationPath: /Resources/Image/Log/LogInformation.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LogInformationImage
		{
			get { return GetImage(LogInformationPath); }
		}
		#endregion LogInformation
		#region Image: LogWarning

		/// <summary>
		/// [Image] LogWarningのリソースパスを取得。
		/// <para>/Resources/Image/Log/LogWarning.png</para>
		/// </summary>
		public static string LogWarningPath
		{
			get { return logWarning; }
		}

		/// <summary>
		/// [Image] LogWarningのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LogWarningPath: /Resources/Image/Log/LogWarning.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LogWarningImage
		{
			get { return GetImage(LogWarningPath); }
		}
		#endregion LogWarning
		#region Image: LogError

		/// <summary>
		/// [Image] LogErrorのリソースパスを取得。
		/// <para>/Resources/Image/Log/LogError.png</para>
		/// </summary>
		public static string LogErrorPath
		{
			get { return logError; }
		}

		/// <summary>
		/// [Image] LogErrorのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LogErrorPath: /Resources/Image/Log/LogError.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LogErrorImage
		{
			get { return GetImage(LogErrorPath); }
		}
		#endregion LogError
		#region Image: LogFatal

		/// <summary>
		/// [Image] LogFatalのリソースパスを取得。
		/// <para>/Resources/Image/Log/LogFatal.png</para>
		/// </summary>
		public static string LogFatalPath
		{
			get { return logFatal; }
		}

		/// <summary>
		/// [Image] LogFatalのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>LogFatalPath: /Resources/Image/Log/LogFatal.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LogFatalImage
		{
			get { return GetImage(LogFatalPath); }
		}
		#endregion LogFatal
		#region Image: SettingSetting

		/// <summary>
		/// [Image] SettingSettingのリソースパスを取得。
		/// <para>/Resources/Image/Setting/Setting.png</para>
		/// </summary>
		public static string SettingSettingPath
		{
			get { return settingSetting; }
		}

		/// <summary>
		/// [Image] SettingSettingのイメージソースを取得。
		/// <para>初回のみ生成を行う。</para>
		/// <para>SettingSettingPath: /Resources/Image/Setting/Setting.png</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource SettingSettingImage
		{
			get { return GetImage(SettingSettingPath); }
		}
		#endregion SettingSetting
	}
}

