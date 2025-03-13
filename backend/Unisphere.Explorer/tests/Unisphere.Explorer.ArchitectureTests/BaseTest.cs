using System.Reflection;
using Unisphere.Explorer.Api.Endpoints;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Domain;
using Unisphere.Explorer.Infrastructure;

namespace Unisphere.Explorer.ArchitectureTests;

#pragma warning disable CA1515 // Consider making public types internal
public abstract class BaseTest
#pragma warning restore CA1515 // Consider making public types internal
{
    protected static readonly Assembly DomainAssembly = typeof(House).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ICommand).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(ExplorerEndpoints).Assembly;
}
