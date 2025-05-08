using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Enums;
using TaskStatus = TaskAssignWebApi.Enums.TaskStatus;

namespace TaskAssignWebApi.Domain.Models.Abstract
{
    public abstract class CommonTask
    {
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Skala trudności musi być w przedziale od 1 do 5.")]
		public int DifficultyScale { get; set; }

		[MaxLength(400)]
		public string Description { get; set; }

		[EnumDataType(typeof(TaskType), ErrorMessage = "Nieznany typ zadania.")]
		public TaskType Type { get; set; }

		[EnumDataType(typeof(TaskStatus), ErrorMessage = "Nieznany status zadania.")]
		public TaskStatus Status { get; set; }

		public int? UserId { get; set; }

		public User User { get; set; }
	}
}
