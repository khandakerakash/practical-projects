using Bogus;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.DLL.Models.General;
using Microsoft.Extensions.DependencyInjection;
using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.DLL.BogusData
{
    public static class DataSeeder
    {
        public static void GenerateApplication(IServiceProvider serviceProvider)
        {
            var applicationManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (applicationManager.FindByClientIdAsync("msa-web-dev").Result is null)
            {
                var appRes = applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ApplicationType = "Ecommerce",
                    ClientId = "msa-web-dev",
                    ClientSecret = "901564A5-E7FE-42CB-W10D-61EF6A8F3654",
                    DisplayName = "This is a Medicine Shop Web Application",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password
                    }
                }).Result;
            }

            if (applicationManager.FindByClientIdAsync("msa-mobile-dev").Result is null)
            {
                var appRes = applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ApplicationType = "Ecommerce",
                    ClientId = "msa-mobile-dev",
                    ClientSecret = "101564A7-E7FE-42CB-M10D-61EF6A8F3607",
                    DisplayName = "This is a Medicine Shop Mobile Application",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken
                    }
                }).Result;
            }
        }

        public static void GenerateRoles(IServiceProvider serviceProvider)
        {
            var rolesWithTypes = new Dictionary<string, string>
            {
                { "Developer", "Admin" },
                { "SuperAdmin", "Admin" },
                { "Admin", "Admin" },
                { "Manager", "Admin" },
                { "Moderator", "Admin" },
                { "Salesman", "Admin" },
                { "Customer", "Customer" }
            };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var role in rolesWithTypes)
            {
                var key = role.Key.ToLower();
                var value = role.Value.ToLower();
                var isRoleExists = roleManager.RoleExistsAsync(key).Result;

                if (!isRoleExists)
                {
                    var response = roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = key,
                        RoleType = value
                    }).Result;
                }
            }
        }

        public static List<ApplicationUser> GenerateUsers(int count)
        {
            //var userFaker = new Faker<ApplicationUser>()
            //    .RuleFor(u => u.UserName, f => f.Internet.UserName())
            //    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            //    .RuleFor(u => u.LastName, f => f.Name.LastName())
            //    .RuleFor(u => u.Email, f => f.Internet.Email())
            //    .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            //    .RuleFor(u => u.Address, f => f.Address.FullAddress())
            //    .RuleFor(u => u.Orders, _ => new List<Order>())
            //    .RuleFor(u => u.Payments, _ => new List<Payment>());

            //return userFaker.Generate(count);
            return null;
        }

        public static List<Category> GenerateCategories(int count)
        {
            var categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Code, f => f.Commerce.Ean8())
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1).First())
                .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
                .RuleFor(c => c.Products, _ => new List<Product>());

            return categoryFaker.Generate(count);
        }

        public static List<Product> GenerateProducts(int count, List<Category> categories, List<Brand> brands, List<UnitOfMeasure> unitOfMeasures)
        {
            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Code, f => f.Commerce.Ean13())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.GenericName, f => f.Commerce.ProductMaterial())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.CostPrice, f => f.Random.Decimal(5, 100))
                .RuleFor(p => p.SellingPrice, (f, p) => p.CostPrice + f.Random.Decimal(1, 50))
                .RuleFor(p => p.Status, f => f.PickRandom<ProductStatus>())
                .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Notes, f => f.Lorem.Sentence())

                // Brand: Pick a random brand and assign both Brand object and BrandId
                .RuleFor(p => p.Brand, f => f.PickRandom(brands))
                .RuleFor(p => p.BrandId, (f, p) => p.Brand.BrandId)

                // UnitOfMeasure: Pick a random unit of measure and assign both UnitOfMeasure object and UnitOfMeasureId
                .RuleFor(p => p.UnitOfMeasure, f => f.PickRandom(unitOfMeasures))
                .RuleFor(p => p.UnitOfMeasureId, (f, p) => p.UnitOfMeasure.UnitOfMeasureId)

                // Category: Pick a random category and assign both Category object and CategoryId
                .RuleFor(p => p.Category, f => f.PickRandom(categories))
                .RuleFor(p => p.CategoryId, (f, p) => p.Category.CategoryId)

                // Initializing collections for CartItems, OrderItems, and Inventories
                .RuleFor(p => p.CartItems, _ => new List<CartItem>())
                .RuleFor(p => p.OrderItems, _ => new List<OrderItem>())
                .RuleFor(p => p.Inventories, _ => new List<Inventory>());

            return productFaker.Generate(count);
        }

        public static List<Inventory> GenerateInventories(int count, List<Product> products)
        {
            var inventoryFaker = new Faker<Inventory>()
                .RuleFor(i => i.QuantityInStock, f => f.Random.Int(0, 500))
                .RuleFor(i => i.ReorderLevel, f => f.Random.Int(10, 50))
                .RuleFor(i => i.Location, f => f.Address.City())

                // Product: Pick a random product and assign both Product object and ProductId
                .RuleFor(i => i.Product, f => f.PickRandom(products))
                .RuleFor(i => i.ProductId, (f, i) => i.Product.ProductId)

                // UnitOfMeasure: Assign UnitOfMeasure and UnitOfMeasureId from the Product
                .RuleFor(i => i.UnitOfMeasure, (f, i) => i.Product.UnitOfMeasure)
                .RuleFor(i => i.UnitOfMeasureId, (f, i) => i.Product.UnitOfMeasure.UnitOfMeasureId)

                // Status: Pick a value from the InventoryStatus enum
                .RuleFor(i => i.Status, f => f.PickRandom<InventoryStatus>())

                .RuleFor(i => i.Notes, f => f.Lorem.Sentence());

            return inventoryFaker.Generate(count);
        }
    }
}
