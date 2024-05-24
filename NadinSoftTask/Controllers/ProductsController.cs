using Microsoft.AspNetCore.Mvc;
using NadinSoftTask.Domain.Entities;
using NadinSoftTask.Infrastructure.Data;

namespace NadinSoftTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(_db.Products);
        }
        [HttpGet("id" ,Name ="GetProduct")]
        public ActionResult<Product> GetProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var product = _db.Products.FirstOrDefault(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (product.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            _db.Products.Add(product);
            _db.SaveChanges();
            return Ok(product);

        }

        [HttpDelete("id",Name ="ProductDelete")]
        public IActionResult DeleteProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var product = _db.Products.FirstOrDefault(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("id" , Name ="UpdateProduct")]

        public IActionResult UpdateProduct(int id , [FromBody] Product product)
        {
            if (product == null || product.Id != id)
            {
                return BadRequest();
            }

            _db.Products.Remove(product);
            _db.SaveChanges();
            return NoContent();
        }

    }
}
