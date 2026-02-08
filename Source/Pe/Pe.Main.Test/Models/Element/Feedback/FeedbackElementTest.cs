using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Element.Feedback
{
    public class FeedbackElementTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Http | TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public async Task SuccessTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            var applicationConfiguration = Test.GetApplicationConfiguration(testIO);

            var mockCultureService = Substitute.For<ICultureService>();
            var mockOrderManager = Substitute.For<IOrderManager>();
            var mockLog = MockLog.Create();

            var mainDatabaseBarrier = Test.DiContainer.Build<IMainDatabaseBarrier>();
            var databaseStatementLoader = Test.DiContainer.Build<IDatabaseStatementLoader>();

            Test.MockHttpUserAgent
                .SendAsync(Arg.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Post), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = JsonContent.Create(new FeedbackResponse() {
                        Success = true,
                        Message = "Message",
                    })
                }))
            ;

            var test = new FeedbackElement(
                applicationConfiguration.Api,
                mainDatabaseBarrier,
                databaseStatementLoader,
                mockCultureService,
                mockOrderManager,
                Test.UserAgentManager,
                mockLog.Factory
            );
            await test.InitializeAsync(CancellationToken.None);

            await test.SendAsync(new Main.Models.Data.FeedbackInputData() {
                Kind = Main.Models.Data.FeedbackKind.Others,
                Subject = "Subject",
                Content = "Content",
            }, CancellationToken.None);

            Assert.Equal(RunningState.End, test.SendStatus.State);

            mockLog.VerifyMessage(LogLevel.Information, "送信完了", 1);
        }

        #endregion
    }
}
