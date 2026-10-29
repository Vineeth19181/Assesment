PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Providers (
    ProviderId INTEGER NOT NULL CONSTRAINT PK_Providers PRIMARY KEY AUTOINCREMENT,
    ProviderName TEXT NOT NULL,
    County TEXT NOT NULL,
    Status TEXT NOT NULL,
    CreatedDate TEXT NOT NULL,
    UpdatedDate TEXT NULL,
    IsDeleted INTEGER NOT NULL DEFAULT 0,
    DeletedDate TEXT NULL
);
CREATE INDEX IF NOT EXISTS IX_Providers_ProviderName ON Providers (ProviderName);
CREATE INDEX IF NOT EXISTS IX_Providers_IsDeleted ON Providers (IsDeleted);

CREATE TABLE IF NOT EXISTS Licenses (
    LicenseId INTEGER NOT NULL CONSTRAINT PK_Licenses PRIMARY KEY AUTOINCREMENT,
    ProviderId INTEGER NOT NULL,
    LicenseNumber TEXT NOT NULL,
    LicenseStatus TEXT NOT NULL,
    ExpirationDate TEXT NOT NULL,
    CreatedDate TEXT NOT NULL,
    UpdatedDate TEXT NULL,
    IsDeleted INTEGER NOT NULL DEFAULT 0,
    DeletedDate TEXT NULL,
    CONSTRAINT FK_Licenses_Providers_ProviderId FOREIGN KEY (ProviderId) REFERENCES Providers (ProviderId) ON DELETE RESTRICT,
    CONSTRAINT UQ_Licenses_ProviderId_LicenseNumber UNIQUE (ProviderId, LicenseNumber)
);
CREATE INDEX IF NOT EXISTS IX_Licenses_ExpirationDate ON Licenses (ExpirationDate);
CREATE INDEX IF NOT EXISTS IX_Licenses_IsDeleted ON Licenses (IsDeleted);
