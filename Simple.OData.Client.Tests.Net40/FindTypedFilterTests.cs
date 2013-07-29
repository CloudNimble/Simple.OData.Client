﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using Entry = System.Collections.Generic.Dictionary<string, object>;

namespace Simple.OData.Client.Tests
{
    public class FindTypedFilterTests : TestBase
    {
        [Fact]
        public void SingleCondition()
        {
            var product = _client
                .For("Products")
                .Filter<Product>(x => x.ProductName == "Chai")
                .FindEntry();
            Assert.Equal("Chai", product["ProductName"]);
        }

        [Fact]
        public void CombinedConditions()
        {
            var product = _client
                .For("Employees")
                .Filter<Employee>(x => x.FirstName == "Nancy" && x.HireDate < DateTime.Now)
                .FindEntry();
            Assert.Equal("Davolio", product["LastName"]);
        }

        [Fact]
        public void TopOne()
        {
            var products = _client
                .For("Products")
                .Filter<Product>(x => x.ProductName == "Chai")
                .Top(1)
                .FindEntries();
            Assert.Equal(1, products.Count());
        }

        [Fact]
        public void Count()
        {
            var count = _client
                .For("Products")
                .Filter<Product>(x => x.ProductName == "Chai")
                .Count()
                .FindScalar();
            Assert.Equal(1, int.Parse(count.ToString()));
        }
    }
}