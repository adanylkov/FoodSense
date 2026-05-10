using AutoMapper;

namespace FoodSense.API.Data.Models.Configuration;

public class FoodSenseProfile : Profile
{
    public FoodSenseProfile()
    {
        CreateMap<OpenFoodFactsCSharp.Models.Product, Product>();
    }
}
