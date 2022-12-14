using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    #region Dto
    public interface IReadOnlyDto: IReadOnlyTransfer
    { }

    public abstract class DtoBase: TransferBase, IReadOnlyDto
    { }

    #endregion

    #region common dto

    public interface IReadOnlyCreateDto: IReadOnlyDto
    {
        #region property

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime CreatedTimestamp { get; }
        string? CreatedAccount { get; }
        string? CreatedProgramName { get; }
        Version? CreatedProgramVersion { get; }

        #endregion
    }
    public interface IWritableCreateDto: IReadOnlyDto
    {
        #region property

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime CreatedTimestamp { get; set; }
        string? CreatedAccount { get; set; }
        string? CreatedProgramName { get; set; }
        Version? CreatedProgramVersion { get; set; }

        #endregion
    }

    public interface IReadOnlyUpdateDto: IReadOnlyDto
    {
        #region property

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime UpdatedTimestamp { get; }
        string? UpdatedAccount { get; }
        long UpdatedCount { get; }
        string? UpdatedProgramName { get; }
        Version? UpdatedProgramVersion { get; }

        #endregion
    }
    public interface IWritableUpdateDto: IReadOnlyDto
    {
        #region property

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime UpdatedTimestamp { get; set; }
        string? UpdatedAccount { get; set; }
        string? UpdatedProgramName { get; set; }
        Version? UpdatedProgramVersion { get; set; }
        long UpdatedCount { get; set; }

        #endregion
    }

    public abstract class CreateDtoBase: IReadOnlyCreateDto, IWritableCreateDto
    {
        #region IReadOnlyCreateDto

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime CreatedTimestamp { get; set; }
        public string? CreatedAccount { get; set; }
        public string? CreatedProgramName { get; set; }
        public Version? CreatedProgramVersion { get; set; }

        #endregion
    }
    public abstract class UpdateDtoBase: IReadOnlyUpdateDto, IWritableUpdateDto
    {
        #region IReadOnlyUpdateDto

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime UpdatedTimestamp { get; set; }
        public string? UpdatedAccount { get; set; }
        public string? UpdatedProgramName { get; set; }
        public Version? UpdatedProgramVersion { get; set; }
        public long UpdatedCount { get; set; }

        #endregion
    }
    public interface IReadOnlyCommonDto: IReadOnlyCreateDto, IReadOnlyUpdateDto
    { }

    public interface IWritableCommonDto: IWritableCreateDto, IWritableUpdateDto
    { }

    public abstract class CommonDtoBase: DtoBase, IReadOnlyCommonDto, IWritableCommonDto
    {
        #region IWritableCreateDto

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime CreatedTimestamp { get; set; }
        public string? CreatedAccount { get; set; }
        public string? CreatedProgramName { get; set; }
        public Version? CreatedProgramVersion { get; set; }

        #endregion

        #region IWritableUpdateDto

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime UpdatedTimestamp { get; set; }
        public string? UpdatedAccount { get; set; }
        public string? UpdatedProgramName { get; set; }
        public Version? UpdatedProgramVersion { get; set; }
        public long UpdatedCount { get; set; }

        #endregion
    }

    public interface IReadOnlyRowDtoBase: IReadOnlyCommonDto
    { }

    /// <summary>
    /// ???????????????????????????
    /// </summary>
    public abstract class RowDtoBase: CommonDtoBase, IReadOnlyRowDtoBase
    { }

    #endregion

    #region setup

    public interface IReadOnlySetupDto: IReadOnlyCommonDto
    {
        #region property

        /// <summary>
        /// ??????????????????????????????
        /// <para>?????????????????????????????????????????????????????????????????????</para>
        /// <para>??????(0.84.0??????)??????0.0.0.0??????</para>
        /// </summary>
        Version? LastVersion { get; }
        /// <summary>
        /// ????????????????????????
        /// </summary>
        Version? ExecuteVersion { get; }

        #endregion
    }

    public class SetupDto: CommonDtoBase, IReadOnlySetupDto
    {
        public SetupDto()
        { }

        #region IReadOnlySetupDto

        public Version? LastVersion { get; set; }
        public Version? ExecuteVersion { get; set; }

        #endregion
    }

    #endregion

}
