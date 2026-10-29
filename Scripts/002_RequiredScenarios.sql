-- Active providers with active licenses
SELECT p.ProviderId, p.ProviderName, p.County, l.LicenseNumber, l.ExpirationDate
FROM Providers p
JOIN Licenses l ON l.ProviderId = p.ProviderId
WHERE p.IsDeleted = 0
  AND l.IsDeleted = 0
  AND p.Status = 'Active'
  AND l.LicenseStatus = 'Active'
  AND date(l.ExpirationDate) >= date('now');

-- Providers that are active but have one or more expired licenses
SELECT p.ProviderId, p.ProviderName, p.County, COUNT(l.LicenseId) AS ExpiredLicenseCount
FROM Providers p
JOIN Licenses l ON l.ProviderId = p.ProviderId
WHERE p.IsDeleted = 0
  AND l.IsDeleted = 0
  AND p.Status = 'Active'
  AND date(l.ExpirationDate) < date('now')
GROUP BY p.ProviderId, p.ProviderName, p.County;

-- Standard provider view: soft-deleted providers excluded
SELECT ProviderId, ProviderName, County, Status, CreatedDate
FROM Providers
WHERE IsDeleted = 0
ORDER BY ProviderName;

-- Audit/troubleshooting view: deleted providers remain queryable
SELECT ProviderId, ProviderName, County, Status, CreatedDate, DeletedDate
FROM Providers
WHERE IsDeleted = 1
ORDER BY DeletedDate DESC;
