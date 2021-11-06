namespace Aurora.Core.Data.Abstractions
{
    /// <summary>
    ///     Interface for supporting uow
    /// </summary>
    public interface ISupportsUnitOfWork
    {
        int SaveChanges(bool acceptAllChangesOnSuccess);
    }
}
