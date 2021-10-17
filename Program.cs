using System;
using System.Threading.Tasks;

namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Clear();
            Chain.frame = new char[Chain.maxX * Chain.maxY];
            for (int i = 0; i < Chain.maxY; i++)
                for (int j = 0; j < Chain.maxX; j++)
                    Chain.frame[j + (i * Chain.maxX)] = ' ';
            const int speed = 50;
            Chain[] chains = new Chain[Chain.maxX * 2];
            for (int i = 0; i < chains.Length; i++)
            {
                if (i % 2 == 0)
                    chains[i] = new Chain(i / 2, Chain.random.Next(Chain.maxY * 2));
                else
                    chains[i] = new Chain(i / 2, Chain.random.Next(Chain.maxY * 2, Chain.maxY * 4));
            }
            while (true)
            {
                Task task = Task.Delay(speed);
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(Chain.frame);
                Console.ForegroundColor = ConsoleColor.Green;
                for (int i = 0; i < chains.Length; i++)
                {
                    if (chains[i].posY > 1 && chains[i].posY - 1 < Chain.maxY)
                    {
                        Console.SetCursorPosition(chains[i].posX, chains[i].posY - 1);
                        Console.Write(Chain.chars[Chain.random.Next(Chain.chars.Length)]);
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < chains.Length; i++)
                {
                    if (chains[i].posY > 0 && chains[i].posY < Chain.maxY)
                    {
                        Console.SetCursorPosition(chains[i].posX, chains[i].posY);
                        Console.Write(Chain.chars[Chain.random.Next(Chain.chars.Length)]);
                    }
                }
                for (int i = 0; i < chains.Length; i++)
                    chains[i].MoveNext();
                task.Wait();
            }
        }
    }
    public class Chain
    {
        public static string chars = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@#$%^&*!";
        public static int maxY = Console.WindowHeight - 1;
        public static int maxX = Console.WindowWidth;
        public static Random random = new Random();
        public static char[] frame;
        public readonly int posX;
        public int posY = 0;
        public int length;
        public int tryies;
        public int maxTryies;
        public Chain(int x, int startSpeed)
        {
            posX = x;
            tryies = startSpeed;
            length = random.Next(10, 20);
            maxTryies = random.Next(1, 3);
        }
        public void MoveNext()
        {
            if (tryies-- > 1) return;
            for (int i = 1; i < length && i <= posY; i++)
            {
                if (posY - i >= maxY) continue;
                frame[posX + ((posY - i) * maxX)] = chars[random.Next(chars.Length)];
            }
            if (posY - length >= 0) frame[posX + ((posY - length) * maxX)] = ' ';
            tryies = maxTryies;
            if (++posY - length < maxY) return;
            posY = 0;
            tryies = random.Next(maxY * 2, maxY * 4);
            maxTryies = random.Next(1, 3);
        }
    }
}
