﻿using System;
using System.Threading.Tasks;
using Xunit;

namespace Simple.OData.Client.Tests.FluentApi;

public class InsertTypedTests : TestBase
{
	[Fact]
	public async Task Insert()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var product = await client
			.For<Product>()
			.Set(new { ProductName = "Test1", UnitPrice = 18m })
			.InsertEntryAsync().ConfigureAwait(false);

		Assert.Equal("Test1", product.ProductName);
	}

	[Fact]
	public async Task InsertAutogeneratedID()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var product = await client
			.For<Product>()
			.Set(new { ProductName = "Test1", UnitPrice = 18m })
			.InsertEntryAsync().ConfigureAwait(false);

		Assert.True(product.ProductID > 0);
		Assert.Equal("Test1", product.ProductName);
	}

	[Fact]
	public async Task InsertWithSelect()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var product = await client
			.For<Product>()
			.Set(new { ProductName = "Test1", UnitPrice = 18m })
			.Select(x => new
			{
				x.ProductID
			})
			.InsertEntryAsync().ConfigureAwait(false);

		Assert.NotEqual(default, product.ProductID);
		Assert.Equal(default, product.ProductName);
		Assert.Equal(default, product.UnitPrice);
	}

	[Fact]
	public async Task InsertWithMappedColumn()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var product = await client
			.For<Product>()
			.Set(new Product { ProductName = "Test1", UnitPrice = 18m, MappedEnglishName = "EnglishTest" })
			.InsertEntryAsync().ConfigureAwait(false);

		Assert.Equal("Test1", product.ProductName);
		Assert.Equal("EnglishTest", product.MappedEnglishName);
	}

	[Fact]
	public async Task InsertProductWithCategoryByID()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var category = await client
			.For<Category>()
			.Set(new { CategoryName = "Test3" })
			.InsertEntryAsync().ConfigureAwait(false);
		var product = await client
			.For<Product>()
			.Set(new { ProductName = "Test4", UnitPrice = 18m, CategoryID = category.CategoryID })
			.InsertEntryAsync().ConfigureAwait(false);

		Assert.Equal("Test4", product.ProductName);
		Assert.Equal(category.CategoryID, product.CategoryID);
		category = await client
			.For<Category>()
			.Expand(x => new { x.Products })
			.Filter(x => x.CategoryName == "Test3")
			.FindEntryAsync().ConfigureAwait(false);
		Assert.True(category.Products.Length == 1);
	}

	[Fact]
	public async Task InsertProductWithCategoryByAssociation()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var category = await client
			.For<Category>()
			.Set(new { CategoryName = "Test5" })
			.InsertEntryAsync().ConfigureAwait(false);
		var product = await client
			.For<Product>()
			.Set(new { ProductName = "Test6", UnitPrice = 18m, Category = category })
			.InsertEntryAsync().ConfigureAwait(false);

		Assert.Equal("Test6", product.ProductName);
		Assert.Equal(category.CategoryID, product.CategoryID);
		category = await client
			.For<Category>()
			.Expand(x => new { x.Products })
			.Filter(x => x.CategoryName == "Test5")
			.FindEntryAsync().ConfigureAwait(false);
		Assert.True(category.Products.Length == 1);
	}

	[Fact]
	public async Task InsertCategoryWithPictureAsBytes()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var category = await client
			.For<Category>()
			.Set(new { CategoryName = "Test7", Picture = new byte[] { 1, 2, 3, 4, 5 } })
			.InsertEntryAsync().ConfigureAwait(false);

		category = await client
			.For<Category>()
			.Expand(x => new { x.Products })
			.Filter(x => x.CategoryName == "Test7")
			.FindEntryAsync().ConfigureAwait(false);
		Assert.True(category.Picture.Length > 0);
	}

	[Fact]
	public async Task InsertCategoryWithPictureAsString()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var category = await client
			.For<Category>()
			.Set(new { CategoryName = "Test7", Picture = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 }) })
			.InsertEntryAsync().ConfigureAwait(false);

		category = await client
			.For<Category>()
			.Expand(x => new { x.Products })
			.Filter(x => x.CategoryName == "Test7")
			.FindEntryAsync().ConfigureAwait(false);
		Assert.True(category.Picture.Length > 0);
	}

	[Fact]
	public async Task InsertShip()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var ship = await client
			.For<Transport>()
			.As<Ship>()
			.Set(new Ship { ShipName = "Test1" })
			.InsertEntryAsync().ConfigureAwait(false);

		Assert.Equal("Test1", ship.ShipName);
	}
}
