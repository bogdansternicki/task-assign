using System.ComponentModel.DataAnnotations;

namespace TaskAssignWebApi.DTOs
{
	public class ImplementationTaskDTO : CommonTaskDTO
	{
		[Required(ErrorMessage = "Treść zadania jest wymagana.")]
		[StringLength(1000, ErrorMessage = "Treść zadania nie może przekraczać 1000 znaków.")]
		public string TaskContent { get; set; }
	}
}
