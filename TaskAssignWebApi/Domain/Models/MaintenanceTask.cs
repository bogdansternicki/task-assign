using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Domain.Models.Abstract;

namespace TaskAssignWebApi.Domain.Models
{
    public class MaintenanceTask : CommonTask
    {
	    [MaxLength(400, ErrorMessage = "Lista serwisów nie może przekraczać 400 znaków.")]
		public string Services { get; set; }

		[MaxLength(400, ErrorMessage = "Lista serwerów nie może przekraczać 400 znaków.")]
		public string Servers { get; set; }

	    public DateTime Date { get; set; }
	}
}
