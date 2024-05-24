using AutoMapper;
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
        private readonly IMapper _mapper;
        public ProductsController(ApplicationDbContext db ,IMapper mapper )
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            IEnumerable<Product> productList = await _db.Products.ToListAsync();
            return Ok(_mapper.Map<ProductDTO>(productList));
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

            return Ok(_mapper.Map<ProductDTO>(product));
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

            Product model = _mapper.Map<Product>(productCreateDTO);
            
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

        public async Task<IActionResult> UpdateProduct(int id , [FromBody] ProductUpdateDto productUpdateDto)
        {
            if (productUpdateDto == null || productUpdateDto.Id != id)
            {
                return BadRequest();
            }
            Product model = _mapper.Map<Product>(productUpdateDto);
            _db.Products.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
