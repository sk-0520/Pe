using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class LauncherFilesEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var testItem = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testFile = Test.BuildDao<LauncherFilesEntityDao>(AccessorKind.Main);
            var item = new LauncherItemData() {
                LauncherItemId = LauncherItemId.NewId(),
                Name = "Data",
                Kind = LauncherItemKind.File,
            };
            testItem.InsertLauncherItem(item, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var file = new LauncherFileData() {
                Caption = "Caption",
                IsEnabledCustomEnvironmentVariable = true,
                IsEnabledStandardInputOutput = true,
                Option = "Option",
                Path = "Path",
                RunAdministrator = true,
                ShowMode = ShowMode.Normal,
                WorkDirectoryPath = "Dir",
            };
            testFile.InsertFile(item.LauncherItemId, file, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actualPath = testFile.SelectPath(item.LauncherItemId);
            Assert.Equal(file.Option, actualPath.Option);
            Assert.Equal(file.Path, actualPath.Path);
            Assert.Equal(file.WorkDirectoryPath, actualPath.WorkDirectoryPath);

            var actualFile = testFile.SelectFile(item.LauncherItemId);
            Assert.Equal(file.Option, actualFile.Option);
            Assert.Equal(file.Path, actualFile.Path);
            Assert.Equal(file.WorkDirectoryPath, actualFile.WorkDirectoryPath);
            // RunAdministrator は挿入時 False なのだ。。。なんでだこれ
            Assert.NotEqual(file.RunAdministrator, actualFile.RunAdministrator);
            Assert.Equal(file.ShowMode, actualFile.ShowMode);
            Assert.NotEqual(file.Caption, actualFile.Caption);
            // IsEnabledCustomEnvironmentVariable は挿入時 False なのだ。。。なんでだこれ
            Assert.NotEqual(file.IsEnabledCustomEnvironmentVariable, actualFile.IsEnabledCustomEnvironmentVariable);
            // IsEnabledStandardInputOutput は挿入時 False なのだ。。。なんでだこれ
            Assert.NotEqual(file.IsEnabledStandardInputOutput, actualFile.IsEnabledStandardInputOutput);

            var updateData = new LauncherFileData() {
                Path = "Path2",
                Option = "Option2",
                WorkDirectoryPath = "WorkDirectoryPath2",
                Caption = "Caption2",
                IsEnabledCustomEnvironmentVariable = false,
                IsEnabledStandardInputOutput = false,
                RunAdministrator = false,
                ShowMode = ShowMode.Maximized,
            };
            testFile.UpdateCustomizeLauncherFile(
                item.LauncherItemId,
                updateData,
                updateData,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testFile.DeleteFileByLauncherItemId(item.LauncherItemId);
            Assert.Throws<InvalidOperationException>(() => testFile.SelectFile(item.LauncherItemId));
        }

        [Theory]
        [InlineData(false, "X:\\dir\\Name.exe", "Z:\\dir\\Name.exe")]
        [InlineData(true, "X:\\dir\\Name.exe", "X:\\dir\\Name.exe")]
        [InlineData(true, "X:\\dir\\Name.exe", "x:\\DIR\\NAME.EXE")]
        [InlineData(true, "X:\\dir\\Name.exe", "x:/dir/Name.exe")]
        [InlineData(true, "X:/dir/Name.exe", "x:\\dir\\Name.exe")]
        [InlineData(true, "X:/dir/Name.exe", "x:/dir/Name.exe")]
        [InlineData(true, "X:\\dir", "x:\\dir\\")]
        [InlineData(true, "X:\\dir\\", "x:\\dir")]
        [InlineData(true, "X:/dir", "x:/dir/")]
        [InlineData(true, "X:/dir/", "x:/dir")]
        public void SelectSearchFileFromPath_Normal_Test(bool expectedSearchResult, string registerPath, string searchPath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var testItem = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testFile = Test.BuildDao<LauncherFilesEntityDao>(AccessorKind.Main);

            var item1 = new LauncherItemData() {
                LauncherItemId = LauncherItemId.NewId(),
                Name = "Data1",
                Kind = LauncherItemKind.File,
            };
            testItem.InsertLauncherItem(item1, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var file1 = new LauncherFileData() {
                Caption = "Caption",
                IsEnabledCustomEnvironmentVariable = true,
                IsEnabledStandardInputOutput = true,
                Option = "Option",
                Path = registerPath,
                RunAdministrator = true,
                ShowMode = ShowMode.Normal,
                WorkDirectoryPath = "Dir",
            };
            testFile.InsertFile(item1.LauncherItemId, file1, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actualExistsIds = testFile.SelectSearchFileFromPath(searchPath);
            if(expectedSearchResult) {
                // TODO: 複数件は datetime が制御できる実装ではないので気が向いたら対応する(テスト自体は別途作成)
                Assert.Single(actualExistsIds);
                Assert.Equal(item1.LauncherItemId, actualExistsIds.First());
            } else {
                Assert.Empty(actualExistsIds);
            }
        }

        #endregion
    }
}
