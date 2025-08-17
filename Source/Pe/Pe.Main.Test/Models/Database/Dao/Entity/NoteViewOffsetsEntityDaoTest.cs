using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class NoteViewOffsetsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function


        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var testNoteViewOffsets = Test.BuildDao<NoteViewOffsetsEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
                FontId = FontId.NewId(),
                ScreenName = "ScreenName",
                Title = "Title",
                LayoutKind = NoteLayoutKind.Absolute,
                IsVisible = true,
                ForegroundColor = Colors.Red,
                BackgroundColor = Colors.Lime,
                IsLocked = true,
                IsTopmost = true,
                TextWrap = true,
                ContentKind = NoteContentKind.RichText,
                HiddenMode = NoteHiddenMode.Compact,
                CaptionPosition = NoteCaptionPosition.Left,
            };

            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testFonts.InsertFont(
                data.FontId,
                new FontData() {
                    FamilyName = "FamilyName",
                    Size = 12,
                    IsBold = false,
                    IsItalic = false,
                    IsUnderline = false,
                    IsStrikeThrough = false,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.Null(testNoteViewOffsets.SelectNoteViewOffset(data.NoteId));

            testNoteViewOffsets.InsertNoteViewOffset(
                data.NoteId,
                new NoteViewOffsetData() {
                    X = 100,
                    Y = 200,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.Throws<SQLiteException>(() => {
                testNoteViewOffsets.InsertNoteViewOffset(
                    data.NoteId,
                    new NoteViewOffsetData() {
                        X = 1000,
                        Y = 2000,
                    },
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                );
            });

            var actual = testNoteViewOffsets.SelectNoteViewOffset(data.NoteId);
            Assert.NotNull(actual);
            Assert.Equal(100, actual.X);
            Assert.Equal(200, actual.Y);

            Assert.True(testNoteViewOffsets.DeleteNoteViewOffset(data.NoteId));
            Assert.False(testNoteViewOffsets.DeleteNoteViewOffset(data.NoteId));
        }

        [Fact]
        public void InsertNoteViewOffset_Throw_FK()
        {
            var testNoteViewOffsets = Test.BuildDao<NoteViewOffsetsEntityDao>(AccessorKind.Main);

            Assert.Throws<SQLiteException>(() => {
                testNoteViewOffsets.InsertNoteViewOffset(
                    NoteId.Empty,
                    new NoteViewOffsetData() {
                        X = 0,
                        Y = 0,
                    },
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                );
            });
        }


        #endregion
    }
}
