using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NadinSoftTask.Domain.Entities;
using NadinSoftTask.Domain.Entities.DTO;
using NadinSoftTask.Domain.Repository;
using NadinSoftTask.Infrastructure.Data;

namespace NadinSoftTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _dbProduct;
        //private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository dbProduct, IMapper mapper)
        {
            _dbProduct = dbProduct;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            IEnumerable<Product> productList = await _dbProduct.GetAllAsync();
            return Ok(_mapper.Map<List<ProductDTO>>(productList));
        }
        [HttpGet("id", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var product = await _dbProduct.GetByIdAsync(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDTO>(product));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            if (await _dbProduct.GetByIdAsync(u => u.Name.ToLower() == productCreateDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("", "Product Already Exist");
                return BadRequest(ModelState);
            }
            if (productCreateDTO == null)
            {
                return BadRequest();
            }

            Product model = _mapper.Map<Product>(productCreateDTO);

            await _dbProduct.CreateAsync(model);
            return CreatedAtRoute("GetProduct", new { id = model.Id }, model);
        }

        [HttpDelete("id", Name = "ProductDelete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var product = await _dbProduct.GetByIdAsync(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            await _dbProduct.RemoveAsync(product);
            return NoContent();
        }

        [HttpPut("id", Name = "UpdateProduct")]

        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO productUpdateDto)
        {
            if (productUpdateDto == null || productUpdateDto.Id != id)
            {
                return BadRequest();
            }
            Product model = _mapper.Map<Product>(productUpdateDto);
            await _dbProduct.UpdateAsync(model);
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialProduct")]
        public async Task<IActionResult> UpdatePartialProduct(int id, JsonPatchDocument<ProductUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var product = await _dbProduct.GetByIdAsync(u => u.Id == id, tracked: false);

            ProductUpdateDTO productUpdateDTO = _mapper.Map<ProductUpdateDTO>(product);

            if (product == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(productUpdateDTO, ModelState);

            Product model = _mapper.Map<Product>(productUpdateDTO);

            await _dbProduct.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
