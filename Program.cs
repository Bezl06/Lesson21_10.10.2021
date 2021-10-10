using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lesson21
{
    public class Program
    {
        public static object locker = new object();
        public readonly static int maxY = Console.WindowHeight;
        public readonly static Random random = new Random();
        public static void Main(string[] args)
        {

            Console.Clear();
            Chain[] colls = new Chain[Console.WindowWidth];
            for (int i = 0; i < colls.Length; i += 2)
            {
                Thread.Sleep(random.Next(200));
                colls[i] = new Chain(i);
            }
            Console.ReadLine();
        }
    }
    public class Chain
    {
        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly int posX;
        private int posY = 0;
        private char[] chain;
        private Timer timer;
        public Chain(int x)
        {
            posX = x;
            chain = new char[Program.random.Next(3, 8)];
            timer = new Timer(MoveNext, null, 0, Program.random.Next(50, 300));
        }
        private void MoveNext(object _)
        {
            for (int i = 0; i < chain.Length; i++)
                chain[i] = chars[Program.random.Next(chars.Length)];
            lock (Program.locker)
            {
                if (posY < Program.maxY)
                {
                    Console.SetCursorPosition(posX, posY);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(chain[0]);
                }
                if (posY > 0 && posY - 1 < Program.maxY)
                {
                    Console.SetCursorPosition(posX, posY - 1);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(chain[1]);
                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                for (int i = 2; i < chain.Length && i <= posY; i++)
                {
                    if (posY - i >= Program.maxY) continue;
                    Console.SetCursorPosition(posX, posY - i);
                    Console.Write(chain[i]);
                }
                Console.ResetColor();
                if (posY - chain.Length >= 0)
                {
                    Console.SetCursorPosition(posX, posY - chain.Length);
                    Console.Write(' ');
                }
            }
            if (++posY - chain.Length < Program.maxY) return;
            chain = new char[Program.random.Next(3, 8)];
            posY = 0;
            timer.Change(50, Program.random.Next(50, 300));
        }
    }
}