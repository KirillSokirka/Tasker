IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'identity')
BEGIN
    CREATE DATABASE [identity];
END

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'application')
BEGIN
    CREATE DATABASE [application];
END
