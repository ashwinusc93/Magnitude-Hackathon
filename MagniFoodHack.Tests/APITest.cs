using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagniFoodHack.Controllers;
using MagniFoodHack.Models;
using System.Web.Http.Results;

namespace MagniFoodHack.Tests
{   
    [TestClass]
    public class APITest
    {
        [TestMethod]
        public void GetAllItems_ShouldReturnAllItems()
        {
            var testItems = GetTestItems();
            var controller = new MagniFoodController(testItems);
            List<Items> result = new List<Items>();

            result = controller.Login("Ashwin", "test123$") as List<Items>;

            Assert.AreEqual(testItems.Count, result.Count);
        }
        [TestMethod]
        public void GetItem_ShouldReturnCorrectItem()
        {
            var testItems = GetTestItems();
            var controller = new MagniFoodController(testItems);

            var result = controller.GetItem("Burger");
            Assert.IsNotNull(result);
            Assert.AreEqual(testItems[0].Name, result.Name);
        }
        

        [TestMethod]
        public void GetProduct_ShouldNotFindProduct()
        {
            var testItems = GetTestItems();
            var controller = new MagniFoodController(testItems);

            var result = controller.GetItem("Chicken Tikka Masala");
            Assert.IsNull(result);
        }
        private List<Items> GetTestItems()
        {
            var items = new List<Items>();
            items.Add(new Items { Name = "Burger", Type = "Veg", Price = 10 });
            items.Add(new Items { Name = "Pizza", Type = "Veg", Price = 20 });
            items.Add(new Items { Name = "Bread", Type = "Veg", Price = 30 });

            return items;
        }
    }
}
