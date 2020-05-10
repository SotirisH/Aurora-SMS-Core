namespace Aurora.Core.Data
{
    /// <summary>
    ///     Interface for supporting uow
    /// </summary>
    public interface ISupportsUnitOfWork
    {
        int SaveChanges(bool acceptAllChangesOnSuccess);
    }
}
