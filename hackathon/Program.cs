using System;
using System.Collections.Generic;
using System.IO;

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

Console.WriteLine("  ______         _________ _           __     _________ _______    _______  _       \r\n / ____ \\  \\__   __/( (    /|   /  \\    \\__   __/(  ___  ) (  ___  )( \\      \r\n( (    \\/     ) (   |  \\  ( |   \\/) )      ) (   | (   ) || (   ) || (      \r\n| (____       | |   |   \\ | |     | |      | |   | |   | || |   | || |      \r\n|  ___ \\      | |   | (\\ \\) |     | |      | |   | |   | || |   | || |      \r\n| (    ) )    | |   | | \\   |     | |      | |   | |   | || |   | || |      \r\n( (___) )  ___) (___| )  \\  |    __) (_     | |   | (___) || (___) || (____/\\\r\n \\_____/   \\_______/|/    )_)  \\____/     )_(   (_______)(_______)(_______/\r\n                                                                           ");
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
Console.WriteLine("1) Gambling center      2) Kačka Kalkulačka dluhů\n3) Počasí               4) Generátor hesel\n5) Exit");

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

    var apiKey = "a6d889e7b8c2970298e5722e99d65782";

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
    Console.BackgroundColor = ConsoleColor.White; // Bílé pozadí
    Console.ForegroundColor = ConsoleColor.Black; // Černý text
    Console.Clear(); // Přemaluje celou obrazovku na bílo

    Console.Write("Délka hesla: ");
    int len = int.Parse(Console.ReadLine());

    string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%*€|\";
    Random r = new();
    string pass = "";

    for (int i = 0; i < len; i++) pass += chars[r.Next(chars.Length)];

    Console.WriteLine($"Tvoje heslo: {pass}");
    Console.ResetColor(); // Vrátí původní barvy konzole (např. černou a bílou)
    Console.ReadKey(true);
    goto gargamel;
}
else if (input_keyboard == 5)
{
    Console.WriteLine("Konec.");
    return;
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