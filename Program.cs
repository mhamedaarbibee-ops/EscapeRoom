 using System;
using System.Diagnostics;

namespace EscapeRoom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Je wordt wakker in een donkere kamer...");
            Console.WriteLine("Wat is je naam?");
            string naam = Console.ReadLine() ?? "";

            Console.WriteLine($"Welkom {naam}!");

            bool playing = true;

            while (playing)
            {
                // === Setup nieuwe game ===
                int health = 100;
                int checkpoint = 1;
                int score = 0;

                bool heeftSleutel = false;
                bool heeftKamer3Sleutel = false;
                bool heeftKamer5Sleutel = false;

                string moeilijkheid = KiesMoeilijkheid(ref health);

                Console.WriteLine();
                Console.WriteLine($"Moeilijkheid: {moeilijkheid}");
                Console.WriteLine($"Health: {health}");
                Console.WriteLine();

                Random random = new Random();
                Stopwatch timer = Stopwatch.StartNew();

                bool gameOver = false;
                bool gewonnen = false;

                while (!gameOver && !gewonnen)
                {
                    if (health <= 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("GAME OVER! Je health is op.");
                        gameOver = true;
                        break;
                    }

                    switch (checkpoint)
                    
{
    case 1:
        Kamer1(ref health, ref score, ref checkpoint, ref heeftSleutel, random);
        break;
    case 2:
        Kamer2(ref health, ref score, ref checkpoint, ref heeftKamer3Sleutel, moeilijkheid);
        break;
    case 3:
        Kamer3(ref score, ref checkpoint, ref heeftKamer3Sleutel, ref gameOver);
        break;
    case 4:
        Kamer4(ref health, ref score, ref checkpoint, ref heeftKamer5Sleutel, moeilijkheid);
        break;
    case 5:
        Kamer5(ref score, ref heeftKamer5Sleutel, ref gewonnen, ref checkpoint, naam, health, timer);
        break;
    case 6:
        Kamer6(ref health, ref score, ref gameOver, ref gewonnen, timer);
        break;
}
                         
                }

                timer.Stop();

                if (gewonnen)
                {
                    // Win-scherm is al getoond in Kamer5
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Eindscore: {score}");
                    Console.WriteLine($"Tijd: {timer.Elapsed.Minutes:00}:{timer.Elapsed.Seconds:00}");
                }

                Console.WriteLine();
                Console.WriteLine("Wil je opnieuw spelen? (ja/nee)");
                string opnieuw = (Console.ReadLine() ?? "").ToLower().Trim();

                if (opnieuw != "ja")
                {
                    playing = false;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Bedankt voor het spelen!");
            Console.ReadLine();
        }

        // =========================
        // HULPMETHODEN
        // =========================

        static string KiesMoeilijkheid(ref int health)
        {
            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine("       ESCAPE ROOM");
            Console.WriteLine("================================");
            Console.WriteLine();
            Console.WriteLine("Kies je moeilijkheid:");
            Console.WriteLine("1. Makkelijk");
            Console.WriteLine("2. Normaal");
            Console.WriteLine("3. Moeilijk");

            string keuze = Console.ReadLine() ?? "";

            switch (keuze)
            {
                case "1":
                    health = 120;
                    return "Makkelijk";
                case "3":
                    health = 80;
                    return "Moeilijk";
                default:
                    if (keuze != "2")
                    {
                        Console.WriteLine("Ongeldige keuze. Normaal wordt gekozen.");
                    }
                    health = 100;
                    return "Normaal";
            }
        }

        static void Kamer1(ref int health, ref int score, ref int checkpoint, ref bool heeftSleutel, Random random)
        {
            Console.WriteLine();
            Console.WriteLine("KAMER 1");
            Console.WriteLine("Je ziet een deur en een raam.");
            Console.WriteLine("Kies: deur, raam of inventaris");

            string keuze = (Console.ReadLine() ?? "").ToLower().Trim();

            switch (keuze)
            {
                case "inventaris":
                    Console.WriteLine(heeftSleutel
                        ? "Inventaris: je hebt een sleutel."
                        : "Je inventaris is leeg.");
                    break;

                case "deur":
                    if (heeftSleutel)
                    {
                        Console.WriteLine("Je gebruikt de sleutel. De deur gaat open!");
                        score += 50;
                        Console.WriteLine($"+50 punten! Score: {score}");
                        checkpoint = 2;
                    }
                    else
                    {
                        Console.WriteLine("De deur zit op slot. Je probeert hem te forceren.");
                        health -= 20;
                        Console.WriteLine($"Health: {health}");

                        if (random.Next(1, 4) == 1)
                        {
                            Console.WriteLine("Je vindt een sleutel onder een kast!");
                            heeftSleutel = true;
                            score += 50;
                            Console.WriteLine($"+50 punten! Score: {score}");
                        }
                        else
                        {
                            Console.WriteLine("Geen sleutel gevonden.");
                        }
                    }
                    break;

                case "raam":
                    health -= 15;
                    Console.WriteLine("Je springt door het raam en snijdt jezelf aan het glas.");
                    Console.WriteLine($"Health: {health}");
                    score += 25;
                    Console.WriteLine($"+25 punten! Score: {score}");
                    checkpoint = 2;
                    break;

                default:
                    Console.WriteLine("Die keuze ken ik niet.");
                    break;
            }
        }

        static void Kamer2(ref int health, ref int score, ref int checkpoint, ref bool heeftKamer3Sleutel, string moeilijkheid)
        {
            Console.WriteLine();
            Console.WriteLine("KAMER 2");
            Console.WriteLine("Je ziet een trap en een donkere gang.");
            Console.WriteLine("In de gang staat een afgesloten kist.");
            Console.WriteLine("Kies: trap, gang of kist");

            string keuze = (Console.ReadLine() ?? "").ToLower().Trim();

            switch (keuze)
            {
                case "trap":
                    if (!heeftKamer3Sleutel)
                    {
                        Console.WriteLine("Je voelt dat je iets belangrijks mist.");
                        Console.WriteLine("Misschien moet je eerst de kist onderzoeken.");
                    }
                    else
                    {
                        int trapSchade = moeilijkheid switch
                        {
                            "Makkelijk" => 20,
                            "Moeilijk" => 40,
                            _ => 30
                        };

                        Console.WriteLine($"Je struikelt op de trap en verliest {trapSchade} health.");
                        health -= trapSchade;
                        Console.WriteLine($"Health: {health}");
                        checkpoint = 3;
                    }
                    break;

                case "gang":
                    Console.WriteLine("Op de muur staat gekrast: π = 3.14");
                    Console.WriteLine("Je loopt voorzichtig door de donkere gang.");
                    Console.WriteLine("Je ziet een oude kist met een cijferslot.");
                    break;

                case "kist":
                    Console.WriteLine("De kist zit op slot. Er is een 3-cijferige code nodig.");
                    Console.WriteLine("Welke code voer je in?");
                    string code = Console.ReadLine() ?? "";

                    if (code == "314")
                    {
                        if (!heeftKamer3Sleutel)
                        {
                            Console.WriteLine("De code is correct! De kist gaat open.");
                            Console.WriteLine("Je vindt een sleutel voor Kamer 3!");
                            heeftKamer3Sleutel = true;
                            score += 100;
                            Console.WriteLine($"+100 punten! Score: {score}");
                        }
                        else
                        {
                            Console.WriteLine("Je hebt deze sleutel al gevonden.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("De code is fout. Je verliest 10 health.");
                        health -= 10;
                        Console.WriteLine($"Health: {health}");
                    }
                    break;

                default:
                    Console.WriteLine("Die keuze ken ik niet.");
                    break;
            }
        }

        static void Kamer3(ref int score, ref int checkpoint, ref bool heeftKamer3Sleutel, ref bool gameOver)
        {
            Console.WriteLine();
            Console.WriteLine("KAMER 3");
            Console.WriteLine("Je bent aangekomen bij de laatste deur van deze kamer.");
            Console.WriteLine("De deur heeft een speciale sleutel nodig.");
            Console.WriteLine("Kies: deur, inventaris of sterven");

            string keuze = (Console.ReadLine() ?? "").ToLower().Trim();

            switch (keuze)
            {
                case "inventaris":
                    Console.WriteLine(heeftKamer3Sleutel
                        ? "Inventaris: je hebt de sleutel van Kamer 3."
                        : "Je hebt de sleutel van Kamer 3 niet.");
                    break;

                case "deur":
                    if (heeftKamer3Sleutel)
                    {
                        Console.WriteLine("Je gebruikt de sleutel. De deur gaat open!");
                        Console.WriteLine("Je loopt naar Kamer 4...");
                        score += 100;
                        Console.WriteLine($"+100 punten! Score: {score}");
                        checkpoint = 4;
                    }
                    else
                    {
                        Console.WriteLine("De deur gaat niet open. Je hebt de sleutel van Kamer 3 nodig.");
                    }
                    break;

                case "sterven":
                    Console.WriteLine("Je geeft het op. GAME OVER!");
                    gameOver = true;
                    break;

                default:
                    Console.WriteLine("Die keuze ken ik niet.");
                    break;
            }
        }

                static void Kamer4(ref int health, ref int score, ref int checkpoint, ref bool heeftKamer5Sleutel, string moeilijkheid)
        {
            Console.WriteLine();
            Console.WriteLine("KAMER 4");
            Console.WriteLine("Je komt een donkere kamer binnen.");
            Console.WriteLine("Voor je staat een beveiligingssysteem.");
            Console.WriteLine();
            Console.WriteLine("Op het scherm staat:");
            Console.WriteLine("1. Rood");
            Console.WriteLine("2. Blauw");
            Console.WriteLine("3. Groen");
            Console.WriteLine();
            Console.WriteLine("Welke kleur kies je?");

            string keuze = (Console.ReadLine() ?? "").ToLower().Trim();

            // Random juiste kleur
            string[] kleuren = { "rood", "blauw", "groen" };
            Random random = new Random();
            string juisteKleur = kleuren[random.Next(0, 3)];

            if (keuze == juisteKleur)
            {
                Console.WriteLine("De juiste kleur! Het beveiligingssysteem gaat uit.");
                Console.WriteLine("Je vindt een sleutel voor Kamer 5!");
                heeftKamer5Sleutel = true;
                score += 150;
                Console.WriteLine($"+150 punten! Score: {score}");
                checkpoint = 5;
            }
            else if (keuze == "rood" || keuze == "blauw" || keuze == "groen")
            {
                int kleurSchade = moeilijkheid switch
                {
                    "Makkelijk" => 10,
                    "Moeilijk" => 30,
                    _ => 20
                };

                Console.WriteLine("Verkeerde kleur! Je krijgt een elektrische schok.");
                Console.WriteLine($"Je verliest {kleurSchade} health.");
                health -= kleurSchade;
                Console.WriteLine($"Health: {health}");
            }
            else
            {
                Console.WriteLine("Die keuze ken ik niet.");
            }
        }

        static void Kamer5(ref int score, ref bool heeftKamer5Sleutel, ref bool gewonnen,
                           ref int checkpoint, string naam, int health, Stopwatch timer)
        {
            Console.WriteLine();
            Console.WriteLine("KAMER 5 - FINALE");
            Console.WriteLine("Je staat voor de laatste deur.");
            Console.WriteLine("Dit is je laatste kans om te ontsnappen.");
            Console.WriteLine("Kies: deur of inventaris");

            string keuze = (Console.ReadLine() ?? "").ToLower().Trim();

            switch (keuze)
            {
                case "inventaris":
                    Console.WriteLine(heeftKamer5Sleutel
                        ? "Inventaris: je hebt de sleutel van Kamer 5."
                        : "Je hebt de sleutel van Kamer 5 niet.");
                    break;

                case "deur":
                    if (heeftKamer5Sleutel)
                    {
                        timer.Stop();

                        Console.WriteLine();
                        Console.WriteLine("Je gebruikt de laatste sleutel...");
                        Console.WriteLine("De deur gaat langzaam open...");
                        Console.WriteLine();
                        Console.WriteLine("================================");
                        Console.WriteLine("          YOU WIN!");
                        Console.WriteLine("================================");
                        Console.WriteLine();
                        Console.WriteLine($"Gefeliciteerd {naam}!");
                        Console.WriteLine();
                        Console.WriteLine($"Health: {health}");
                        Console.WriteLine($"Score: {score}");
                        Console.WriteLine($"Tijd: {timer.Elapsed.Minutes:00}:{timer.Elapsed.Seconds:00}");
                        Console.WriteLine();
                        Console.WriteLine("Je hebt alle 5 kamers overleefd.");
                        Console.WriteLine("Je bent ontsnapt uit de Escape Room!");

                        string rang = score >= 400 && health >= 50 ? "ESCAPE MASTER"
                                    : score >= 250 ? "ESCAPE EXPERT"
                                    : "ESCAPE SURVIVOR";

                        Console.WriteLine();
                        Console.WriteLine($"Rang: {rang}");
                        Console.WriteLine();

                        // ===== GEHEIME ROUTE =====
                        if (score >= 350)
                        {
                            Console.WriteLine("Terwijl je naar buiten loopt, zie je iets glimmen...");
                            Console.WriteLine("Wil je de geheime deur onderzoeken? (ja/nee)");

                            string geheim = (Console.ReadLine() ?? "").ToLower().Trim();

                            if (geheim == "ja")
                            {
                                checkpoint = 6;
                                return;
                            }
                        }

                        // Normale ending
                        gewonnen = true;
                    }
                    else
                    {
                        Console.WriteLine("De deur gaat niet open. Je hebt de sleutel van Kamer 5 nodig.");
                    }
                    break;

                default:
                    Console.WriteLine("Die keuze ken ik niet.");
                    break;
            }
        }

         static void Kamer6(ref int health, ref int score, ref bool gameOver, ref bool gewonnen, Stopwatch timer)
{
    Console.WriteLine();
    Console.WriteLine("================================");
    Console.WriteLine("        GEHEIME KAMER 6");
    Console.WriteLine("================================");
    Console.WriteLine();
    Console.WriteLine("Een stem klinkt door de kamer...");
    Console.WriteLine();
    Console.WriteLine("Ik heb sleutels maar geen sloten.");
    Console.WriteLine("Ik heb ruimte maar geen kamers.");
    Console.WriteLine("Ik laat je binnen maar heb geen deur.");
    Console.WriteLine();
    Console.WriteLine("Wat ben ik?");

    string keuze = (Console.ReadLine() ?? "").ToLower().Trim();

    if (keuze == "toetsenbord" || keuze == "keyboard")
    {
        score += 250;
        timer.Stop();
        gewonnen = true;

        Console.WriteLine();
        Console.WriteLine("De muur schuift open!");
        Console.WriteLine("Je hebt het geheim van de Escape Room ontdekt!");
        Console.WriteLine();
        Console.WriteLine("================================");
        Console.WriteLine("      SECRET ENDING UNLOCKED");
        Console.WriteLine("================================");
        Console.WriteLine();
        Console.WriteLine($"Health: {health}");
        Console.WriteLine($"Score: {score}");
        Console.WriteLine($"Tijd: {timer.Elapsed.Minutes:00}:{timer.Elapsed.Seconds:00}");
        Console.WriteLine();
        Console.WriteLine("Rang: ESCAPE LEGEND");
        Console.WriteLine();
        Console.WriteLine("Je hebt het geheime einde gevonden!");
    }
    else
    {
        health -= 20;
        timer.Stop();
        gameOver = true;

        Console.WriteLine();
        Console.WriteLine("Fout antwoord!");
        Console.WriteLine("De geheime deur valt onmiddellijk dicht.");
        Console.WriteLine();
        Console.WriteLine("Je verliest 20 health.");
        Console.WriteLine($"Health: {health}");
        Console.WriteLine();
        Console.WriteLine("================================");
        Console.WriteLine("         SECRET FAILURE");
        Console.WriteLine("================================");
        Console.WriteLine();
        Console.WriteLine("Je was zo dichtbij...");
        Console.WriteLine("De geheime route is mislukt.");
    }
  }
    }
}

    


