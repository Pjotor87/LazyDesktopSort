using LazyDesktopSort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class FolderStructureTests
    {
        [TestMethod]
        public void FindByKeywords()
        {
            FolderStructure fs = new FolderStructure("foldername1;keyword1,keyword2;%r|foldername2>subfoldername1>subfoldername2;keyword3,keyword4|foldername3;keyword5,keyword6");
            string filename1 = "aaakeyword2aaakeyword3aaakeyword1aaakeyword1keyword1.exe";
            try
            {
                string folderPath1 = fs.FindPathToPrependToFilename(filename1);
                Assert.Fail("Code should not reach this path");
            }
            catch (Exception) { }

            string filename2 = "aaakeyword2aaakeyword1aaakeyword1keyword1.exe";
            string folderPath2 = fs.FindPathToPrependToFilename(filename2);

            string expected = "foldername1";
            Assert.AreEqual(expected, folderPath2);
        }

        [TestMethod]
        public void RenamingOfFilenameWorksAsIntended()
        {
            FolderStructure fs = new FolderStructure("foldername1;keyword1,keyword2;%r|foldername2>subfoldername1>subfoldername2;keyword3,keyword4|foldername3;keyword5,keyword6");
            string filename = "aaakeyword2aaakeyword1aaakeyword1keyword1.exe";
            string folderPath = fs.FindPathToPrependToFilename(filename);
            string actual = fs.RemoveKeywords(filename, folderPath);
            string expected = "aaaaaaaaa.exe";
            Assert.AreEqual(expected, actual);
        }
    }
}
