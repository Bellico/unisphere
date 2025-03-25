using System.Reflection;
using NetArchTest.Rules;
using Unisphere.Explorer.Api.Endpoints;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Domain;
using Unisphere.Explorer.Infrastructure;

namespace Unisphere.Explorer.ArchitectureTests;

public class LayerTests
{
    protected static readonly Assembly DomainAssembly = typeof(House).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(IExplorerDbContext).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(ExplorerDbContext).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(ExplorerEndpoints).Assembly;

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_Application_Infrastructure_Presentation_Layer()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .And()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .And()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer_Presentation_Layer()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .And()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
