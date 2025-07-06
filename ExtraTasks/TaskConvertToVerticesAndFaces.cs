using System.Globalization;
using System.Text;

namespace ExtraTasks
{
    public static class TaskConvertToVerticesAndFaces
    {
        public static void Run(string inputPath)
        {
            string outputVertices = "vertices.txt";
            string outputFaces = "faces.txt";

            var vertices = new List<double[]>();
            var faces = new List<int[]>();

            foreach (var line in File.ReadLines(inputPath))
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("v "))
                {
                    var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 4)
                    {
                        vertices.Add(new double[]
                        {
                        double.Parse(parts[1], CultureInfo.InvariantCulture),
                        double.Parse(parts[2], CultureInfo.InvariantCulture),
                        double.Parse(parts[3], CultureInfo.InvariantCulture)
                        });
                    }
                }
                else if (trimmed.StartsWith("f "))
                {
                    var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var face = new int[parts.Length - 1];
                    for (int i = 1; i < parts.Length; i++)
                    {
                        face[i - 1] = int.Parse(parts[i]);
                    }
                    faces.Add(face);
                }
            }

            File.WriteAllText(outputVertices, FormatArray(vertices));
            File.WriteAllText(outputFaces, FormatArray(faces));

            Console.WriteLine("Готово. Файлы сохранены.");
        }
        static string FormatArray<T>(List<T[]> data)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");
            for (int i = 0; i < data.Count; i++)
            {
                sb.Append("   [");
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (typeof(T) == typeof(double))
                        sb.Append(((double)(object)data[i][j]).ToString(CultureInfo.InvariantCulture));
                    else
                        sb.Append(data[i][j]);

                    if (j != data[i].Length - 1)
                        sb.Append(", ");
                }
                sb.Append("]");
                if (i != data.Count - 1)
                    sb.Append(",");
                sb.AppendLine();
            }
            sb.Append("];");
            return sb.ToString();
        }

    }
}
