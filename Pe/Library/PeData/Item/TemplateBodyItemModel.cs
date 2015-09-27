﻿namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	/// <summary>
	/// テンプレートインデックスのボディ部。
	/// </summary>
	[DataContract, Serializable]
	public class TemplateBodyItemModel : IndexBodyItemModelBase
	{
		/// <summary>
		/// 対象文字列。
		/// </summary>
		[DataMember]
		public string Source { get; set; }

		#region IndexBodyItemModelBase

		public override IndexKind IndexKind { get { return IndexKind.Template; } }

		public override void DeepCloneTo(IDeepClone target)
		{
			base.DeepCloneTo(target);

			var obj = (TemplateBodyItemModel)target;
			obj.Source = Source;
		}

		public override IDeepClone DeepClone()
		{
			var result = new TemplateBodyItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
