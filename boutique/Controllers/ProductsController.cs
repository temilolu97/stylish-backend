using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcommerceCRUD.Attributes;
using EcommerceCRUD.Contexts;
using EcommerceCRUD.DTOs;
using EcommerceCRUD.Enums;
using EcommerceCRUD.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly Settings _settings;

        public ProductsController(DatabaseContext databaseContext, IWebHostEnvironment hostEnvironment, IOptions<Settings> settings)
        {
            _databaseContext = databaseContext;
            _hostEnvironment = hostEnvironment;
            _settings = settings.Value;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            var products = _databaseContext.Products.Select(x=> new Product
            {
                Id=x.Id,
                Name = x.Name,
                Description = x.Description,
                Featured = x.Featured,
                ImageUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase,x.ImageUrl),
                Price = x.Price,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                ImageFile = x.ImageFile,
            })
                .ToList();
            return products;
        }

        [HttpGet("featured-products")]
        public ActionResult <IEnumerable<Product>> GetFeaturedProducts()
        {
            try
            {
                var result = _databaseContext.Products.Where(product => product.Featured == true);
                if (result.Any())
                {
                    foreach (var item in result)
                    {
                        item.ImageUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, item.ImageUrl);
                    }
                    return Ok(result);
                }

                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            var product = _databaseContext.Products.FirstOrDefault(x=>x.Id == id);
            product.ImageUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, product.ImageUrl);
            if (product == null) throw new Exception("There's no product with this id");
            return product;
        }

        // POST api/<ProductsController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromForm] Product request)
        {
            try 
            {
                //string[] stringFileName = UploadImage(request);
                request.ImageUrl = await SaveImage(request.ImageFile);
                request.DateCreated = DateTime.UtcNow;
                _databaseContext.Products.Add(request);
                _databaseContext.SaveChangesAsync();

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        // PUT api/<ProductsController>/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var product = _databaseContext.Products.FirstOrDefault(x=>x.Id == id);
            if (product == null) throw new Exception("There's no product with this id");
            _databaseContext.Products.Remove(product);
            _databaseContext.SaveChanges();
        }

        [AllowAnonymous]
        [HttpGet("/search/{searchparam}")]
        public ActionResult<IEnumerable<Product>> Search(string searchparam)
        {
            try
            {
                var result = _databaseContext.Products.Where(product => product.Name.ToLower().Contains(searchparam.ToLower()));

                if (result.Any())
                {
                    foreach (var item in result)
                    {
                        item.ImageUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, item.ImageUrl);
                    }
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }




        [NonAction]
        public async Task<string> SaveImage(IFormFile image)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(image.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(image.FileName);
            var imagePath =Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);

            using (var fs = new FileStream(imagePath, FileMode.Create))
            {
               await image.CopyToAsync(fs);
            }
            return imageName;
        }
        [NonAction]
        public string[] UploadImage(Product product)
        {
           string fileName = null;
            string filePath = null;
            if(product.ImageFile != null)
            {
                string uploadDir = Path.Combine(_hostEnvironment.ContentRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + product.ImageFile.FileName;
                filePath = Path.Combine(uploadDir, fileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    product.ImageFile.CopyTo(fs);
                }

            }
            return new [] {fileName,filePath};
        }
    }
}
