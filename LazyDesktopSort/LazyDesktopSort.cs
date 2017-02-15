using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LazyDesktopSort
{
    public partial class LazyDesktopSort : Form
    {
        public IDictionary<string, TextBox> StoredTextboxValues { get; set; }
        public Configuration Config { get; set; }

        public enum ItemType
        {
            Directory,
            File,
            Shortcut
        }

        public LazyDesktopSort()
        {
            InitializeComponent();
            {// MAP APP CONFIG KEYS TO TEXTBOXES
                this.StoredTextboxValues = new Dictionary<string, TextBox>
                {
                    { "desktoppath", this.textBoxDesktopPath },
                    { "folders", this.textBoxFolders },
                    { "files", this.textBoxFiles },
                    { "shortcuts", this.textBoxShortcuts },
                    { "ignorefolders", this.textBoxIgnoreFolders },
                    { "ignorefiles", this.textBoxIgnoreFiles },
                    { "ignoreshortcuts", this.textBoxIgnoreShortcuts }
                };
                // Load web.config
                this.Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // Populate textboxes with app.config values
                foreach (var storedTextboxValue in this.StoredTextboxValues)
                    storedTextboxValue.Value.Text = this.Config.AppSettings.Settings[storedTextboxValue.Key].Value;
            }
        }

        #region Events

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

        #endregion
        
        public void SortDesktop(string desktopPath, string directoriesFolderName, string filesFolderName,
            string shortcutsFolderName, string ignoreDirectories, string ignoreFiles, string ignoreShortcuts)
        {
            {// ENSURE TARGET FOLDERS
                List<string> targetFolders =
                    new List<string>
                    {
                        Path.Combine(desktopPath, directoriesFolderName),
                        Path.Combine(desktopPath, filesFolderName),
                        Path.Combine(desktopPath, shortcutsFolderName)
                    };
                targetFolders.ForEach(targetFolder => {
                    if (!Directory.Exists(targetFolder))
                        Directory.CreateDirectory(targetFolder);
                });
            }

            //{// MOVE ITEMS
            //    var itemsToIgnore = new Dictionary<ItemType, ICollection<string>>();
            //    {// POPULATE ITEMS TO IGNORE
            //        // Populate collections
            //        itemsToIgnore.Add(ItemType.Directory, new List<string>());
            //        itemsToIgnore.Add(ItemType.Shortcut, new List<string>());
            //        itemsToIgnore.Add(ItemType.File, new List<string>());
            //        // Populate base directories to ignore with the target folders
            //        itemsToIgnore[ItemType.Directory].Add(filesFolderName);
            //        itemsToIgnore[ItemType.Directory].Add(shortcutsFolderName);
            //        itemsToIgnore[ItemType.Directory].Add(directoriesFolderName);
            //        // Populate itemsToIgnore with entries from app.config
            //        foreach (var itemsToIgnoreCollection in itemsToIgnore)
            //        {
            //            if (itemsToIgnoreCollection.Key == ItemType.Directory)
            //            {
            //                // Get items from app.config
            //                string[] directoriesToIgnore = !string.IsNullOrEmpty(ignoreDirectories) ? ignoreDirectories.Contains('|') ? ignoreDirectories.Split('|') : new string[] { ignoreDirectories } : null;
            //                if (directoriesToIgnore != null && directoriesToIgnore.Length > 0)
            //                    foreach (string directoryToIgnore in directoriesToIgnore)
            //                        if (!itemsToIgnoreCollection.Value.Contains(directoryToIgnore))
            //                            itemsToIgnoreCollection.Value.Add(directoryToIgnore);
            //            }
            //            else if (itemsToIgnoreCollection.Key == ItemType.Shortcut)
            //            {
            //                // Get items from app.config
            //                string[] shortcutsToIgnore = !string.IsNullOrEmpty(ignoreShortcuts) ? ignoreShortcuts.Contains('|') ? ignoreShortcuts.Split('|') : new string[] { ignoreShortcuts } : null;
            //                if (shortcutsToIgnore != null && shortcutsToIgnore.Length > 0)
            //                    foreach (string shortcutToIgnore in shortcutsToIgnore)
            //                        if (!itemsToIgnoreCollection.Value.Contains(shortcutToIgnore))
            //                            itemsToIgnoreCollection.Value.Add(shortcutToIgnore);
            //            }
            //            else if (itemsToIgnoreCollection.Key == ItemType.File)
            //            {
            //                // Get items from app.config
            //                string[] filesToIgnore = !string.IsNullOrEmpty(ignoreFiles) ? ignoreFiles.Contains('|') ? ignoreFiles.Split('|') : new string[] { ignoreFiles } : null;
            //                if (filesToIgnore != null && filesToIgnore.Length > 0)
            //                    foreach (string fileToIgnore in filesToIgnore)
            //                        if (!itemsToIgnoreCollection.Value.Contains(fileToIgnore))
            //                            itemsToIgnoreCollection.Value.Add(fileToIgnore);
            //            }
            //        }
            //    }
            //    {// MOVE DIRECTORIES
            //        IEnumerable<string> directories = Directory.GetDirectories(desktopPath);
            //        if (directories != null)
            //            foreach (string directory in directories)
            //                if (!itemsToIgnore[ItemType.Directory].Contains(Path.GetFileName(directory)))
            //                    Directory.Move(directory, Path.Combine(Path.Combine(desktopPath, directoriesFolderName), Path.GetFileName(directory)));
            //    }
            //    {// MOVE SHORTCUTS FROM SPECIAL FOLDER
            //        IEnumerable<string> shortcuts = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
            //        if (shortcuts != null)
            //            foreach (string shortcut in shortcuts)
            //                if (itemsToIgnore[ItemType.Shortcut] != null && !itemsToIgnore[ItemType.Shortcut].Contains(shortcut))
            //                    MoveFile(shortcut, Path.Combine(Path.Combine(desktopPath, shortcutsFolderName), Path.GetFileName(shortcut)));
            //    }
            //    {// MOVE FILES AND SHORTCUTS FROM USER DESKTOP PATH
            //        IEnumerable<string> files = Directory.GetFiles(desktopPath);
            //        if (files != null)
            //            foreach (string file in files)
            //                if (IsShortcut(file))
            //                {
            //                    if (itemsToIgnore[ItemType.Shortcut] == null ||
            //                        (itemsToIgnore[ItemType.Shortcut] != null &&
            //                            (!itemsToIgnore[ItemType.Shortcut].Contains(Path.GetFileName(file))) &&
            //                            !itemsToIgnore[ItemType.Shortcut].Contains(Path.GetFileNameWithoutExtension(file))
            //                        ))
            //                        MoveFile(file, Path.Combine(Path.Combine(desktopPath, shortcutsFolderName), Path.GetFileName(file)));
            //                }
            //                else if (itemsToIgnore[ItemType.File] == null ||
            //                    (itemsToIgnore[ItemType.File] != null &&
            //                        !Path.GetFileName(file).StartsWith("~$") && // ignore temp files
            //                        !itemsToIgnore[ItemType.File].Contains(Path.GetFileName(file))
            //                    ))
            //                    MoveFile(file, Path.Combine(Path.Combine(desktopPath, filesFolderName), Path.GetFileName(file)));
            //    }
            //}

            {// SAVE AND EXIT
                {// SAVE VALUES TO APP.CONFIG
                    // Set values in memory
                    bool changesMadeAndShouldSave = false;
                    foreach (var storedTextboxValue in this.StoredTextboxValues)
                        if (this.Config.AppSettings.Settings[storedTextboxValue.Key].Value != storedTextboxValue.Value.Text)
                        {
                            this.Config.AppSettings.Settings[storedTextboxValue.Key].Value = storedTextboxValue.Value.Text;
                            changesMadeAndShouldSave = true;
                        }
                    // Save changes to app.config
                    if (changesMadeAndShouldSave)
                    {
                        this.Config.Save(ConfigurationSaveMode.Full);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                }
                {// MINIMIZE ALL WINDOWS TO REVEAL DESKTOP AND EXIT APPLICATION
                    Type typeShell = Type.GetTypeFromProgID("Shell.Application");
                    typeShell.InvokeMember("MinimizeAll", System.Reflection.BindingFlags.InvokeMethod, null, Activator.CreateInstance(typeShell), null);
                    Application.Exit();
                }
            }
        }

        #region Private

        private void MoveFile(string currentPath, string newPath)
        {
            File.Move(
                currentPath, 
                (!File.Exists(newPath) ? 
                    newPath : 
                    GenerateNewFileNameSuffix(newPath)
                )
            );
        }

        private bool IsShortcut(string shortcut)
        {
            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder folder = shell.NameSpace(Path.GetDirectoryName(shortcut));
            Shell32.FolderItem folderItem = folder.ParseName(Path.GetFileName(shortcut));
            return
                (folderItem != null) ?
                folderItem.IsLink :
                false; // not found
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

        #endregion
    }
}