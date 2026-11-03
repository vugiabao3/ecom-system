using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Persistence
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        public DataSeeder(AppDbContext context) { _context = context; }

        public async Task SeedAsync()
        {
            if (await _context.Products.AnyAsync(p => p.CreatedBy == "Seed"))
                return;

            if (!await _context.Categories.AnyAsync())
            {
                var cats = new[]
                {
                    "Smartphones","Tablets","Laptops","Computer Accessories","Fashion",
                    "Shoes","Bags","Beauty","Skincare","Cosmetics",
                    "Home Appliances","Household","Kitchenware","Furniture","Sports",
                    "Outdoor","Books","Stationery","Toys","Accessories"
                };
                foreach (var c in cats)
                    _context.Categories.Add(new Category { Name = c, Slug = c.ToLower().Replace(" ","-") });
                await _context.SaveChangesAsync();
            }
            var categoryIds = await _context.Categories.OrderBy(c=>c.Id).Select(c=>c.Id).ToListAsync();

            var sellerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var brandNames = new[] { "Apple","Samsung","Sony","Nike","Adidas","L'Oreal","Dyson","IKEA","Hasbro","Logitech","Dell","HP" };
            var brandEntities = new List<Brand>();
            foreach (var b in brandNames)
            {
                var brand = new Brand { Id = Guid.NewGuid(), Name = b, SellerId = sellerId, IsActive = true };
                brandEntities.Add(brand);
                _context.Brands.Add(brand);
            }
            await _context.SaveChangesAsync();

            var names = new Dictionary<int,string[]>
            {
                {1,new[]{"iPhone 15 Pro","Galaxy S24 Ultra","Pixel 8 Pro","Xiaomi 14","OnePlus 12"}},
                {2,new[]{"iPad Pro 12.9","Galaxy Tab S9","Surface Pro 9","Lenovo Tab P12","Amazon Fire HD 10"}},
                {3,new[]{"MacBook Pro 14","Dell XPS 13","HP Spectre x360","Lenovo ThinkPad X1","ASUS ZenBook 14"}},
                {4,new[]{"Logitech MX Master 3S","Keychron K2","Samsung 27\" Monitor","WD 2TB SSD","Anker USB-C Hub"}},
                {5,new[]{"Men's Cotton T-Shirt","Women's Summer Dress","Denim Jacket","Wool Sweater","Cargo Pants"}},
                {6,new[]{"Nike Air Max 270","Adidas Ultraboost","Converse Chuck Taylor","Puma RS-X","New Balance 574"}},
                {7,new[]{"Leather Backpack","Canvas Tote Bag","Travel Suitcase","Crossbody Bag","Laptop Bag"}},
                {8,new[]{"Perfume Eau de Parfum","Lipstick Set","Mascara Volume","Foundation","Eye Shadow Palette"}},
                {9,new[]{"Vitamin C Serum","Moisturizer Cream","Sunscreen SPF50","Night Cream","Face Cleanser"}},
                {10,new[]{"Matte Lipstick","Liquid Foundation","Blush Powder","Concealer","Highlighter"}},
                {11,new[]{"Dyson Vacuum V15","Air Purifier","Microwave Oven","Refrigerator","Washing Machine"}},
                {12,new[]{"Detergent Liquid","Paper Towels","Trash Bags","Cleaning Spray","Laundry Pods"}},
                {13,new[]{"Non-stick Pan Set","Chef Knife","Cutting Board","Blender","Coffee Maker"}},
                {14,new[]{"Wooden Dining Table","Office Chair","Bookshelf","Sofa","Bed Frame"}},
                {15,new[]{"Yoga Mat","Dumbbell Set","Resistance Bands","Treadmill","Jump Rope"}},
                {16,new[]{"Camping Tent","Hiking Backpack","Sleeping Bag","Portable Stove","Water Bottle"}},
                {17,new[]{"Atomic Habits","Sapiens","The Alchemist","Deep Work","Thinking Fast and Slow"}},
                {18,new[]{"Notebook Set","Pen Collection","Highlighters","Sticky Notes","Planner"}},
                {19,new[]{"LEGO City Set","RC Car","Board Game","Puzzle 1000pcs","Action Figure"}},
                {20,new[]{"Smart Watch Band","Phone Case","Sunglasses","Wallet","Belt"}},
            };

            var rnd = new Random(7);
            var products = new List<Product>();
            int idx = 0;
            foreach (var kv in names)
            {
                int catId = kv.Key;
                foreach (var nm in kv.Value)
                {
                    idx++;
                    var brand = brandEntities[rnd.Next(brandEntities.Count)];
                    var price = 15 + rnd.Next(0, 1200);
                    var original = price + rnd.Next(10, 200);
                    var p = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = nm,
                        Price = price,
                        ImageUrl = $"https://picsum.photos/seed/{nm.Replace(" ","")}{idx}/600/600",
                        Description = $"{nm} offers premium quality, durable build and excellent performance. Ideal for everyday use and gifting.",
                        CategoryId = 1,
                        SellerId = sellerId,
                        BrandId = brand.Id,
                        CreatedBy = "Seed",
                        CreatedAt = DateTime.UtcNow.AddDays(-rnd.Next(0,90)),
                        IsActive = true,
                        IsApproved = true,
                        Status = "Active",
                    };
                    for (int im = 0; im < 3; im++)
                        p.Images.Add(new ProductImage { Id = Guid.NewGuid(), ProductId = p.Id, ImageUrl = $"https://picsum.photos/seed/{nm.Replace(" ","")}{idx}_{im}/600/600", IsMain = im==0, DisplayOrder = im });
                    var variants = new[] { "Small","Medium","Large" };
                    foreach (var v in variants)
                        p.Variants.Add(new ProductVariant { Id = Guid.NewGuid(), ProductId = p.Id, Name = v, SKU = $"SKU-{idx}-{v[0]}", Price = price + (v=="Large"?10:0), Stock = 50, Attributes = $"{{\"size\":\"{v}\"}}" });
                    products.Add(p);
                }
            }
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();
        }
    }
}
