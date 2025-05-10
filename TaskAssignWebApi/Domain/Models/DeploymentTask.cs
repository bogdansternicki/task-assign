using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Domain.Models.Abstract;

namespace TaskAssignWebApi.Domain.Models
{
    public class DeploymentTask : CommonTask
    {
	    [MaxLength(400, ErrorMessage = "Zakres wdrożenia nie może przekraczać 400 znaków.")]
		public string DeploymentScope { get; set; }

	    public DateTime Date { get; set; }
	}
}
