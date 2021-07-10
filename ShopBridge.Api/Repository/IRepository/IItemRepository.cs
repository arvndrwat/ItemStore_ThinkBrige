using ShopBridge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.Repository
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllItemsAsync(string sortBy, string searchString, int pageNumber, int pageSize);
        Task<Item> GetItemByIdAsync(int id);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Item item);
    }
}
