using AutoMapper;
using boutique.DTOs;
using EcommerceCRUD.Attributes;
using EcommerceCRUD.Contexts;
using EcommerceCRUD.DTOs;
using EcommerceCRUD.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace EcommerceCRUD.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private  IUserService _userService;
        private IMapper _mapper;
        private readonly Settings _settings;

        public UsersController(IUserService userService, IMapper mapper, IOptions<Settings> settings)
        {
            _userService = userService;
            _mapper = mapper;
            _settings = settings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {

                var response = _userService.Login(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            try
            {
                var response = _userService.Register(request);
                return new ObjectResult(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
           // return Ok(new { message = "Registration successful" });

        }

        [AllowAnonymous]
        [HttpPost("admin-register")]
        public IActionResult AdminRegister(RegisterRequest request)
        {
            try
            {
                var response = _userService.AdminRegister(request);
                return new ObjectResult(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            //return Ok(new { message = "Registration successful" });

        }
    }
}
