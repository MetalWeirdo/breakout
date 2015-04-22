using System;

namespace BreakOut
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BreakOut game = new BreakOut())
            {
                game.Run();
            }
        }
    }
#endif
}

