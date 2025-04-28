namespace SpartaDungeon
{
    public enum Color
    {
        Black,
        DarkBlue,
        DarkGreen,
        DarkCyan,
        DarkRed,
        DarkMagenta,
        DarkYellow,
        Gray,
        DarkGray,
        Blue,
        Green,
        Cyan,
        Red,
        Magenta,
        Yellow,
        White,
    }

    public class ColorFont
    {
        public static void Write(string text, Color color)
        {
            switch (color)
            {
                case Color.Black:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;

                case Color.DarkBlue:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;

                case Color.DarkGreen:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;

                case Color.DarkCyan:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;

                case Color.DarkRed:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case Color.DarkMagenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case Color.DarkYellow:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case Color.Gray:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case Color.DarkGray:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case Color.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case Color.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case Color.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case Color.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case Color.Magenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case Color.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case Color.White:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                default:
                    break;
            }

            Console.Write(text);
            Console.ResetColor();
        }
    }
}
