using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions.Execution;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;

namespace PAP.NSubstitute.FluentAssertionsBridge
{
    internal class FluentAssertionsMatcherAdapter<T> : IArgumentMatcher<T>, IDescribeNonMatches
    {
        private List<string> _errors;
        private readonly Action<T> _assertion;
        public FluentAssertionsMatcherAdapter(Action<T> assertion)
        {
            _assertion = assertion;
        }
        public bool IsSatisfiedBy(T argument)
        {
            using var scope = new AssertionScope();
            _assertion(argument);
            _errors = scope.Discard().ToList();
            return _errors.Count == 0;
        }
        public string DescribeFor(object argument)
        {
            return string.Join(Environment.NewLine, _errors);
        }
    }
}