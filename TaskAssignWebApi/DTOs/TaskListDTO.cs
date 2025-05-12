namespace TaskAssignWebApi.DTOs
{
	public class TaskListDTO
	{
		public int Count { get; set; }
		public IEnumerable<CommonTaskDTO> Tasks = new List<CommonTaskDTO>();
	}
}
