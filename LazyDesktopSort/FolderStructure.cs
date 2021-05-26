using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyDesktopSort
{
    public class FolderStructure
    {
        public IDictionary<string, string[]> FolderPathsAndKeyWords { get; set; } = new Dictionary<string, string[]>();
        public bool RemoveKeywordsWhenMovingFile { get; set; }

        public FolderStructure(string inputString)
        {
            // foldername;keyword1,keyword2;%r|foldername>subfoldername>subfoldername;keyword1,keyword2|foldername;keyword1,keyword2

            if (!string.IsNullOrEmpty(inputString))
            {
                string[] rootFolders = inputString.Contains('|') ? inputString.Split('|') : new string[] { inputString };
                foreach (string rootFolder in rootFolders)
                {
                    string[] folderParts = rootFolder.Split(';');

                    string folderName = string.Empty;
                    
                    if (!rootFolder.Contains('>'))
                    {
                        folderName = folderParts[0];
                    }
                    else
                    {
                        string[] folders = folderParts[0].Split('>');
                        StringBuilder folderPathBuilder = new StringBuilder();
                        foreach (string folder in folders)
                        {
                            folderPathBuilder.Append(folder);
                            folderPathBuilder.Append('\\');
                        }
                        folderName = folderPathBuilder.ToString().TrimEnd('\\');
                    }

                    string[] keyWords = folderParts[1].Split(',');

                    if (folderParts.Length >= 3)
                    {
                        if (folderParts[2].Contains('%'))
                        {
                            string[] parameters = folderParts[2].Split('%');
                            foreach (string parameter in parameters)
                            {
                                string loweredParameter = parameter.ToLower();
                                switch (loweredParameter)
                                {
                                    case "r": // Remove keywords
                                        this.RemoveKeywordsWhenMovingFile = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        this.RemoveKeywordsWhenMovingFile = false;
                    }

                    FolderPathsAndKeyWords.Add(folderName, keyWords);
                }
            }
        }

        public string FindPathToPrependToFilename(string filename)
        {
            IList<string> matches = new List<string>();

            if (this.FolderPathsAndKeyWords.Count > 0)
                foreach (var item in this.FolderPathsAndKeyWords)
                    foreach (string keyword in item.Value)
                        if (filename.Contains(keyword))
                        {
                            matches.Add(item.Key);
                            break;
                        }

            if (matches.Count >= 2)
            {
                throw new Exception("keyword matches was not unique");
            }
            else if (matches.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return matches[0];
            }
        }

        public string RemoveKeywords(string filename, string folderPath)
        {
            string newFilename = filename;

            if (this.RemoveKeywordsWhenMovingFile)
            {
                foreach (string keyWord in this.FolderPathsAndKeyWords[folderPath])
                    newFilename = newFilename.Replace(keyWord, "");
            }
            
            return newFilename;
        }
    }
}
