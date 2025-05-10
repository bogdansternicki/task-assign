using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Enums;
using TaskStatus = TaskAssignWebApi.Enums.TaskStatus;

namespace TaskAssignWebApi.DTOs
{
	public class CommonTaskDTO
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Opis zadania jest wymagany.")]
		[StringLength(400, ErrorMessage = "Opis zadania nie może mieć więcej niż 400 znaków.")]
		public string Description { get; set; }

		[Range(1, 5, ErrorMessage = "Skala trudności musi być w przedziale od 1 do 5.")]
		public int DifficultyScale { get; set; }

		[Required(ErrorMessage = "Typ zadania jest wymagany.")]
		public TaskType Type { get; set; }

		[Required(ErrorMessage = "Status zadania jest wymagany.")]
		public TaskStatus Status { get; set; }

		public int? UserId { get; set; }

		public UserDTO User { get; set; }
	}
}
