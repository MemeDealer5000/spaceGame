using System;
using System.Windows.Forms;

namespace gayshit
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            GameMap.PrepareMaps();
            GameMap.CreateMap();
            Application.Run(new StartMenu());
        }
    }
}
