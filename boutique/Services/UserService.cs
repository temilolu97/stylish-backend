using AutoMapper;
using BCrypt.Net;
using boutique.DTOs;
using EcommerceCRUD.Contexts;
using EcommerceCRUD.DTOs;
using EcommerceCRUD.Enums;
using EcommerceCRUD.Interfaces;
using EcommerceCRUD.Models;
using System;
using System.Linq;

namespace EcommerceCRUD.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _databaseContext;
        private DatabaseContext _databasecontext;
        private JwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        public UserService(DatabaseContext databaseContext, JwtUtils jwtUtils, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }
        public LoginResponse Login(LoginRequest request)
        {
            try
            {
                var user = _databaseContext.Users.SingleOrDefault(x => x.Email == request.Email);
                if (user == null) throw new Exception("User with this email does not exist");
                var passwordVerified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

                if (!passwordVerified) throw new Exception("Incorrect password entered");
                var response = _mapper.Map<LoginResponse>(user);
                response.Token = _jwtUtils.GenerateToken(user);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not login. Looks like you don't have an account registered");
            }
        }

        public RegisterResponse Register(RegisterRequest request)
        {
            
            try
            {

                var existingUser = _databaseContext.Users.SingleOrDefault(u => u.Email == request.Email);
                if (existingUser!=null) throw new Exception("Email " + request.Email + " is already taken");
                var user = _mapper.Map<User>(request);

                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.Role = (int)RolesEnum.User;
                user.DateCreated = DateTime.UtcNow;
                _databaseContext.Users.Add(user);
                _databaseContext.SaveChanges();
                return new RegisterResponse { Status = "successful", StatusCode = "00", Message = "Successfully created account" };
            }
            catch (Exception ex)
            {
                throw;
            }
        
        }

        public RegisterResponse AdminRegister(RegisterRequest request)
        {

            try
            {

                var existingUser = _databaseContext.Users.SingleOrDefault(u => u.Email == request.Email);
                if (existingUser != null) throw new Exception("Email " + request.Email + " is already taken");
                var user = _mapper.Map<User>(request);

                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.Role = (int)RolesEnum.Admin;
                user.DateCreated = DateTime.UtcNow;
                _databaseContext.Users.Add(user);
                _databaseContext.SaveChanges();
                return new RegisterResponse { Status = "successful", StatusCode = "00", Message = "Successfully created admin" };
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public User GetById(int id)
        {
            return GetUser(id);
        }

        private User GetUser(int id)
        {
            var user = _databaseContext.Users.Find(id);
            if (user == null) throw new Exception("User not found");
            return user;
        }
    }
}
