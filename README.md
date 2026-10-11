# NexusStore — Premium E-Commerce Demo (ASP.NET 10 Core)

A client-ready, multi-category e-commerce demonstration application built on **ASP.NET 10 Core MVC**, designed for showcasing in freelance pitches, portfolio presentations, and client deliverables.

---

## 🌟 Key Features & Architectural Highlights

### 🛒 Customer Storefront
- **Dynamic Landing Page (`/`)**:
  - High-impact promotional hero section with call-to-actions.
  - Multi-category showcase cards (**Electronics & Gadgets**, **Modern Living & Home**, **Lifestyle & Gear**).
  - Value propositions & trust badges (Express shipping, 30-day returns, secure checkout).
  - Curated featured collection and promotional discount banner.
- **Product Catalog & Filtering (`/Shop`)**:
  - Live category filtering, keyword search, price range filtering, and in-stock toggling.
  - Multi-criteria sorting (Featured picks, Price: Low to High, Price: High to Low, Highest Rating, Newest Arrivals).
  - Responsive product cards with image hover zoom, ratings, and instant Quick-Add buttons.
- **Product Details (`/Shop/Details/{id}`)**:
  - High-resolution gallery preview with alternate view switcher.
  - Real-time stock status indicator (Low stock alerts, In-stock counters).
  - Key specification badges and ratings breakdown.
  - Quantity adjuster and Add to Cart trigger.
  - Related product recommendations in the same category.
- **Interactive Cart Drawer & Dedicated Cart (`/Cart`)**:
  - Smooth slide-out cart drawer accessible globally across all pages.
  - Asynchronous AJAX additions, removals, and quantity modifications without full page reload.
  - Promotional discount coupon support (Try code **`DEMO20`** for 20% off or **`SAVE10`** for 10% off).
  - Automated shipping calculation ($0 free shipping on orders over $100).
- **Simulated Checkout & Order Processing (`/Checkout`)**:
  - 3-step structured checkout form (Customer info, Shipping destination, Simulated payment selector).
  - Dynamic inventory deduction upon successful placement.
  - Immediate redirect to receipt confirmation.
- **Order Tracking & Confirmation (`/Orders/Confirmation` & `/Orders/Track`)**:
  - Visual 4-step progress tracker (*Placed* ➔ *Processing* ➔ *Dispatched* ➔ *Delivered*).
  - Itemized printable receipt with one-click print function (`window.print()`).
  - Self-serve order lookup tracking tool.

---

### ⚙️ Admin Operations Console (`/Admin`)
- **Executive KPI Dashboard**:
  - Gross Revenue calculation.
  - Total Orders and Average Order Value metrics.
  - Active SKUs count across catalog.
  - Real-time Low-Stock warning badge (alerts when stock $\le$ 5 units).
- **Inventory Management (`/Admin/Products`)**:
  - Tabular inventory overview with search and category filtering.
  - Product creation form (`/Admin/CreateProduct`) with automatic slug generation, image links, and specs.
  - Product editing (`/Admin/EditProduct/{id}`) and deletion.
- **Order Management & Fulfillment (`/Admin/Orders`)**:
  - Filter orders by fulfillment state (*Processing*, *Shipped*, *Delivered*, *Cancelled*).
  - Inline status update dropdown to simulate dispatching packages.
- **Demo State Reset**:
  - One-click button in the admin bar to instantly re-seed the SQLite database back to its pristine sample state.

---

## 🚀 Running the Project Locally

### Prerequisites
- .NET 10 SDK (`dotnet --version` $\ge$ 10.0.x)

### Commands
```bash
# 1. Restore & Build
dotnet build

# 2. Run Application
dotnet run --launch-profile http
```

Once running, navigate to:
- **Storefront**: [http://localhost:5019](http://localhost:5019)
- **Admin Dashboard**: [http://localhost:5019/Admin](http://localhost:5019/Admin)
- **Order Tracking**: [http://localhost:5019/Orders/Track](http://localhost:5019/Orders/Track)

---

## 🛠️ Technology Stack
- **Framework**: ASP.NET 10 Core (C# 14 / .NET 10)
- **Architecture**: MVC Pattern (Model-View-Controller)
- **ORM & Database**: Entity Framework Core 10 with SQLite (`nexusstore.db`)
- **Styling**: Warm Solar Amber & Yellow-Orange Design System (`wwwroot/css/nexus.css`) with Google Fonts (*Plus Jakarta Sans* & *Space Grotesk*), citrus accents, and luminous gradients
- **Frontend Interactivity**: Modern Vanilla JavaScript (`wwwroot/js/nexus.js`)
