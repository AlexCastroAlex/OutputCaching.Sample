using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using OutputCaching.Sample.Model;
using OutputCaching.Sample.Service;

namespace OutputCaching.Sample.Endpoints
{
    public static class ProductEndpoint
    {
        public static RouteGroupBuilder MapProductEndpoint(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/products");

            group.WithTags("Product");

            //simplest implementation
            group.MapGet("/", async (IProductService service) =>
            {
                await Task.Delay(1000);
                return TypedResults.Ok(service.GetProducts());
            })
            .CacheOutput(c=> {
                c.Tag("Product");
                });

            group.MapGet("/{id}", async (IProductService service, [FromRoute] int id) =>
            {
                await Task.Delay(1000);
                return TypedResults.Ok(service.GetProductById(id));
            }).CacheOutput(
                c => { 
                    c.SetVaryByQuery("id"); 
                    c.Tag("Product");
                });

            group.MapPost("/", async (IProductService service, IOutputCacheStore _cache,[FromBody] Product product, CancellationToken cancellationToken) =>
            {
                await Task.Delay(1000);
                await service.AddProduct(product, cancellationToken);
                await _cache.EvictByTagAsync("Product", cancellationToken);
                return TypedResults.Created($"/products/{product.Id}");
            });


            return group;
        }
    }
}
