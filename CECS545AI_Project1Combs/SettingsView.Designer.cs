namespace CECS545AI_Project1Combs
{
    partial class SettingsView
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
            this.inputFileLabel = new System.Windows.Forms.Label();
            this.inputFileBrowseButton = new System.Windows.Forms.Button();
            this.settingsFilePathTextBox = new System.Windows.Forms.TextBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.SettingsTabControl = new System.Windows.Forms.TabControl();
            this.Spawn = new System.Windows.Forms.TabPage();
            this.Breed = new System.Windows.Forms.TabPage();
            this.Cull = new System.Windows.Forms.TabPage();
            this.Mutate = new System.Windows.Forms.TabPage();
            this.End = new System.Windows.Forms.TabPage();
            this.settingsOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SettingsTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputFileLabel
            // 
            this.inputFileLabel.AutoSize = true;
            this.inputFileLabel.Location = new System.Drawing.Point(13, 323);
            this.inputFileLabel.Name = "inputFileLabel";
            this.inputFileLabel.Size = new System.Drawing.Size(67, 13);
            this.inputFileLabel.TabIndex = 5;
            this.inputFileLabel.Text = "Settings File:";
            // 
            // inputFileBrowseButton
            // 
            this.inputFileBrowseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputFileBrowseButton.Location = new System.Drawing.Point(541, 339);
            this.inputFileBrowseButton.Name = "inputFileBrowseButton";
            this.inputFileBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.inputFileBrowseButton.TabIndex = 4;
            this.inputFileBrowseButton.Text = "&Browse";
            this.inputFileBrowseButton.UseVisualStyleBackColor = true;
            // 
            // settingsFilePathTextBox
            // 
            this.settingsFilePathTextBox.AcceptsReturn = true;
            this.settingsFilePathTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsFilePathTextBox.Location = new System.Drawing.Point(12, 339);
            this.settingsFilePathTextBox.Name = "settingsFilePathTextBox";
            this.settingsFilePathTextBox.Size = new System.Drawing.Size(523, 23);
            this.settingsFilePathTextBox.TabIndex = 3;
            // 
            // loadButton
            // 
            this.loadButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.loadButton.Location = new System.Drawing.Point(12, 368);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(197, 47);
            this.loadButton.TabIndex = 11;
            this.loadButton.Text = "&Load";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(420, 368);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(197, 47);
            this.closeButton.TabIndex = 10;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.saveButton.Location = new System.Drawing.Point(215, 368);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(199, 47);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // SettingsTabControl
            // 
            this.SettingsTabControl.Controls.Add(this.Spawn);
            this.SettingsTabControl.Controls.Add(this.Breed);
            this.SettingsTabControl.Controls.Add(this.Mutate);
            this.SettingsTabControl.Controls.Add(this.Cull);
            this.SettingsTabControl.Controls.Add(this.End);
            this.SettingsTabControl.Location = new System.Drawing.Point(12, 12);
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.Size = new System.Drawing.Size(605, 308);
            this.SettingsTabControl.TabIndex = 12;
            // 
            // Spawn
            // 
            this.Spawn.Location = new System.Drawing.Point(4, 22);
            this.Spawn.Name = "Spawn";
            this.Spawn.Padding = new System.Windows.Forms.Padding(3);
            this.Spawn.Size = new System.Drawing.Size(597, 282);
            this.Spawn.TabIndex = 0;
            this.Spawn.Text = "Spawn";
            this.Spawn.UseVisualStyleBackColor = true;
            // 
            // Breed
            // 
            this.Breed.Location = new System.Drawing.Point(4, 22);
            this.Breed.Name = "Breed";
            this.Breed.Padding = new System.Windows.Forms.Padding(3);
            this.Breed.Size = new System.Drawing.Size(597, 282);
            this.Breed.TabIndex = 1;
            this.Breed.Text = "Breed";
            this.Breed.UseVisualStyleBackColor = true;
            // 
            // Cull
            // 
            this.Cull.Location = new System.Drawing.Point(4, 22);
            this.Cull.Name = "Cull";
            this.Cull.Size = new System.Drawing.Size(597, 282);
            this.Cull.TabIndex = 2;
            this.Cull.Text = "Cull";
            this.Cull.UseVisualStyleBackColor = true;
            // 
            // Mutate
            // 
            this.Mutate.Location = new System.Drawing.Point(4, 22);
            this.Mutate.Name = "Mutate";
            this.Mutate.Size = new System.Drawing.Size(597, 282);
            this.Mutate.TabIndex = 3;
            this.Mutate.Text = "Mutate";
            this.Mutate.UseVisualStyleBackColor = true;
            // 
            // End
            // 
            this.End.Location = new System.Drawing.Point(4, 22);
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(597, 282);
            this.End.TabIndex = 4;
            this.End.Text = "End";
            this.End.UseVisualStyleBackColor = true;
            // 
            // settingsOpenFileDialog
            // 
            this.settingsOpenFileDialog.Filter = "Settings files|*.stt|All files|*.*";
            // 
            // SettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(628, 427);
            this.Controls.Add(this.SettingsTabControl);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.inputFileLabel);
            this.Controls.Add(this.inputFileBrowseButton);
            this.Controls.Add(this.settingsFilePathTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SettingsView";
            this.Text = "Settings";
            this.SettingsTabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label inputFileLabel;
        private System.Windows.Forms.Button inputFileBrowseButton;
        private System.Windows.Forms.TextBox settingsFilePathTextBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TabControl SettingsTabControl;
        private System.Windows.Forms.TabPage Spawn;
        private System.Windows.Forms.TabPage Breed;
        private System.Windows.Forms.TabPage Mutate;
        private System.Windows.Forms.TabPage Cull;
        private System.Windows.Forms.TabPage End;
        private System.Windows.Forms.OpenFileDialog settingsOpenFileDialog;
    }
}