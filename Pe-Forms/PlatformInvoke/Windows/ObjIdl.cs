﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows
{
	[ComImport, Guid("0000010c-0000-0000-c000-000000000046"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersist
	{
		[PreserveSig]
		void GetClassID(out Guid pClassID);
	}

	[ComImport, Guid("0000010b-0000-0000-C000-000000000046"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistFile: IPersist
	{
		new void GetClassID(out Guid pClassID);
		[PreserveSig]
		int IsDirty();

		[PreserveSig]
		void Load(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, 
			uint dwMode
		);

		[PreserveSig]
		void Save(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
			[In, MarshalAs(UnmanagedType.Bool)] bool fRemember
		);

		[PreserveSig]
		void SaveCompleted([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		[PreserveSig]
		void GetCurFile([In, MarshalAs(UnmanagedType.LPWStr)] string ppszFileName);
	}

}
