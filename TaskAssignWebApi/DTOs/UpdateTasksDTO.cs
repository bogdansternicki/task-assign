namespace TaskAssignWebApi.DTOs
{
	public class UpdateTasksDto
	{
		public int UserId { get; set; }
		public List<int> AssignTaskIds { get; set; } = [];
		public List<int> UnAssignTaskIds { get; set; } = [];

	}
}
