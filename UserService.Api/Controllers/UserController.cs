using EcomSystem.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Users.AssignRole;
using UserService.Application.Users.BlockUser;
using UserService.Application.Users.CreateUser;
using UserService.Application.Users.CreateUserAddress.UserService.Application.Users.CreateUserAddress;
using UserService.Application.Users.CreateUserDevice;
using UserService.Application.Users.GetAllUsers;
using UserService.Application.Users.GetUserActivity;
using UserService.Application.Users.GetUserAddresses;
using UserService.Application.Users.GetUserByEmail;
using UserService.Application.Users.GetUserById;
using UserService.Application.Users.GetUserDevices;
using UserService.Application.Users.LogoutAllDevices;
using UserService.Application.Users.RemoveRole;
using UserService.Application.Users.RestoreUser;
using UserService.Application.Users.SearchUsers;
using UserService.Application.Users.SoftDeleteUser;
using UserService.Application.Users.UnblockUser;
using UserService.Application.Users.UpdateUser;
using UserService.Domain.Entities;



namespace UserService.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand
            {
                Id = request.Id, // 🔥 BẮT BUỘC
                Email = request.Email,
                Password = request.Password,
                FullName = request.FullName
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        // GET /users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return Ok(result);
        }
        
        // 🔥 FLOW 3
        // GET /users/by-email?email=abc@gmail.com
        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var result = await _mediator.Send(new GetUserByEmailQuery(email));
            return Ok(result);
        }


        // 🔥 FLOW 4
        // PUT /users/{id} cái này cũng dùng cái UpdateUserRequest từ contracts để nhận data
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var command = new UpdateUserCommand
            {
                Id = id,
                FullName = request.FullName,
                PasswordHash = request.PasswordHash
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }


        
        
       

        [HttpGet("getAllUser")]
        public async Task<IActionResult> GetAllUsers(int page = 1, int pageSize = 10)
        {
            var query = new GetAllUsersQuery
            {
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers(
    string keyword,
    int page = 1,
    int pageSize = 10)
        {
            var query = new SearchUsersQuery
            {
                Keyword = keyword,
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/block")]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            var command = new BlockUserCommand { UserId = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/unblock")]
        public async Task<IActionResult> UnblockUser(Guid id)
        {
            var command = new UnblockUserCommand { UserId = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteUser(Guid id)
        {
            var command = new SoftDeleteUserCommand { UserId = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreUser(Guid id)
        {
            var command = new RestoreUserCommand
            {
                UserId = id
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        
        [HttpGet("{id}/activity")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var result = await _mediator.Send(
                new GetUserActivityQuery { UserId = id });

            return Ok(result);
        }
        [HttpGet("{id}/addresses")]
        public async Task<IActionResult> GetAddresses(Guid id)
        {
            var result = await _mediator.Send(new GetUserAddressesQuery
            {
                UserId = id
            });

            return Ok(result);
        }
        [HttpPost("{id}/addresses")]
        public async Task<IActionResult> AddAddress(Guid id, CreateUserAddressCommand command)
        {
            command.UserId = id;

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpGet("{id}/devices")]
        public async Task<IActionResult> GetDevices(Guid id)
        {
            var result = await _mediator.Send(new GetUserDevicesQuery
            {
                UserId = id
            });

            return Ok(result);
        }

        [HttpPost("{id}/logout-all")]
        public async Task<IActionResult> LogoutAll(Guid id)
        {
            var result = await _mediator.Send(new LogoutAllDevicesCommand
            {
                UserId = id
            });

            return Ok(result);
        }
        [HttpPost("{id}/devices")]
        public async Task<IActionResult> CreateDevice(Guid id, [FromBody] CreateUserDeviceCommand command)
        {
            command.UserId = id;

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}