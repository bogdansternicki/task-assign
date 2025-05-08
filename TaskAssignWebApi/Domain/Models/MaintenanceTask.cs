using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Domain.Models.Abstract;

namespace TaskAssignWebApi.Domain.Models
{
    public class MaintenanceTask : CommonTask
    {
	    [MaxLength(400)]
		public string Services { get; set; }

	    [MaxLength(400)]
		public string Servers { get; set; }

	    public DateTime Date { get; set; }
	}
}
