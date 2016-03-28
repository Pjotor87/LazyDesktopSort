using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LazyDesktopSort
{
    public partial class LazyDesktopSort : Form
    {
        #region AppConfig

        public static string[] appConfigKeys
        {
            get
            {
                return new string[]
                {
                    "desktoppath",
                    "folders",
                    "files",
                    "shortcuts",
                    "ignorefolders",
                    "ignorefiles",
                    "ignoreshortcuts"
                };
            }
        }

        private void GetConfigValues()
        {
            this.textBoxDesktopPath.Text = GetAppConfigValue(appConfigKeys[0]);
            this.textBoxFolders.Text = GetAppConfigValue(appConfigKeys[1]);
            this.textBoxFiles.Text = GetAppConfigValue(appConfigKeys[2]);
            this.textBoxShortcuts.Text = GetAppConfigValue(appConfigKeys[3]);
            this.textBoxIgnoreFolders.Text = GetAppConfigValue(appConfigKeys[4]);
            this.textBoxIgnoreFiles.Text = GetAppConfigValue(appConfigKeys[5]);
            this.textBoxIgnoreShortcuts.Text = GetAppConfigValue(appConfigKeys[6]);
        }

        private void SaveConfigValues()
        {
            SaveConfigValue(appConfigKeys[0], this.textBoxDesktopPath.Text);
            SaveConfigValue(appConfigKeys[1], this.textBoxFolders.Text);
            SaveConfigValue(appConfigKeys[2], this.textBoxFiles.Text);
            SaveConfigValue(appConfigKeys[3], this.textBoxShortcuts.Text);
            SaveConfigValue(appConfigKeys[4], this.textBoxIgnoreFolders.Text);
            SaveConfigValue(appConfigKeys[5], this.textBoxIgnoreFiles.Text);
            SaveConfigValue(appConfigKeys[6], this.textBoxIgnoreShortcuts.Text);
        }

        private string GetAppConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private void SaveConfigValue(string key, string value)
        {
            ConfigurationManager.AppSettings[key] = value;
        }

        #endregion
        
        public LazyDesktopSort()
        {
            InitializeComponent();
            GetConfigValues();
        }
        
        private void buttonBrowseDesktopPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
                this.textBoxDesktopPath.Text = this.folderBrowserDialog.SelectedPath;
        }

        private void buttonSaveAndSort_Click(object sender, EventArgs e)
        {
            SortDesktop(
                this.textBoxDesktopPath.Text,
                this.textBoxFolders.Text,
                this.textBoxFiles.Text,
                this.textBoxShortcuts.Text,
                this.textBoxIgnoreFolders.Text,
                this.textBoxIgnoreFiles.Text,
                this.textBoxIgnoreShortcuts.Text
            );
        }

        public void SortDesktop(
            string desktopPath,
            string foldersFolderName,
            string filesFolderName,
            string shortcutsFolderName,
            string ignoreFolders,
            string ignoreFiles,
            string ignoreShortcuts)
        {
            string foldersPath = this.GetTargetFolder(foldersFolderName, desktopPath);
            string filesPath = this.GetTargetFolder(filesFolderName, desktopPath);
            string shortcutsPath = this.GetTargetFolder(shortcutsFolderName, desktopPath);
            EnsureTargetDirectories(foldersPath, filesPath, shortcutsPath);

            ICollection<string> foldersToIgnore = new List<string>();
            foldersToIgnore.Add(filesFolderName);
            foldersToIgnore.Add(shortcutsFolderName);
            foldersToIgnore.Add(foldersFolderName);
            if (!string.IsNullOrEmpty(ignoreFolders))
                if (ignoreFolders.Contains('|'))
                    foreach (string folder in ignoreFolders.Split('|'))
                        foldersToIgnore.Add(folder);
                else
                    foldersToIgnore.Add(ignoreFolders);
            
            ICollection<string> filesToIgnore = !string.IsNullOrEmpty(ignoreFiles) ? ignoreFiles.Contains('|') ? ignoreFiles.Split('|') : new string[] { ignoreFiles } : null;
            ICollection<string> shortcutsToIgnore = !string.IsNullOrEmpty(ignoreShortcuts) ? ignoreShortcuts.Contains('|') ? ignoreShortcuts.Split('|') : new string[] { ignoreShortcuts } : null;

            IEnumerable<string> directories = Directory.GetDirectories(desktopPath);
            IEnumerable<string> files = Directory.GetFiles(desktopPath);
            IEnumerable<string> shortcuts = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));

            if (directories != null)
                foreach (string directory in directories)
                {
                    string directoryName = Path.GetFileName(directory);
                    if (!foldersToIgnore.Contains(directoryName))
                        Directory.Move(directory, GetTargetPath(foldersFolderName, desktopPath, directory));
                }

            if (shortcuts != null)
                foreach (string shortcut in shortcuts)
                    if (shortcutsToIgnore != null && !shortcutsToIgnore.Contains(shortcut))
                        MoveFile(shortcut, GetTargetPath(shortcutsFolderName, desktopPath, shortcut));

            if (files != null)
                foreach (string file in files)
                    if (IsLink(file))
                    {
                        if (shortcutsToIgnore == null || 
                            (
                            shortcutsToIgnore != null && 
                                (
                                !shortcutsToIgnore.Contains(Path.GetFileName(file))) && 
                                !shortcutsToIgnore.Contains(Path.GetFileNameWithoutExtension(file))
                                )
                            )
                            MoveFile(file, GetTargetPath(shortcutsFolderName, desktopPath, file));
                    }
                    else
                    {
                        if (filesToIgnore == null || 
                            !filesToIgnore.Contains(Path.GetFileName(file), StringComparer.InvariantCultureIgnoreCase))
                            MoveFile(file, GetTargetPath(filesFolderName, desktopPath, file));
                    }

            SaveConfigValues();
            MinimizeAllWindowsAndExitApplication();
        }

        private void MoveFile(string currentPath, string newPath)
        {
            if (!File.Exists(newPath))
                File.Move(currentPath, newPath);
            else
                File.Move(currentPath, GenerateNewFileNameSuffix(newPath));
        }

        private string GenerateNewFileNameSuffix(string newPath)
        {
            StringBuilder newPathBuilder = new StringBuilder();
            newPathBuilder.Append(newPath.Remove(newPath.LastIndexOf(Path.GetFileName(newPath))));

            string filename = Path.GetFileNameWithoutExtension(newPath);
            if (filename.Length >= 3)
            {
                char lastChar = filename[filename.Length - 1];
                char secondLastChar = filename[filename.Length - 2];

                if (secondLastChar == '_' && char.IsNumber(lastChar))
                    if (lastChar != '9')
                        newPathBuilder.Append(string.Concat(filename.Remove(filename.Length - 1), lastChar++, Path.GetExtension(newPath)));
                    else
                        throw new NotImplementedException("Code can't handle two digits yet");
                else
                    newPathBuilder.Append(string.Concat(filename, '_', '0', Path.GetExtension(newPath)));
            }
            else
                newPathBuilder.Append(string.Concat(filename, '_', '0', Path.GetExtension(newPath)));

            return newPathBuilder.ToString();
        }

        private static void MinimizeAllWindowsAndExitApplication()
        {
            Type typeShell = Type.GetTypeFromProgID("Shell.Application");
            typeShell.InvokeMember(
                "MinimizeAll", 
                System.Reflection.BindingFlags.InvokeMethod, 
                null, 
                Activator.CreateInstance(typeShell), 
                null
            ); // Call function MinimizeAll
            Application.Exit();
        }

        public bool IsLink(string shortcut)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcut);
            string filenameOnly = System.IO.Path.GetFileName(shortcut);

            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder folder = shell.NameSpace(pathOnly);
            Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                return folderItem.IsLink;
            }
            return false; // not found
        }

        #region EnsureTargetDirectories

        private void EnsureTargetDirectories(string foldersFolderName, string filesFolderName, string shortcutsFolderName)
        {
            EnsureTargetDirectory(foldersFolderName);
            EnsureTargetDirectory(filesFolderName);
            EnsureTargetDirectory(shortcutsFolderName);
        }

        private void EnsureTargetDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        #endregion

        #region GetPaths

        private string GetTargetPath(string targetFolderName, string desktopPath, string item)
        {
            return 
                string.Concat(
                    this.GetTargetFolder(targetFolderName, desktopPath),
                    Path.DirectorySeparatorChar,
                    Path.GetFileName(item)
                );
        }

        private string GetTargetFolder(string targetFolderName, string desktopPath)
        {
            return
                string.Concat(
                    desktopPath.TrimEnd(Path.DirectorySeparatorChar),
                    Path.DirectorySeparatorChar,
                    targetFolderName
                );
        }

        #endregion
    }
}
