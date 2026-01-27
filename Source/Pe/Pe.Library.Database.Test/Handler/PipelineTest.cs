using ContentTypeTextNet.Pe.Library.Database.Handler;
using NSubstitute;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test.Handler
{
    public class StatementPipelineTest
    {
        #region function

        [Fact]
        public void Build_Use_Inner_Middle_Outer_Test()
        {
            var middlewareOuter = Substitute.For<IStatementMiddleware>();
            middlewareOuter
                .Next(Arg.Any<IStatementHandler>(), Arg.Any<string>())
                .Returns(callInfo => {
                    var handler = callInfo.Arg<IStatementHandler>();
                    var input = callInfo.Arg<string>();
                    return handler.Invoke(input + "+Outer");
                })
            ;
            var middlewareMiddle = Substitute.For<IStatementMiddleware>();
            middlewareMiddle
                .Next(Arg.Any<IStatementHandler>(), Arg.Any<string>())
                .Returns(callInfo => {
                    var handler = callInfo.Arg<IStatementHandler>();
                    var input = callInfo.Arg<string>();
                    return handler.Invoke(input + "+Middle");
                })
            ;
            var middlewareInner = Substitute.For<IStatementMiddleware>();
            middlewareInner
                .Next(Arg.Any<IStatementHandler>(), Arg.Any<string>())
                  .Returns(callInfo => {
                      var handler = callInfo.Arg<IStatementHandler>();
                      var input = callInfo.Arg<string>();
                      return handler.Invoke(input + "+Inner");
                  })
            ;
            var process = Substitute.For<IStatementHandler>();
            process.Invoke(Arg.Any<string>())
                .Returns(callInfo => {
                    var input = callInfo.Arg<string>();
                    return input + "+Process";
                })
            ;
            var pipeline = new StatementPipeline();
            pipeline.Use(middlewareOuter);
            pipeline.Use(middlewareMiddle);
            pipeline.Use(middlewareInner);
            var handler = pipeline.Build(process);
            var actual = handler.Invoke("input");
            Assert.Equal("input+Outer+Middle+Inner+Process", actual);
        }

        [Fact]
        public void Build_UseRange_Inner_Middle_Outer_Test()
        {
            var middlewareOuter = Substitute.For<IStatementMiddleware>();
            middlewareOuter
                .Next(Arg.Any<IStatementHandler>(), Arg.Any<string>())
                .Returns(callInfo => {
                    var handler = callInfo.Arg<IStatementHandler>();
                    var input = callInfo.Arg<string>();
                    return handler.Invoke(input + "+Outer");
                })
            ;
            var middlewareMiddle = Substitute.For<IStatementMiddleware>();
            middlewareMiddle
                .Next(Arg.Any<IStatementHandler>(), Arg.Any<string>())
                .Returns(callInfo => {
                    var handler = callInfo.Arg<IStatementHandler>();
                    var input = callInfo.Arg<string>();
                    return handler.Invoke(input + "+Middle");
                })
            ;
            var middlewareInner = Substitute.For<IStatementMiddleware>();
            middlewareInner
                .Next(Arg.Any<IStatementHandler>(), Arg.Any<string>())
                  .Returns(callInfo => {
                      var handler = callInfo.Arg<IStatementHandler>();
                      var input = callInfo.Arg<string>();
                      return handler.Invoke(input + "+Inner");
                  })
            ;
            var process = Substitute.For<IStatementHandler>();
            process.Invoke(Arg.Any<string>())
                .Returns(callInfo => {
                    var input = callInfo.Arg<string>();
                    return input + "+Process";
                })
            ;
            var pipeline = new StatementPipeline();
            pipeline.UseRange([
                middlewareOuter,
                middlewareMiddle,
                middlewareInner,
            ]);
            var handler = pipeline.Build(process);
            var actual = handler.Invoke("input");
            Assert.Equal("input+Outer+Middle+Inner+Process", actual);
        }

        #endregion
    }
}
