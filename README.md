# Onlinelibrary
## Project Description

Onlinelibrary is a web application built using ASP.NET Core MVC, allowing users to browse books, add books, view reviews, and add reviews. The project utilizes SQLite database to store data and session mechanism to manage logged-in users. User passwords are stored as hashes.

## Technologies

- ASP.NET Core MVC (.NET 7.0)
- Entity Framework Core
- SQLite
- Bootstrap (for styling)

## Features

- User registration and login
- Browsing list of books
- Viewing detailed information about books
- Adding books
- Adding reviews for books
- Displaying latest books and top-rated books

## Project Structure

Onlinelibrary/
│
├── Controllers/
│ ├── AccountController.cs
│ ├── BooksController.cs
│
├── Data/
│ ├── DbInitializer.cs
│
├── Models/
│ ├── Author.cs
│ ├── Book.cs
│ ├── LibraryContext.cs
│ ├── Review.cs
│ ├── User.cs
│ ├── ErrorViewModel.cs 
│
├── Views/
│ ├── Account/
│ │ ├── Login.cshtml
│ │ ├── Register.cshtml
│ │
│ ├── Books/
│ │ ├── Index.cshtml
│ │ ├── Details.cshtml
│ │ ├── Create.cshtml
│ │ ├── AddReview.cshtml
│ │ ├── TopRatedBooks.cshtml
│ │
│ ├── Shared/
│ │ ├── _Layout.cshtml
│ │ ├── _ValidationScriptsPartial.cshtml
│ │
│ ├── Home/
│ │ ├── Index.cshtml
│ │ ├── Privacy.cshtml
│ │
│ ├── _ViewImports.cshtml
│ ├── _ViewStart.cshtml
│
├── wwwroot/
│ ├── css/
│ ├── js/
│
├── appsettings.json
├── Program.cs
├── Onlinelibrary.csproj
├── README.md

## Running the Application

1. Clone the repository:
   git clone https://github.com/yourusername/BibliotekaOnline.git

2. Navigate to the project directory:
    cd BibliotekaOnline

3. Restore dependencies:
    dotnet restore

4. Build the project:
    dotnet build

5. Run database migrations:
    dotnet ef migrations add InitialCreate
    dotnet ef database update

6. Run the application:
    dotnet run

7. Program will provide local link. Go there and done! Everything is working!

Author
Tomasz Madeja

License
The project is licensed under the MIT License. Details can be found in the LICENSE file.