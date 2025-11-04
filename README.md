# Stockie

## Resumen de funciones (Español)

- Autenticación de usuarios: Inicio de sesión con usuarios almacenados en LocalDB. Soporta roles y estado activo/inactivo.
- Gestión de usuarios: Inicialización de usuarios por defecto. (Interfaz avanzada pendiente).
- Gestión de inventario (`Items`): Visualización de items, creación y eliminación desde la UI. Las modificaciones están restringidas a usuarios con rol `Administrator`.
- Calendario central: Muestra un calendario en el área principal de la aplicación.
- Menú lateral colapsable: Navegación con las opciones: Inventario, Transferencias, Envios, Consulta de Stat, Reportes, Items, Configuración.
- Base de datos local (LocalDB): Dos archivos `.mdf` separados: `StockieUsers.mdf` y `StockieInventory.mdf`.
- Inicialización automática: Al iniciar la app se crean las bases y tablas si no existen.
- Mensajes y validaciones: Validaciones básicas de login y manejo de errores amigable al usuario.

## Summary of features (English)

- User authentication: Login using users stored in LocalDB. Supports roles and active/inactive status.
- User management: Default users initialized on first run. (Full UI for user management pending).
- Inventory management (`Items`): View items, add and delete items from the UI. Modifications are restricted to users with `Administrator` role.
- Central calendar: Displays a calendar in the main area of the application.
- Collapsible side menu: Navigation options include: Inventario, Transferencias, Envios, Consulta de Stat, Reportes, Items, Configuracion.
- Local database (LocalDB): Two separate `.mdf` files: `StockieUsers.mdf` and `StockieInventory.mdf`.
- Automatic initialization: The app creates databases and tables if they do not exist on startup.
- Validation and user messages: Basic login validation and friendly error reporting.