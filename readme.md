# 📌 Task Assign – Opis podjętych decyzji projektowych

## ℹ️ Opis aplikacji

Task Assign to aplikacja webowa umożliwiająca przypisywanie zadań użytkownikom w zależności od ich typu (Programista, DevOps/Administrator). Obsługiwane są trzy typy zadań: wdrożenie, implementacja oraz utrzymanie. Aplikacja pozwala przypisywać zadania zgodnie z określonymi regułami walidacyjnymi, uwzględniając trudność oraz typ użytkownika. Użytkownik może przypisać wiele zadań i zatwierdzić zmiany jednorazowo, a interfejs aplikacji zapobiega utracie niezapisanych modyfikacji.

---

## 🔧 Backend (.NET 8.0)

### ✅ Technologie i architektura

- **Platforma:** .NET 8.0
- **Struktura:** MVC
- **Kontrolery:** `TasksController`, `UsersController`

### ✅ Model danych i dziedziczenie

Zadania reprezentowane są poprzez wspólną klasę bazową:

```csharp
public abstract class CommonTask
```

Dla poprawnej implementacji dziedziczenia w Entity Framework, zastosowano **dyskryminator**:

```csharp
modelBuilder.Entity<CommonTask>()
    .HasDiscriminator<TaskType>("TaskType")
    .HasValue<DeploymentTask>(TaskType.Deployment)
    .HasValue<MaintenanceTask>(TaskType.Maintenance)
    .HasValue<ImplementationTask>(TaskType.Implementation);
```

Pozwala to EF Core poprawnie mapować typy zadań w czasie działania.

### ✅ Przechowywanie danych

Zamiast bazy danych użyto `UseInMemoryDatabase`, co pozwala symulować działanie bazy danych w pamięci operacyjnej. Dane są przechowywane tylko na czas działania aplikacji i resetują się po restarcie. Rozwiązanie to umożliwia szybki rozwój i testowanie bez potrzeby konfigurowania zewnętrznej bazy.

### ✅ DTO + AutoMapper

Zastosowano wzorzec DTO do przesyłania danych do frontendu, a mapowanie pomiędzy encjami a DTO odbywa się z pomocą **AutoMapper**. Pozwala to uprościć logikę kontrolerów i zapewnia separację warstw.

### ✅ Wykorzystane biblioteki

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.InMemory`
- `Swashbuckle.AspNetCore`
- `Newtonsoft.Json`
- `Microsoft.AspNetCore.Mvc.NewtonsoftJson`
- `AutoMapper`

---

## 💻 Frontend (Angular 19)

### ✅ Technologie i organizacja kodu

- **Framework:** Angular 19
- **UI Library:** Angular Material
- **Typowanie:** Wszystkie dane reprezentowane za pomocą interfejsów TypeScript

### ✅ Serwisy i logika danych

Dwa główne serwisy:
- `tasks-service`
- `users-service`

Odpowiadają za komunikację z backendem oraz przechowywanie danych w kontekście aplikacji. Wykorzystano **Angular signals** oraz **computed**, zgodnie z najnowszymi możliwościami frameworka.

### ✅ Komponenty

- `task-list` – wyświetla zadania przypisane oraz dostępne
- `list-item` – komponent reprezentujący pojedyncze zadanie

### ✅ Ochrona przed utratą danych

Zaimplementowano **route guard** `UnsavedChangesGuard`, który zapobiega utracie niezapisanych zmian poprzez ostrzeganie użytkownika przed opuszczeniem widoku.

---

## ✅ Podsumowanie

Aplikacja **Task Assign** implementuje wszystkie wymagania funkcjonalne opisane w scenariuszu zadania. Kluczowe cechy to:

- Obsługa dziedziczenia typów zadań i ich rozróżnianie po stronie EF
- Ograniczenia walidacyjne dla przypisywania zadań
- Separacja warstw logiki i danych poprzez DTO oraz AutoMapper
- Współczesne podejście do frontendowej reaktywności przy użyciu Angular signals
- Wykorzystanie Angular Material do szybkiego prototypowania interfejsu

Projekt gotowy jest do dalszego rozwoju: dodania bazy danych, logowania, testów i kolejnych funkcji produkcyjnych.