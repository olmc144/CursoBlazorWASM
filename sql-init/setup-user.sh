# sql-init/setup-user.sh

# NOTA: Las líneas anteriores (sqlservr & y sleep 20) han sido ELIMINADAS.
# Docker ya se encarga de iniciar y esperar al servidor SQL.

# 1. Ejecutar el script SQL usando la nueva ruta correcta de sqlcmd
/opt/mssql/bin/sqlcmd -S localhost -U sa -P "Admin_123!" -Q "
    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'dbMiBlazorMud')
        BEGIN
            CREATE DATABASE dbMiBlazorMud;
        END;
    
    USE dbMiBlazorMud;
    
    IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'oscarc')
        BEGIN
            CREATE LOGIN oscarc WITH PASSWORD = 'Oscar_2025!';
            CREATE USER oscarc FOR LOGIN oscarc;
            EXEC sp_addrolemember 'db_owner', 'oscarc';
        END;
"