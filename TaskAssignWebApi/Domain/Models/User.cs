using TaskAssignWebApi.Domain.Models.Abstract;
using TaskAssignWebApi.Enums;

namespace TaskAssignWebApi.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserType Type { get; set; }

        public ICollection<CommonTask> Tasks { get; set; } = new List<CommonTask>();
	}
}
