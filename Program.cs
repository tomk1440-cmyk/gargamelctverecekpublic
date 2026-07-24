using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// --- Načtení .env souboru ---
// Když spouštíš přes "dotnet run", aktuální složka je složka projektu (kde je .csproj).
// Když spustíš zkompilovaný .exe přímo, aktuální složka je bin/Debug/netX.X/.
// Proto hledáme .env i ve složce s .exe a postupně i ve složkách nad ní (nahoru ke složce projektu).
string FindEnvFile(string fileName)
{
    var slozky = new List<string> { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };

    foreach (var start in slozky)
    {
        string dir = start;
        for (int i = 0; i < 6 && dir != null; i++)
        {
            string candidate = Path.Combine(dir, fileName);
            if (File.Exists(candidate)) return candidate;

            var parent = Directory.GetParent(dir);
            dir = parent?.FullName;
        }
    }

    return null;
}

void LoadEnv(string fileName = ".env")
{
    string path = FindEnvFile(fileName);
    if (path == null) return;

    foreach (string radek in File.ReadAllLines(path))
    {
        string line = radek.Trim();
        if (line.Length == 0 || line.StartsWith("#")) continue;

        int eq = line.IndexOf('=');
        if (eq <= 0) continue;

        string key = line.Substring(0, eq).Trim();
        string value = line.Substring(eq + 1).Trim().Trim('"');

        Environment.SetEnvironmentVariable(key, value);
    }
}

LoadEnv();

string soubor = "penize.txt";
int penize;

if (File.Exists(soubor))
{
    string obsah = File.ReadAllText(soubor);
    if (!int.TryParse(obsah, out penize)) penize = 1000;
}
else
{
    penize = 1000;
    File.WriteAllText(soubor, penize.ToString());
}

Console.WriteLine(" ______    _________ _           __     _________ _______  _______  _       \n/ ___  \\   \\__   __/( (    /|   /  \\    \\__   __/(  ___  )(  ___  )( \\      \n\\/   )  )     ) (   |  \\  ( |   \\/) )      ) (   | (   ) || (   ) || (      \n    /  /      | |   |   \\ | |     | |      | |   | |   | || |   | || |      \n   /  /       | |   | (\\ \\) |     | |      | |   | |   | || |   | || |      \n  /  /        | |   | | \\   |     | |      | |   | |   | || |   | || |      \n /  /      ___) (___| )  \\  |   __) (_     | |   | (___) || (___) || (____/\\\n \\_/       \\_______/|/    )_)   \\____/     )_(   (_______)(_______)(_______/\n                                                                            ");
Console.WriteLine("");
Console.WriteLine("     ");
Console.WriteLine("     ");
Console.WriteLine("     ");
Console.WriteLine("Před používáním dejte aplikaci na full screen (nebo maximalizujte) ");
Console.WriteLine("     ");
Console.WriteLine("Stiskněte jakoukoli klávesu pro pokračování");
Console.ReadKey();

gargamel:
Console.Clear();
Console.WriteLine("Vyberte jednu z možností");
Console.WriteLine("1) Gambling center      2) Kačka Kalkulačka dluhů\n3) Počasí               4) Generátor hesel\n5) Kalkulačka měny      6) Guess the number\n7) Crossy Road          8) Konec");

char key = Console.ReadKey(true).KeyChar;
int input_keyboard = key - '0';

