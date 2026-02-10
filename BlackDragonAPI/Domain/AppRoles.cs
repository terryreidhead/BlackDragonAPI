namespace BlackDragonAPI.Domain
{
    /// <summary>
    /// Provides a set of constant string values that represent application roles for authorization and access control.
    /// </summary>
    /// <remarks>Use these role names to assign or check user permissions within the application. The
    /// constants defined in this class can be referenced when configuring role-based security policies or when
    /// evaluating user roles in authentication logic.</remarks>
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string InPersonStudent = "InPersonStudent";
        
    }
}
