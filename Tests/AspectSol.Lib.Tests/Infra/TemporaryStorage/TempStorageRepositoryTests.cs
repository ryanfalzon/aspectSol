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
    public void GivenATemporaryStorageRepository_WhenCallingAdd_ShouldCreateNewFile(string content)
    {
        // Given
        var givenTempStorageRepository = new TempStorageRepository();
        
        // When
        givenTempStorageRepository.Add(content, out var filepath);
        
        // Should
        Assert.IsTrue(File.Exists(filepath));
    }

    [TestMethod]
    public void GivenTempFile_WhenCallingDelete_ShouldRemoveFile()
    {
        // Given
        var givenTempStorageRepository = new TempStorageRepository();
        givenTempStorageRepository.Add("Some Content", out var filepath);
        
        // When
        givenTempStorageRepository.Delete(filepath);
        
        // Should
        Assert.IsFalse(File.Exists(filepath));
    }
}