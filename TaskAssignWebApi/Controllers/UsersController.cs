using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAssignWebApi.Domain;
using TaskAssignWebApi.DTOs;

namespace TaskAssignWebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly TaskAssignDbContext _context;
		private readonly IMapper _mapper;

		public UsersController(TaskAssignDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsersAsync()
		{
			return Ok(_mapper.Map<List<UserDTO>>(await _context.Users.ToListAsync()));
		}
	}
}
