using System;
using NSubstitute.Core.Arguments;

namespace PAP.NSubstitute.FluentAssertionsBridge
{
    public static class Verify
    {
        public static T That<T>(Action<T> action)
        {
            return ArgumentMatcher.Enqueue(new FluentAssertionsMatcherAdapter<T>(action));
        }
    }
}