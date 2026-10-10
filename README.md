🎬 eTickets - Cinema Ticketing Web App
A full-featured cinema ticketing web application built with ASP.NET Core MVC and Entity Framework Core. Visitors can browse movies, read and write reviews, fill a shopping cart, and pay securely via PayPal. Administrators have full control over catalog management and order history.
---
📋 Table of Contents
Features
Tech Stack
Project Structure
Getting Started
Roles & Permissions
Author
---
✨ Features
🌐 For Everyone (Guests & Users)
Browse & Search: Explore movies, filter by category, and search by title.
Detailed Pages: Dedicated view pages for movies (including cast, producer, cinema, price, showing dates, and user reviews), actors, producers, and cinemas.
Shopping Cart: Session-based cart functionality (works for guests, supports quantity updates or item removal).
Error Handling: Contact page and custom-themed error pages (404 and general errors).
👤 For Signed-in Customers
Authentication: Account registration and login managed via ASP.NET Core Identity.
Checkout Integration: PayPal integration supporting the Create & Capture order flow.
Order History: View personal order history, order details, and profile page.
Reviews & Ratings: Leave a 1–5 star rating with an optional comment, and delete personal reviews.
🔐 For Administrators
Catalog Management: Complete CRUD (Create, Read, Update, Delete) operations for movies, actors, producers, and cinemas.
Order Management: View and delete any order in the system.
Review Moderation: Ability to delete any user review across the platform.
🛡️ Security Highlights
Role-Based Authorization: `Administrator` and `Customer` roles (new registrations default to `Customer`).
Password Policy: Minimum 8 characters, requiring at least one digit, uppercase, and lowercase letter.
Account Protection: Unique email enforcement and a 15-minute lockout after 5 failed login attempts.
Security Tokens: `HttpOnly` and secure-only authentication cookies, plus Anti-Forgery tokens on cart, review, contact, and checkout POST actions.
---
🛠️ Tech Stack
Area	Technology
Framework	ASP.NET Core MVC (.NET 10)
Data Access	Entity Framework Core 8 (SQL Server), Code-First Migrations
Authentication	ASP.NET Core Identity with Role Management
Mapping	AutoMapper
Payment Gateway	PayPal REST API (Sandbox by default)
Front End	Razor Views, Bootstrap, Bootstrap Icons, Custom CSS
---
📁 Project Structure
The solution (`eTickets.slnx`) is split into three main projects following a clean layered architecture:
```text
eTickets/                   # Web Project (MVC)
├── Controllers/            # Home, Movies, Actors, Producers, Cinemas, ShoppingCart, Orders, MovieReviews, Account
├── Views/                  # Razor views per controller + Shared layouts
├── ViewModels/             # Request models (Create/Edit/Login/Register/PayPal)
├── Services/               # PayPalService implementation
├── Configuration/          # PayPalOptions configuration
├── Mapping/                # AutoMapper profile
└── wwwroot/                # Static files (CSS, JS, Libraries, Images)

eTickets.Business/          # Business Layer
├── Interfaces/             # IMoviesService, IOrdersService, IShoppingCartService, etc.
└── Services/               # Business logic implementations

eTickets.Data/              # Data Layer
├── Models/                 # Entities (Movie, Actor, Producer, Cinema, Order, OrderItem, MovieReview, ShoppingCart, ApplicationUser)
├── Configurations/         # EF Core entity configurations
├── Repository/             # Generic entity base repository
├── Migrations/             # EF Core database migrations
└── Data/                   # AppDbContext, Sample Data Seeder, Identity/Role Seeder
```
> **Dependency Flow:** `eTickets` ➔ `eTickets.Business` ➔ `eTickets.Data`
---
🚀 Getting Started
Prerequisites
.NET 10 SDK
SQL Server (LocalDB, Express, or Full Instance) reachable from your machine
The EF Core CLI tool:
```bash
  dotnet tool install --global dotnet-ef
  ```
---
Step-by-Step Installation
1. Clone the Repository
```bash
git clone https://github.com/Ahm3dHegazy/eTickets.git
cd eTickets
```
2. Configure the Database Connection
The default connection string in `eTickets/appsettings.json` points to a local SQL Server instance using Windows authentication:
```text
Server=.;Database=eTickets;Integrated Security=True;TrustServerCertificate=True;
```
If your SQL Server configuration differs, override it using User Secrets (without modifying tracked files):
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your connection string>" --project eTickets
```
3. Apply Database Migrations
Run the migrations once to set up the database schema and seed initial lookup data (cinemas, producers, actors, movies, and default roles):
```bash
dotnet ef database update --project eTickets.Data --startup-project eTickets
```
4. Create an Admin Account (Optional)
An initial administrator user is created at startup when both secret values are configured:
```bash
dotnet user-secrets set "SeedAdmin:Email" "admin@example.com" --project eTickets
dotnet user-secrets set "SeedAdmin:Password" "YourStrongPassword1" --project eTickets
```
(Ensure the password satisfies the minimum password policy requirements).
5. Configure PayPal (For Checkout)
Create a sandbox application in the PayPal Developer Dashboard and register its credentials:
```bash
dotnet user-secrets set "PayPal:ClientId" "<sandbox client id>" --project eTickets
dotnet user-secrets set "PayPal:ClientSecret" "<sandbox client secret>" --project eTickets
```
Setting	Default Value	Description
`PayPal:UseSandbox`	`true`	Set to `false` to use the live PayPal API
`PayPal:Currency`	`USD`	Target currency used for orders
> **Note:** All features except checkout function properly without PayPal credentials configured.
6. Run the Application
```bash
dotnet run --project eTickets --launch-profile https
```
Then open `https://localhost:7001` in your browser (an HTTP profile is also available on `http://localhost:5080`).
> **Note:** Authentication cookies are marked as secure-only, so always use the **HTTPS** profile when testing sign-in and authentication flow.
---
👥 Roles & Permissions
Action	Guest	Customer	Administrator
Browse & search movies, actors, producers, cinemas	✅	✅	✅
Use shopping cart	✅	✅	✅
Checkout, view personal orders, write reviews	❌	✅	✅
Add, edit, and delete catalog items	❌	❌	✅
View all orders & delete orders	❌	❌	✅
---
👤 Author
Ahmed Abdel Samea Hegazy - @Ahm3dHegazy
