﻿using Eventix.Modules.Attendance.ArchitectureTests.Abstractions;
using Eventix.Shared.Application.EventBus;
using NetArchTest.Rules;

namespace Eventix.Modules.Attendance.ArchitectureTests.Infrastructure;

public class InfrastructureTests : BaseTest
{
    [Fact(DisplayName = "Integration Event Handler Should Not Be Public")]
    [Trait("Attendance Arhcitecture Tests", "Infrastructure Tests")]
    public void IntegrationEventHandler_Should_NotBePublic()
    {
        Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            .NotBePublic()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Integration Event Handler Should Be Sealed")]
    [Trait("Attendance Arhcitecture Tests", "Infrastructure Tests")]
    public void IntegrationEventHandler_Should_BeSealed()
    {
        Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Integration Event Handler Should Have Name Ending With IntegrationEventHandler")]
    [Trait("Attendance Architecture Tests", "Infrastructure Tests")]
    public void IntegrationEventHandler_ShouldHave_NameEndingWith_IntegrationEventHandler()
    {
        Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            .HaveNameMatching(".*IntegrationEventHandler(\\`\\d+)?$")
            .GetResult()
            .ShouldBeSuccessful();
    }
}