using ContentTypeTextNet.Pe.Library.Database.Handler;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test.Handler
{
    public class StatementPipelineTest
    {
        #region function

        [Fact]
        public void Build_Use_Inner_Middle_Outer_Test()
        {
            var middlewareOuter = new Mock<IStatementMiddleware>();
            middlewareOuter.Name = nameof(middlewareOuter);
            middlewareOuter
                .Setup(a => a.Next(It.IsAny<IStatementHandler>(), It.IsAny<string>()))
                .Returns((IStatementHandler handler, string input) => handler.Handle(input + "+Outer"))
            ;
            var middlewareMiddle = new Mock<IStatementMiddleware>();
            middlewareMiddle.Name = nameof(middlewareMiddle);
            middlewareMiddle
                .Setup(a => a.Next(It.IsAny<IStatementHandler>(), It.IsAny<string>()))
                .Returns((IStatementHandler handler, string input) => handler.Handle(input + "+Middle"))
            ;
            var middlewareInner = new Mock<IStatementMiddleware>();
            middlewareInner.Name = nameof(middlewareInner);
            middlewareInner
                .Setup(a => a.Next(It.IsAny<IStatementHandler>(), It.IsAny<string>()))
                .Returns((IStatementHandler handler, string input) => handler.Handle(input + "+Inner"))
            ;
            var process = new Mock<IStatementHandler>();
            process.Setup(a => a.Handle(It.IsAny<string>()))
                .Returns((string input) => input + "+Process")
            ;
            var pipeline = new StatementPipeline();
            pipeline.Use(middlewareOuter.Object);
            pipeline.Use(middlewareMiddle.Object);
            pipeline.Use(middlewareInner.Object);
            var handler = pipeline.Build(process.Object);
            var actual = handler.Handle("input");
            Assert.Equal("input+Outer+Middle+Inner+Process", actual);
        }

        [Fact]
        public void Build_UseRange_Inner_Middle_Outer_Test()
        {
            var middlewareOuter = new Mock<IStatementMiddleware>();
            middlewareOuter
                .Setup(a => a.Next(It.IsAny<IStatementHandler>(), It.IsAny<string>()))
                .Returns((IStatementHandler handler, string input) => handler.Handle(input + "+Outer"))
            ;
            var middlewareMiddle = new Mock<IStatementMiddleware>();
            middlewareMiddle
                .Setup(a => a.Next(It.IsAny<IStatementHandler>(), It.IsAny<string>()))
                .Returns((IStatementHandler handler, string input) => handler.Handle(input + "+Middle"))
            ;
            var middlewareInner = new Mock<IStatementMiddleware>();
            middlewareInner
                .Setup(a => a.Next(It.IsAny<IStatementHandler>(), It.IsAny<string>()))
                .Returns((IStatementHandler handler, string input) => handler.Handle(input + "+Inner"))
            ;
            var process = new Mock<IStatementHandler>();
            process.Setup(a => a.Handle(It.IsAny<string>()))
                .Returns((string input) => input + "+Process")
            ;
            var pipeline = new StatementPipeline();
            pipeline.UseRange([
                middlewareOuter.Object,
                middlewareMiddle.Object,
                middlewareInner.Object,
            ]);
            var handler = pipeline.Build(process.Object);
            var actual = handler.Handle("input");
            Assert.Equal("input+Outer+Middle+Inner+Process", actual);
        }

        #endregion
    }
}
