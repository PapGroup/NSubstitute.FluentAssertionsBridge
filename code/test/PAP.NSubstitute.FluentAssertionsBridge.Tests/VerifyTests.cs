using System;
using FluentAssertions;
using NSubstitute;
using PAP.NSubstitute.FluentAssertionsBridge.Tests.TestUtils;
using Xunit;

namespace PAP.NSubstitute.FluentAssertionsBridge.Tests
{
    public class VerifyTests
    {
        [Fact]
        public void should_verify_method_arguments_using_fluent_assertions()
        {
            var service = Substitute.For<IRegistrationService>();
            service.Register("Admin");

            service.Received(1).Register(Verify.That<string>(a=> a.Should().StartWithEquivalent("adm")));
        }

        [Fact]
        public void should_include_fluent_assertions_message_in_exceptions()
        {
            var service = Substitute.For<IRegistrationService>();
            service.Register("FakeUser");
            void VerificationWithNSubstitute() =>
                service.Received(1)
                    .Register(Verify.That<string>(a => a.Should().StartWithEquivalent("adm")));

            var nsubstituteMessage = GetExceptionMessageOf(VerificationWithNSubstitute);

            nsubstituteMessage.Should().Contain("to start with equivalent of \"adm\", but \"FakeUser\" differs");
        }

        private string GetExceptionMessageOf(Action action)
        {
            try
            {
                action();
                return string.Empty;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}