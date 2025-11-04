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

### 2. Data Layer
Located in the `Data` namespace, handles database operations and connections.

#### DatabaseHelper.cs
Manages database connections and initialization with two main databases:
1. Users Database (`StockieUsers.mdf`)
   - Stores user authentication and profile information
   - Connection string: `Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\StockieUsers.mdf;Integrated Security=True`

2. Stock Database (`StockieInventory.mdf`)
   - Stores stock and calendar events information
   - Connection string: `Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\StockieInventory.mdf;Integrated Security=True`

Key Methods:
- `GetUsersConnection()`: Returns SQL connection for users database
- `GetStockConnection()`: Returns SQL connection for stock database
- `TestConnection()`: Verifies database connectivity
- `InitializeDatabase()`: Sets up both databases with required tables
- `InitializeUsersDatabase()`: Creates users table and default users
- `InitializeStockDatabase()`: Creates calendar_events table

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

### 3. Services Layer
Located in the `Services` namespace, contains business logic and data operations.

#### UserService.cs
Handles user authentication and user-related operations:
- `AuthenticateUser(string username, string password)`: Validates user credentials and returns user information
  - Checks username and password against the database
  - Returns null if authentication fails
  - Returns User object with roles and permissions if successful

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
- Error handling with user-friendly messages

## Technical Details

### Database Technology
- Uses SQL Server LocalDB
- Two separate database files:
  - StockieUsers.mdf for user management
  - StockieInventory.mdf for stock operations
- Integrated Windows Authentication

### Security Features
- Password storage (Note: Currently stored as plain text - should be enhanced with proper hashing)
- Active/Inactive user status tracking
- Role-based access control
- Unique username constraints

### Error Handling
- Database connection errors are caught and reported
- User-friendly error messages
- Validation for empty username/password

### Default Users
The system initializes with three default users:
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