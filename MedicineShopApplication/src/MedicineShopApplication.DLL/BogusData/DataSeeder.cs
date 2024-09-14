using Bogus;
using MedicineShopApplication.DLL.Models;

namespace MedicineShopApplication.DLL.BogusData
{
    public static class DataSeeder
    {
        public static List<User> GenerateUsers(int count)
        {
            var userFaker = new Faker<User>()
                .RuleFor(u => u.UserId, f => f.IndexFaker + 1)
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Address, f => f.Address.FullAddress())
                .RuleFor(u => u.Orders, _ => new List<Order>())
                .RuleFor(u => u.Payments, _ => new List<Payment>());

            return userFaker.Generate(count);
        }

        public static List<Category> GenerateCategories(int count)
        {
            var categoryFaker = new Faker<Category>()
                .RuleFor(c => c.CategoryId, f => f.IndexFaker + 1)
                .RuleFor(c => c.Code, f => f.Commerce.Ean8())
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1).First())
                .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
                .RuleFor(c => c.Products, _ => new List<Product>());

            return categoryFaker.Generate(count);
        }

        public static List<Product> GenerateProducts(int count, List<Category> categories)
        {
            var productFaker = new Faker<Product>()
                .RuleFor(p => p.ProductId, f => f.IndexFaker + 1)
                .RuleFor(p => p.Code, f => f.Commerce.Ean13())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.GenericName, f => f.Commerce.ProductMaterial())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Brand, f => f.Company.CompanyName())
                .RuleFor(p => p.CostPrice, f => f.Random.Decimal(5, 100))
                .RuleFor(p => p.SellingPrice, (f, p) => p.CostPrice + f.Random.Decimal(1, 50))
                .RuleFor(p => p.UnitOfMeasure, f => f.PickRandom(new[] { "Piece", "Kg", "Litre" }))
                .RuleFor(p => p.Status, f => f.PickRandom(new[] { "Available", "Out of Stock", "Discontinued" }))
                .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Notes, f => f.Lorem.Sentence())
                .RuleFor(p => p.Category, f => f.PickRandom(categories))
                .RuleFor(p => p.CategoryId, (f, p) => p.Category.CategoryId)
                .RuleFor(p => p.CartItems, _ => new List<CartItem>())
                .RuleFor(p => p.OrderItems, _ => new List<OrderItem>())
                .RuleFor(p => p.Inventories, _ => new List<Inventory>());

            return productFaker.Generate(count);
        }

        public static List<Inventory> GenerateInventories(int count, List<Product> products)
        {
            var inventoryFaker = new Faker<Inventory>()
                .RuleFor(i => i.InventoryId, f => f.IndexFaker + 1)
                .RuleFor(i => i.QuantityInStock, f => f.Random.Int(0, 500))
                .RuleFor(i => i.ReorderLevel, f => f.Random.Int(10, 50))
                .RuleFor(i => i.Location, f => f.Address.City())
                .RuleFor(i => i.CostPrice, (f, i) => i.Product.CostPrice) // Match product cost price
                .RuleFor(i => i.SellingPrice, (f, i) => i.Product.SellingPrice) // Match product selling price
                .RuleFor(i => i.Status, f => f.PickRandom(new[] { "Available", "Out of Stock", "Damaged" }))
                .RuleFor(i => i.Notes, f => f.Lorem.Sentence())
                .RuleFor(i => i.Product, f => f.PickRandom(products))
                .RuleFor(i => i.ProductId, (f, i) => i.Product.ProductId);

            return inventoryFaker.Generate(count);
        }
    }
}
