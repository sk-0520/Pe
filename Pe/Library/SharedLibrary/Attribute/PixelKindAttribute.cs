﻿namespace ContentTypeTextNet.Library.SharedLibrary.Attribute
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	/// <summary>
	/// ピクセル情報を指定。
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple=false)]
	public class PixelKindAttribute : System.Attribute
	{
		/// <summary>
		/// ピクセル情報を指定。
		/// </summary>
		/// <param name="px"></param>
		public PixelKindAttribute(Px px)
		{
			Px = px;
		}

		#region property

		/// <summary>
		/// ピクセル情報。
		/// </summary>
		public Px Px { get; private set; }

		#endregion
	}
}
