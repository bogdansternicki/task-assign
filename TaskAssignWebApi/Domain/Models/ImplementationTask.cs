using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Domain.Models.Abstract;

namespace TaskAssignWebApi.Domain.Models
{
    public class ImplementationTask : CommonTask
    {
	    [MaxLength(1000)]
		public string TaskContent { get; set; }
    }
}
