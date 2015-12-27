namespace LazyDesktopSort
{
    partial class LazyDesktopSort
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.labelDesktopPath = new System.Windows.Forms.Label();
            this.textBoxDesktopPath = new System.Windows.Forms.TextBox();
            this.buttonBrowseDesktopPath = new System.Windows.Forms.Button();
            this.labelFolders = new System.Windows.Forms.Label();
            this.textBoxFolders = new System.Windows.Forms.TextBox();
            this.labelFiles = new System.Windows.Forms.Label();
            this.labelShortcuts = new System.Windows.Forms.Label();
            this.textBoxFiles = new System.Windows.Forms.TextBox();
            this.textBoxShortcuts = new System.Windows.Forms.TextBox();
            this.buttonSaveAndSort = new System.Windows.Forms.Button();
            this.labelIgnoreFolders = new System.Windows.Forms.Label();
            this.textBoxIgnoreFolders = new System.Windows.Forms.TextBox();
            this.labelIgnoreFiles = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIgnoreFiles = new System.Windows.Forms.TextBox();
            this.textBoxIgnoreShortcuts = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelDesktopPath
            // 
            this.labelDesktopPath.AutoSize = true;
            this.labelDesktopPath.Location = new System.Drawing.Point(10, 9);
            this.labelDesktopPath.Name = "labelDesktopPath";
            this.labelDesktopPath.Size = new System.Drawing.Size(74, 13);
            this.labelDesktopPath.TabIndex = 0;
            this.labelDesktopPath.Text = "Desktop path:";
            // 
            // textBoxDesktopPath
            // 
            this.textBoxDesktopPath.Location = new System.Drawing.Point(102, 6);
            this.textBoxDesktopPath.Name = "textBoxDesktopPath";
            this.textBoxDesktopPath.Size = new System.Drawing.Size(211, 20);
            this.textBoxDesktopPath.TabIndex = 1;
            this.textBoxDesktopPath.Text = "C:\\Users\\<username>\\Desktop";
            // 
            // buttonBrowseDesktopPath
            // 
            this.buttonBrowseDesktopPath.Location = new System.Drawing.Point(319, 4);
            this.buttonBrowseDesktopPath.Name = "buttonBrowseDesktopPath";
            this.buttonBrowseDesktopPath.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseDesktopPath.TabIndex = 2;
            this.buttonBrowseDesktopPath.Text = "Browse";
            this.buttonBrowseDesktopPath.UseVisualStyleBackColor = true;
            this.buttonBrowseDesktopPath.Click += new System.EventHandler(this.buttonBrowseDesktopPath_Click);
            // 
            // labelFolders
            // 
            this.labelFolders.AutoSize = true;
            this.labelFolders.Location = new System.Drawing.Point(10, 35);
            this.labelFolders.Name = "labelFolders";
            this.labelFolders.Size = new System.Drawing.Size(44, 13);
            this.labelFolders.TabIndex = 3;
            this.labelFolders.Text = "Folders:";
            // 
            // textBoxFolders
            // 
            this.textBoxFolders.Location = new System.Drawing.Point(102, 32);
            this.textBoxFolders.Name = "textBoxFolders";
            this.textBoxFolders.Size = new System.Drawing.Size(211, 20);
            this.textBoxFolders.TabIndex = 4;
            this.textBoxFolders.Text = "Folders";
            // 
            // labelFiles
            // 
            this.labelFiles.AutoSize = true;
            this.labelFiles.Location = new System.Drawing.Point(10, 61);
            this.labelFiles.Name = "labelFiles";
            this.labelFiles.Size = new System.Drawing.Size(31, 13);
            this.labelFiles.TabIndex = 5;
            this.labelFiles.Text = "Files:";
            // 
            // labelShortcuts
            // 
            this.labelShortcuts.AutoSize = true;
            this.labelShortcuts.Location = new System.Drawing.Point(10, 87);
            this.labelShortcuts.Name = "labelShortcuts";
            this.labelShortcuts.Size = new System.Drawing.Size(55, 13);
            this.labelShortcuts.TabIndex = 6;
            this.labelShortcuts.Text = "Shortcuts:";
            // 
            // textBoxFiles
            // 
            this.textBoxFiles.Location = new System.Drawing.Point(102, 58);
            this.textBoxFiles.Name = "textBoxFiles";
            this.textBoxFiles.Size = new System.Drawing.Size(211, 20);
            this.textBoxFiles.TabIndex = 7;
            this.textBoxFiles.Text = "Files";
            // 
            // textBoxShortcuts
            // 
            this.textBoxShortcuts.Location = new System.Drawing.Point(102, 84);
            this.textBoxShortcuts.Name = "textBoxShortcuts";
            this.textBoxShortcuts.Size = new System.Drawing.Size(211, 20);
            this.textBoxShortcuts.TabIndex = 8;
            this.textBoxShortcuts.Text = "Shortcuts";
            // 
            // buttonSaveAndSort
            // 
            this.buttonSaveAndSort.Location = new System.Drawing.Point(257, 188);
            this.buttonSaveAndSort.Name = "buttonSaveAndSort";
            this.buttonSaveAndSort.Size = new System.Drawing.Size(56, 23);
            this.buttonSaveAndSort.TabIndex = 9;
            this.buttonSaveAndSort.Text = "Sort!";
            this.buttonSaveAndSort.UseVisualStyleBackColor = true;
            this.buttonSaveAndSort.Click += new System.EventHandler(this.buttonSaveAndSort_Click);
            // 
            // labelIgnoreFolders
            // 
            this.labelIgnoreFolders.AutoSize = true;
            this.labelIgnoreFolders.Location = new System.Drawing.Point(10, 113);
            this.labelIgnoreFolders.Name = "labelIgnoreFolders";
            this.labelIgnoreFolders.Size = new System.Drawing.Size(74, 13);
            this.labelIgnoreFolders.TabIndex = 10;
            this.labelIgnoreFolders.Text = "Ignore folders:";
            // 
            // textBoxIgnoreFolders
            // 
            this.textBoxIgnoreFolders.Location = new System.Drawing.Point(102, 110);
            this.textBoxIgnoreFolders.Name = "textBoxIgnoreFolders";
            this.textBoxIgnoreFolders.Size = new System.Drawing.Size(211, 20);
            this.textBoxIgnoreFolders.TabIndex = 11;
            this.textBoxIgnoreFolders.Text = "Documents";
            // 
            // labelIgnoreFiles
            // 
            this.labelIgnoreFiles.AutoSize = true;
            this.labelIgnoreFiles.Location = new System.Drawing.Point(10, 139);
            this.labelIgnoreFiles.Name = "labelIgnoreFiles";
            this.labelIgnoreFiles.Size = new System.Drawing.Size(61, 13);
            this.labelIgnoreFiles.TabIndex = 12;
            this.labelIgnoreFiles.Text = "Ignore files:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Ignore shortcuts:";
            // 
            // textBoxIgnoreFiles
            // 
            this.textBoxIgnoreFiles.Location = new System.Drawing.Point(102, 136);
            this.textBoxIgnoreFiles.Name = "textBoxIgnoreFiles";
            this.textBoxIgnoreFiles.Size = new System.Drawing.Size(211, 20);
            this.textBoxIgnoreFiles.TabIndex = 14;
            // 
            // textBoxIgnoreShortcuts
            // 
            this.textBoxIgnoreShortcuts.Location = new System.Drawing.Point(102, 162);
            this.textBoxIgnoreShortcuts.Name = "textBoxIgnoreShortcuts";
            this.textBoxIgnoreShortcuts.Size = new System.Drawing.Size(211, 20);
            this.textBoxIgnoreShortcuts.TabIndex = 15;
            this.textBoxIgnoreShortcuts.Text = "Downloads";
            // 
            // LazyDesktopSort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 221);
            this.Controls.Add(this.textBoxIgnoreShortcuts);
            this.Controls.Add(this.textBoxIgnoreFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelIgnoreFiles);
            this.Controls.Add(this.textBoxIgnoreFolders);
            this.Controls.Add(this.labelIgnoreFolders);
            this.Controls.Add(this.labelDesktopPath);
            this.Controls.Add(this.textBoxDesktopPath);
            this.Controls.Add(this.buttonSaveAndSort);
            this.Controls.Add(this.buttonBrowseDesktopPath);
            this.Controls.Add(this.textBoxShortcuts);
            this.Controls.Add(this.textBoxFiles);
            this.Controls.Add(this.labelShortcuts);
            this.Controls.Add(this.labelFiles);
            this.Controls.Add(this.textBoxFolders);
            this.Controls.Add(this.labelFolders);
            this.Name = "LazyDesktopSort";
            this.Text = "LazyDesktopSort";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label labelDesktopPath;
        private System.Windows.Forms.TextBox textBoxDesktopPath;
        private System.Windows.Forms.Button buttonBrowseDesktopPath;
        private System.Windows.Forms.Label labelFolders;
        private System.Windows.Forms.TextBox textBoxFolders;
        private System.Windows.Forms.Label labelFiles;
        private System.Windows.Forms.Label labelShortcuts;
        private System.Windows.Forms.TextBox textBoxFiles;
        private System.Windows.Forms.TextBox textBoxShortcuts;
        private System.Windows.Forms.Button buttonSaveAndSort;
        private System.Windows.Forms.Label labelIgnoreFolders;
        private System.Windows.Forms.TextBox textBoxIgnoreFolders;
        private System.Windows.Forms.Label labelIgnoreFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIgnoreFiles;
        private System.Windows.Forms.TextBox textBoxIgnoreShortcuts;
    }
}

