using AutoMapper;
using EcommerceCRUD.DTOs;
using EcommerceCRUD.Models;

namespace EcommerceCRUD.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            // User -> AuthenticateResponse
            CreateMap<User, LoginResponse>();

            // RegisterRequest -> User
            CreateMap<RegisterRequest, User>();

        }
    }
}
