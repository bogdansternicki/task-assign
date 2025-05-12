using Microsoft.EntityFrameworkCore;
using TaskAssignWebApi.Domain.Models;
using TaskAssignWebApi.Domain.Models.Abstract;
using TaskAssignWebApi.Enums;
using TaskStatus = TaskAssignWebApi.Enums.TaskStatus;

namespace TaskAssignWebApi.Domain
{
	public class TaskAssignDbContext : DbContext
	{
		public DbSet<CommonTask> Tasks { get; set; }
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
		}

		public void SeedData()
		{
			Users.AddRange(
				new User { Id = 1, Name = "Anna", Type = UserType.Developer },
				new User { Id = 2, Name = "Tomek", Type = UserType.DevOps },
				new User { Id = 3, Name = "Ola", Type = UserType.DevOps },
				new User { Id = 4, Name = "Jan", Type = UserType.Developer },
				new User { Id = 5, Name = "Bartek", Type = UserType.DevOps },
				new User { Id = 6, Name = "Kasia", Type = UserType.Developer },
				new User { Id = 7, Name = "Marek", Type = UserType.DevOps },
				new User { Id = 8, Name = "Zofia", Type = UserType.Developer },
				new User { Id = 9, Name = "Piotr", Type = UserType.Developer },
				new User { Id = 10, Name = "Agnieszka", Type = UserType.DevOps });

			Tasks.AddRange(
				new DeploymentTask { Id = 1, Description = "Wdrożenie nowej wersji aplikacji", DifficultyScale = 5, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(2), Type = TaskType.Deployment, DeploymentScope = "Wdrożenie wersji 1.2 na produkcję", UserId = 2 },
				new DeploymentTask { Id = 2, Description = "Hotfix bezpieczeństwa", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(1), Type = TaskType.Deployment, DeploymentScope = "Krytyczny patch do autoryzacji", UserId = 3 },
				new DeploymentTask { Id = 3, Description = "Migracja bazy danych", DifficultyScale = 5, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(3), Type = TaskType.Deployment, DeploymentScope = "Nowy schemat danych dla logów użytkowników", UserId = 5 },
				new DeploymentTask { Id = 4, Description = "Release sprint 14", DifficultyScale = 3, Status = TaskStatus.Completed, Date = DateTime.UtcNow.AddDays(5), Type = TaskType.Deployment, DeploymentScope = "Zbiorcze wdrożenie funkcji sprintu 14"},
				new DeploymentTask { Id = 5, Description = "Wdrożenie nowego modułu raportów", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(4), Type = TaskType.Deployment, DeploymentScope = "Raporty miesięczne + eksport do PDF" },
				new DeploymentTask { Id = 6, Description = "Release sprint 15", DifficultyScale = 5, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(5), Type = TaskType.Deployment, DeploymentScope = "Nowe funkcjonalności + poprawki" },
				new ImplementationTask { Id = 7, Description = "Dodanie logiki walidacji użytkownika", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Wdrożyć walidację loginu i hasła z ograniczeniami.", UserId = 1 },
			    new ImplementationTask { Id = 8, Description = "Obsługa błędów formularza", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Zaimplementować obsługę błędów w formularzach rejestracji.", UserId = 4 },
			    new ImplementationTask { Id = 9, Description = "Refaktoryzacja kontrolera użytkowników", DifficultyScale = 2, Status = TaskStatus.Completed, Type = TaskType.Implementation, TaskContent = "Poprawić strukturę i wydzielić metody.", UserId = 1 },
			    new ImplementationTask { Id = 10, Description = "Wprowadzenie CQRS", DifficultyScale = 5, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Podzielić logikę read/write przy użyciu wzorca CQRS.", UserId = 4 },
			    new ImplementationTask { Id = 11, Description = "Dodanie automatycznych testów jednostkowych", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Napisać testy do głównych komponentów logiki." },
			    new ImplementationTask { Id = 12, Description = "Wdrożenie uploadu plików", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Obsłużyć upload plików z walidacją rozszerzeń." },
			    new ImplementationTask { Id = 13, Description = "Wyszukiwarka zadań", DifficultyScale = 2, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Dodać możliwość wyszukiwania zadań po nazwie i typie." },
			    new ImplementationTask { Id = 14, Description = "Logowanie i monitoring", DifficultyScale = 5, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Zaimplementować Serilog + monitoring błędów.", UserId = 2 },
			    new ImplementationTask { Id = 15, Description = "Optymalizacja zapytań LINQ", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Poprawić wydajność kluczowych zapytań do bazy."},
			    new ImplementationTask { Id = 16, Description = "Zabezpieczenie endpointów API", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Dodać autoryzację do endpointów admina.", UserId = 9 },
			    new ImplementationTask { Id = 17, Description = "Zmiana layoutu dashboardu", DifficultyScale = 2, Status = TaskStatus.Completed, Type = TaskType.Implementation, TaskContent = "Zmiany UI według nowego mockupu." },
			    new ImplementationTask { Id = 18, Description = "Dodanie paginacji do listy użytkowników", DifficultyScale = 1, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Wprowadzić paginację + filtrowanie.", UserId = 6 },
			    new ImplementationTask { Id = 19, Description = "Dodanie GraphQL do projektu", DifficultyScale = 5, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Wprowadzić alternatywę do REST API." },
			    new ImplementationTask { Id = 20, Description = "Logowanie zdarzeń do Elastic", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Integracja z ElasticSearch." },
			    new ImplementationTask { Id = 21, Description = "Obsługa dark mode w UI", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Dodanie trybu ciemnego do interfejsu.", UserId = 8},
				new MaintenanceTask { Id = 22, Description = "Przegląd serwisów backendowych", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(1), Type = TaskType.Maintenance, Services = "AuthService, LoggingService", Servers = "SRV-1, SRV-2" },
			    new MaintenanceTask { Id = 23, Description = "Aktualizacja certyfikatów SSL", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(2), Type = TaskType.Maintenance, Services = "API Gateway", Servers = "SRV-4", UserId = 3 },
			    new MaintenanceTask { Id = 24, Description = "Czyszczenie logów systemowych", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(3), Type = TaskType.Maintenance, Services = "MonitoringService", Servers = "SRV-3" },
			    new MaintenanceTask { Id = 25, Description = "Kontrola uprawnień użytkowników", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(2), Type = TaskType.Maintenance, Services = "UserService, AdminPanel", Servers = "SRV-2, SRV-5" },
			    new MaintenanceTask { Id = 26, Description = "Backup bazy danych", DifficultyScale = 1, Status = TaskStatus.Completed, Date = DateTime.UtcNow.AddDays(0), Type = TaskType.Maintenance, Services = "DB-BackupService", Servers = "SRV-6", UserId = 5 },
			    new MaintenanceTask { Id = 27, Description = "Sprawdzenie statusów mikroserwisów", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(2), Type = TaskType.Maintenance, Services = "User, Auth, Mail", Servers = "SRV-2, SRV-8" },
			    new MaintenanceTask { Id = 28, Description = "Rotacja kluczy dostępowych", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(1), Type = TaskType.Maintenance, Services = "Admin API", Servers = "SRV-9" },
			    new MaintenanceTask { Id = 29, Description = "Upgrade biblioteki RabbitMQ", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(3), Type = TaskType.Maintenance, Services = "MessagingService", Servers = "SRV-2" },
			    new MaintenanceTask { Id = 30, Description = "Sprawdzenie dostępności CDN", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(1), Type = TaskType.Maintenance, Services = "ImageService", Servers = "SRV-CDN-1" },
				new DeploymentTask { Id = 31, Description = "Wdrożenie nowej wersji API", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(5), Type = TaskType.Deployment, DeploymentScope = "Nowa wersja API dla klientów", UserId = 10 },
				new DeploymentTask { Id = 32, Description = "Aktualizacja frameworka", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(6), Type = TaskType.Deployment, DeploymentScope = "Aktualizacja do wersji 5.x .NET", UserId = 2 },
				new DeploymentTask { Id = 33, Description = "Przeniesienie aplikacji do chmury", DifficultyScale = 5, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(7), Type = TaskType.Deployment, DeploymentScope = "Migracja na AWS", UserId = 3 },
				new DeploymentTask { Id = 34, Description = "Zabezpieczenie aplikacji przed atakami XSS", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(8), Type = TaskType.Deployment, DeploymentScope = "Zastosowanie walidacji danych wejściowych", UserId = 7 },
				new DeploymentTask { Id = 35, Description = "Aktualizacja certyfikatów SSL", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(9), Type = TaskType.Deployment, DeploymentScope = "Zaktualizowanie certyfikatów na serwerach produkcyjnych", UserId = 5 },
				new ImplementationTask { Id = 36, Description = "Wprowadzenie systemu powiadomień e-mail", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Dodanie systemu powiadomień e-mail o ważnych wydarzeniach.", UserId = 1 },
				new ImplementationTask { Id = 37, Description = "Dodanie API do generowania raportów", DifficultyScale = 5, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Stworzenie API do generowania raportów w formacie PDF i CSV.", UserId = 2 },
				new ImplementationTask { Id = 38, Description = "Refaktoryzacja kontrolera logowania", DifficultyScale = 3, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Optymalizacja kontrolera logowania użytkowników.", UserId = 3 },
				new ImplementationTask { Id = 39, Description = "Implementacja WebSocket do komunikacji w czasie rzeczywistym", DifficultyScale = 4, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Wprowadzenie komunikacji WebSocket w aplikacji.", UserId = 4 },
				new ImplementationTask { Id = 40, Description = "Przeniesienie logiki do mikroserwisów", DifficultyScale = 5, Status = TaskStatus.Todo, Type = TaskType.Implementation, TaskContent = "Migracja logiki biznesowej do mikroserwisów.", UserId = 5 },
				new MaintenanceTask { Id = 41, Description = "Przegląd konfiguracji serwera bazodanowego", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(10), Type = TaskType.Maintenance, Services = "DBServer", Servers = "SRV-1" },
				new MaintenanceTask { Id = 42, Description = "Aktualizacja systemu monitorowania", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(11), Type = TaskType.Maintenance, Services = "MonitoringService", Servers = "SRV-2" },
				new MaintenanceTask { Id = 43, Description = "Optymalizacja zapytań SQL", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(12), Type = TaskType.Maintenance, Services = "DB", Servers = "SRV-3" },
				new MaintenanceTask { Id = 44, Description = "Zarządzanie uprawnieniami do serwisów", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(13), Type = TaskType.Maintenance, Services = "UserService", Servers = "SRV-4", UserId = 7 },
				new MaintenanceTask { Id = 45, Description = "Rewizja polityki backupów", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(14), Type = TaskType.Maintenance, Services = "BackupService", Servers = "SRV-5" },
				new MaintenanceTask { Id = 46, Description = "Optymalizacja wydajności serwera aplikacji", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(15), Type = TaskType.Maintenance, Services = "AppServer", Servers = "SRV-6" },
				new MaintenanceTask { Id = 47, Description = "Monitorowanie zużycia pamięci na serwerach", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(16), Type = TaskType.Maintenance, Services = "AppServer", Servers = "SRV-7" },
				new MaintenanceTask { Id = 48, Description = "Zarządzanie logami aplikacji", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(17), Type = TaskType.Maintenance, Services = "LoggingService", Servers = "SRV-8" },
				new MaintenanceTask { Id = 49, Description = "Optymalizacja użycia dysku w bazie danych", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(18), Type = TaskType.Maintenance, Services = "DBServer", Servers = "SRV-9" },
				new MaintenanceTask { Id = 50, Description = "Przegląd konfiguracji sieciowej", DifficultyScale = 2, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(19), Type = TaskType.Maintenance, Services = "NetworkService", Servers = "SRV-10" },
				new DeploymentTask { Id = 51, Description = "Zastosowanie nowego narzędzia CI/CD", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(20), Type = TaskType.Deployment, DeploymentScope = "Wdrożenie i konfiguracja narzędzia CI/CD", UserId = 5 },
				new DeploymentTask { Id = 52, Description = "Ustawienie automatycznych testów wdrożeniowych", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(21), Type = TaskType.Deployment, DeploymentScope = "Automatyzacja testów wdrożeniowych w pipeline'ach", UserId = 2 },
				new DeploymentTask { Id = 53, Description = "Przeniesienie aplikacji do nowego regionu chmury", DifficultyScale = 5, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(22), Type = TaskType.Deployment, DeploymentScope = "Migracja do AWS region 2", UserId = 3 },
				new DeploymentTask { Id = 54, Description = "Zainstalowanie nowych serwerów produkcyjnych", DifficultyScale = 4, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(23), Type = TaskType.Deployment, DeploymentScope = "Nowe serwery w regionie produkcyjnym", UserId = 7 },
				new DeploymentTask { Id = 55, Description = "Aktualizacja systemów operacyjnych na serwerach", DifficultyScale = 3, Status = TaskStatus.Todo, Date = DateTime.UtcNow.AddDays(24), Type = TaskType.Deployment, DeploymentScope = "Aktualizacja serwerów do nowszych wersji OS", UserId = 5 });

			SaveChanges();
		}
	}
}
