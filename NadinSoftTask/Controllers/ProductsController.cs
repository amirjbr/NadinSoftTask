using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NadinSoftTask.Domain.Entities;
using NadinSoftTask.Domain.Entities.DTO;
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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            return Ok(await _db.Products.ToListAsync());
        }
        [HttpGet("id" ,Name ="GetProduct")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var product = await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            if (await _db.Products.FirstOrDefaultAsync(u => u.Name.ToLower() == productCreateDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("", "Product Already Exist");
                return BadRequest(ModelState);
            }
            if (productCreateDTO == null)
            {
                return BadRequest();
            }

            Product model = new Product
            {
                Name = productCreateDTO.Name,
                ManufactureEmail = productCreateDTO.ManufactureEmail,
                ManufacturePhone = productCreateDTO.ManufacturePhone
            };
            await _db.Products.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetProduct", new { id = model.Id }, model);

        }

        [HttpDelete("id",Name ="ProductDelete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var product = await _db.Products.FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("id" , Name ="UpdateProduct")]

        public async Task<IActionResult> UpdateProduct(int id , [FromBody] ProductUpdateDto ProductUpdateDto)
        {
            if (ProductUpdateDto == null || ProductUpdateDto.Id != id)
            {
                return BadRequest();
            }
            Product model = new Product
            {
                Id = ProductUpdateDto.Id,
                Name = ProductUpdateDto.Name,
                ManufactureEmail = ProductUpdateDto.ManufactureEmail,
                ManufacturePhone = ProductUpdateDto.ManufacturePhone
            };

            _db.Products.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
