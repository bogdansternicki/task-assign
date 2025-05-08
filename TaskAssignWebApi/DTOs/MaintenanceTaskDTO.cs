using System.ComponentModel.DataAnnotations;

namespace TaskAssignWebApi.DTOs
{
	public class MaintenanceTaskDTO
	{
		[Required(ErrorMessage = "Lista serwisów jest wymagana.")]
		[StringLength(400, ErrorMessage = "Lista serwisów nie może przekraczać 400 znaków.")]
		public string Services { get; set; }

		[Required(ErrorMessage = "Lista serwerów jest wymagana.")]
		[StringLength(400, ErrorMessage = "Lista serwerów nie może przekraczać 400 znaków.")]
		public string Servers { get; set; }

		[Required(ErrorMessage = "Data serwisowania jest wymagana")]
		public DateTime Date { get; set; }
	}
}
