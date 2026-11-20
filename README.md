# Bankomat – xUnit & TDD

En enkel bankomat i C# som jag har testat med xUnit i kursen
"Test, integration och leverans". Jag har skrivit enhetstester för
alla klasser och använt TDD för att hitta och rätta fel i koden.

## Projektstruktur

```
Bankomat/
├── ATM/            # Själva bankomaten (konsolapp)
│   ├── Account.cs
│   ├── Card.cs
│   ├── AtmService.cs
│   ├── WithdrawResult.cs
│   ├── ConsoleRunner.cs
│   └── Program.cs
└── ATM.Tests/      # Enhetstester med xUnit
    ├── AccountTest.cs
    ├── CardTest.cs
    └── AtmServiceTest.cs
```

## Vad som testas

- **Account** – saldo, insättning och uttag, samt att belopp som är 0 eller
  negativa nekas.
- **Card** – att rätt och fel PIN-kod ger rätt resultat.
- **AtmService** – sätta i och mata ut kort, PIN-kod, uttag och insättning,
  för lite pengar på kontot eller i bankomaten, spärrat kort efter tre fel
  PIN-koder och kvitto med belopp, saldo och datum.

## Fel som hittades och rättades med TDD

- Felräknaren för PIN-koden nollställdes inte när ett nytt kort sattes i.
- Ogiltiga belopp (0 eller minus) godkändes.
- Kvittot saknade datum.
- `int.Parse` kraschade vid felaktig inmatning – ersatt med `int.TryParse`.

## Kom igång

Kräver [.NET 10 SDK](https://dotnet.microsoft.com/download).

Kör programmet:

```bash
dotnet run --project ATM
```

Kör testerna:

```bash
dotnet test ATM.Tests
```
