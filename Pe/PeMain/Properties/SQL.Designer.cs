﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34209
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContentTypeTextNet.Pe.Application.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SQL {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SQL() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ContentTypeTextNet.Pe.Application.Properties.SQL", typeof(SQL).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   select
        ///    count(*) NUM
        ///from
        ///    SQLITE_MASTER
        ///where
        ///    NAME = :table_name
        ///    and
        ///    TYPE = &apos;table&apos;
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string CheckTable {
            get {
                return ResourceManager.GetString("CheckTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   create table M_NOTE (
        ///    NOTE_ID      integer  primary key,
        ///    CMN_ENABLED  integer  not null,
        ///    CMN_CREATE   text     not null,
        ///    CMN_UPDATE   text     not null,
        ///    NOTE_TITLE   text,
        ///    NOTE_TYPE    integer  not null
        ///)
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string CreateNoteMasterTable {
            get {
                return ResourceManager.GetString("CreateNoteMasterTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   create table T_NOTE_STYLE (
        ///    NOTE_ID         integer  primary key,
        ///    CMN_CREATE      text     not null,
        ///    CMN_UPDATE      text     not null,
        ///    FONT_FAMILY     text     not null,
        ///    FONT_SIZE       real     not null,
        ///    FONT_ITALIC     integer  not null,
        ///    FONT_BOLD       integer  not null,
        ///    COLOR_FORE      text     not null,
        ///    COLOR_BACK      text     not null,
        ///    WINDOW_VISIBLED integer  not null,
        ///    WINDOW_LOCKED   integer  not null,
        ///    WINDOW_TOPMOST  integer  not null,
        /// [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string CreateNoteStyleTransactionTable {
            get {
                return ResourceManager.GetString("CreateNoteStyleTransactionTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   create table T_NOTE (
        ///    NOTE_ID      integer  primary key,
        ///    CMN_CREATE   text     not null,
        ///    CMN_UPDATE   text     not null,
        ///    NOTE_BODY    text
        ///)
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string CreateNoteTransactionTable {
            get {
                return ResourceManager.GetString("CreateNoteTransactionTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   create table M_VERSION
        ///(
        ///    NAME     text primary key,
        ///    VERSION  integer not null
        ///)
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string CreateVersionMasterTable {
            get {
                return ResourceManager.GetString("CreateVersionMasterTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   update
        ///	M_NOTE
        ///set
        ///	CMN_ENABLED = :enabled,
        ///	CMN_UPDATE  = :update
        ///where
        ///	({ID_LIST})
        ///
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string EnabledSwitch {
            get {
                return ResourceManager.GetString("EnabledSwitch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   select
        ///    coalesce(max({id_column_name}), 0) MAX_ID,
        ///    coalesce(min({id_column_name}), 0) MIN_ID
        ///from
        ///    {table_name}
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string GetId {
            get {
                return ResourceManager.GetString("GetId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   select
        ///	CMN_ENABLED,
        ///	NOTE_ID,
        ///	NOTE_TITLE,
        ///	NOTE_TYPE,
        ///	NOTE_BODY,
        ///	WINDOW_VISIBLED,
        ///	WINDOW_LOCKED,
        ///	WINDOW_TOPMOST,
        ///	WINDOW_COMPACT,
        ///	WINDOW_POS_X,
        ///	WINDOW_POS_Y,
        ///	WINDOW_SIZE_W,
        ///	WINDOW_SIZE_H,
        ///	FONT_FAMILY,
        ///	FONT_SIZE,
        ///	FONT_ITALIC,
        ///	FONT_BOLD,
        ///	COLOR_FORE,
        ///	COLOR_BACK
        ///from
        ///	M_NOTE
        ///	inner join T_NOTE using(NOTE_ID)
        ///	inner join T_NOTE_STYLE using(NOTE_ID)
        ///order by
        ///	M_NOTE.CMN_CREATE
        ///
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string GetNoteItemList {
            get {
                return ResourceManager.GetString("GetNoteItemList", resourceCulture);
            }
        }
    }
}
