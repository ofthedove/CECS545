namespace CECS545AI_Project1Combs
{
    partial class MainForm
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
            this.inputFilePathTextBox = new System.Windows.Forms.TextBox();
            this.inputFileBrowseButton = new System.Windows.Forms.Button();
            this.inputFileLabel = new System.Windows.Forms.Label();
            this.outputTextLabel = new System.Windows.Forms.Label();
            this.outputText = new System.Windows.Forms.TextBox();
            this.runButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.inputOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.outputSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.clearButton = new System.Windows.Forms.Button();
            this.connectionsOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.displayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inputFilePathTextBox
            // 
            this.inputFilePathTextBox.AcceptsReturn = true;
            this.inputFilePathTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputFilePathTextBox.Location = new System.Drawing.Point(12, 29);
            this.inputFilePathTextBox.Name = "inputFilePathTextBox";
            this.inputFilePathTextBox.Size = new System.Drawing.Size(523, 23);
            this.inputFilePathTextBox.TabIndex = 0;
            // 
            // inputFileBrowseButton
            // 
            this.inputFileBrowseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputFileBrowseButton.Location = new System.Drawing.Point(541, 29);
            this.inputFileBrowseButton.Name = "inputFileBrowseButton";
            this.inputFileBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.inputFileBrowseButton.TabIndex = 1;
            this.inputFileBrowseButton.Text = "&Browse";
            this.inputFileBrowseButton.UseVisualStyleBackColor = true;
            this.inputFileBrowseButton.Click += new System.EventHandler(this.inputFileBrowseButton_Click);
            // 
            // inputFileLabel
            // 
            this.inputFileLabel.AutoSize = true;
            this.inputFileLabel.Location = new System.Drawing.Point(13, 13);
            this.inputFileLabel.Name = "inputFileLabel";
            this.inputFileLabel.Size = new System.Drawing.Size(53, 13);
            this.inputFileLabel.TabIndex = 2;
            this.inputFileLabel.Text = "Input File:";
            // 
            // outputTextLabel
            // 
            this.outputTextLabel.AutoSize = true;
            this.outputTextLabel.Location = new System.Drawing.Point(13, 55);
            this.outputTextLabel.Name = "outputTextLabel";
            this.outputTextLabel.Size = new System.Drawing.Size(63, 13);
            this.outputTextLabel.TabIndex = 3;
            this.outputTextLabel.Text = "Output Text";
            // 
            // outputText
            // 
            this.outputText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputText.Location = new System.Drawing.Point(12, 71);
            this.outputText.Multiline = true;
            this.outputText.Name = "outputText";
            this.outputText.ReadOnly = true;
            this.outputText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputText.Size = new System.Drawing.Size(604, 248);
            this.outputText.TabIndex = 0;
            this.outputText.TabStop = false;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(12, 378);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(197, 47);
            this.runButton.TabIndex = 5;
            this.runButton.Text = "&Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(215, 325);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(199, 47);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(420, 325);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(197, 47);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // inputOpenFileDialog
            // 
            this.inputOpenFileDialog.AddExtension = false;
            this.inputOpenFileDialog.DefaultExt = "tsp";
            this.inputOpenFileDialog.Filter = "TSP files|*.tsp|All files|*.*";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(12, 325);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(197, 47);
            this.clearButton.TabIndex = 8;
            this.clearButton.Text = "C&lear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // connectionsOpenFileDialog
            // 
            this.connectionsOpenFileDialog.AddExtension = false;
            this.connectionsOpenFileDialog.Filter = "All files|*.*";
            // 
            // displayButton
            // 
            this.displayButton.Location = new System.Drawing.Point(420, 378);
            this.displayButton.Name = "displayButton";
            this.displayButton.Size = new System.Drawing.Size(197, 47);
            this.displayButton.TabIndex = 9;
            this.displayButton.Text = "&Display Graph";
            this.displayButton.UseVisualStyleBackColor = true;
            this.displayButton.Click += new System.EventHandler(this.displayButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.runButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(629, 436);
            this.Controls.Add(this.displayButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.outputText);
            this.Controls.Add(this.outputTextLabel);
            this.Controls.Add(this.inputFileLabel);
            this.Controls.Add(this.inputFileBrowseButton);
            this.Controls.Add(this.inputFilePathTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Travelling Salesperson";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputFilePathTextBox;
        private System.Windows.Forms.Button inputFileBrowseButton;
        private System.Windows.Forms.Label inputFileLabel;
        private System.Windows.Forms.Label outputTextLabel;
        private System.Windows.Forms.TextBox outputText;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.OpenFileDialog inputOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog outputSaveFileDialog;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.OpenFileDialog connectionsOpenFileDialog;
        private System.Windows.Forms.Button displayButton;
    }
}

