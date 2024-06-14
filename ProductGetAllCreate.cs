using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace CrudAzureFunc
{
    public  class ProductGetAllCreate
    {

        private readonly AppDbContext _ctx;

        public ProductGetAllCreate(AppDbContext ctx)
        {
            _ctx = ctx; 
        }

        [FunctionName("ProductGetAllCreate")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "products")] HttpRequest req)
        {
            /*
                string requestBoday = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<Product>(requestBoday);

                return new CreatedResult("/products", product);
               */
            if (req.Method == HttpMethods.Post)
        {
            string requestBoday = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(requestBoday);
            _ctx.Products.Add(product);
            await _ctx.SaveChangesAsync();
            return new CreatedResult("/products", product);

        }

        var products = await _ctx.Products.ToListAsync();
        return new OkObjectResult(products);
     

        }
    }
}
