﻿using Microsoft.VisualBasic.FileIO;
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
                    { "ignoreshortcuts", this.textBoxIgnoreShortcuts },
                    { "folderStructure", this.textBoxFolderStructure }
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
                this.textBoxIgnoreShortcuts.Text,
                this.textBoxFolderStructure.Text
            );
        }

        #endregion

        public void SortDesktop(string desktopPath, string directoriesFolderName, string filesFolderName,
            string shortcutsFolderName, string ignoreDirectories, string ignoreFiles, string ignoreShortcuts, string folderStructureForFiles)
        {
            ErrorHandler errorHandler = new ErrorHandler();

            List<string> targetFolders =
                new List<string>
                {
                    Path.Combine(desktopPath, directoriesFolderName),
                    Path.Combine(desktopPath, filesFolderName),
                    Path.Combine(desktopPath, shortcutsFolderName)
                };

            {// ENSURE TARGET FOLDERS
                targetFolders.ForEach(targetFolder =>
                {
                    if (!Directory.Exists(targetFolder))
                        Directory.CreateDirectory(targetFolder);
                });
            }

            {// MOVE ITEMS
                var folderStructure = new FolderStructure(folderStructureForFiles);
                var itemsToIgnore = new Dictionary<ItemType, ICollection<string>>();
                {// POPULATE ITEMS TO IGNORE
                    // Populate collections
                    itemsToIgnore.Add(ItemType.Directory, new List<string>());
                    itemsToIgnore.Add(ItemType.Shortcut, new List<string>());
                    itemsToIgnore.Add(ItemType.File, new List<string>());
                    // Populate base directories to ignore with the target folders
                    itemsToIgnore[ItemType.Directory].Add(filesFolderName);
                    itemsToIgnore[ItemType.Directory].Add(shortcutsFolderName);
                    itemsToIgnore[ItemType.Directory].Add(directoriesFolderName);
                    // Populate itemsToIgnore with entries from app.config
                    foreach (var itemsToIgnoreCollection in itemsToIgnore)
                    {
                        if (itemsToIgnoreCollection.Key == ItemType.Directory)
                        {
                            // Get items from app.config
                            string[] directoriesToIgnore = !string.IsNullOrEmpty(ignoreDirectories) ? ignoreDirectories.Contains('|') ? ignoreDirectories.Split('|') : new string[] { ignoreDirectories } : null;
                            if (directoriesToIgnore != null && directoriesToIgnore.Length > 0)
                                foreach (string directoryToIgnore in directoriesToIgnore)
                                    if (!itemsToIgnoreCollection.Value.Contains(directoryToIgnore))
                                        itemsToIgnoreCollection.Value.Add(directoryToIgnore);
                        }
                        else if (itemsToIgnoreCollection.Key == ItemType.Shortcut)
                        {
                            // Get items from app.config
                            string[] shortcutsToIgnore = !string.IsNullOrEmpty(ignoreShortcuts) ? ignoreShortcuts.Contains('|') ? ignoreShortcuts.Split('|') : new string[] { ignoreShortcuts } : null;
                            if (shortcutsToIgnore != null && shortcutsToIgnore.Length > 0)
                                foreach (string shortcutToIgnore in shortcutsToIgnore)
                                    if (!itemsToIgnoreCollection.Value.Contains(shortcutToIgnore))
                                        itemsToIgnoreCollection.Value.Add(shortcutToIgnore);
                        }
                        else if (itemsToIgnoreCollection.Key == ItemType.File)
                        {
                            // Get items from app.config
                            string[] filesToIgnore = !string.IsNullOrEmpty(ignoreFiles) ? ignoreFiles.Contains('|') ? ignoreFiles.Split('|') : new string[] { ignoreFiles } : null;
                            if (filesToIgnore != null && filesToIgnore.Length > 0)
                                foreach (string fileToIgnore in filesToIgnore)
                                    if (!itemsToIgnoreCollection.Value.Contains(fileToIgnore))
                                        itemsToIgnoreCollection.Value.Add(fileToIgnore);
                        }
                    }
                }
                {// MOVE DIRECTORIES
                    IEnumerable<string> directories = Directory.GetDirectories(desktopPath);
                    if (directories != null)
                        foreach (string directory in directories)
                            if (!itemsToIgnore[ItemType.Directory].Contains(Path.GetFileName(directory)))
                                errorHandler.Add(MoveDirectory(directory, desktopPath, directoriesFolderName));
                }
                {// MOVE SHORTCUTS FROM SPECIAL FOLDER
                    IEnumerable<string> shortcuts = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
                    if (shortcuts != null)
                        foreach (string shortcut in shortcuts)
                            if (itemsToIgnore[ItemType.Shortcut] != null && !itemsToIgnore[ItemType.Shortcut].Contains(shortcut))
                                errorHandler.Add(MoveFile(shortcut, Path.Combine(Path.Combine(desktopPath, shortcutsFolderName), Path.GetFileName(shortcut)), true));
                }
                {// MOVE FILES AND SHORTCUTS FROM USER DESKTOP PATH
                    IEnumerable<string> files = Directory.GetFiles(desktopPath);
                    if (files != null)
                        foreach (string file in files)
                            if (IsShortcut(file))
                            {
                                if (itemsToIgnore[ItemType.Shortcut] == null ||
                                    (itemsToIgnore[ItemType.Shortcut] != null &&
                                        (!itemsToIgnore[ItemType.Shortcut].Contains(Path.GetFileName(file))) &&
                                        !itemsToIgnore[ItemType.Shortcut].Contains(Path.GetFileNameWithoutExtension(file))
                                    ))
                                {
                                    string newPath = Path.Combine(Path.Combine(desktopPath, shortcutsFolderName), Path.GetFileName(file));
                                    errorHandler.Add(MoveFile(file, newPath, true));
                                }
                            }
                            else if (itemsToIgnore[ItemType.File] == null ||
                                (itemsToIgnore[ItemType.File] != null &&
                                    !Path.GetFileName(file).StartsWith("~$") && // ignore temp files
                                    !itemsToIgnore[ItemType.File].Contains(Path.GetFileName(file))
                                ))
                            {
                                string newPath = string.Empty;

                                string filesPath = Path.Combine(desktopPath, filesFolderName);
                                string filename = Path.GetFileName(file);

                                string folderStructurePath = folderStructure.FindPathToPrependToFilename(Path.GetFileNameWithoutExtension(file));
                                if (!string.IsNullOrEmpty(folderStructurePath))
                                {
                                    string[] folderStructureFolders = folderStructurePath.Split('\\');
                                    StringBuilder folderStructurefolderBuilder = new StringBuilder();
                                    folderStructurefolderBuilder.Append(filesPath);
                                    folderStructurefolderBuilder.Append('\\');
                                    foreach (string folderStructureFolder in folderStructureFolders)
                                    {
                                        folderStructurefolderBuilder.Append(folderStructureFolder);
                                        if (!Directory.Exists(folderStructurefolderBuilder.ToString()))
                                            Directory.CreateDirectory(folderStructurefolderBuilder.ToString());
                                        folderStructurefolderBuilder.Append('\\');
                                    }

                                    newPath = Path.Combine(filesPath, folderStructurePath, folderStructure.RemoveKeywords(filename, folderStructurePath));
                                }
                                else
                                {
                                    newPath = Path.Combine(filesPath, filename);
                                }

                                errorHandler.Add(MoveFile(file, newPath));
                            }
                }
            }     

            {// REMOVE EMPTY TARGET FOLDERS
                targetFolders.ForEach(targetFolder =>
                {
                    if (Directory.Exists(targetFolder) &&
                        !Directory.EnumerateFileSystemEntries(targetFolder).Any())
                        Directory.Delete(targetFolder);
                });
            }

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

            {// TERMINATE APPLICATION
                {// Halt and display errors
                    errorHandler.DisplayErrors();
                }

                {// Minimize all windows to reveal desktop
                    Type typeShell = Type.GetTypeFromProgID("Shell.Application");
                    typeShell.InvokeMember("MinimizeAll", System.Reflection.BindingFlags.InvokeMethod, null, Activator.CreateInstance(typeShell), null);
                }

                {// Exit application
                    Application.Exit();
                }
            }
        }

        #region Private

        private string MoveDirectory(string directory, string desktopPath, string directoriesFolderName)
        {
            try
            {
                Directory.Move(
                    directory,
                    Path.Combine(
                        Path.Combine(
                            desktopPath,
                            directoriesFolderName),
                        Path.GetFileName(
                            directory
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                return 
                    string.Format("Directory: {0}{1}DesktopPath: {2}{3} DirecoriesFolderName: {4}{5}{6}", 
                    directory, Environment.NewLine,
                    desktopPath, Environment.NewLine,
                    directoriesFolderName, Environment.NewLine,
                    ex.ToString());
            }
            return string.Empty;
        }

        private string MoveFile(string currentPath, string newPath, bool overwrite = false)
        {
            try
            {
                if (!File.Exists(newPath))
                {
                    File.Move(currentPath, newPath);
                }
                else if (!overwrite)
                {
                    File.Move(currentPath, GenerateNewFileNameSuffix(newPath));
                }
                else
                {
                    FileSystem.MoveFile(currentPath, newPath, true);
                }
            }
            catch (Exception ex)
            {
                return
                    string.Format("CurrentPath: {0}{1}NewPath: {2}{3}{4}",
                    currentPath, Environment.NewLine,
                    newPath, Environment.NewLine,
                    ex.ToString());
            }
            return string.Empty;
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