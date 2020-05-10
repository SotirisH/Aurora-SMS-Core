using System;

namespace Aurora.Insurance.EFModel
{
    /// <summary>
    ///     Represents an insurance contract for a vehicle
    /// </summary>
    public class Contract
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string PlateNumber { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime? CanceledDate { get; set; }
        public virtual Company Company { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
