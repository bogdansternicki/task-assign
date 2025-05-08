using Microsoft.EntityFrameworkCore;
using TaskAssignWebApi.Domain.Models;
using TaskAssignWebApi.Domain.Models.Abstract;
using TaskAssignWebApi.Enums;
using TaskStatus = TaskAssignWebApi.Enums.TaskStatus;

namespace TaskAssignWebApi.Domain
{
	public class TaskAssignDbContext : DbContext
	{
		public DbSet<Task> Tasks { get; set; }
		public DbSet<User> Users { get; set; }

		public TaskAssignDbContext(DbContextOptions<TaskAssignDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<CommonTask>()
				.HasDiscriminator<TaskType>("TaskType")
				.HasValue<DeploymentTask>(TaskType.Deployment)
				.HasValue<MaintenanceTask>(TaskType.Maintenance)
				.HasValue<ImplementationTask>(TaskType.Implementation);

			modelBuilder.Entity<CommonTask>()
				.HasOne(t => t.User)
				.WithMany(u => u.Tasks)
				.HasForeignKey(t => t.UserId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<User>().HasData(
				new User { Id = 1, Name = "Anna", Type = UserType.Developer },
				new User { Id = 2, Name = "Tomek", Type = UserType.DevOps },
				new User { Id = 3, Name = "Ola", Type = UserType.DevOps },
				new User { Id = 4, Name = "Jan", Type = UserType.Developer },
				new User { Id = 5, Name = "Bartek", Type = UserType.DevOps }
			);

			modelBuilder.Entity<CommonTask>().HasData(
			   new DeploymentTask { Id = 1, Description = "Wdrożenie nowej wersji aplikacji", DifficultyScale = 5, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(2), Type = TaskType.Deployment, DeploymentScope = "Wdrożenie wersji 1.2 na produkcję" },
			   new DeploymentTask { Id = 2, Description = "Hotfix bezpieczeństwa", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(1), Type = TaskType.Deployment, DeploymentScope = "Krytyczny patch do autoryzacji" },
			   new DeploymentTask { Id = 3, Description = "Migracja bazy danych", DifficultyScale = 5, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(3), Type = TaskType.Deployment, DeploymentScope = "Nowy schemat danych dla logów użytkowników" },
			   new DeploymentTask { Id = 4, Description = "Release sprint 14", DifficultyScale = 3, Status = TaskStatus.Completed, Date = DateTime.UtcNow.AddDays(5), Type = TaskType.Deployment, DeploymentScope = "Zbiorcze wdrożenie funkcji sprintu 14" },
			   new DeploymentTask { Id = 5, Description = "Wdrożenie nowego modułu raportów", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(4), Type = TaskType.Deployment, DeploymentScope = "Raporty miesięczne + eksport do PDF" },
			   new DeploymentTask { Id = 6, Description = "Release sprint 15", DifficultyScale = 5, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(5), Type = TaskType.Deployment, DeploymentScope = "Nowe funkcjonalności + poprawki" },

			   new ImplementationTask { Id = 7, Description = "Dodanie logiki walidacji użytkownika", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Wdrożyć walidację loginu i hasła z ograniczeniami." },
			   new ImplementationTask { Id = 8, Description = "Obsługa błędów formularza", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Zaimplementować obsługę błędów w formularzach rejestracji." },
			   new ImplementationTask { Id = 9, Description = "Refaktoryzacja kontrolera użytkowników", DifficultyScale = 2, Status = TaskStatus.Completed, Type = TaskType.Implementation, TaskContent = "Poprawić strukturę i wydzielić metody." },
			   new ImplementationTask { Id = 10, Description = "Wprowadzenie CQRS", DifficultyScale = 5, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Podzielić logikę read/write przy użyciu wzorca CQRS." },
			   new ImplementationTask { Id = 11, Description = "Dodanie automatycznych testów jednostkowych", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Napisać testy do głównych komponentów logiki." },
			   new ImplementationTask { Id = 12, Description = "Wdrożenie uploadu plików", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Obsłużyć upload plików z walidacją rozszerzeń." },
			   new ImplementationTask { Id = 13, Description = "Wyszukiwarka zadań", DifficultyScale = 2, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Dodać możliwość wyszukiwania zadań po nazwie i typie." },
			   new ImplementationTask { Id = 14, Description = "Logowanie i monitoring", DifficultyScale = 5, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Zaimplementować Serilog + monitoring błędów." },
			   new ImplementationTask { Id = 15, Description = "Optymalizacja zapytań LINQ", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Poprawić wydajność kluczowych zapytań do bazy." },
			   new ImplementationTask { Id = 16, Description = "Zabezpieczenie endpointów API", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Dodać autoryzację do endpointów admina." },
			   new ImplementationTask { Id = 17, Description = "Zmiana layoutu dashboardu", DifficultyScale = 2, Status = TaskStatus.Completed, Type = TaskType.Implementation, TaskContent = "Zmiany UI według nowego mockupu." },
			   new ImplementationTask { Id = 18, Description = "Dodanie paginacji do listy użytkowników", DifficultyScale = 1, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Wprowadzić paginację + filtrowanie." },
			   new ImplementationTask { Id = 19, Description = "Dodanie GraphQL do projektu", DifficultyScale = 5, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Wprowadzić alternatywę do REST API." },
			   new ImplementationTask { Id = 20, Description = "Logowanie zdarzeń do Elastic", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Integracja z ElasticSearch." },
			   new ImplementationTask { Id = 21, Description = "Obsługa dark mode w UI", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Dodanie trybu ciemnego do interfejsu." },

			   new MaintenanceTask { Id = 22, Description = "Przegląd serwisów backendowych", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(1), Type = TaskType.Maintenance, Services = "AuthService, LoggingService", Servers = "SRV-1, SRV-2" },
			   new MaintenanceTask { Id = 23, Description = "Aktualizacja certyfikatów SSL", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(2), Type = TaskType.Maintenance, Services = "API Gateway", Servers = "SRV-4" },
			   new MaintenanceTask { Id = 24, Description = "Czyszczenie logów systemowych", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(3), Type = TaskType.Maintenance, Services = "MonitoringService", Servers = "SRV-3" },
			   new MaintenanceTask { Id = 25, Description = "Kontrola uprawnień użytkowników", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(2), Type = TaskType.Maintenance, Services = "UserService, AdminPanel", Servers = "SRV-2, SRV-5" },
			   new MaintenanceTask { Id = 26, Description = "Backup bazy danych", DifficultyScale = 1, Status = TaskStatus.Completed, Date = DateTime.UtcNow.AddDays(0), Type = TaskType.Maintenance, Services = "DB-BackupService", Servers = "SRV-6" },
			   new MaintenanceTask { Id = 27, Description = "Sprawdzenie statusów mikroserwisów", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(2), Type = TaskType.Maintenance, Services = "User, Auth, Mail", Servers = "SRV-2, SRV-8" },
			   new MaintenanceTask { Id = 28, Description = "Rotacja kluczy dostępowych", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(1), Type = TaskType.Maintenance, Services = "Admin API", Servers = "SRV-9" },
			   new MaintenanceTask { Id = 29, Description = "Upgrade biblioteki RabbitMQ", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(3), Type = TaskType.Maintenance, Services = "MessagingService", Servers = "SRV-2" },
			   new MaintenanceTask { Id = 30, Description = "Sprawdzenie dostępności CDN", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(1), Type = TaskType.Maintenance, Services = "ImageService", Servers = "SRV-CDN-1" }
		   );
		}
	}
}
