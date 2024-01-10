using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkintechRestApiDemo.Business.Authentication;
using WorkintechRestApiDemo.Domain.Entities;

namespace WorkintechRestApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService= _userService;
        }

        [HttpGet]
        public async Task<List<UserEntity>> Get()
        {
           return await userService.GetUsers();
        }

        [HttpGet("{id}")]

        public async Task<UserEntity> Get(int id)
        {
            return await userService.GetUserById(id);
        }

        [HttpPost]
        public async Task<UserEntity> Post(UserEntity userEntity)
        {
            return await userService.CreateUser(userEntity);
        }   
    }
}
