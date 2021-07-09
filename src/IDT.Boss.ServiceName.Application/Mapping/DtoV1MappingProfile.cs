using AutoMapper;
using IDT.Boss.ServiceName.Application.DTOs;
using IDT.Boss.ServiceName.Application.Models;

namespace IDT.Boss.ServiceName.Application.Mapping
{
    /// <summary>
    /// Mapping profile for AutoMapper with configuration for DTO models (V1).
    /// </summary>
    public sealed class DtoV1MappingProfile : Profile
    {
        public DtoV1MappingProfile()
        {
            ConfigureSharedMappings();
        }

        /// <summary>
        /// Configuration for shared models mappings that will be use for all the Api Versions.
        /// </summary>
        private void ConfigureSharedMappings()
        {
            CreateMap<StatusResponse, StatusResponseDto>();
            CreateMap<AppInfo, AppInfoDto>();
            CreateMap<AmazonInfo, AmazonInfoDto>();
        }
    }
}