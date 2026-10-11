using NexusStore.Models;

namespace NexusStore.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Categories.Any())
        {
            return; // DB has been seeded
        }

        var categories = new List<Category>
        {
            new Category
            {
                Name = "Electronics & Gadgets",
                Slug = "electronics",
                Description = "Cutting-edge audio, computing, smart peripherals, and personal tech.",
                ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=800&q=80",
                DisplayOrder = 1
            },
            new Category
            {
                Name = "Modern Living & Home",
                Slug = "home-living",
                Description = "Minimalist interior essentials, ergonomic lighting, and artisanal goods.",
                ImageUrl = "https://images.unsplash.com/photo-1513519245088-0e12902e5a38?w=800&q=80",
                DisplayOrder = 2
            },
            new Category
            {
                Name = "Lifestyle & Gear",
                Slug = "lifestyle-gear",
                Description = "Everyday carry accessories, travel bags, chronographs, and outdoor goods.",
                ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=800&q=80",
                DisplayOrder = 3
            }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        var catElectronics = categories.First(c => c.Slug == "electronics");
        var catHome = categories.First(c => c.Slug == "home-living");
        var catLifestyle = categories.First(c => c.Slug == "lifestyle-gear");

        var products = new List<Product>
        {
            // Electronics
            new Product
            {
                Name = "Aura Pro Wireless ANC Headphones",
                Slug = "aura-pro-wireless-anc-headphones",
                ShortDescription = "Studio-grade active noise cancellation with 40-hour battery life.",
                Description = "Immerse yourself in rich, high-fidelity sound. Crafted with precision custom 40mm beryllium drivers, active hybrid noise cancellation, breathable memory foam earcups, and multi-device Bluetooth 5.4 connectivity.",
                Price = 249.99m,
                CompareAtPrice = 299.99m,
                CategoryId = catElectronics.Id,
                ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=800&q=80",
                SecondaryImageUrl = "https://images.unsplash.com/photo-1484704849700-f032a568e944?w=800&q=80",
                StockQuantity = 24,
                Sku = "NX-AUD-01",
                Rating = 4.9,
                ReviewCount = 142,
                IsFeatured = true,
                Badge = "Top Seller",
                Features = "Hybrid Active Noise Cancellation;40mm Custom Drivers;40h Battery with Fast Charge;Bluetooth 5.4 & Multipoint;Spatial Audio Support"
            },
            new Product
            {
                Name = "Lumix Horizon Mechanical Keyboard",
                Slug = "lumix-horizon-mechanical-keyboard",
                ShortDescription = "Gasket-mounted hot-swappable keyboard with CNC aluminum chassis.",
                Description = "A masterpiece for engineers and writers. Features lubed linear switches, custom PBT dye-sub keycaps, RGB per-key backlighting with ambient diffuse side glow, and seamless wireless 2.4G/Bluetooth/USB-C modes.",
                Price = 169.50m,
                CompareAtPrice = 199.00m,
                CategoryId = catElectronics.Id,
                ImageUrl = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=800&q=80",
                SecondaryImageUrl = "https://images.unsplash.com/photo-1618384887929-16ec33fab9ef?w=800&q=80",
                StockQuantity = 15,
                Sku = "NX-KBD-02",
                Rating = 4.8,
                ReviewCount = 89,
                IsFeatured = true,
                Badge = "Staff Pick",
                Features = "CNC Anodized Aluminum Case;Hot-Swappable 5-Pin Sockets;Factory Lubed Switches;Tri-Mode Connectivity;South-Facing Per-Key RGB"
            },
            new Product
            {
                Name = "Pulse Spatial Smart Speaker",
                Slug = "pulse-spatial-smart-speaker",
                ShortDescription = "360-degree room-filling acoustic smart speaker with acoustic tuning.",
                Description = "Engineered with dual neodymium tweeters and a downward-firing woofer. Automatically measures room acoustics in real-time to deliver pristine soundstage and crystal clear vocals.",
                Price = 129.00m,
                CompareAtPrice = null,
                CategoryId = catElectronics.Id,
                ImageUrl = "https://images.unsplash.com/photo-1543512214-318c7553f230?w=800&q=80",
                StockQuantity = 32,
                Sku = "NX-SPK-03",
                Rating = 4.7,
                ReviewCount = 64,
                IsFeatured = false,
                Badge = "New",
                Features = "Room-Sensing Acoustic Calibration;360° Omnidirectional Audio;Wi-Fi 6 & AirPlay 2;Capacitive Touch Controls;Water-Resistant Fabric"
            },
            new Product
            {
                Name = "Vantage Ultra-Wide Desk Mat",
                Slug = "vantage-ultra-wide-desk-mat",
                ShortDescription = "Dual-sided vegan leather and merino wool desk pad.",
                Description = "Transform your workspace with an elegant protective barrier for your desk. Anti-slip micro-texture, spill-resistant coating, and stitched perimeter edges that never fray.",
                Price = 39.99m,
                CompareAtPrice = 49.99m,
                CategoryId = catElectronics.Id,
                ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=800&q=80",
                StockQuantity = 50,
                Sku = "NX-MAT-04",
                Rating = 4.9,
                ReviewCount = 210,
                IsFeatured = false,
                Badge = "-20% Off",
                Features = "Premium Textured Vegan Leather;Waterproof & Easy to Clean;Reinforced Stitched Edge;Dimensions: 900 x 400 x 3 mm"
            },

            // Home & Living
            new Product
            {
                Name = "Nordic Arc Ambient Table Lamp",
                Slug = "nordic-arc-ambient-table-lamp",
                ShortDescription = "Sculptural dimmable lamp with brushed brass and frosted glass orb.",
                Description = "A quiet statement piece for bedside or credenza. Smooth stepless capacitive touch dimmer allows you to adjust color temperature from warm 2200K sunset light to crisp 3500K task lighting.",
                Price = 145.00m,
                CompareAtPrice = 180.00m,
                CategoryId = catHome.Id,
                ImageUrl = "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?w=800&q=80",
                StockQuantity = 18,
                Sku = "NX-LMP-05",
                Rating = 4.9,
                ReviewCount = 76,
                IsFeatured = true,
                Badge = "Trending",
                Features = "Hand-blown Frosted Glass;Brushed Matte Brass Finish;Stepless Capacitive Dimming;Energy Efficient 8W Warm LED"
            },
            new Product
            {
                Name = "Kanso Stoneware Ceramic Pour-Over Set",
                Slug = "kanso-stoneware-ceramic-pour-over-set",
                ShortDescription = "Artisanal speckled ceramic dripper with thermal carafe.",
                Description = "Handcrafted by master ceramicists using natural volcanic clay. Optimized 60-degree flow ribs maximize extraction and elevate the subtle tasting notes of specialty single-origin coffees.",
                Price = 64.00m,
                CompareAtPrice = null,
                CategoryId = catHome.Id,
                ImageUrl = "https://images.unsplash.com/photo-1514432324607-a09d9b4aefdd?w=800&q=80",
                StockQuantity = 22,
                Sku = "NX-COF-06",
                Rating = 4.8,
                ReviewCount = 53,
                IsFeatured = false,
                Badge = "Artisanal",
                Features = "Lead-Free Natural Clay;Thermal Heat Retention Carafe;Includes 40 Compostable Filters;Dishwasher Safe"
            },
            new Product
            {
                Name = "Elysian Linen Throw Blanket",
                Slug = "elysian-linen-throw-blanket",
                ShortDescription = "100% stonewashed French flax linen with eyelash fringe.",
                Description = "Breathe easy with all-season natural linen that softens with every wash. Naturally hypoallergenic, temperature-regulating, and textured with effortless wabi-sabi sophistication.",
                Price = 89.00m,
                CompareAtPrice = 110.00m,
                CategoryId = catHome.Id,
                ImageUrl = "https://images.unsplash.com/photo-1584100936595-c0654b55a2e2?w=800&q=80",
                StockQuantity = 4,
                Sku = "NX-BLN-07",
                Rating = 4.7,
                ReviewCount = 38,
                IsFeatured = false,
                Badge = "Low Stock",
                Features = "100% Certified French Flax;Pre-washed for Ultra Softness;Dimensions: 130 x 170 cm;Eco-Friendly OEKO-TEX Standard"
            },
            new Product
            {
                Name = "Kyoto Zen Desktop Planter",
                Slug = "kyoto-zen-desktop-planter",
                ShortDescription = "Architectural fluted concrete planter with internal drainage tray.",
                Description = "Cast in raw mineral concrete with a minimalist geometric fluted body. Perfect for succulents, snake plants, and desktop greenery to bring life into modern workstations.",
                Price = 32.00m,
                CompareAtPrice = 40.00m,
                CategoryId = catHome.Id,
                ImageUrl = "https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=800&q=80",
                StockQuantity = 40,
                Sku = "NX-PLT-08",
                Rating = 4.6,
                ReviewCount = 92,
                IsFeatured = false,
                Badge = "-20% Off",
                Features = "Natural Cast Architectural Concrete;Hidden Water Catch Tray;Scratch-Resistant Cork Base;Dimensions: 14 x 14 x 15 cm"
            },

            // Lifestyle & Gear
            new Product
            {
                Name = "Apex Commuter Weatherproof Backpack",
                Slug = "apex-commuter-weatherproof-backpack",
                ShortDescription = "22L weatherproof ballistic nylon bag with suspended 16'' laptop sleeve.",
                Description = "Built for relentless urban commutes and weekend wanderlust. Features water-repellent YKK AquaGuard zippers, magnetic Fidlock quick-release buckles, ergonomic ventilated back padding, and luggage pass-through.",
                Price = 179.00m,
                CompareAtPrice = 219.00m,
                CategoryId = catLifestyle.Id,
                ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=800&q=80",
                StockQuantity = 19,
                Sku = "NX-BCK-09",
                Rating = 4.9,
                ReviewCount = 184,
                IsFeatured = true,
                Badge = "Best Seller",
                Features = "840D Cordura Ballistic Nylon;Suspended 16'' Laptop Compartment;Fidlock Magnetic Fasteners;YKK AquaGuard Waterproof Zippers;Expandable 22L-26L"
            },
            new Product
            {
                Name = "Chronos Field Automatic Watch",
                Slug = "chronos-field-automatic-watch",
                ShortDescription = "38mm sapphire crystal automatic field watch with Horween leather strap.",
                Description = "Timeless analog utility. Powered by a Japanese 24-jewel automatic movement with 41-hour power reserve. Water resistant to 100m with Super-LumiNova luminescence on dial numerals and syringe hands.",
                Price = 295.00m,
                CompareAtPrice = 350.00m,
                CategoryId = catLifestyle.Id,
                ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=800&q=80",
                StockQuantity = 9,
                Sku = "NX-WTC-10",
                Rating = 4.9,
                ReviewCount = 115,
                IsFeatured = true,
                Badge = "Premium",
                Features = "Japanese Automatic 24-Jewel Calibre;Double-Domed Sapphire Crystal;10 ATM / 100m Water Resistance;Handcrafted Horween Leather Band"
            },
            new Product
            {
                Name = "Nomad Titanium Carabiner & EDC Keyclip",
                Slug = "nomad-titanium-carabiner-keyclip",
                ShortDescription = "Grade 5 titanium unibody keychain with integrated bottle pry tool.",
                Description = "Ultralight, rustproof, and indestructible. Precision CNC cut from aerospace-grade Grade 5 titanium. Weighs a mere 18 grams while supporting over 100 lbs of load.",
                Price = 45.00m,
                CompareAtPrice = null,
                CategoryId = catLifestyle.Id,
                ImageUrl = "https://images.unsplash.com/photo-1608248597359-bb035cbeea8f?w=800&q=80",
                StockQuantity = 60,
                Sku = "NX-KEY-11",
                Rating = 4.8,
                ReviewCount = 77,
                IsFeatured = false,
                Badge = null,
                Features = "Aerospace Grade 5 Titanium;Integrated Box Opener / Pry;Includes 3 Titanium Keyrings;Stonewashed Satin Finish"
            },
            new Product
            {
                Name = "Sylvan Vacuum Insulated Flask 750ml",
                Slug = "sylvan-vacuum-insulated-flask",
                ShortDescription = "Double-wall stainless steel bottle with bamboo cap and silicone bumper.",
                Description = "Keeps liquids ice-cold for 24 hours or steaming hot for 12 hours. Pure 18/8 food-grade stainless steel with ceramic interior lining that preserves pure taste with zero metallic aftertaste.",
                Price = 38.00m,
                CompareAtPrice = 45.00m,
                CategoryId = catLifestyle.Id,
                ImageUrl = "https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=800&q=80",
                StockQuantity = 3,
                Sku = "NX-FLK-12",
                Rating = 4.7,
                ReviewCount = 88,
                IsFeatured = false,
                Badge = "Low Stock",
                Features = "Ceramic Core Lining for Pure Flavor;Double-Wall Vacuum Insulation;FSC-Certified Bamboo Cap;Leakproof Silicone Seal"
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();

        // Seed sample orders for demo metrics
        var sampleOrders = new List<Order>
        {
            new Order
            {
                OrderNumber = "NEX-2026-9041",
                CustomerName = "Sophia Bennett",
                CustomerEmail = "sophia.b@techdesign.io",
                CustomerPhone = "+1 (555) 234-8891",
                ShippingAddress = "1042 Mission Street, Suite 400",
                City = "San Francisco",
                State = "CA",
                PostalCode = "94103",
                Country = "United States",
                PaymentMethod = "Apple Pay (Demo)",
                Status = OrderStatus.Delivered,
                Subtotal = 419.49m,
                DiscountAmount = 0m,
                ShippingFee = 0m,
                TaxAmount = 33.56m,
                TotalAmount = 453.05m,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = products[0].Id,
                        ProductName = products[0].Name,
                        ProductImageUrl = products[0].ImageUrl,
                        UnitPrice = products[0].Price,
                        Quantity = 1,
                        TotalPrice = products[0].Price
                    },
                    new OrderItem
                    {
                        ProductId = products[1].Id,
                        ProductName = products[1].Name,
                        ProductImageUrl = products[1].ImageUrl,
                        UnitPrice = products[1].Price,
                        Quantity = 1,
                        TotalPrice = products[1].Price
                    }
                }
            },
            new Order
            {
                OrderNumber = "NEX-2026-9042",
                CustomerName = "Marcus Zhao",
                CustomerEmail = "marcus.zhao@venture.co",
                CustomerPhone = "+1 (555) 778-1920",
                ShippingAddress = "580 5th Avenue, Fl 18",
                City = "New York",
                State = "NY",
                PostalCode = "10036",
                Country = "United States",
                PaymentMethod = "Credit Card (Demo)",
                Status = OrderStatus.Shipped,
                Subtotal = 295.00m,
                DiscountAmount = 29.50m,
                ShippingFee = 0m,
                TaxAmount = 21.24m,
                TotalAmount = 286.74m,
                PromoCode = "DEMO10",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = products[9].Id,
                        ProductName = products[9].Name,
                        ProductImageUrl = products[9].ImageUrl,
                        UnitPrice = products[9].Price,
                        Quantity = 1,
                        TotalPrice = products[9].Price
                    }
                }
            },
            new Order
            {
                OrderNumber = "NEX-2026-9043",
                CustomerName = "Elena Rostova",
                CustomerEmail = "elena.r@architects.de",
                CustomerPhone = "+1 (555) 883-2041",
                ShippingAddress = "214 Bellevue Way NE",
                City = "Seattle",
                State = "WA",
                PostalCode = "98004",
                Country = "United States",
                PaymentMethod = "Credit Card (Demo)",
                Status = OrderStatus.Processing,
                Subtotal = 209.00m,
                DiscountAmount = 0m,
                ShippingFee = 0m,
                TaxAmount = 16.72m,
                TotalAmount = 225.72m,
                CreatedAt = DateTime.UtcNow.AddHours(-14),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = products[4].Id,
                        ProductName = products[4].Name,
                        ProductImageUrl = products[4].ImageUrl,
                        UnitPrice = products[4].Price,
                        Quantity = 1,
                        TotalPrice = products[4].Price
                    },
                    new OrderItem
                    {
                        ProductId = products[5].Id,
                        ProductName = products[5].Name,
                        ProductImageUrl = products[5].ImageUrl,
                        UnitPrice = products[5].Price,
                        Quantity = 1,
                        TotalPrice = products[5].Price
                    }
                }
            }
        };

        context.Orders.AddRange(sampleOrders);
        context.SaveChanges();
    }

    public static void ResetAndSeed(AppDbContext context)
    {
        context.OrderItems.RemoveRange(context.OrderItems);
        context.Orders.RemoveRange(context.Orders);
        context.Products.RemoveRange(context.Products);
        context.Categories.RemoveRange(context.Categories);
        context.SaveChanges();

        Initialize(context);
    }
}
