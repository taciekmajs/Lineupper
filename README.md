# Projekt: Lineupper

## Cel projektu:
Celem projektu było stworzenie aplikacji internetowej pozwalającej na generowanie harmonogramów dla festiwali muzycznych w taki sposób, by były jak najbardziej dopasowane do preferencji muzycznych użytkowników.

## Funkcjonalności:
- Organizator może tworzyć festiwale, dodawać zespoły i ustalać ich czas trwania.
- Uczestnik może głosować na zespoły i preferowany harmonogram.
- System generuje propozycję harmonogramu na podstawie głosów.
- Organizator może zatwierdzić lub edytować harmonogram.
- Statystyki głosowania są dostępne dla organizatora.
- Rejestracja użytkowników z wyborem roli (organizator / uczestnik).
- Blazor WebAssembly i WebAPI z podziałem na role, logowaniem, autoryzacją, walidacją i loggerem.

## Skład zespołu i podział prac:
- Jakub Sornat – Modele bazy danych
- Marcin Mika – Algorytm generujący harmonogram
- Maciej Tajs – WebAPI + logika backendowa
- Filip Kopańko – Aplikacja Blazor WebAssembly

## Technologie:
- .NET 8 (WebAPI, Blazor WebAssembly, Blazor Server)
- Entity Framework Core + SQLite
- AutoMapper
- MudBlazor (komponenty UI)
- JSInterop (integracja z JS dla np. kalendarza)
- Logger z dziennymi plikami i oddzielnym plikiem błędów

## Instrukcja uruchomienia:
1. Otwórz plik `Lineupper.sln` w Visual Studio 2022 lub nowszym.
2. Przywróć zależności NuGet.
3. Uruchom `Lineupper.WebAPI` i `Lineupper.BlazorWASM`.
