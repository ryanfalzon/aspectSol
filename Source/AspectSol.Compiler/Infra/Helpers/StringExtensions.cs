using System;
using System.Linq;

namespace AspectSol.Compiler.Infra.Helpers
{
    public sealed class StringExtensions
    {
        #region SINGLETON

        private StringExtensions()
        {
            random = new Random();
        }

        private static readonly Lazy<StringExtensions> stringExtensions = new Lazy<StringExtensions>(() => new StringExtensions());

        public static StringExtensions Instance
        {
            get
            {
                return stringExtensions.Value;
            }
        }

        #endregion

        private readonly Random random;

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}