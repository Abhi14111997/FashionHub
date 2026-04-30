# 👗 FashionHub — Flipkart Style Fashion E-Commerce

ASP.NET Core MVC + SQL Server + Docker + Jenkins CI/CD

---

## 🚀 SETUP GUIDE

### Option 1 — Local Development (Without Docker)

**Requirements:**
- .NET 8 SDK
- SQL Server (LocalDB ya full)

**Steps:**
```bash
git clone https://github.com/AAPKA-USERNAME/fashionhub.git
cd FashionHub
dotnet restore
dotnet run
```
Open: `https://localhost:5001`

---

### Option 2 — Docker Compose (Recommended)

**Requirements:** Docker Desktop

```bash
git clone https://github.com/AAPKA-USERNAME/fashionhub.git
cd FashionHub
docker-compose up --build
```
Open: `http://localhost:5000`

---

### Option 3 — Jenkins CI/CD Pipeline

**Jenkins mein:**
1. New Item → Pipeline
2. Pipeline Definition → "Pipeline script from SCM"
3. SCM → Git
4. Repository URL: `https://github.com/AAPKA-USERNAME/fashionhub.git`
5. Script Path: `Jenkinsfile`
6. Save → Build Now

---

## 📁 Project Structure

```
FashionHub/
├── Controllers/      → HomeController, ProductController, CartController
├── Models/           → Product, CartItem, Order, OrderItem
├── Data/             → AppDbContext (SQL Server + Seed Data)
├── Views/
│   ├── Home/         → Index (Homepage), Search
│   ├── Product/      → Index (Category), Details
│   ├── Cart/         → Cart, Checkout, OrderSuccess
│   └── Shared/       → _Layout (Navbar + Footer)
├── wwwroot/
│   ├── css/site.css  → Full Flipkart-style CSS
│   └── js/site.js
├── Dockerfile        → Multi-stage Docker build
├── docker-compose.yml → App + SQL Server together
└── Jenkinsfile       → CI/CD Pipeline
```

---

## 🌟 Features

- ✅ 15 Fashion Products with real images
- ✅ Category wise browsing (Men/Women/Kids/Footwear/Accessories)
- ✅ Product Search + Filter + Sort
- ✅ Product Detail Page with Size/Color selector
- ✅ Add to Cart (Session based)
- ✅ Checkout + Order Placement
- ✅ Order Confirmation Page
- ✅ Flipkart-style UI Design
- ✅ Responsive Design
- ✅ Docker Support
- ✅ Jenkins CI/CD Pipeline

---

## 🐳 Docker Commands

```bash
# Build image
docker build -t fashionhub .

# Run container
docker run -d -p 5000:80 fashionhub

# Docker Compose (app + database)
docker-compose up --build -d

# Stop
docker-compose down
```

---

## 🔧 Connection String (appsettings.json)

Local SQL Server:
```json
"Server=localhost;Database=FashionHubDB;Trusted_Connection=True;TrustServerCertificate=True"
```

Docker:
```json
"Server=sqlserver;Database=FashionHubDB;User Id=sa;Password=FashionHub@123;TrustServerCertificate=True"
```
