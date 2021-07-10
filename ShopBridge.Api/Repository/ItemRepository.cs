using Microsoft.EntityFrameworkCore;
using ShopBridge.Api.Data;
using ShopBridge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _appDbContext;

        public ItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Item>> GetAllItemsAsync(string sortBy, string searchString, int pageNumber, int pageSize)
        {
            var allItems = await _appDbContext.Items.OrderBy(i => i.Name).ToListAsync();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "item_desc":
                        allItems = allItems.OrderByDescending(i => i.Name).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                allItems = allItems.Where(i => i.Name.ToLower().Contains(searchString)).ToList();
            }

            return allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _appDbContext.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

        }
        public async Task AddItemAsync(Item item)
        {
            await _appDbContext.Items.AddAsync(item);
            await SaveItemAsync();

        }

        public async Task UpdateItemAsync(Item item)
        {
            _appDbContext.Items.Update(item);
            await SaveItemAsync();
        }

        public async Task DeleteItemAsync(Item item)
        {
            var result = _appDbContext.Items.Remove(item);
            await SaveItemAsync();

        }

        public async Task SaveItemAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
