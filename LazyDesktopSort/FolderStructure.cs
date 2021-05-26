using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyDesktopSort
{
    public class FolderStructure
    {
        public IList<MoveDirective> MoveDirectives { get; set; } = new List<MoveDirective>();

        public FolderStructure(string inputString)
        {
            // foldername;keyword1,keyword2;%r|foldername>subfoldername>subfoldername;keyword1,keyword2|foldername;keyword1,keyword2

            if (!string.IsNullOrEmpty(inputString))
            {
                string[] rootFolders = inputString.Contains('|') ? inputString.Split('|') : new string[] { inputString };
                foreach (string rootFolder in rootFolders)
                {
                    MoveDirective moveDirective = new MoveDirective();

                    string[] folderParts = rootFolder.Split(';');
                    
                    // Set Foldername
                    if (!rootFolder.Contains('>'))
                    {
                        moveDirective.Foldername = folderParts[0];
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
                        moveDirective.Foldername = folderPathBuilder.ToString().TrimEnd('\\');
                    }

                    moveDirective.Keywords = folderParts[1].Split(',');

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
                                        moveDirective.ReplaceKeywords = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        moveDirective.ReplaceKeywords = false;
                    }

                    if (this.MoveDirectives.Where(x => x.Foldername == moveDirective.Foldername).SingleOrDefault() != null)
                        throw new Exception("Duplicate foldername found in folder structure");
                    
                    MoveDirectives.Add(moveDirective);
                }
            }
        }

        public string FindPathToPrependToFilename(string filename)
        {
            IList<string> matches = new List<string>();

            if (this.MoveDirectives.Count > 0)
                foreach (var item in this.MoveDirectives)
                    foreach (string keyword in item.Keywords)
                        if (filename.Contains(keyword))
                        {
                            matches.Add(item.Foldername);
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

            MoveDirective moveDirective = this.MoveDirectives.Where(x => x.Foldername == folderPath).SingleOrDefault();
            if (moveDirective != null && moveDirective.ReplaceKeywords)
                foreach (string keyWord in moveDirective.Keywords)
                    newFilename = newFilename.Replace(keyWord, "");
            
            return newFilename;
        }
    }
}
