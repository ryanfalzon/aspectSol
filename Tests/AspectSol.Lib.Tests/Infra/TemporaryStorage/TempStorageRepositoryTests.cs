using System;
using System.IO;
using AspectSol.Lib.Infra.TemporaryStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspectSol.Lib.Tests.Infra.TemporaryStorage;

[TestClass]
public class TempStorageRepositoryTests
{
    [TestMethod]
    [DataRow("Some test data for testing.")]
    public void WhenCallingAdd_ShouldCreateNewFile(string content)
    {
        // When
        TempStorageRepository.Add(content, out Guid filename);
        
        // Should
        Assert.IsTrue(File.Exists($"tmp/{filename}.txt"));
    }

    [TestMethod]
    public void GivenTempFile_WhenCallingDelete_ShouldRemoveFile()
    {
        // Given
        TempStorageRepository.Add("Some Content", out Guid filename);
        
        // When
        TempStorageRepository.Delete(filename);
        
        // Should
        Assert.IsFalse(File.Exists($"tmp/{filename}.txt"));
    }
}