if (input_keyboard == 1)
{
    Console.Clear();
    Blackjack();
    goto gargamel;
}
else if (input_keyboard == 2)
{
    Console.Clear();
    Kalkulacka();
    goto gargamel;
}
else if (input_keyboard == 3)
{
    Console.Clear();

    var apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");
    if (string.IsNullOrEmpty(apiKey))
    {
        Console.WriteLine("Chybí OPENWEATHER_API_KEY v .env souboru.");
        Console.WriteLine("Stiskněte klávesu pro návrat do menu...");
        Console.ReadKey(true);
        goto gargamel;
    }

    Console.Write("City: ");
    var city = Console.ReadLine();

    try
    {
        var web = new System.Net.Http.HttpClient();
        var link = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

        var json = web.GetStringAsync(link).Result;

        var doc = System.Text.Json.JsonDocument.Parse(json);
        var temp = doc.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
        var desc = doc.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();

        Console.WriteLine($"Temperature: {temp}°C");
        Console.WriteLine($"Condition: {desc}");
    }
    catch (Exception)
    {
        Console.WriteLine("Chyba při načítání počasí (zkontrolujte název města nebo připojení k internetu).");
    }

    Console.WriteLine("\nStiskněte klávesu pro návrat do menu...");
    Console.ReadKey(true);
    goto gargamel;
}
else if (input_keyboard == 4)
{
    Console.Write("Password length: ");
    int length = int.Parse(Console.ReadLine());

    string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?";

    Random r = new();
    string password = "";

    for (int i = 0; i < length; i++)
    {
        password += chars[r.Next(chars.Length)];
    }

    Console.WriteLine("\nPassword: " + password);
    Console.ResetColor(); // Vrátí původní barvy konzole (např. černou a bílou)
    Console.ReadKey(true);
    goto gargamel;
}
else if (input_keyboard == 5)
{
    string apiKey = Environment.GetEnvironmentVariable("EXCHANGERATE_API_KEY");
    if (string.IsNullOrEmpty(apiKey))
    {
        Console.WriteLine("Chybí EXCHANGERATE_API_KEY v .env souboru.");
        Console.WriteLine("Stiskněte klávesu pro návrat do menu...");
        Console.ReadKey(true);
        goto gargamel;
    }
    Console.Write("Částka v Kč: ");
    double czk = double.Parse(Console.ReadLine());

    Console.Write("Měna (např. EUR, USD, GBP...): ");
    string mena = Console.ReadLine().ToUpper();

    using HttpClient client = new HttpClient();
    string url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/CZK";

    string json = await client.GetStringAsync(url);
    using JsonDocument doc = JsonDocument.Parse(json);
    var rates = doc.RootElement.GetProperty("conversion_rates");

    if (rates.TryGetProperty(mena, out JsonElement rateElement))
    {
        double kurz = rateElement.GetDouble();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Dostaneš: {czk * kurz:F2} {mena}");
        Console.WriteLine("Stiskněte klávesu pro návrat do menu...");
        Console.ReadKey(true);
        Console.ResetColor();
        goto gargamel;
        
    }
    else
    {
        Console.WriteLine("Neznámá měna!");
        Console.WriteLine("Stiskněte klávesu pro návrat do menu...");
        Console.ReadKey(true);
        goto gargamel;
    }
}
else if (input_keyboard == 6)
{
    Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black;

// Tady začíná tvoje hra
    int secret = new Random().Next(1, 101), pokusy = 7; bool vyhra = false;
    Console.Clear(); Console.WriteLine("Myslím si číslo od 1 do 100. Máš 7 pokusů.");

    while (pokusy > 0)
    {
        Console.Write($"Tvůj tip (zbývá {pokusy}): ");
        int tip = int.Parse(Console.ReadLine()); pokusy--;

        if (tip == secret) { Console.WriteLine("Správně! Vyhrál jsi."); vyhra = true; break; }
        Console.WriteLine(tip < secret ? "VÍC!" : "MÍŇ!");
    }

    if (!vyhra) Console.WriteLine($"Prohra! Číslo bylo: {secret}");

// Výběr akce po skončení hry
    Console.Write("\n[1] Hrát znovu | [2] Návrat do menu: ");
    string volba = Console.ReadLine();

    if (volba == "2") goto gargamel;
// Pokud zadá 1 (nebo cokoliv jiného), program normálně pokračuje dál (např. do dalšího cyklu)
}
else if (input_keyboard == 7)
{
    Console.CursorVisible = false;
        menu:
        Console.Clear();
        Console.WriteLine("=== ENDLESS GARGAMEL ROAD ===\n1 - Hrat\n2 - Konec");
        char v = Console.ReadKey(true).KeyChar;
        if (v == '2') return;
        if (v != '1') goto menu;

        // START HRY (10 řádků, šířka 30 znaků)
        int hX = 15, skore = 0, pocitadlo = 0;
        Random rnd = new Random();

        // X pozice překážek pro všech 10 řádků
        int x1 = rnd.Next(30), x2 = rnd.Next(30), x3 = rnd.Next(30), x4 = rnd.Next(30), x5 = rnd.Next(30);
        int x6 = rnd.Next(30), x7 = 0, x8 = 0, x9 = 0, x10 = 0;
       
        // Typy překážek (1=Auto, 2=Kamion, 3=Vlak). Spodní řádky jsou SAFE LINE (typ 0)
        int t1 = rnd.Next(1, 4), t2 = rnd.Next(1, 4), t3 = rnd.Next(1, 4), t4 = rnd.Next(1, 4), t5 = rnd.Next(1, 4);
        int t6 = rnd.Next(1, 4), t7 = 0, t8 = 0, t9 = 0, t10 = 0;

        Console.Clear();

        while (true)
        {
            // 1. OVLÁDÁNÍ A POSUN SVĚTA
            if (Console.KeyAvailable)
            {
                ConsoleKey k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.A && hX > 0) hX--;
                if (k == ConsoleKey.D && hX < 29) hX++;
                if (k == ConsoleKey.M) goto menu;
                if (k == ConsoleKey.W)
                {
                    skore++;
                   
                    // Posun všech 10 řádků směrem dolů
                    x10 = x9; t10 = t9;
                    x9 = x8; t9 = t8;
                    x8 = x7; t8 = t7;
                    x7 = x6; t7 = t6;
                    x6 = x5; t6 = t5;
                    x5 = x4; t5 = t4;
                    x4 = x3; t4 = t3;
                    x3 = x2; t3 = t2;
                    x2 = x1; t2 = t1;
                   
                    // Nový řádek nahoře
                    x1 = rnd.Next(30); t1 = rnd.Next(1, 4);
                }
            }

            // 2. AUTOMATICKÝ POHYB PŘEKÁŽEK DO STRAN
            if (++pocitadlo >= 2)
            {
                if (t1 != 0) x1 = (x1 + (t1 == 3 ? 2 : 1)) % 30;
                if (t2 != 0) x2 = (x2 + (t2 == 3 ? 2 : 1)) % 30;
                if (t3 != 0) x3 = (x3 + (t3 == 3 ? 2 : 1)) % 30;
                if (t4 != 0) x4 = (x4 + (t4 == 3 ? 2 : 1)) % 30;
                if (t5 != 0) x5 = (x5 + (t5 == 3 ? 2 : 1)) % 30;
                if (t6 != 0) x6 = (x6 + (t6 == 3 ? 2 : 1)) % 30;
                if (t7 != 0) x7 = (x7 + (t7 == 3 ? 2 : 1)) % 30;
                if (t8 != 0) x8 = (x8 + (t8 == 3 ? 2 : 1)) % 30;
                if (t9 != 0) x9 = (x9 + (t9 == 3 ? 2 : 1)) % 30;
                if (t10 != 0) x10 = (x10 + (t10 == 3 ? 2 : 1)) % 30;
                pocitadlo = 0;
            }

            // 3. KONTROLA SRAŽENÍ (Hráč stabilně stojí na řádku 7)
            bool srazen = false;
            if (t7 == 1 && hX == x7) srazen = true;
            if (t7 == 2 && (hX == x7 || hX == (x7 + 1) % 30 || hX == (x7 + 2) % 30)) srazen = true;
            if (t7 == 3 && (hX == x7 || hX == (x7 + 1) % 30 || hX == (x7 + 2) % 30 || hX == (x7 + 3) % 30 || hX == (x7 + 4) % 30)) srazen = true;

            if (srazen)
            {
                Console.Clear();
                Console.WriteLine($"GAME OVER! Skóre: {skore} bodů");

                int rekord = 0;
                if (File.Exists("gargamel_road_highscore.txt"))
                {
                    rekord = int.Parse(File.ReadAllText("gargamel_road_highscore.txt"));
                }

                if (skore > rekord)
                {
                    Console.WriteLine($"NOVÝ REKORD! Staré maximum bylo {rekord} bodů.");
                    File.WriteAllText("gargamel_road_highscore.txt", skore.ToString());
                }
                else
                {
                    Console.WriteLine($"Nejlepší historický výkon: {rekord} bodů.");
                }

                Console.WriteLine("\nStiskni libovolnou klávesu pro menu...");
                Console.ReadKey(true);
                goto menu;
            }

            // 4. VYKRESLENÍ 10 ŘÁDKŮ NA OBRAZOVKU
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"SKÓRE: {skore}       \n==============================");

            // Mechanický výpis řádek po řádku pro 30 znaků šířky
            for (int x = 0; x < 30; x++) { if (t1 == 1 && x == x1) Console.Write(">"); else if (t1 == 2 && (x == x1 || x == (x1 + 1) % 30 || x == (x1 + 2) % 30)) Console.Write("█"); else if (t1 == 3 && (x == x1 || x == (x1 + 1) % 30 || x == (x1 + 2) % 30 || x == (x1 + 3) % 30 || x == (x1 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
            for (int x = 0; x < 30; x++) { if (t2 == 1 && x == x2) Console.Write(">"); else if (t2 == 2 && (x == x2 || x == (x2 + 1) % 30 || x == (x2 + 2) % 30)) Console.Write("█"); else if (t2 == 3 && (x == x2 || x == (x2 + 1) % 30 || x == (x2 + 2) % 30 || x == (x2 + 3) % 30 || x == (x2 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
            for (int x = 0; x < 30; x++) { if (t3 == 1 && x == x3) Console.Write(">"); else if (t3 == 2 && (x == x3 || x == (x3 + 1) % 30 || x == (x3 + 2) % 30)) Console.Write("█"); else if (t3 == 3 && (x == x3 || x == (x3 + 1) % 30 || x == (x3 + 2) % 30 || x == (x3 + 3) % 30 || x == (x3 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
            for (int x = 0; x < 30; x++) { if (t4 == 1 && x == x4) Console.Write(">"); else if (t4 == 2 && (x == x4 || x == (x4 + 1) % 30 || x == (x4 + 2) % 30)) Console.Write("█"); else if (t4 == 3 && (x == x4 || x == (x4 + 1) % 30 || x == (x4 + 2) % 30 || x == (x4 + 3) % 30 || x == (x4 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
            for (int x = 0; x < 30; x++) { if (t5 == 1 && x == x5) Console.Write(">"); else if (t5 == 2 && (x == x5 || x == (x5 + 1) % 30 || x == (x5 + 2) % 30)) Console.Write("█"); else if (t5 == 3 && (x == x5 || x == (x5 + 1) % 30 || x == (x5 + 2) % 30 || x == (x5 + 3) % 30 || x == (x5 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
            for (int x = 0; x < 30; x++) { if (t6 == 1 && x == x6) Console.Write(">"); else if (t6 == 2 && (x == x6 || x == (x6 + 1) % 30 || x == (x6 + 2) % 30)) Console.Write("█"); else if (t6 == 3 && (x == x6 || x == (x6 + 1) % 30 || x == (x6 + 2) % 30 || x == (x6 + 3) % 30 || x == (x6 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
           
            // ŘÁDEK 7 (ZDE JSI TY "O")
            for (int x = 0; x < 30; x++) { if (hX == x) Console.Write("O"); else if (t7 == 1 && x == x7) Console.Write(">"); else if (t7 == 2 && (x == x7 || x == (x7 + 1) % 30 || x == (x7 + 2) % 30)) Console.Write("█"); else if (t7 == 3 && (x == x7 || x == (x7 + 1) % 30 || x == (x7 + 2) % 30 || x == (x7 + 3) % 30 || x == (x7 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
           
            for (int x = 0; x < 30; x++) { if (t8 == 1 && x == x8) Console.Write(">"); else if (t8 == 2 && (x == x8 || x == (x8 + 1) % 30 || x == (x8 + 2) % 30)) Console.Write("█"); else if (t8 == 3 && (x == x8 || x == (x8 + 1) % 30 || x == (x8 + 2) % 30 || x == (x8 + 3) % 30 || x == (x8 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
            for (int x = 0; x < 30; x++) { if (t9 == 1 && x == x9) Console.Write(">"); else if (t9 == 2 && (x == x9 || x == (x9 + 1) % 30 || x == (x9 + 2) % 30)) Console.Write("█"); else if (t9 == 3 && (x == x9 || x == (x9 + 1) % 30 || x == (x9 + 2) % 30 || x == (x9 + 3) % 30 || x == (x9 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");
            for (int x = 0; x < 30; x++) { if (t10 == 1 && x == x10) Console.Write(">"); else if (t10 == 2 && (x == x10 || x == (x10 + 1) % 30 || x == (x10 + 2) % 30)) Console.Write("█"); else if (t10 == 3 && (x == x10 || x == (x10 + 1) % 30 || x == (x10 + 2) % 30 || x == (x10 + 3) % 30 || x == (x10 + 4) % 30)) Console.Write("T"); else Console.Write(" "); } Console.WriteLine("    ");

            Console.WriteLine("==============================\nW = Skok vpred | A/D = Do stran | M = Menu");
            Thread.Sleep(20);
        }

}
else if (input_keyboard == 8)
{
    Console.WriteLine("Nazdar");
}
else
{
    Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&$&$$$&&&&&&&&&&&&$$$$$$$$$$$$$$$$$$&&&&&&&&&&&&&&&&$&&&&$$$&&&&&&&&&&&&&&&&&&&&&&&&&\r\n&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&$&&&&&&&&&&&&$$$$$$$$$$$$$$$$$$$$$$$$$$&&&&&&&&&&&&&&&&&$$&&&&$$$&&&&&&&&&&&&&&&&&&&&&&&\r\n&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$&&&&&&&&&&&&&&&&&$&&&&$$$$&&&&&&&&&&&&&&&&&&&&\r\n&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&$$$$$$$$$$$$$$$$*o!!!!!!!!!;;!;;;;;!!!o*$$$$$$$$$$&&&&&&&&&&&&&$&$$&&&&$$$&&&&&&&&&&&&&&&&&&&\r\n&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&$$$$$$$$$$$$*o!!!!!!!!!!!!!!!!!!!!!!!!!;;;;;;;;;;;;!*$$$$$$$&&&$*$&$*oo$$$$&&&&$$$&&&&&&&&&&&&&&&&\r\n&&&&&&&&&&&&&&&&&&&&&&&&######&##&&##$$&$$**ooooooooo&$o!!!!!!!!!!!!!!!!;;;;;;;;;;;;;;;o$$$$$$oo$$*o$&$$&$$$$$&&$$$&&&&&&&&&&&&&&&\r\n&&&&&&&&&&&&&&&&&&&&&&&&&#####################$*o$****$#&&&&&&$*ooo!!!!!!!!!!;;;;;;;;;;;;;;;;;!$$$o$$*!oo**$$*o!!o$$$$$$&&&&&&&&&&&&&&\r\n$$$$$$&&&&&&&&&&&&&&&&&&#######################&*$$$$############&&&&&&$$$*o!!o*o!;;;;;;;;;;;;;;;$*$$o!!*ooo****$$***$$$$$$&&&&&&&&&&&\r\n$$$&&&&&&&&&&&&&$&&&&$$##&&$$$$*$$$$$$$$$&#######&&$$&####################&&&&&&&&$$$$$$$$$o;;;;;$o$$*$$$$***$*$********$&$$&&&&&&&&&&\r\n&&&&&&&&&&&&&&$$$$$$$$&&$$$$$*$$$$$$$$$$$$$$$$####$$$$$############################&&$$o!!;;;;;!$$!*&$&&&$$$$**$$&$*oo$*$$$&&&&&&&&&&&\r\n&&&&&&$$$&&&&&$$$$$$$$$$&&$*$$$$$***o******$$$$**$$$$$$&&&$&&#############&&$$$$$$$$*!!!!!!!;;;o$&*&&#&#&$$$$&$$$$$$#&&$&&&&&$&&&&&&&&\r\n&&&&&&&&&&&$$&#&$$$$$$$&$**$$$$*oooooooo*****$$$***$$$&$$$**$$$$$$$$$$$$$$*ooo*$$$$$$o!!!!!;;;;!$&$####&&&#&&&$$&&$&&&#####&&&$&&&&&&&\r\n&&&&&&&&&$$&&&&&#$$$$$$$***$$*oooooooooooo****$$$**$$&$$***&&&&&&&&$$$$$$$*oo!!!o$$***oo!!!!;;;!$&&###&&###&##&&&&#######&&$&$&&&&&&&&\r\n&&&&&&&$&&$$$&&##&$&$$$***$$ooo!!!ooo$&&&$$$&$*$$**$$$$**&&####&o$$&$**$$$****ooo$$$o*ooo!!!!!;!$&$&########&$&&&&$$$&&&&&$$$$&&&&&&&&\r\n&&&&&&&&&$&&#&&&$####&****$*oo!!!!!o&&####$&&&$$$**$$$$**&$#######&&$*******$***$$$*oo**oo!!!!!!o$&&&######&$ooo!!!!;;;;;;;;o$&&&&&&&&\r\n&&&&&&&&&############$***$$oo!!!!!!*$$######&&$*$**$$$$***$**$&$&&$**********$$$$$$***$**oo!!!!!o$&&###&&$*****oooooooooooooo!!$&&&&&&\r\n$$$$$$$&#############****$$o!!!!!!!!*$**$$$$*o*****$$$$$$********************$$$$$$$$$$$**o!!!oo*$$&&&&&$o$$$$$$$$$$$$$$$*******$&&&&&\r\n$$$$$&$&############&****$$o!!!!!!!!oooooo!!!;;;;;;;!$$$$$$$**********$*$$$$$$$$$$$$$$$$*ooo****$$$&&&$$!o*$$$$$$$$$$$$**********&&&&&\r\n$$$$$$$$############$****$$$o!!!!!!oo***ooo!!!;;;;;; ;;!$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*$$*o!!!!!!*$$$$$!o$$$$$$$$$$$$$$*********&&&&&\r\n$$$$$$$$&###########$****$$$$o!!oo**********oo!!!;;;;;;;;!*$$$$$$$$$$$$$$$$$$$$$$$$**$$oo!!!!!;;;;!!o*$$o$$$$$$$$$$$$$$$$$******$&&&&&\r\n$$$$$$$$$$##########*****$$$$$$$**********$****o!!!;;;;;;;;;;;;;o$$$$$$$$$$$$$$**$$$**oo!!!!!;;;;;;!!o**$$$$$$$&&&$$$$$$$$******&&&&&&\r\n$$$$$$$$$$$$$$#####&******$$$$$$*********$$$$$$**oo!!!;;;;;;;;;;;;;*$$$$$$$$$$$$$$$$$$$$$*oo!!!!;;;;;;!o$$$$$&&&&&$$$$$$$$$****$&&&&&&\r\n$$$$$*******$**&&##&********$$$**********$$$$$$$$$*oo!!!!!;;;!!!!;;;$$$$$$$$$$$$$$$$$$$$$$$$$**o!!!!;;;!$$$$&$$$&&$$$$$$$$$$*$$&&&&&&&\r\n*********$$$*****$#$*********************$$$$$$$$$$**oo!!!!!!!!!!!!!$$$$$$$$$$$$$$$$$$$$$$$$$$$$*oo!!!!!$$$$$$$$$$$$$$$$$$$$$$&&&&&&&&\r\n**********$$$$***$#$*********************$$$$$$$$$$$$*ooo!!!ooooo**$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*o!!!!*$$$$$$&&&$$$$$$$$$$&&&&&&&&&&\r\n******ooooo*$$$$**&$*******************$$$$$$$$$$$$$$$**oo***$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*o!!!o$$$$$&&&&$$$$$$$$$&&&&&&&&&&&\r\n**ooo**oooooo$$$$$******************$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*o!!!$$$$$$$$$$$$$$$$&&&&&&&&&&&&&\r\n****oooooooooo*$$$$***************$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*o!!$$$$$$$$$$$$$&&&&&&&&&&&&&&&&\r\n&&$*oooooooooo**$$$$$**************$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*o!o$$$$$$$$$$&&&&&&&&&&&&&&&&###\r\n####&&&&*ooooo****$$$$$********$**$$$$****************$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$o!*$$$$$$&&&&&&&&&&&&&&&########\r\n#############&&$$*$$$$$$$$****$$$$$$$$$*****************$$$$$$$$$$$$$$$$$$$$$$$$$$$$&#&$$$$$$$$$$$$$$*o!*$$&&&&&&&&&&&&&&&&&&&&&&&&&&&\r\n####################&$$$$$$$$$$$$$$$$$$$$$*****************$$$$$$$$$$$$$$$$$$$$$$&###&$$$$$$$$$$$$$$$*o!$$*$&&&&&&&&&&&&&&&&&&&&&&&&&&\r\n######################&$$$$$$$$$$$$$$$$$$$$$***************$$$$$$$$$$$$$$$$$$$$$$&##&$$$$$$$$$$$$$$$$ooo$$$$$$&&&&&&&&&&&&&&&&&&&&&&&&\r\n#######################&$$$$$$$$$$$$$$$$$$$$$$**************$$$$$$$$$$$$$$*$$$$$##&$$$$$$$$$$$$$$$$$*o!$$$$$$$$$&&&&&&&&&&&&&&&&&&&&&&\r\n########################&$$$$$$$$$$$$$$$$$$$$$$$#####&&&&&&&&&&&&&&&&&&&$$$**$$$&$$$$$$$$$$$$$$$$$$$*oo$&$$$$$$$$$&&&&&&&&&&&&&&&&&&&&\r\n#########################&$$$$$$$$$$$$$$$$$$$$$$$$$$&&&&&&&&&&&&&$******$$$$$$$$$$$$$$$$$$$$$$$$$$$$oo&&&&$$$$$$$$$&&&&&&&&&&&&&&&&&&&\r\n###########################$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*o&#&&&&&$$$$$$$$&&&&&&&&&&&&&&&&&&\r\n############################$$$$$$$$$$$$$$$$$$$$$$$$$$$$$******$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*&#####&&&&$$$$$$$&&&&&&&&&&&&&&&&&\r\n#############################$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*#########&&&&$$$$$$&&&&&&&&&&&&&&&&\r\n##############################&$$$$$$$$$$$$$$$$$$$$$$$$$$$****$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$##########&&&&&$$$$$&&&&&&&&&&&&&&&&\r\n################################$$$$$$$$$$$$$$$$$$$$$$$$$*******$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$&############&&&&&$$$$&&&&&&&&&&&&&&&&\r\n##################################$$$$$$$$$$$$$$$$$$$$$$$*$$$$*$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$&###############&&&&&$$&&&&&&&&&&&&&&&&&\r\n####################################$$$$$$$$$$$$$$$$$$$$$$$$*$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$&#################&&&&&$&&&&&&&&&&&&&&&&&&\r\n######################################$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$######################&&&&&&&&&&&&&&&&&&&&&&\r\n#############&&&$$**o**$$################$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$&########################&&&&&&&&&&&&&&&&&&&&&&\r\n######&&&&$*o!oooooooooo&###################&$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$##########################&&&&&&&&&&&&&&&&&&&&&&&&\r\n&&&&$*o!oo!oooooooooooo$&#######################&$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$&###########################$*$&&&&&&&&&&&&&&&&&&&&&&&&\r\n!!oooooooo!ooooooooooo*$&##############################&$$$$$$$$$$$$$$$$$&###############################&&&$$*$&&&&&&&&&&&&&&&&&&&&&&\r\noooooooo!ooooo!ooooooo$&&####################################################################################&&$$$&&&&&&&&&&&&&&&&&&&&\r\n");
    Console.WriteLine("Stiskněte klávesu pro návrat do menu...");
    Console.ReadKey(true);
    goto gargamel;
}

// --- Kalkulačka dluhů ---
void Kalkulacka()
{
    Console.Write("Číslo 1: ");
    if (!double.TryParse(Console.ReadLine(), out double a)) a = 0;

    Console.Write("Operace (+,-,*,/): ");
    string op = Console.ReadLine();

    Console.Write("Číslo 2: ");
    if (!double.TryParse(Console.ReadLine(), out double b)) b = 0;

    double res = op switch
    {
        "+" => a + b,
        "-" => a - b,
        "*" => a * b,
        "/" => b != 0 ? a / b : 0,
        _ => 0
    };

    Console.WriteLine($"Výsledek: {res}");
    Console.WriteLine("\nStiskněte klávesu pro návrat do menu...");
    Console.ReadKey(true);
}

// --- Blackjack ---
void Blackjack()
{
    string[] jmenaKaret = "A,2,3,4,5,6,7,8,9,10,J,Q,K".Split(',');
    Random r = new Random();

    while (true)
    {
        if (penize <= 0)
        {
            Console.WriteLine("Došly ti peníze. Konec hry.");
            File.WriteAllText(soubor, penize.ToString());
            break;
        }

        Console.Write($"Konto: {penize} Kč. Vsaď (0 pro konec): ");
        if (!int.TryParse(Console.ReadLine(), out int sazka) || sazka <= 0 || sazka > penize)
        {
            Console.WriteLine("Konec hry.");
            File.WriteAllText(soubor, penize.ToString());
            break;
        }

        List<int> balicek = new List<int>();
        for (int i = 0; i < 52; i++) balicek.Add(i);

        for (int i = 0; i < 100; i++)
        {
            int p1 = r.Next(52), p2 = r.Next(52);
            int tmp = balicek[p1];
            balicek[p1] = balicek[p2];
            balicek[p2] = tmp;
        }

        List<int> hrac = new List<int> { balicek[0], balicek[1] };
        List<int> dealer = new List<int> { balicek[2], balicek[3] };
        balicek.RemoveRange(0, 4);

        // --- Tah hráče ---
        while (true)
        {
            int soucetHrac = 0, esaHrac = 0;
            foreach (int c in hrac)
            {
                int v = c % 13;
                soucetHrac += v == 0 ? 11 : (v >= 9 ? 10 : v + 1);
                if (v == 0) esaHrac++;
            }
            while (soucetHrac > 21 && esaHrac > 0) { soucetHrac -= 10; esaHrac--; }

            if (soucetHrac >= 21) break;

            Console.Write($"Dealer: [{jmenaKaret[dealer[0] % 13]}] [??] ");
            Console.WriteLine();
            Console.Write("Hráč: ");
            foreach (int c in hrac) Console.Write($"[{jmenaKaret[c % 13]}] ");
            Console.WriteLine($"(Body: {soucetHrac})");

            Console.Write("\n[h]it / [s]tand: ");
            string tah = Console.ReadLine();
            if (tah != "h") break;

            hrac.Add(balicek[0]);
            balicek.RemoveAt(0);
        }

        int souPHrac = 0, esaP = 0;
        foreach (int c in hrac)
        {
            int v = c % 13;
            souPHrac += v == 0 ? 11 : (v >= 9 ? 10 : v + 1);
            if (v == 0) esaP++;
        }
        while (souPHrac > 21 && esaP > 0) { souPHrac -= 10; esaP--; }

        // --- Tah dealera ---
        if (souPHrac <= 21)
        {
            while (true)
            {
                int souD = 0, esaD = 0;
                foreach (int c in dealer)
                {
                    int v = c % 13;
                    souD += v == 0 ? 11 : (v >= 9 ? 10 : v + 1);
                    if (v == 0) esaD++;
                }
                while (souD > 21 && esaD > 0) { souD -= 10; esaD--; }

                if (souD >= 17) break;

                dealer.Add(balicek[0]);
                balicek.RemoveAt(0);
            }
        }

        int souFDealer = 0, esaFD = 0;
        foreach (int c in dealer)
        {
            int v = c % 13;
            souFDealer += v == 0 ? 11 : (v >= 9 ? 10 : v + 1);
            if (v == 0) esaFD++;
        }
        while (souFDealer > 21 && esaFD > 0) { souFDealer -= 10; esaFD--; }

        // --- Výpis ---
        Console.Write("Dealer: ");
        foreach (int c in dealer) Console.Write($"[{jmenaKaret[c % 13]}] ");
        Console.WriteLine($"(Body: {souFDealer})");

        Console.Write("Hráč: ");
        foreach (int c in hrac) Console.Write($"[{jmenaKaret[c % 13]}] ");
        Console.WriteLine($"(Body: {souPHrac})");

        bool prohral = false;

        if (souPHrac > 21)
        {
            Console.WriteLine($"-{sazka} Kč");
            penize -= sazka;
            prohral = true;
        }
        else if (souFDealer > 21 || souPHrac > souFDealer)
        {
            Console.WriteLine($"+{sazka} Kč");
            penize += sazka;
        }
        else if (souPHrac < souFDealer)
        {
            Console.WriteLine($"-{sazka} Kč");
            penize -= sazka;
            prohral = true;
        }
        else
        {
            Console.WriteLine("Remíza");
        }

        File.WriteAllText(soubor, penize.ToString());

        if (prohral)
        {
            if (penize <= 0)
            {
                Console.WriteLine("\nDošly ti peníze. Konec hry.");
                break;
            }

            Console.Write("\nProhrál jsi. Stiskni 1 pro pokračování, cokoliv jiného pro konec: ");
            string volba = Console.ReadLine();
            if (volba != "1")
            {
                Console.WriteLine("Konec hry.");
                break;
            }
        }
    }

    Console.WriteLine("\nKonečný stav peněz: " + penize + " Kč (uloženo do souboru " + soubor + ")");
    Console.WriteLine("Stiskněte klávesu pro návrat do menu...");
    Console.ReadKey(true);
}