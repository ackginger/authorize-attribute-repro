using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1;
using Xunit;

namespace TestProject1
{
    public class AuthorizationTests
    {
        private WebApplicationFactory<Startup> _factory;

        public AuthorizationTests()
        {
            _factory = new WebApplicationFactory<Startup>();
        }
        
        [Fact]
        public void AllRoutesAreAuthorized()
        {
            var except = new[] { "Health" };

            var apiExplorer = _factory.Services.GetRequiredService<IApiDescriptionGroupCollectionProvider>();

            var authorisedRoutes = apiExplorer.ApiDescriptionGroups.Items
                .SelectMany(x => x.Items)
                .Where(x => !except.Contains(x.RelativePath));
            
            foreach (var apiDescription in authorisedRoutes)
            {
                Assert.Contains(apiDescription.ActionDescriptor.EndpointMetadata, x => x is AuthorizeAttribute);
            }
        }
    }
}