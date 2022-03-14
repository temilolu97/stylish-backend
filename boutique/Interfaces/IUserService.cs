using boutique.DTOs;
using EcommerceCRUD.DTOs;
using EcommerceCRUD.Models;

namespace EcommerceCRUD.Interfaces
{
    public interface IUserService
    {
        LoginResponse Login(LoginRequest request);
        RegisterResponse Register(RegisterRequest request);
        RegisterResponse AdminRegister(RegisterRequest request);
        User GetById(int id);
    }
}
