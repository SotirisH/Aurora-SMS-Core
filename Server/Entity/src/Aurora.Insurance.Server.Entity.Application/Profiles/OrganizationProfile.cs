using Aurora.Insurance.Server.Entity.Domain.Models.Dtos;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;
using AutoMapper;

namespace Aurora.Insurance.Server.Entity.Application.Profiles
{
    public class OrganizationProfile: Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Agent, Broker>();
            CreateMap<Organization, NewOrganizationResponse>();
        }
    }
}
