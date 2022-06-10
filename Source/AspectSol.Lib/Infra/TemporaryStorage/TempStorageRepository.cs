using System.Text;

namespace AspectSol.Lib.Infra.TemporaryStorage;

public class TempStorageRepository
{
    private const string Directory = "tmp/";
    
    public static void Add(string content, out Guid filename)
    {
        if (!System.IO.Directory.Exists(Directory))
        {
            System.IO.Directory.CreateDirectory(Directory);
        }

        filename = Guid.NewGuid();
        var filepath = ConstructFilepath(filename);
        
        using (FileStream fs = File.Create(filepath))
        {
            var info = new UTF8Encoding(true).GetBytes(content);
            fs.Write(info, 0, info.Length);
        }
    }

    public static void Delete(Guid filename)
    {
        var filepath = ConstructFilepath(filename);
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
    }

    private static string ConstructFilepath(Guid filename)
    {
        return $"{Directory}{filename}.txt";
    }
}