using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NadinSoftTask.Domain.Entities;
using NadinSoftTask.Domain.Entities.DTO;
using NadinSoftTask.Domain.Repository;
using NadinSoftTask.Infrastructure.Data;
using System.Net;

namespace NadinSoftTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _dbProduct;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository dbProduct, IMapper mapper)
        {
            _dbProduct = dbProduct;
            _mapper = mapper;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            try
            {
                IEnumerable<Product> productList = await _dbProduct.GetAllAsync();
                _response.Result = _mapper.Map<List<ProductDTO>>(productList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;

        }
        [HttpGet("id", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            try
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

                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            try
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

                Product product = _mapper.Map<Product>(productCreateDTO);

                await _dbProduct.CreateAsync(product);
                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("GetProduct", new { id = product.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;
        }

        [HttpDelete("id", Name = "ProductDelete")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
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
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;
        }

        [HttpPut("id", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody] ProductUpdateDTO productUpdateDto)
        {
            try
            {
                if (productUpdateDto == null || productUpdateDto.Id != id)
                {
                    return BadRequest();
                }
                Product model = _mapper.Map<Product>(productUpdateDto);

                await _dbProduct.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
