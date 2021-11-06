namespace Aurora.Insurance.Server.Entity.Domain.Models.Dtos
{
    public record NewOrganizationRequest
    {
        public string Title { get; init; } = null!;
        public string TaxId { get; init; } = null!;
        public string EmailAddress { get; init; } = null!;
    }
}
