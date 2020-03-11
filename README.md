# NSubstitute.FluentAssertionsBridge
a library for bridging between NSubstitute and FluentAssertions in order to verify arguments of called methods using FluentAssertions.

## Build
| Branch | Status |
|:------:|:------:|
| Master | [![Build Status](https://dev.azure.com/papgroup/NSubstitute.FluentAssertionsBridge/_apis/build/status/PapGroup.NSubstitute.FluentAssertionsBridge?branchName=master)](https://dev.azure.com/papgroup/NSubstitute.FluentAssertionsBridge/_build/latest?definitionId=1&branchName=master) |
| Develop | [![Build Status](https://dev.azure.com/papgroup/NSubstitute.FluentAssertionsBridge/_apis/build/status/PapGroup.NSubstitute.FluentAssertionsBridge?branchName=develop)](https://dev.azure.com/papgroup/NSubstitute.FluentAssertionsBridge/_build/latest?definitionId=1&branchName=develop) |

## Installation
| Source | Link |
|:------:|:------:|
| Nuget | [![Build Status](https://img.shields.io/nuget/v/PAP.NSubstitute.FluentAssertionsBridge)](https://www.nuget.org/packages/PAP.NSubstitute.FluentAssertionsBridge/) |

## Usage
The library provides a static method called 'Verify.That<T>' that takes an 'Action<T>' parameter for passing an assertion.
For example, the following code verifies that 'Register' method called with expected object one time :

```c#
using PAP.NSubstitute.FluentAssertionsBridge;

...
var service = Substitute.For<IRegistrationService>();
var expected = new RegistrationModel(){ Username = "Admin", Password="123456" };
...
...
service.Received(1).Register(Verify.That<RegistrationModel>(a=> a.Should().BeEquivalentTo(expected)));
```