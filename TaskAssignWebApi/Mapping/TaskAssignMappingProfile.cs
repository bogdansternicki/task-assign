using AutoMapper;
using TaskAssignWebApi.Domain.Models;
using TaskAssignWebApi.Domain.Models.Abstract;
using TaskAssignWebApi.DTOs;

namespace TaskAssignWebApi.Mapping
{
	public class TaskAssignMappingProfile : Profile
	{
		public TaskAssignMappingProfile()
		{
			CreateMap<User, UserDTO>();

			CreateMap<CommonTask, CommonTaskDTO>()
				.Include<DeploymentTask, DeploymentTaskDTO>()
				.Include<ImplementationTask, ImplementationTaskDTO>()
				.Include<MaintenanceTask, MaintenanceTaskDTO>();

			CreateMap<DeploymentTask, DeploymentTaskDTO>();

			CreateMap<ImplementationTask, ImplementationTaskDTO>();

			CreateMap<MaintenanceTask, MaintenanceTaskDTO>();
		}
	}
}
