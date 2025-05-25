using System;

namespace ContentTypeTextNet.Pe.CommonTest
{
    /// <summary>
    /// テストインフラ系例外。
    /// </summary>
    /// <remarks>こいつを捕まえてどうこうすることはない。</remarks>
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TestException: System.Exception
    {
        public TestException()
        { }
        public TestException(string message)
            : base(message)
        { }
        public TestException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    #region PrivateObject

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PrivateObjectException: TestException
    {
        public PrivateObjectException()
        { }

        public PrivateObjectException(string message)
            : base(message)
        { }

        public PrivateObjectException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PrivateObjectFieldException: PrivateObjectException
    {
        public PrivateObjectFieldException()
        { }

        public PrivateObjectFieldException(string message)
            : base(message)
        { }

        public PrivateObjectFieldException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PrivateObjectPropertyException: PrivateObjectException
    {
        public PrivateObjectPropertyException()
        { }

        public PrivateObjectPropertyException(string message)
            : base(message)
        { }

        public PrivateObjectPropertyException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PrivateObjectMethodException: PrivateObjectException
    {
        public PrivateObjectMethodException()
        { }

        public PrivateObjectMethodException(string message)
            : base(message)
        { }

        public PrivateObjectMethodException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PrivateObjectMethodParametersException: PrivateObjectException
    {
        public PrivateObjectMethodParametersException()
        { }

        public PrivateObjectMethodParametersException(string message)
            : base(message)
        { }

        public PrivateObjectMethodParametersException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    #endregion
}
