namespace Aurora.Core.Data.Abstractions
{
    /// <summary>
    ///     Unit of work that supports multiple data sources
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Registers a new context
        /// </summary>
        /// <param name="context"></param>
        void AddContext(ISupportsUnitOfWork context);

        /// <summary>
        ///     Unregisters a context
        /// </summary>
        /// <param name="context"></param>
        void RemoveContext(ISupportsUnitOfWork context);

        /// <summary>
        ///     Commits all the changes using a single transaction
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        void CommitAll(bool acceptAllChangesOnSuccess = true);
    }
}
