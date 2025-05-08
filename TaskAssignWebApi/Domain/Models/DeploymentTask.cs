using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Domain.Models.Abstract;

namespace TaskAssignWebApi.Domain.Models
{
    public class DeploymentTask : CommonTask
    {
	    [MaxLength(400)]
		public string DeploymentScope;

	    public DateTime Date { get; set; }
	}
}
