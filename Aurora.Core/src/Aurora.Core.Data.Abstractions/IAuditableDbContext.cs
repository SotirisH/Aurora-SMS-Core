namespace Aurora.Core.Data.Abstractions
{
    /// <summary>
    ///     All DBContext instances should implement this interface in order
    ///     the Entity object to be auditable
    /// </summary>
    public interface IAuditableDbContext
    {
        int SaveChanges(string userName);
    }
}
