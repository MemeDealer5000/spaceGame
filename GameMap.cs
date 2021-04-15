using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace gayshit
{
    public static class GameMap
    {
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);
        public static int CurrentLevel = 1;
        public static int EnemyCounter = 50;
        public static int Lives = 3;
        public static IGameObject[,] Map;
        public static bool IsGameOver = false;

        public static int Scores;
        public static int RegularScore = 10;
        public static bool IsOver = false;
        public static ObjectCommand PartitialMovement = new ObjectCommand { DeltaX = 1, DeltaY = 0 };

        private static DirectoryInfo mapTextInfo;
        private static Dictionary<string,string> levels = new Dictionary<string, string>();


        public static Keys KeyPressed;

        public static void PrepareMaps()
        {
            mapTextInfo = new DirectoryInfo(@"C:\Users\Семен\Desktop\GayNiggasOuttaSpace-master\Levels");
            foreach (var e in mapTextInfo.GetFiles("*.txt"))
            {
                var stream = e.OpenText();
                levels[e.Name] = stream.ReadToEnd();
                stream.Close();
            }
        }

        public static void CreateMap()
        {
            if (CurrentLevel > 3 || IsGameOver)
            {
                Application.Restart();
            }

            Map = MapCreator.CreateMap(levels[CurrentLevel+".txt"]);
            CurrentLevel++;
        }
    }
}
