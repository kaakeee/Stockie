# Stockie Application Documentation

## Overview
Stockie is a Windows Forms application built with .NET Framework 4.8 that provides user authentication and stock management capabilities. The application uses SQL Server LocalDB for data storage and follows a structured architecture pattern.

## Project Structure

### 1. Models
Located in the `Models` namespace, these classes define the data structures used throughout the application.

#### UserRole.cs
```csharp
public enum UserRole
{
    Administrator,  // Highest level access
    System,        // System-level operations
    Operator,      // Regular operations
    Supervisor     // Supervision tasks
}
```
An enumeration that defines the different user roles in the system. Each role represents a different level of access and permissions.

#### User.cs
Represents a user entity in the system with properties for:
- Id
- Username
- Role (using UserRole enum)
- IsActive
- Email

#### Item.cs
Represents an inventory item (used in the `items` table):
- `Id` (int)
- `Nombre` (string)
- `Tipo` (string)
- `CodigoSn` (string)

### 2. Data Layer
Located in the `Data` namespace, handles database operations and connections.

#### DatabaseHelper.cs
Manages database connections and initialization with two main databases:
1. Users Database (`StockieUsers.mdf`)
   - Stores user authentication and profile information
   - Connection string: `Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\StockieUsers.mdf;Integrated Security=True`

2. Stock Database (`StockieInventory.mdf`)
   - Stores stock, calendar events and items information
   - Connection string: `Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\StockieInventory.mdf;Integrated Security=True`

Key Methods:
- `GetUsersConnection()`: Returns SQL connection for users database
- `GetStockConnection()`: Returns SQL connection for stock database
- `TestConnection()`: Verifies database connectivity
- `InitializeDatabase()`: Sets up both databases with required tables
- `InitializeUsersDatabase()`: Creates users table and default users
- `InitializeStockDatabase()`: Creates calendar_events and items tables

Database Schema:

1. Users Table
```sql
CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(20) NOT NULL,
    is_active BIT DEFAULT 1,
  email VARCHAR(100),
    created_at DATETIME DEFAULT GETDATE()
)
```

2. Calendar Events Table
```sql
CREATE TABLE calendar_events (
    id INT IDENTITY(1,1) PRIMARY KEY,
    event_date DATE NOT NULL,
event_type VARCHAR(50) NOT NULL,
    description TEXT,
created_by INT,
    created_at DATETIME DEFAULT GETDATE(),
    status VARCHAR(20) DEFAULT 'Pending'
)
```

3. Items Table (new)
```sql
CREATE TABLE items (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    tipo VARCHAR(50),
    codigo_sn VARCHAR(100)
)
```
Note: Management (create/delete/edit) of `items` is restricted to users with role `Administrator`. Regular users can view the items list but the UI disables modification actions for non-admins.

### 3. Services Layer
Located in the `Services` namespace, contains business logic and data operations.

#### UserService.cs
Handles user authentication and user-related operations:
- `AuthenticateUser(string username, string password)`: Validates user credentials and returns user information
  - Checks username and password against the database
  - Returns null if authentication fails
  - Returns User object with roles and permissions if successful

#### ItemService.cs
Provides basic CRUD operations for `items`:
- `GetAllItems()`
- `AddItem(Item item)` (admin-only in UI)
- `DeleteItem(int id)` (admin-only in UI)

### 4. User Interface
Windows Forms-based UI components.

#### LoginForm (Form1.cs)
The main entry point and login interface:
- Shows application version
- Handles user login attempts
- Initializes database on startup
- Validates user input
- Displays appropriate success/error messages

Features:
- Version display (current: V0.01)
- Username and password input fields
- Login button with validation
- Pressing Enter triggers login (`AcceptButton` is set to the login button)
- Error handling with user-friendly messages

#### MainForm
Opens after successful login. Key behaviors:
- Window title: `Stockie` (displayed at top)
- Left collapsible menu with the following items (left menu order):
 - `Inventario`
 - `Transferencias`
 - `Envios`
 - `Consulta de Stat`
 - `Reportes`
 - `Items` <-- appears above `Configuracion`
 - `Configuracion`
- Center area shows a single calendar control (`MonthCalendar`)
- `Items` opens an `ItemsForm` which lists items from the `items` table; modification actions are enabled only for `Administrator` users.

## Technical Details

### Database Technology
- Uses SQL Server LocalDB
- Two separate database files:
 - StockieUsers.mdf for user management
 - StockieInventory.mdf for stock operations
- The application sets `DataDirectory` to the application base directory and ensures an `App_Data` folder exists. Database files are created under `App_Data`.
- Integrated Windows Authentication

### Security Features
- Password storage: currently stored as plain text. This must be replaced by secure hashing (salted bcrypt/Argon2) in production.
- Active/Inactive user status tracking
- Role-based access control (enforced in UI and services where applicable)
- Unique username constraints

### Error Handling
- Database connection errors are caught and reported
- User-friendly error messages
- Validation for empty username/password

### Default Users
The system initializes with three default users. Note: role names must exactly match the `UserRole` enum values; see note after this list.
1. Administrator
 - Username: "admin"
 - Password: "admin123"
 - Email: "admin@stockie.com"

2. Supervisor
 - Username: "supervisor"
 - Password: "supervisor123"
 - Email: "supervisor@stockie.com"

3. Operator
 - Username: "operador"
 - Password: "operador123"
 - Email: "operador@stockie.com"

NOTE: The `UserRole` enum uses English values (`Administrator`, `System`, `Operator`, `Supervisor`). The default user creation must use role strings that match these enum names exactly. In the current code the third default user has username `operador` and role string `Operador` (Spanish) — this mismatch can cause `Enum.Parse` to fail at runtime. Recommended fixes:
- Change the default role string to `Operator` when inserting the default user, or
- Change the `UserRole` enum to include `Operador` (not recommended).

## Future Improvements
1. Implement password hashing for security
2. Add password complexity requirements
3. Implement password reset functionality
4. Add session management
5. Implement audit logging
6. Add user management interface
7. Enhance error logging and monitoring
8. Implement main form for stock management
9. Add data export/import capabilities
10. Implement backup and restore functionality

## Dependencies
The project uses several NuGet packages:
- System.Data.SqlClient for database operations
- Various Microsoft.Extensions packages for enhanced functionality
- System component libraries for Windows Forms

## Build and Deployment
- Targets .NET Framework 4.8
- Requires SQL Server LocalDB
- Windows Forms application (WinForms)
- Built using Visual Studio tooling

## Troubleshooting / Notes
- If LocalDB cannot create or attach `.mdf` files, check permissions for the `App_Data` directory and that LocalDB is installed.
- The app sets `AppDomain.CurrentDomain.SetData("DataDirectory", baseDirectory)` and creates an `App_Data` folder under the app base path; database files are created there.
- If you see `Enum.Parse` errors at login, verify default user `role` strings match `UserRole` enum values.
- Use `Data.DatabaseHelper.TestConnection()` to validate connectivity programmatically.