using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace gayshit
{
    public class MapCreator
    {
        private static readonly Dictionary<string, Func<IGameObject>> factory = new Dictionary<string, Func<IGameObject>>();

        public static IGameObject[,] CreateMap(string map)
        {
            var os = Environment.OSVersion.Platform;
            string[] rows = null;
            if (os is PlatformID.Unix)
            {
                var separator = '\n';
                rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                var separator = "\n";
                rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong test map '{map}'");
            var result = new IGameObject[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                    result[x, y] = CreateCreatureBySymbol(rows[y][x]);
            return result;
        }

        private static IGameObject CreateCreatureByTypeName(string name)
        {
            if (!factory.ContainsKey(name))
            {
                var type = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(z => z.Name == name);
                if (type == null)
                    throw new Exception($"Can't find type '{name}'");
                factory[name] = () => (IGameObject)Activator.CreateInstance(type);
            }

            return factory[name]();
        }


        private static IGameObject CreateCreatureBySymbol(char c)
        {
            switch (c)
            {
                case 'P':
                    return CreateCreatureByTypeName("Player");
                case 'T':
                    return CreateCreatureByTypeName("Terrain");
                case 'E':
                    return CreateCreatureByTypeName("Enemy");
                case ' ':
                    return null;
                case 'B':
                    return CreateCreatureByTypeName("Bullet");
                case 'Z':
                    return CreateCreatureByTypeName("LevelBoss");
                default:
                    throw new Exception($"wrong character for IGameObject {c}");
            }
        }
    }
}
