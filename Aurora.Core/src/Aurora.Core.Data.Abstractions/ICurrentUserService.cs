namespace Aurora.Core.Data.Abstractions
{
    /// <summary>
    ///     Interface for providing the name of the user who had performed the changes changes on the entity objects
    /// </summary>
    public interface ICurrentUserService
    {
        string GetCurrentUser();
    }
}
