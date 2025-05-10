using System.ComponentModel.DataAnnotations;
using TaskAssignWebApi.Enums;

namespace TaskAssignWebApi.DTOs
{
	public class UserDTO
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Imię użytkownika jest wymagane.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Typ użytkownika jest wymagany.")]
		public UserType Type { get; set; }
	}
}
