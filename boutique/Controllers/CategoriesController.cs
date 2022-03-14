//using EcommerceCRUD.Attributes;
using EcommerceCRUD.Attributes;
using EcommerceCRUD.Contexts;
using EcommerceCRUD.Enums;
using EcommerceCRUD.Models;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private DatabaseContext _databasecontext;
        public CategoriesController(DatabaseContext databaseContext)
        {
            _databasecontext = databaseContext;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _databasecontext.Categories;
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            var category = _databasecontext.Categories.FirstOrDefault(cat => cat.Id == id);
            if (category == null) throw new Exception("This category doesn't exist");
            return category;
        }

        // POST api/<CategoriesController>
        [Authorize]
        [HttpPost]
        public void Post([FromBody] Category request)
        {
            _databasecontext.Categories.Add(request);
            _databasecontext.SaveChanges();
        }

        // PUT api/<CategoriesController>/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Category request)
        {
            var category = _databasecontext.Categories.FirstOrDefault(cat => cat.Id == id);
            if (category == null) throw new Exception("This category does not exist");
            category.Name = request.Name;
            category.DateUpdated = DateTime.UtcNow;
            _databasecontext.SaveChanges();
        }

        // DELETE api/<CategoriesController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var category = _databasecontext.Categories.FirstOrDefault(cat => cat.Id == id);
            if (category == null) throw new Exception("This category does not exist");
            _databasecontext.Categories.Remove(category);
            _databasecontext.SaveChanges();
        }
    }
}
