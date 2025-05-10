using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Domain.Models.Abstract;
using TaskAssignWebApi.Enums;

namespace TaskAssignWebApi.Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [EnumDataType(typeof(UserType), ErrorMessage = "Nieznany typ użytkownika.")]
		public UserType Type { get; set; }

        public ICollection<CommonTask> Tasks { get; set; } = new List<CommonTask>();
	}
}
