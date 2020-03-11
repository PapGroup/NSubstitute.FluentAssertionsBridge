using System;
using NSubstitute.Core.Arguments;

namespace PAP.NSubstitute.FluentAssertionsBridge
{
    /// <summary>
    /// The class that integrates NSubstitute with FluentAssertions
    /// </summary>
    public static class Verify
    {
        /// <summary>
        /// The method that verifies the arguments of called methods using FluentAssertions
        /// </summary>
        /// <typeparam name="T">type of arguments to verify</typeparam>
        /// <param name="action">delegate that contains assertion</param>
        /// <returns>parameter of called method</returns>
        public static T That<T>(Action<T> action)
        {
            return ArgumentMatcher.Enqueue(new FluentAssertionsMatcherAdapter<T>(action));
        }
    }
}