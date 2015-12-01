using System;
using System.Linq;
using System.Security.Principal;

using Raven.Abstractions;
using Raven.Abstractions.Data;
using Raven.Abstractions.Logging;
using Raven.Database.Server.Abstractions;

// Raven.Database.Extensions.RoleFinder interface to Raven.SpecificPlatform.Windows as Raven should NOT reference System.DirectoryServices.AccountManagement

namespace Raven.Database.Extensions
{
    public static class RoleFinder
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public class PrimitiveParams
        {
            public PrimitiveParams(IPrincipal principal)
            {
                var oauthPrincipal = principal as OAuthPrincipal;
                if (oauthPrincipal == null)
                    IsOAuthNull = true;
                else
                    IsGlobalAdmin = oauthPrincipal.IsGlobalAdmin();
            }

            public bool IsOAuthNull = false;
            public bool IsGlobalAdmin = false;
        }

        public static bool IsInRole(this IPrincipal principal, Raven.Database.Server.AnonymousUserAccessMode mode, WindowsBuiltInRole role)
        {
            var primitiveParameters = new PrimitiveParams(principal);
            if (EnvironmentUtils.RunningOnPosix == false)
            {
                bool isModeAdmin = (mode == Raven.Database.Server.AnonymousUserAccessMode.Admin);

                if (principal == null || principal.Identity == null | principal.Identity.IsAuthenticated == false)
                {
                    return isModeAdmin;
                }
                var databaseAccessPrincipal = principal as PrincipalWithDatabaseAccess;
                var windowsPrincipal = databaseAccessPrincipal == null ? principal as WindowsPrincipal : databaseAccessPrincipal.Principal;

                return Raven.SpecificPlatform.Windows.RoleFinder.IsInRole(windowsPrincipal, isModeAdmin, role, SystemTime.UtcNow, s => log.Debug(s),
                    primitiveParameters.IsOAuthNull,
                    primitiveParameters.IsGlobalAdmin,
                    log.WarnException);
            }
            else
                throw new FeatureNotSupportedOnPosixException("IsInRole is not supported when running on posix");
        }

        public static bool IsAdministrator(this IPrincipal principal, Raven.Database.Server.AnonymousUserAccessMode mode)
        {
            var primitiveParameters = new PrimitiveParams(principal);
            if (EnvironmentUtils.RunningOnPosix == false)
            {
                bool isModeAdmin = (mode == Raven.Database.Server.AnonymousUserAccessMode.Admin);

                if (principal == null || principal.Identity == null | principal.Identity.IsAuthenticated == false)
                {
                    return isModeAdmin;
                }
                var databaseAccessPrincipal = principal as PrincipalWithDatabaseAccess;
                var windowsPrincipal = databaseAccessPrincipal == null ? principal as WindowsPrincipal : databaseAccessPrincipal.Principal;

                return Raven.SpecificPlatform.Windows.RoleFinder.IsInRole(windowsPrincipal, isModeAdmin, WindowsBuiltInRole.Administrator, SystemTime.UtcNow, s => log.Debug(s),
                    primitiveParameters.IsOAuthNull,
                    primitiveParameters.IsGlobalAdmin,
                    log.WarnException);
            }
            else
                throw new FeatureNotSupportedOnPosixException("IsInRole is not supported when running on posix");
        }

        public static bool IsBackupOperator(this IPrincipal principal, Raven.Database.Server.AnonymousUserAccessMode mode)
        {
            var primitiveParameters = new PrimitiveParams(principal);
            if (EnvironmentUtils.RunningOnPosix == false)
            {
                bool isModeAdmin = (mode == Raven.Database.Server.AnonymousUserAccessMode.All);

                if (principal == null || principal.Identity == null | principal.Identity.IsAuthenticated == false)
                {
                    return isModeAdmin;
                }
                var databaseAccessPrincipal = principal as PrincipalWithDatabaseAccess;
                var windowsPrincipal = databaseAccessPrincipal == null ? principal as WindowsPrincipal : databaseAccessPrincipal.Principal;

                return Raven.SpecificPlatform.Windows.RoleFinder.IsInRole(windowsPrincipal, isModeAdmin, WindowsBuiltInRole.BackupOperator, SystemTime.UtcNow, s => log.Debug(s),
                    primitiveParameters.IsOAuthNull,
                    primitiveParameters.IsGlobalAdmin,
                    log.WarnException);
            }
            else
                throw new FeatureNotSupportedOnPosixException("IsInRole is not supported when running on posix");
        }

        public class CachingRoleFinder
        {
            private Raven.SpecificPlatform.Windows.RoleFinder.CachingRoleFinder cachingRoleFinder = new Raven.SpecificPlatform.Windows.RoleFinder.CachingRoleFinder();
            private static readonly ILog log = LogManager.GetCurrentClassLogger();

            public bool IsInRole(WindowsIdentity windowsIdentity, WindowsBuiltInRole role)
            {
                if (EnvironmentUtils.RunningOnPosix == false)
                    return cachingRoleFinder.IsInRole(windowsIdentity, role, SystemTime.UtcNow, s => log.Debug(s),
                        false,
                        false,
                        log.WarnException);
                else
                    throw new FeatureNotSupportedOnPosixException("IsInRole is not supported when running on posix");
            }
        }

        public static bool IsAdministrator(this IPrincipal principal, DocumentDatabase database)
        {
            var name = database.Name ?? Constants.SystemDatabase;
            return IsAdministrator(principal, name);
        }

        private static bool isDbAccessAdmin(IPrincipal principal, string databaseNane)
        {
            var oauthPrincipal = principal as OAuthPrincipal;
            if (oauthPrincipal != null)
            {
                foreach (var dbAccess in oauthPrincipal.TokenBody.AuthorizedDatabases.Where(x => x.Admin))
                {
                    if (dbAccess.TenantId == "*" && databaseNane != null && databaseNane != Constants.SystemDatabase)
                        return true;
                    if (string.Equals(dbAccess.TenantId, databaseNane, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                    if (databaseNane == null &&
                        string.Equals(dbAccess.TenantId, Constants.SystemDatabase, StringComparison.InvariantCultureIgnoreCase))
                        return false;
                }
            }
            return false;
        }

        public static bool IsAdministrator(this IPrincipal principal, string databaseName)
        {
            if (EnvironmentUtils.RunningOnPosix == false)
            {
                var databaseAccessPrincipal = principal as PrincipalWithDatabaseAccess;
                if (databaseAccessPrincipal != null)
                {
                    if (databaseAccessPrincipal.AdminDatabases.Any(name => name == "*")
                        && databaseName != null && databaseName != Constants.SystemDatabase)
                        return true;
                    if (databaseAccessPrincipal.AdminDatabases.Any(name => string.Equals(name, databaseName, StringComparison.InvariantCultureIgnoreCase)))
                        return true;
                    if (databaseName == null &&
                        databaseAccessPrincipal.AdminDatabases.Any(
                            name => string.Equals(name, Constants.SystemDatabase, StringComparison.InvariantCultureIgnoreCase)))
                        return true;
                    return false;
                }

                return isDbAccessAdmin(principal, databaseName);
            }
            else
                throw new FeatureNotSupportedOnPosixException("IsInRole is not supported when running on posix");
        }


        public static bool IsReadOnly(this IPrincipal principal, string databaseName)
        {

            var databaseAccessPrincipal = principal as PrincipalWithDatabaseAccess;
            if (databaseAccessPrincipal != null)
            {
                if (databaseName != null && databaseAccessPrincipal.ReadOnlyDatabases.Contains(databaseName))
                    return true;
            }
            return false;
        }
    }
}