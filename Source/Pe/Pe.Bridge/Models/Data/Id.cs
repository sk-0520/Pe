
// <auto-generated>
// [T4] build 2022-03-26 14:00:00Z(UTC)
// </auto-generated>
using System.Text.Json.Serialization;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// ランチャーアイテムID。
    /// </summary>
    public readonly record struct LauncherItemId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public LauncherItemId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static LauncherItemId Empty
        {
            get
            {
                return new LauncherItemId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        ///  <see cref="LauncherItemId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out LauncherItemId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new LauncherItemId(val);
                return true;
            }

            result = default;
            return false;
        }

        public static LauncherItemId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new LauncherItemId(id);
        }

        #endregion

        #region object

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
    /// <summary>
    /// 資格情報ID。
    /// </summary>
    public readonly record struct CredentialIdId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public CredentialIdId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static CredentialIdId Empty
        {
            get
            {
                return new CredentialIdId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        ///  <see cref="CredentialIdId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out CredentialIdId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new CredentialIdId(val);
                return true;
            }

            result = default;
            return false;
        }

        public static CredentialIdId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new CredentialIdId(id);
        }

        #endregion

        #region object

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
    /// <summary>
    /// ランチャーツールバーID。
    /// </summary>
    public readonly record struct LauncherToolbarId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public LauncherToolbarId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static LauncherToolbarId Empty
        {
            get
            {
                return new LauncherToolbarId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        ///  <see cref="LauncherToolbarId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out LauncherToolbarId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new LauncherToolbarId(val);
                return true;
            }

            result = default;
            return false;
        }

        public static LauncherToolbarId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new LauncherToolbarId(id);
        }

        #endregion

        #region object

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
    /// <summary>
    /// フォントID。
    /// </summary>
    public readonly record struct FontId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public FontId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static FontId Empty
        {
            get
            {
                return new FontId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        ///  <see cref="FontId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out FontId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new FontId(val);
                return true;
            }

            result = default;
            return false;
        }

        public static FontId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new FontId(id);
        }

        #endregion

        #region object

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
    /// <summary>
    /// ランチャーグループID。
    /// </summary>
    public readonly record struct LauncherGroupId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public LauncherGroupId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static LauncherGroupId Empty
        {
            get
            {
                return new LauncherGroupId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        ///  <see cref="LauncherGroupId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out LauncherGroupId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new LauncherGroupId(val);
                return true;
            }

            result = default;
            return false;
        }

        public static LauncherGroupId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new LauncherGroupId(id);
        }

        #endregion

        #region object

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
    /// <summary>
    /// ノートID。
    /// </summary>
    public readonly record struct NoteId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public NoteId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static NoteId Empty
        {
            get
            {
                return new NoteId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        ///  <see cref="NoteId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out NoteId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new NoteId(val);
                return true;
            }

            result = default;
            return false;
        }

        public static NoteId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new NoteId(id);
        }

        #endregion

        #region object

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
    /// <summary>
    /// ノートファイルID。
    /// </summary>
    public readonly record struct NoteFileId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public NoteFileId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static NoteFileId Empty
        {
            get
            {
                return new NoteFileId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        ///  <see cref="NoteFileId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out NoteFileId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new NoteFileId(val);
                return true;
            }

            result = default;
            return false;
        }

        public static NoteFileId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new NoteFileId(id);
        }

        #endregion

        #region object

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
    /// <summary>
    /// キーアクションID。
    /// </summary>
    public readonly record struct KeyActionId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public KeyActionId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static KeyActionId Empty
        {
            get
            {
                return new KeyActionId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        ///  <see cref="KeyActionId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out KeyActionId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new KeyActionId(val);
                return true;
            }

            result = default;
            return false;
        }

        public static KeyActionId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new KeyActionId(id);
        }

        #endregion

        #region object

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
