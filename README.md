# GARGAMEL

Konzolová aplikace v C# (.NET), která spojuje několik menších miniher a nástrojů do jednoho hlavního menu. Postavena jako jeden top-level skript s jednoduchou navigací pomocí `goto` labelů.

## Funkce

| # | Název | Popis |
|---|-------|-------|
| 1 | **Gambling Center (Blackjack)** | Klasický Blackjack proti dealerovi. Hráč vsází virtuální peníze, které se ukládají do souboru `penize.txt` a načítají se i po restartu aplikace. |
| 2 | **Kačka – Kalkulačka dluhů** | Jednoduchá kalkulačka na základní operace (`+ - * /`). |
| 3 | **Počasí** | Zobrazí aktuální teplotu a stav počasí pro zadané město pomocí [OpenWeatherMap API](https://openweathermap.org/api). |
| 4 | **Generátor hesel** | Vygeneruje náhodné heslo zadané délky (písmena, čísla, speciální znaky). |
| 5 | **Kalkulačka měny** | Převede částku v Kč na libovolnou měnu pomocí živého kurzu z [ExchangeRate-API](https://www.exchangerate-api.com/). |
| 6 | **Guess the Number** | Hra "hádej číslo" — program si myslí číslo 1–100, hráč má 7 pokusů. |
| 7 | **Crossy Road** | ASCII verze hry Crossy Road — hráč (`O`) přechází silnici a uhýbá projíždějícím autům (`>`). Ovládání WASD, restart (R), návrat do menu (M). |
| 8 | **Konec** | Ukončí aplikaci. |

## Požadavky

- **.NET 6.0 nebo novější** (kvůli top-level statements a `async`/`await` v `Main`)
- Připojení k internetu (pro počasí a kurz měn)
- Doporučeno spouštět konzoli **na celou obrazovku / maximalizovanou** kvůli hře Crossy Road

## Použité API klíče

Aplikace obsahuje natvrdo zapsané API klíče pro:

- **OpenWeatherMap** (`https://api.openweathermap.org`) — pro funkci Počasí
- **ExchangeRate-API** (`https://v6.exchangerate-api.com`) — pro funkci Kalkulačka měny

> ⚠️ **Bezpečnostní upozornění:** API klíče by neměly být commitované do veřejného repozitáře. Před nahráním na GitHub doporučujeme klíče přesunout do proměnných prostředí nebo konfiguračního souboru (např. `appsettings.json` / `.env`), který je v `.gitignore`.

## Instalace a spuštění

```bash
git clone https://github.com/<tvuj-ucet>/<nazev-repa>.git
cd <nazev-repa>
dotnet run
```

Při prvním spuštění se vytvoří soubor `penize.txt` s výchozím zůstatkem 1000 Kč pro Blackjack.

## Ovládání Crossy Road

| Klávesa | Akce |
|---------|------|
| `W` `A` `S` `D` | Pohyb postavičky |
| `R` | Restart úrovně |
| `M` | Návrat do menu hry |
| `2` (po prohře/výhře) | Návrat do hlavního menu GARGAMEL |

## Struktura kódu

Celý program běží jako jeden top-level skript (`Program.cs` bez explicitní třídy `Program` / metody `Main`) s hlavní smyčkou menu označenou labelem `gargamel:`. Jednotlivé volby přepínají mezi bloky kódu pomocí `if`/`else if` a `goto`. Pomocné funkce `Blackjack()` a `Kalkulacka()` jsou definovány jako lokální funkce na konci souboru.

## Možná vylepšení do budoucna

- Přesunout API klíče mimo zdrojový kód
- Nahradit `goto` strukturu přehlednějším stavovým automatem nebo metodami
- Přidat ukládání skóre pro Crossy Road a Guess the Number
- Ošetřit vstupy uživatele robustněji (např. neplatné hodnoty u `int.Parse`)

## Licence

Doplň dle vlastního uvážení (např. MIT).
