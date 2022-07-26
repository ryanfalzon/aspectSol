using System.Text;

namespace AspectSol.Lib.Infra.TemporaryStorage;

public class TempStorageRepository
{
    private const string Directory = "tmp/";
    
    public void Add(string content, out string filepath)
    {
        if (!System.IO.Directory.Exists(Directory))
        {
            System.IO.Directory.CreateDirectory(Directory);
        }

        var filename = Guid.NewGuid();
        filepath = ConstructFilepath(filename);
        
        using (FileStream fs = File.Create(filepath))
        {
            var info = new UTF8Encoding(true).GetBytes(content);
            fs.Write(info, 0, info.Length);
        }
    }

    public void Delete(Guid filename)
    {
        var filepath = ConstructFilepath(filename);
        Delete(filepath);
    }

    public void Delete(string filepath)
    {
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
    }

    private string ConstructFilepath(Guid filename)
    {
        return $"{Directory}{filename}.txt";
    }
}