using AspectSol.Compiler.Domain;
using System.Collections.Generic;
using System.IO;

namespace AspectSol.Compiler.Processors
{
    public class Tokenizer : ITokenizer
    {
        public List<string> Start(string filepath)
        {
            using var textReader = File.OpenText(filepath);
            while(textReader.Peek() != -1)
            {
                var ch = (char)textReader.Peek();

                if (char.IsWhiteSpace(ch))
                {
                    textReader.Read();
                }
                else if()
            }
        }
    }
}