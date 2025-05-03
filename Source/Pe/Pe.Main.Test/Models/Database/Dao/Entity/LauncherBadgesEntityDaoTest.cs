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
using ContentTypeTextNet.Pe.Library.Database;
using System.Data.SQLite;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class LauncherBadgesEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testItem = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testBadge = Test.BuildDao<LauncherBadgesEntityDao>(AccessorKind.Main);

            var data = new LauncherItemData() {
                LauncherItemId = LauncherItemId.NewId(),
                Name = "Data",
                Kind = LauncherItemKind.File,
            };
            testItem.InsertLauncherItem(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Null(testBadge.SelectLauncherBadge(data.LauncherItemId));
            Assert.Throws<DatabaseManipulationException>(() => testBadge.UpdateLauncherBadge(data.LauncherItemId, BadgeData.CreateEmpty(), Test.DiContainer.Build<IDatabaseCommonStatus>()));

            var badgeData1 = new BadgeData() {
                IsVisible = true,
                Display = "Test",
                BadgeShape = BadgeShape.SolidSquare,
                Background = System.Windows.Media.Colors.Red,
            };
            testBadge.InsertLauncherBadge(data.LauncherItemId, badgeData1, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual1 = testBadge.SelectLauncherBadge(data.LauncherItemId);
            Assert.Equal(badgeData1, actual1);

            var exception = Assert.Throws<SQLiteException>(() => testBadge.InsertLauncherBadge(data.LauncherItemId, badgeData1, Test.DiContainer.Build<IDatabaseCommonStatus>()));
            exception.Message.Contains("UNIQUE constraint failed: LauncherBadges.LauncherItemId");

            var badgeData2 = new BadgeData() {
                IsVisible = false,
                Display = "Test2",
                BadgeShape = BadgeShape.Circle,
                Background = System.Windows.Media.Colors.Lime,
            };

            testBadge.UpdateLauncherBadge(data.LauncherItemId, badgeData2, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = testBadge.SelectLauncherBadge(data.LauncherItemId);
            Assert.Equal(badgeData2, actual2);

            Assert.True(testBadge.DeleteLauncherBadge(data.LauncherItemId));
            Assert.False(testBadge.DeleteLauncherBadge(data.LauncherItemId));
        }
    }
}
