using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sna
{
    internal class Snake
    {

        private static int score = 0;
        private static int foodX;
        private static int foodY;
        private static int headX;
        private static int headY;
        private static List<int> tailX = new List<int>();
        private static List<int> tailY = new List<int>();
        private static bool dead = false;
        private static char[] directions = { 'W', 'A', 'S', 'D' };
        private static char direction;

        private static Random random = new Random();

        public void GameStart()
        {

            go();

            Thread newthread = new Thread(Klavisha);
            newthread.Start();

            
            while (!dead)
            {
                Move();
                if (dead)
                    break;

                Console.Clear();

                DrawMap();
                Eda();
                DrawSnake();

                Thread.Sleep(200);
            }

            newthread.Join();

            Console.Clear();
            Console.WriteLine("Ты подох!");
            Console.WriteLine("Твой счёт: " + score);
            Console.ReadLine();
        }

        private void go()
        {
            direction = 'D';

            headX = (int)Border.width / 2;
            headY = (int)Border.height / 2;

            tailX.Clear();
            tailY.Clear();

            foodX = random.Next((int)Border.width - 2) + 1;
            foodY = random.Next((int)Border.height - 2) + 1;

            score = 0;
        }

        private static void Klavisha()
        {
            while (!dead)
            {
               
                ConsoleKeyInfo moving = Console.ReadKey(true);
                char najatayaklavisha = char.ToUpper(moving.KeyChar);

                if (Array.IndexOf(directions, najatayaklavisha) >= 0)
                {
                    if (TrueDirection(najatayaklavisha))
                        direction = najatayaklavisha;
                }
                
            }
        }

        private static bool TrueDirection(char newDirection)
        {
            if (direction == 'W' && newDirection == 'S')
                return false;
            if (direction == 'S' && newDirection == 'W')
                return false;
            if (direction == 'A' && newDirection == 'D')
                return false;
            if (direction == 'D' && newDirection == 'A')
                return false;

            return true;
        }

        private void Move()
        {
            int prevX = tailX.Count > 0 ? tailX[tailX.Count - 1] : headX;
            int prevY = tailY.Count > 0 ? tailY[tailY.Count - 1] : headY;

            tailX.Insert(0, headX);
            tailY.Insert(0, headY);

            switch (direction)
            {
                case 'W':
                    headY--;
                    break;
                case 'S':
                    headY++;
                    break;
                case 'A':
                    headX--;
                    break;
                case 'D':
                    headX++;
                    break;
            }

            if (headX < 1 || headX >= (int)Border.width - 1 || headY < 1 || headY >= (int)Border.height - 1 || selfdestruction())
                dead = true;

            if (headX == foodX && headY == foodY)
            {
                score++;

                foodX = random.Next((int)Border.width - 2) + 1;
                foodY = random.Next((int)Border.height - 2) + 1;
            }
            else if (tailX.Count > 0)
            {
                tailX.RemoveAt(tailX.Count - 1);
                tailY.RemoveAt(tailY.Count - 1);
            }
        }

        private bool selfdestruction()
        {
            for (int i = 0; i < tailX.Count; i++)
            {
                if (tailX[i] == headX && tailY[i] == headY)
                    return true;
            }
            return false;
        }

        private void DrawMap()
        {
            for (int i = 0; i < (int)Border.height; i++)
            {
                for (int j = 0; j < (int)Border.width; j++)
                {
                    if (i == 0 || i == (int)Border.height - 1 || j == 0 || j == (int)Border.width - 1)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write("#");
                    }
                }
            }
        }

        private void Eda()
        {
            Console.SetCursorPosition(foodX, foodY);
            Console.Write("x");
        }

        private void DrawSnake()
        {
            Console.SetCursorPosition(headX, headY);
            Console.Write("O");

            for (int i = 0; i < tailX.Count; i++)
            {
                Console.SetCursorPosition(tailX[i], tailY[i]);
                Console.Write("o");
            }
        }
    }
}