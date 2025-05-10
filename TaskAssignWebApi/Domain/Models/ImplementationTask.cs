using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Domain.Models.Abstract;

namespace TaskAssignWebApi.Domain.Models
{
    public class ImplementationTask : CommonTask
    {
	    [MaxLength(1000, ErrorMessage = "Treść zadania nie może przekraczać 1000 znaków.")]
		public string TaskContent { get; set; }
    }
}
