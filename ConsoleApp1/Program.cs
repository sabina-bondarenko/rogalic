using System;
using System.Collections.Generic;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        Console.WriteLine("Добро пожаловать в земли Авалона, воин!");
        Console.Write("Объяви своё имя, герой: ");
        string playerName = Console.ReadLine();

        Player player = new Player(playerName);
        Console.WriteLine($"Вас зовут {player.Name}!");
        Console.WriteLine($"Вам вручен меч {player.Weapon.Name} с силой атаки {player.Weapon.Damage}, а также {player.Aid.Name} для лечения на {player.Aid.Healing}hp.");
        Console.WriteLine($"У вас сейчас {player.GetCurrentHealth()}hp.");

        Random rnd = new Random();
        int score = 0;

        while (player.GetCurrentHealth() > 0)
        {
            Enemy enemy = Enemy.GenerateRandomEnemy(rnd);
            Console.WriteLine($"{player.Name} сталкивается с {enemy.Name} ({enemy.GetCurrentHealth()}hp), вооружённым {enemy.Weapon.Name} ({enemy.Weapon.Damage})");

            while (enemy.GetCurrentHealth() > 0 && player.GetCurrentHealth() > 0)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Атаковать");
                Console.WriteLine("2. Пропустить ход");
                Console.WriteLine("3. Использовать аптечку");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    int playerDamage = player.Attack();
                    int enemyDamage = enemy.Attack();

                    enemy.UpdateHealth(-playerDamage);
                    player.UpdateHealth(-enemyDamage);

                    Console.WriteLine($"{player.Name} нанёс {playerDamage} урона {enemy.Name}");
                    Console.WriteLine($"{enemy.Name} нанёс {enemyDamage} урона {player.Name}");
                    Console.WriteLine($"У {enemy.Name} осталось {enemy.GetCurrentHealth()}hp, у {player.Name} осталось {player.GetCurrentHealth()}hp");
                }
                else if (choice == "2")
                {
                    Console.WriteLine($"{player.Name} пропустил ход");
                    int enemyDamage = enemy.Attack();
                    player.UpdateHealth(-enemyDamage);
                    Console.WriteLine($"{enemy.Name} нанёс {enemyDamage} урона {player.Name}");
                    Console.WriteLine($"У {enemy.Name} осталось {enemy.GetCurrentHealth()}hp, у {player.Name} осталось {player.GetCurrentHealth()}hp");
                }
                else if (choice == "3")
                {
                    player.UseAid();
                    Console.WriteLine($"{player.Name} использовал аптечку");
                    Console.WriteLine($"У {player.Name} теперь {player.GetCurrentHealth()}hp");
                }
            }

            if (player.GetCurrentHealth() > 0)
            {
                Console.WriteLine($"Поздравляем! Вы победили {enemy.Name} и заработали {enemy.Score} очков.");
                score += enemy.Score;
                Console.WriteLine($"У вас теперь {score} очков.");
            }
            else
            {
                Console.WriteLine("Игра окончена. Вы проиграли.");
                break;
            }
        }
    }
}

class Player
{
    public string Name { get; }
    public int MaxHealth { get; }
    protected int CurrentHealth { get; set; }
    public Aid Aid { get; }
    public Weapon Weapon { get; }

    public Player(string name)
    {
        Name = name;
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
        Aid = new Aid("Лечебное зелье", 20);
        Weapon = new Weapon("Клинок Света", 25);
    }

    public int Attack()
    {
        return Weapon.Damage;
    }

    public void UseAid()
    {
        CurrentHealth = Math.Min(MaxHealth, CurrentHealth + Aid.Healing);
    }

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public void UpdateHealth(int value)
    {
        CurrentHealth += value;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }
}

class Enemy
{
    public string Name { get; }
    public int MaxHealth { get; }
    protected int CurrentHealth { get; set; }
    public Weapon Weapon { get; }
    public int Score { get; }

    private static readonly List<string> EnemyNames = new List<string> { "Тёмный Рыцарь", "Тролль", "Виверна", "Чародей", "Разбойник" };

    public Enemy(string name, int maxHealth, Weapon weapon, int score)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        Weapon = weapon;
        Score = score;
    }

    public int Attack()
    {
        return Weapon.Damage;
    }

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public void UpdateHealth(int value)
    {
        CurrentHealth += value;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }

    public static Enemy GenerateRandomEnemy(Random rnd)
    {
        string name = EnemyNames[rnd.Next(0, EnemyNames.Count)];
        int maxHealth = rnd.Next(40, 80);

        List<Weapon> weapons = new List<Weapon>
        {
            new Weapon("Огненный Кинжал", 15),
            new Weapon("Ледяной Топор", 20),
            new Weapon("Копьё Смерти", 18)
        };
        Weapon weapon = weapons[rnd.Next(weapons.Count)];

        int score = rnd.Next(5, 15);

        return new Enemy(name, maxHealth, weapon, score);
    }
}

class Aid
{
    public string Name { get; }
    public int Healing { get; }

    public Aid(string name, int healing)
    {
        Name = name;
        Healing = healing;
    }
}

class Weapon
{
    public string Name { get; }
    public int Damage { get; }

    public Weapon(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }
}
