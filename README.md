eTickets
A cinema ticketing web app built with ASP.NET Core MVC and Entity Framework Core. Visitors can browse movies, read and write reviews, fill a cart, and pay through PayPal; administrators manage the catalog and see every order.
Features
For everyone
Browse movies, filter them by category, and search by name
Movie pages with cast, producer, cinema, price, showing dates, and customer reviews
Browse actors, producers, and cinemas, each with its own details page
Session-based shopping cart (works for guests, update quantities or remove items)
Contact page and themed error pages (404 and general errors)
For signed-in customers
Register and log in with ASP.NET Core Identity
Checkout with PayPal (create and capture order flow)
Order history and order details, plus a profile page
Leave a 1-5 star rating and an optional comment on a movie, and delete your own review (administrators can delete any review)
For administrators
Create, edit, and delete movies, actors, producers, and cinemas
View and delete all orders
Security basics
Two roles: `Administrator` and `Customer` (new registrations are Customers)
Password rules: minimum 8 characters, with a digit, an uppercase, and a lowercase letter
Unique emails, and a 15-minute lockout after 5 failed sign-in attempts
HttpOnly, secure-only auth cookies, and antiforgery tokens on the cart, review, contact, and checkout POST actions
Tech stack
Area	Technology
Framework	ASP.NET Core MVC on .NET 10
Data access	Entity Framework Core 8 (SQL Server), code-first migrations
Auth	ASP.NET Core Identity with roles
Mapping	AutoMapper
Payments	PayPal REST API (sandbox by default)
Front end	Razor views, Bootstrap, Bootstrap Icons, custom CSS theme
Project structure
The solution (`eTickets.slnx`) is split into three projects:
```
eTickets/                 Web project (MVC)
  Controllers/            Home, Movies, Actors, Producers, Cinemas,
                          ShoppingCart, Orders, MovieReviews, Account
  Views/                  Razor views per controller + Shared layout
  ViewModels/             Create/Edit/Login/Register/PayPal request models
  Services/               PayPalService
  Configuration/          PayPalOptions
  Mapping/                AutoMapper profile
  wwwroot/                CSS, JS, libraries, and images

eTickets.Business/        Service layer
  Interfaces/             IMoviesService, IOrdersService, IShoppingCartService, ...
  Services/               Implementations of those interfaces

eTickets.Data/            Data layer
  Models/                 Movie, Actor, Producer, Cinema, Order, OrderItem,
                          MovieReview, ShoppingCart, ApplicationUser, ...
  Configurations/         EF Core entity configurations
  Repository/             Generic entity base repository
  Migrations/             EF Core migrations
  Data/                   AppDbContext, sample data seeder, identity/role seeder
```
Dependencies flow `eTickets` -> `eTickets.Business` -> `eTickets.Data`.
Getting started
Prerequisites
.NET 10 SDK
SQL Server (LocalDB, Express, or full) reachable from your machine
The EF Core CLI: `dotnet tool install --global dotnet-ef`
1. Clone
```bash
git clone https://github.com/Ahm3dHegazy/eTickets.git
cd eTickets
```
2. Configure the database
The default connection string in `eTickets/appsettings.json` points at a local SQL Server instance with Windows authentication:
```
Server=.;Database=eTickets;Integrated Security=True;TrustServerCertificate=True;
```
If your SQL Server setup is different, override it without editing the tracked file:
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your connection string>" --project eTickets
```
3. Create the database
The app does not apply migrations on startup, so run them once:
```bash
dotnet ef database update --project eTickets.Data --startup-project eTickets
```
On first run the app also seeds sample cinemas, producers, actors, and movies (only if there are no cinemas yet), and creates the `Administrator` and `Customer` roles.
4. Create an admin account
An admin user is created at startup only when both values below are configured. Store them as user secrets (do not commit them):
```bash
dotnet user-secrets set "SeedAdmin:Email" "admin@example.com" --project eTickets
dotnet user-secrets set "SeedAdmin:Password" "YourStrongPassword1" --project eTickets
```
The password must satisfy the password rules above.
5. Configure PayPal (needed for checkout)
Create a sandbox app in the PayPal Developer Dashboard and add its credentials:
```bash
dotnet user-secrets set "PayPal:ClientId" "<sandbox client id>" --project eTickets
dotnet user-secrets set "PayPal:ClientSecret" "<sandbox client secret>" --project eTickets
```
Setting	Default	Notes
`PayPal:UseSandbox`	`true`	Set to `false` to use the live PayPal API
`PayPal:Currency`	`USD`	Currency used for orders
Everything except checkout works without PayPal configured.
6. Run
```bash
dotnet run --project eTickets --launch-profile https
```
Then open `https://localhost:7001` (an HTTP profile is also available on `http://localhost:5080`). Auth cookies are marked secure-only, so use the HTTPS profile when testing sign-in.
Roles at a glance
Action	Guest	Customer	Administrator
Browse and search movies, actors, producers, cinemas	Yes	Yes	Yes
Use the cart	Yes	Yes	Yes
Checkout, view own orders, write reviews	-	Yes	Yes
Add, edit, delete catalog items	-	-	Yes
View all orders, delete orders	-	-	Yes
Author
Ahmed Abdel Samea Hegazy - @Ahm3dHegazy
