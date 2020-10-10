using Aurora.Core.Data;

namespace Aurora.Insurance.EFModel
{
    /// <summary>
    ///     Registered insurance companies in the system
    /// </summary>
    public class Company : EntityBase
    {
        /// <summary>
        ///     The widely known code of the company
        /// </summary>
        public string Id { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string LogoData { get; set; }
    }
}
