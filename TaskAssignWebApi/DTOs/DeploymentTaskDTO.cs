using System.ComponentModel.DataAnnotations;

namespace TaskAssignWebApi.DTOs
{
	public class DeploymentTaskDTO : CommonTaskDTO
	{
		[Required(ErrorMessage = "Zakres wdrożenia jest wymagany.")]
		[StringLength(400, ErrorMessage = "Zakres wdrożenia nie może przekraczać 400 znaków.")]
		public string DeploymentScope { get; set; }

		[Required(ErrorMessage = "Data wdrożenia jest wymagana")]
		public DateTime Date { get; set; }
	}
}
