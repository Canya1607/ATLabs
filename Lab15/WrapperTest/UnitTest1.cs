using System;
using System.Text.RegularExpressions;
using Atata;
using NUnit.Framework;

namespace WrapperTest
{
    using _ = PageRozetka;
    public class PageRozetka : Page<_>
    {
        [FindByName("search")]
        public TextInput<_> search { get; set; }
        [FindByClass("suggest-goods")]
        [FindByIndex(0)]
        public Link<PageGoods, _> firstSearchResultLink { get; set; }
        [FindByClass("suggest-goods__price", OuterXPath = "//*[@class='suggest-goods']//")]
        [FindByIndex(0)]
        public Text<_> firstSearchResultPrice { get; set; }
    }

    public class PageGoods : Page<PageGoods>
    {
        [FindByClass("product__heading")]
        public Text<PageGoods> goodsName { get; set; }
        [FindByClass("product-prices__big")]
        public Text<PageGoods> goodsPrice { get; set; }
    }

    class Tests
    {
        [SetUp]
        public void SetUp()
        {
            AtataContext.Configure().
                UseChrome().
                WithDriverPath(".\\chromedriver").
                UseBaseUrl("https://rozetka.com.ua").
                Build();
        }

        [TearDown]
        public void TearDown()
        {
            AtataContext.Current.CleanUp();
        }

        decimal searchPrice = -1;
        decimal GoodsPagePrice { get; set; }

        private decimal convertStringPrice(string price)
        {
            return Convert.ToDecimal(Regex.Replace(price, @"[^0-9.]", ""));
        }

        [Test]
        [TestCase("Dell Latitude 5510")]
        public void CheckMainPagePrice(string searchText)
        {
            searchPrice = convertStringPrice(Go.To<PageRozetka>()
                .search.Set(searchText)
                .Wait(1)
                .firstSearchResultPrice.Get());

            Console.WriteLine($"found price: {searchPrice} UAH");
            Assert.IsTrue(searchPrice >= 0);
        }

        [Test]
        [TestCase("Lenovo Yoga S22")]
        public void CompareTwoPagesTypes(string searchText)
        {
            searchPrice = convertStringPrice(Go.To<PageRozetka>()
                .search.Set(searchText)
                .Wait(1)
                .firstSearchResultPrice.Get());

            Console.WriteLine($"price: {searchPrice} UAH");
            Assert.IsTrue(searchPrice >= 0);

            GoodsPagePrice = convertStringPrice(Go.To<PageRozetka>()
                .search.Set(searchText)
                .Wait(1)
                .firstSearchResultLink.ClickAndGo()
                .goodsPrice.Get());

            Console.WriteLine($"found price goods: {GoodsPagePrice} UAH");
            Assert.AreEqual(GoodsPagePrice, searchPrice);
        }
    }
}
