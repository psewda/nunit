// ***********************************************************************
// Copyright (c) 2014 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using NUnit.Framework.Constraints;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Framework
{
    /// <summary>
    /// Delegate used by tests that execute code and
    /// capture any thrown exception.
    /// </summary>
    public delegate void TestDelegate();

#if NET_4_0 || NET_4_5 || PORTABLE
    /// <summary>
    /// Delegate used by tests that execute async code and
    /// capture any thrown exception.
    /// </summary>
    public delegate System.Threading.Tasks.Task AsyncTestDelegate();
#endif

    /// <summary>
    /// The Assert class contains a collection of static methods that
    /// implement the most common assertions used in NUnit.
    /// </summary>
    public partial class Assert
    {
        #region Constructor

        /// <summary>
        /// We don't actually want any instances of this object, but some people
        /// like to inherit from it to add other static methods. Hence, the
        /// protected constructor disallows any instances of this object. 
        /// </summary>
        protected Assert() { }

        #endregion

        #region Equals and ReferenceEquals

        /// <summary>
        /// The Equals method throws an InvalidOperationException. This is done 
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b)
        {
            throw new InvalidOperationException("Assert.Equals should not be used for Assertions");
        }

        /// <summary>
        /// override the default ReferenceEquals to throw an InvalidOperationException. This 
        /// implementation makes sure there is no mistake in calling this function 
        /// as part of Assert. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static new void ReferenceEquals(object a, object b)
        {
            throw new InvalidOperationException("Assert.ReferenceEquals should not be used for Assertions");
        }

        #endregion

        #region Pass

        /// <summary>
        /// Throws a <see cref="SuccessException"/> with the message and arguments 
        /// that are passed in. This allows a test to be cut short, with a result
        /// of success returned to NUnit.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Pass(string message, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new SuccessException(message);
        }

        /// <summary>
        /// Throws a <see cref="SuccessException"/> with the message and arguments 
        /// that are passed in. This allows a test to be cut short, with a result
        /// of success returned to NUnit.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        static public void Pass(string message)
        {
            Assert.Pass(message, null);
        }

        /// <summary>
        /// Throws a <see cref="SuccessException"/> with the message and arguments 
        /// that are passed in. This allows a test to be cut short, with a result
        /// of success returned to NUnit.
        /// </summary>
        static public void Pass()
        {
            Assert.Pass(string.Empty, null);
        }

        #endregion

        #region Fail

        /// <summary>
        /// Throws an <see cref="AssertionException"/> with the message and arguments 
        /// that are passed in. This is used by the other Assert functions. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Fail(string message, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an <see cref="AssertionException"/> with the message that is 
        /// passed in. This is used by the other Assert functions. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        static public void Fail(string message)
        {
            Assert.Fail(message, null);
        }

        /// <summary>
        /// Throws an <see cref="AssertionException"/>. 
        /// This is used by the other Assert functions. 
        /// </summary>
        static public void Fail()
        {
            Assert.Fail(string.Empty, null);
        }

        #endregion

        #region Ignore

        /// <summary>
        /// Throws an <see cref="IgnoreException"/> with the message and arguments 
        /// that are passed in.  This causes the test to be reported as ignored.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Ignore(string message, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new IgnoreException(message);
        }

        /// <summary>
        /// Throws an <see cref="IgnoreException"/> with the message that is 
        /// passed in. This causes the test to be reported as ignored. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        static public void Ignore(string message)
        {
            Assert.Ignore(message, null);
        }

        /// <summary>
        /// Throws an <see cref="IgnoreException"/>. 
        /// This causes the test to be reported as ignored. 
        /// </summary>
        static public void Ignore()
        {
            Assert.Ignore(string.Empty, null);
        }

        #endregion

        #region InConclusive

        /// <summary>
        /// Throws an <see cref="InconclusiveException"/> with the message and arguments 
        /// that are passed in.  This causes the test to be reported as inconclusive.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="InconclusiveException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Inconclusive(string message, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new InconclusiveException(message);
        }

        /// <summary>
        /// Throws an <see cref="InconclusiveException"/> with the message that is 
        /// passed in. This causes the test to be reported as inconclusive. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="InconclusiveException"/> with.</param>
        static public void Inconclusive(string message)
        {
            Assert.Inconclusive(message, null);
        }

        /// <summary>
        /// Throws an <see cref="InconclusiveException"/>. 
        /// This causes the test to be reported as Inconclusive. 
        /// </summary>
        static public void Inconclusive()
        {
            Assert.Inconclusive(string.Empty, null);
        }

        #endregion

        #region Contains

        /// <summary>
        /// Asserts that an object is contained in a list.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The list to be examined</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Contains(object expected, ICollection actual, string message, params object[] args)
        {
            Assert.That(actual, new CollectionContainsConstraint(expected) ,message, args);
        }

        /// <summary>
        /// Asserts that an object is contained in a list.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The list to be examined</param>
        public static void Contains(object expected, ICollection actual)
        {
            Assert.That(actual, new CollectionContainsConstraint(expected) ,null, null);
        }

        #endregion

        #region Multiple

        /// <summary>
        /// Wraps code containing a series of assertions, which should all
        /// be executed, even if they fail. Failed results are saved and
        /// reported at the end of the code block.
        /// </summary>
        /// <param name="testDelegate">A TestDelegate to be executed in Multiple Assertion mode.</param>
        public static void Multiple(TestDelegate testDelegate)
        {
            TestExecutionContext context = TestExecutionContext.CurrentContext;
            Guard.OperationValid(context != null, "Assert.Multiple called outside of a valid TestExecutionContext");

            context.MultipleAssertLevel++;

            try
            {
                testDelegate();
            }
            finally
            {
                context.MultipleAssertLevel--;
            }

            if (context.MultipleAssertLevel == 0)
            {
                int count = context.CurrentResult.AssertionResults.Count;

                if (count > 0)
                {
                    var writer = new TextMessageWriter("Multiple Assert block had {0} failure(s).", count);
                    writer.WriteLine();

                    int counter = 0;
                    foreach (var assertion in context.CurrentResult.AssertionResults)
                    {
                        writer.WriteLine(string.Format("  {0}) {1}", ++counter, assertion.Message));
                        writer.WriteLine();
                    }

                    writer.Write(' '); // So the last newline doesn't get dropped

                    throw new AssertionException(writer.ToString());
                }
            }
        }

        #endregion

        #region Helper Methods

        private static void ReportFailure(ConstraintResult result, string message)
        {
            ReportFailure(result, message, null);
        }

        private static void ReportFailure(ConstraintResult result, string message, params object[] args)
        {
            MessageWriter writer = new TextMessageWriter(message, args);
            result.WriteMessageTo(writer);

#if PORTABLE
            try
            {
                // Throw to get stack trace
                throw new AssertionException(message);
            }
            catch (AssertionException ex)
            {
                TestExecutionContext.CurrentContext.CurrentResult.RecordAssertion(
                    AssertionStatus.Failed, message, ex.StackTrace);
            }
#else
            TestExecutionContext.CurrentContext.CurrentResult.RecordAssertion(
                AssertionStatus.Failed, writer.ToString(), Environment.StackTrace);
#endif

            if (TestExecutionContext.CurrentContext.MultipleAssertLevel == 0)
                throw new AssertionException(writer.ToString());
        }

        private static void IncrementAssertCount()
        {
            TestExecutionContext.CurrentContext.IncrementAssertCount();
        }

        #endregion
    }
}
