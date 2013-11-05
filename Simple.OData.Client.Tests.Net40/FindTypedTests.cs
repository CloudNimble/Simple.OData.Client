﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using Entry = System.Collections.Generic.Dictionary<string, object>;

namespace Simple.OData.Client.Tests
{
    public class FindTypedTests : TestBase
    {
        [Fact]
        public void SingleCondition()
        {
            var product = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName == "Chai")
                .FindEntry();
            Assert.Equal("Chai", product["ProductName"]);
        }

        [Fact]
        public void SingleConditionWithVariable()
        {
            var productName = "Chai";
            var product = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName == productName)
                .FindEntry();
            Assert.Equal("Chai", product["ProductName"]);
        }

        [Fact]
        public void CombinedConditions()
        {
            var product = _client
                .For<Employee>()
                .Filter<Employee>(x => x.FirstName == "Nancy" && x.HireDate < DateTime.Now)
                .FindEntry();
            Assert.Equal("Davolio", product["LastName"]);
        }

        [Fact]
        public void StringContains()
        {
            var products = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName.Contains("ai"))
                .FindEntries();
            Assert.Equal("Chai", products.Single()["ProductName"]);
        }

        [Fact]
        public void StringNotContains()
        {
            var products = _client
                .For<Product>()
                .Filter<Product>(x => !x.ProductName.Contains("ai"))
                .FindEntries();
            Assert.NotEqual("Chai", products.First()["ProductName"]);
        }

        [Fact]
        public void StringStartsWith()
        {
            var products = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName.StartsWith("Ch"))
                .FindEntries();
            Assert.Equal("Chai", products.First()["ProductName"]);
        }

        [Fact]
        public void LengthOfStringEqual()
        {
            var products = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName.Length == 4)
                .FindEntries();
            Assert.Equal("Chai", products.First()["ProductName"]);
        }

        [Fact]
        public void SubstringWithPositionAndLengthEqual()
        {
            var products = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName.Substring(1, 2) == "ha")
                .FindEntries();
            Assert.Equal("Chai", products.First()["ProductName"]);
        }

        [Fact]
        public void TopOne()
        {
            var products = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName == "Chai")
                .Top(1)
                .FindEntries();
            Assert.Equal(1, products.Count());
        }

        [Fact]
        public void Count()
        {
            var count = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName == "Chai")
                .Count()
                .FindScalar();
            Assert.Equal(1, int.Parse(count.ToString()));
        }

        [Fact]
        public void SelectSingle()
        {
            var product = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName == "Chai")
                .Select<Product>(x => x.ProductName)
                .FindEntry();
            Assert.Equal("Chai", product["ProductName"]);
        }

        [Fact]
        public void SelectMultiple()
        {
            var product = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName == "Chai")
                .Select<Product>(x => new { x.ProductID, x.ProductName })
                .FindEntry();
            Assert.Equal("Chai", product["ProductName"]);
        }

        [Fact]
        public void OrderBySingle()
        {
            var product = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName == "Chai")
                .OrderBy<Product>(x => x.ProductName)
                .FindEntry();
            Assert.Equal("Chai", product["ProductName"]);
        }

        [Fact]
        public void OrderByMultiple()
        {
            var product = _client
                .For<Product>()
                .Filter<Product>(x => x.ProductName == "Chai")
                .OrderBy<Product>(x => new { x.ProductID, x.ProductName })
                .FindEntry();
            Assert.Equal("Chai", product["ProductName"]);
        }

        public class ODataOrgProduct
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        [Fact]
        public void TypedCombinedConditionsFromODataOrg()
        {
            var client = new ODataClient("http://services.odata.org/V3/OData/OData.svc/");
            var product = client
                .For("Product")
                .Filter<ODataOrgProduct>(x => x.Name == "Bread" && x.Price < 1000)
                .FindEntry();
            Assert.Equal(2.5m, product["Price"]);
        }
    }
}