using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopBridge.Api.Controllers;
using ShopBridge.Api.Models;
using ShopBridge.Api.Models.Dtos;
using ShopBridge.Api.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShopBridge.Test
{
    public class ItemControllerTest
    {
        private readonly Mock<IItemRepository> repositoryStub = new Mock<IItemRepository>();
        private readonly Random _random = new Random();

        [Fact]
        public async Task GetAllItemsAsync_WithUnExistingItems_ReturnAllItems()
        {
            //Arrange
            var expectedItem = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
            repositoryStub.Setup(repo => repo.GetAllItemsAsync("name_desc",expectedItem[0].Name,1,3)).ReturnsAsync(expectedItem);

            //Act
            var controller = new ItemController(repositoryStub.Object);

            //Assert
            var result = await controller.GetAllItemsAsync("name_desc", expectedItem[0].Name, 1, 3);

            result.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task CreateItemAsync_ItemToCreate_ReturnCreatedItem()
        {
            //Arrange
            var itemToCreate = new CreateItemDto
            {
                Price = _random.Next(1, 1000),
                Description = "Big Description ",
                Active = false,
                Name = "Random Item " + _random.Next(1, 100).ToString(),
            };

            var controller = new ItemController(repositoryStub.Object);

            var result = await controller.CreateItemAsync(itemToCreate);

            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(createdItem, options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());
            //createdItem.Id.Should().NotBeEmpty();
            createdItem.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);

        }
        [Fact]
        public async Task UpdateItemAsync_ItemToUpdate_ReturnNoContent()
        {
            Item existingItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemByIdAsync(It.IsAny<int>())).ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto()
            {
                Price = existingItem.Price + 5,
                Description = "Big Description",
                Active = true,
                Name = "Update Random Item " + _random.Next(1, 100).ToString(),
            }; 

            var controller = new ItemController(repositoryStub.Object);

            var result = await controller.UpateItemAsync(itemId, itemToUpdate);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_ItemToDelete_ReturnNoContent()
        {
            Item existingItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemByIdAsync(It.IsAny<int>())).ReturnsAsync(existingItem);

            var controller = new ItemController(repositoryStub.Object);

            var result = await controller.DeleteItemAsync(existingItem.Id);

            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async Task DeleteItemAsync_ItemToDeleteNotExist_ReturnNotFound()
        {
            //Item existingItem = CreateRandomItem();
            var noItemExisitngId = 101;

            //repositoryStub.Setup(repo => repo.GetItemByIdAsync(noItemExisitngId)).ReturnsAsync(ItemRepository);

            var controller = new ItemController(repositoryStub.Object);

            

            var result = await controller.DeleteItemAsync(noItemExisitngId);

            result.Should().BeOfType<NotFoundResult>();
        }
        private Item CreateRandomItem()
        {
            return new Item()
            {
                Id = _random.Next(1, 100),
                Name = "Random Item" + _random.Next(1, 100).ToString(),
                Description = $"A big description",
                Price = _random.Next(1, 1000),
                Active = true,
                Created = DateTimeOffset.UtcNow
            };
        }
    }
}
