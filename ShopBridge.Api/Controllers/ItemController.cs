using Microsoft.AspNetCore.Mvc;
using ShopBridge.Api.Models.Dtos;
using ShopBridge.Api.Repository;
using System.Threading.Tasks;
using System.Linq;
using ShopBridge.Api.Models;
using System;
using ShopBridge.Api.Helper.Extensions;
using System.Collections.Generic;

namespace ShopBridge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _repo;

        public ItemController(IItemRepository repo)
        {
            _repo = repo;
        }

        //GET /items/
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync(string sortBy, string searchString, int pageNumber, int pageSize)
        {
            var items = (await _repo.GetAllItemsAsync(sortBy, searchString, pageNumber,  pageSize)).Select(item => item.AsDto());

            return items;
        }

        //POST /items/
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new Item()
            {
                Name = itemDto.Name,
                Price = itemDto.Price,
                Active = itemDto.Active,
                Description = itemDto.Description,
                Created = DateTimeOffset.UtcNow,
            };

            await _repo.AddItemAsync(item);

            return CreatedAtAction(nameof(GetAllItemsAsync), new { id = item.Id }, item.AsDto());
        }

        //PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpateItemAsync(int id, UpdateItemDto itemDto)
        {

            //check if item exist 
            var item = _repo.GetItemByIdAsync(id).Result;

            if (item == null)
            { 
                return NotFound();
            }

            Item inputItem = new Item()
            {
                Id = id,
                Name = itemDto.Name,
                Price = itemDto.Price,
                Active = itemDto.Active,
                Description = itemDto.Description,
                Created = DateTimeOffset.UtcNow
            };

            

            await _repo.UpdateItemAsync(inputItem);

            return NoContent();
        }

        //DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(int id)
        {

            //check if item exist 
            var item = await _repo.GetItemByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            await _repo.DeleteItemAsync(item);
            return NoContent();
                 
        }
    }
}
