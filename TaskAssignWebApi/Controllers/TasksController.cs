using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAssignWebApi.Domain;
using TaskAssignWebApi.DTOs;

namespace TaskAssignWebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TasksController : ControllerBase
	{
		private readonly TaskAssignDbContext _context;
		private readonly IMapper _mapper;

		public TasksController(TaskAssignDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetTasksAsync()
		{
			var tasks = await _context.Tasks
				.Include(x => x.User)
				.OrderByDescending(task => task.DifficultyScale)
				.ToListAsync();

			return Ok(_mapper.Map<List<CommonTaskDTO>>(tasks));
		}
	}
}
