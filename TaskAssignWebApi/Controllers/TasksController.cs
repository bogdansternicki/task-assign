using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAssignWebApi.Domain;
using TaskAssignWebApi.DTOs;
using TaskAssignWebApi.Enums;

namespace TaskAssignWebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TasksController : ControllerBase
	{
		private readonly TaskAssignDbContext _context;
		private readonly IMapper _mapper;
		private readonly int MIN_TASK_COUNT = 5;
		private readonly int MAX_TASK_COUNT = 11;
		private readonly int[] DIFFICULT_TASKS = [4, 5];
		private readonly int[] SIMPLE_TASKS = [1, 2];
		private readonly int MAX_DIFFICULT_TASK_PERCENTAGE = 30;
		private readonly int MIN_DIFFICULT_TASK_PERCENTAGE = 10;
		private readonly int MAX_SIMPLE_TASK_PERCENTAGE = 50;
		private readonly int PAGE_SIZE = 10;

		public TasksController(TaskAssignDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet("available")]
		public async Task<IActionResult> GetAvailableTasksAsync([FromQuery] int pageIndex, [FromQuery] int userId)
		{
			var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

			if (user == null)
				return BadRequest("User does not exist.");

			var tasks = await _context.Tasks
				.Where(task => task.UserId == null && (user.Type == UserType.DevOps || task.Type == TaskType.Implementation))
				.OrderByDescending(task => task.DifficultyScale)
				.Skip(pageIndex * PAGE_SIZE)
				.Take(PAGE_SIZE)
				.ToListAsync();

			var taskCount = await _context.Tasks
				.Where(task => task.UserId == null && (user.Type == UserType.DevOps || task.Type == TaskType.Implementation))
				.CountAsync();

			return Ok(new TaskListDTO() { Count = taskCount, Tasks = _mapper.Map<List<CommonTaskDTO>>(tasks) });
		}

		[HttpGet("assigned")]
		public async Task<IActionResult> GetAssignedTasksAsync([FromQuery] int pageIndex, [FromQuery] int userId)
		{
			var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

			if (user == null)
				return BadRequest("User does not exist.");

			var tasks = await _context.Tasks
				.Include(task => task.User)
				.Where(task => task.UserId == userId && (user.Type == UserType.DevOps || task.Type == TaskType.Implementation))
				.OrderByDescending(task => task.DifficultyScale)
				.Skip(pageIndex * PAGE_SIZE)
				.Take(PAGE_SIZE)
				.ToListAsync();

			var taskCount = await _context.Tasks
				.Where(task => task.UserId == userId && (user.Type == UserType.DevOps || task.Type == TaskType.Implementation))
				.CountAsync();

			return Ok(new TaskListDTO() { Count = taskCount, Tasks = _mapper.Map<List<CommonTaskDTO>>(tasks) });
		}

		[HttpPut]
		public async Task<IActionResult> UpdateAssignedUsersAsync([FromBody] UpdateTasksDto updateTasksDto)
		{
			var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == updateTasksDto.UserId);
			if (user == null)
				return BadRequest("User does not exist.");

			var allTasks = await _context.Tasks
				.Where(task => user.Type == UserType.DevOps ? true : task.Type == TaskType.Implementation)
				.ToListAsync();

			allTasks.Where(task => task.UserId == updateTasksDto.UserId && updateTasksDto.UnAssignTaskIds.Contains(task.Id))
				.ToList()
				.ForEach(task => task.UserId = null);

			allTasks.Where(task => updateTasksDto.AssignTaskIds.Contains(task.Id))
				.ToList()
				.ForEach(task => task.UserId = updateTasksDto.UserId);

			var userTasks = allTasks.Where(task => task.UserId == updateTasksDto.UserId).ToList();

			if (userTasks.Count < MIN_TASK_COUNT || MAX_TASK_COUNT < userTasks.Count)
				return BadRequest("Too many or too little tasks assigned. Assign at least 5 tasks and not more than 11 tasks.");

			var difficultTaskPercentage = (double)userTasks.Where(task => DIFFICULT_TASKS.Contains(task.DifficultyScale)).Count() / userTasks.Count() * 100;
			if (difficultTaskPercentage < MIN_DIFFICULT_TASK_PERCENTAGE || difficultTaskPercentage > MAX_DIFFICULT_TASK_PERCENTAGE)
				return BadRequest("Difficult tasks must fall within the range of 10% to 30%.");

			var simpleTaskPercentage = (double)userTasks.Where(task => SIMPLE_TASKS.Contains(task.DifficultyScale)).Count() / userTasks.Count() * 100;
			if (simpleTaskPercentage > MAX_SIMPLE_TASK_PERCENTAGE)
				return BadRequest("The number of simple tasks must not exceed 50%.");

			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
