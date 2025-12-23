# 🦷 System Zapisów do Kliniki Stomatologicznej - Backend

System zarządzania zapisami do kliniki stomatologicznej stworzony jako część pracy inżynierskiej. Aplikacja backend zbudowana przy użyciu ASP.NET Core i C#.

## 📋 Opis projektu

Nowoczesne API RESTful umożliwiające zarządzanie wizytami w klinice stomatologicznej. System oferuje kompleksowe funkcje do obsługi pacjentów, personelu medycznego oraz zarządzania harmonogramem wizyt.

## 🚀 Technologie

- **ASP.NET Core** - framework do budowy API
- **C#** - język programowania
- **Entity Framework Core** - ORM do zarządzania bazą danych
- **SQL Server** - baza danych
- **JWT** - autoryzacja i uwierzytelnianie
- **Swagger** - dokumentacja API

## 📁 Struktura projektu

```
SystemZapisowDoKlinikiApi/
├── Controllers/    # Kontrolery API
├── Models/         # Modele danych
├── Services/       # Logika biznesowa
├── Data/           # Kontekst bazy danych
├── DTOs/           # Data Transfer Objects
└── Middleware/     # Middleware aplikacji
```

## 🛠️ Instalacja

1. Sklonuj repozytorium:
```bash
git clone https://github.com/IgorKotecki/SystemZapis-wDoKlinikiStomatologicznejBackend.git
cd SystemZapis-wDoKlinikiStomatologicznejBackend
```

2. Otwórz solution w Visual Studio lub Rider:
```bash
dotnet restore
```

3. Skonfiguruj połączenie z bazą danych w `appsettings.json`

4. Uruchom migracje:
```bash
dotnet ef database update
```

5. Uruchom aplikację: 
```bash
dotnet run
```

## 📜 Dostępne komendy

- `dotnet run` - uruchamia serwer API
- `dotnet build` - buduje projekt
- `dotnet test` - uruchamia testy
- `dotnet ef migrations add [nazwa]` - tworzy nową migrację
- `dotnet ef database update` - aktualizuje bazę danych

## ✨ Funkcjonalności

- 🔐 System autoryzacji i uwierzytelniania JWT
- 📅 Zarządzanie harmonogramem wizyt
- 👥 CRUD dla pacjentów
- 👨‍⚕️ Zarządzanie personelem medycznym
- 🏥 Zarządzanie zabiegami stomatologicznymi
- 📊 API RESTful z dokumentacją Swagger
- 🔒 Bezpieczne endpointy z rolami użytkowników

## 📝 Autorzy

- Paweł Szeliga
- Igor Kotecki

Projekt stworzony jako część pracy inżynierskiej.