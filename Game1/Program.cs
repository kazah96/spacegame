using System;

namespace Game1
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Dsadasdsa");
            Microsoft.Xna.Framework.GameRunBehavior f;
            f = Microsoft.Xna.Framework.GameRunBehavior.Synchronous;
            
            using (var game = new Game1())  
                game.Run(f);
                
        }
    }
#endif
}
