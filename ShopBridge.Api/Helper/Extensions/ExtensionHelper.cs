using ShopBridge.Api.Models;
using ShopBridge.Api.Models.Dtos;

namespace ShopBridge.Api.Helper.Extensions
{
    public static class ExtensionHelper
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Active = item.Active,
                Description = item.Description,
                Created = item.Created,
                Price = item.Price
            };
        }
    }
}
