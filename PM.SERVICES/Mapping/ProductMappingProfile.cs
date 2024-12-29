using AutoMapper;
using PM.CORE.DTOs;
using PM.CORE.Entities;

namespace PM.SERVICES.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Basic Product <-> ProductDto mapping
            CreateMap<Product, ProductDto>()
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.StockValue, opt => opt.MapFrom(src => src.Price * src.Stock));

            // Reverse mapping from DTO to Entity
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Product -> CategoryStatsDto mapping for grouping
            CreateMap<IGrouping<string, Product>, CategoryStatsDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.AveragePrice,
                    opt => opt.MapFrom(src => src.Average(p => p.Price)))
                .ForMember(dest => dest.TotalStock,
                    opt => opt.MapFrom(src => src.Sum(p => p.Stock)))
                .ForMember(dest => dest.StockValue,
                    opt => opt.MapFrom(src => src.Sum(p => p.Price * p.Stock)));
        }
    }
}
