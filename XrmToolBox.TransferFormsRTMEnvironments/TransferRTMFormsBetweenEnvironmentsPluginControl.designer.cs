namespace XrmToolBox.TransferRTMFormsBetweenEnvironments
{
    partial class TransferRTMFormsBetweenEnvironmentsPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferRTMFormsBetweenEnvironmentsPluginControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.p_control = new System.Windows.Forms.Panel();
            this.p_Forms = new System.Windows.Forms.Panel();
            this.p_ListViewForms = new System.Windows.Forms.Panel();
            this.gb_SourceForms = new System.Windows.Forms.GroupBox();
            this.lv_SourceForms = new System.Windows.Forms.ListView();
            this.gb_TargetForms = new System.Windows.Forms.GroupBox();
            this.lv_TargetForms = new System.Windows.Forms.ListView();
            this.p_Status = new System.Windows.Forms.Panel();
            this.p_FormsSourceStatus = new System.Windows.Forms.Panel();
            this.l_FormsSourceStatus = new System.Windows.Forms.Label();
            this.p_settings = new System.Windows.Forms.Panel();
            this.gb_settings = new System.Windows.Forms.GroupBox();
            this.cb_TransfersFormsRedirect = new System.Windows.Forms.CheckBox();
            this.cb_TransfersFormsInDraft = new System.Windows.Forms.CheckBox();
            this.gb_environments = new System.Windows.Forms.GroupBox();
            this.p_environmentSources = new System.Windows.Forms.Panel();
            this.l_environmentSourceValue = new System.Windows.Forms.Label();
            this.l_environmentSource = new System.Windows.Forms.Label();
            this.l_environmentTargetValue = new System.Windows.Forms.Label();
            this.bt_SelectTarget = new System.Windows.Forms.Button();
            this.bt_LoadForms = new System.Windows.Forms.ToolStripButton();
            this.bt_TransferForms = new System.Windows.Forms.ToolStripButton();
            this.bt_Donate = new System.Windows.Forms.ToolStripButton();
            this.toolStripMenu.SuspendLayout();
            this.p_control.SuspendLayout();
            this.p_Forms.SuspendLayout();
            this.p_ListViewForms.SuspendLayout();
            this.gb_SourceForms.SuspendLayout();
            this.gb_TargetForms.SuspendLayout();
            this.p_Status.SuspendLayout();
            this.p_FormsSourceStatus.SuspendLayout();
            this.p_settings.SuspendLayout();
            this.gb_settings.SuspendLayout();
            this.gb_environments.SuspendLayout();
            this.p_environmentSources.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt_LoadForms,
            this.toolStripSeparator1,
            this.tsbClose,
            this.tssSeparator1,
            this.bt_TransferForms,
            this.bt_Donate});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(838, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(40, 28);
            this.tsbClose.Text = "Close";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // p_control
            // 
            this.p_control.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.p_control.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.p_control.Controls.Add(this.p_Forms);
            this.p_control.Controls.Add(this.p_settings);
            this.p_control.Location = new System.Drawing.Point(0, 25);
            this.p_control.Name = "p_control";
            this.p_control.Padding = new System.Windows.Forms.Padding(5);
            this.p_control.Size = new System.Drawing.Size(838, 647);
            this.p_control.TabIndex = 6;
            // 
            // p_Forms
            // 
            this.p_Forms.Controls.Add(this.p_ListViewForms);
            this.p_Forms.Controls.Add(this.p_Status);
            this.p_Forms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_Forms.Location = new System.Drawing.Point(5, 82);
            this.p_Forms.Name = "p_Forms";
            this.p_Forms.Size = new System.Drawing.Size(828, 560);
            this.p_Forms.TabIndex = 8;
            // 
            // p_ListViewForms
            // 
            this.p_ListViewForms.Controls.Add(this.gb_SourceForms);
            this.p_ListViewForms.Controls.Add(this.gb_TargetForms);
            this.p_ListViewForms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_ListViewForms.Location = new System.Drawing.Point(0, 0);
            this.p_ListViewForms.Name = "p_ListViewForms";
            this.p_ListViewForms.Size = new System.Drawing.Size(828, 529);
            this.p_ListViewForms.TabIndex = 10;
            // 
            // gb_SourceForms
            // 
            this.gb_SourceForms.Controls.Add(this.lv_SourceForms);
            this.gb_SourceForms.Dock = System.Windows.Forms.DockStyle.Left;
            this.gb_SourceForms.Location = new System.Drawing.Point(0, 0);
            this.gb_SourceForms.Margin = new System.Windows.Forms.Padding(0);
            this.gb_SourceForms.Name = "gb_SourceForms";
            this.gb_SourceForms.Size = new System.Drawing.Size(400, 529);
            this.gb_SourceForms.TabIndex = 6;
            this.gb_SourceForms.TabStop = false;
            this.gb_SourceForms.Text = "Forms in source";
            // 
            // lv_SourceForms
            // 
            this.lv_SourceForms.CheckBoxes = true;
            this.lv_SourceForms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_SourceForms.FullRowSelect = true;
            this.lv_SourceForms.HideSelection = false;
            this.lv_SourceForms.Location = new System.Drawing.Point(3, 16);
            this.lv_SourceForms.Margin = new System.Windows.Forms.Padding(5);
            this.lv_SourceForms.Name = "lv_SourceForms";
            this.lv_SourceForms.Size = new System.Drawing.Size(394, 510);
            this.lv_SourceForms.TabIndex = 0;
            this.lv_SourceForms.UseCompatibleStateImageBehavior = false;
            this.lv_SourceForms.View = System.Windows.Forms.View.Details;
            this.lv_SourceForms.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lv_SourceForms_ItemChecked);
            this.lv_SourceForms.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lv_ItemSelectionChanged);
            // 
            // gb_TargetForms
            // 
            this.gb_TargetForms.Controls.Add(this.lv_TargetForms);
            this.gb_TargetForms.Dock = System.Windows.Forms.DockStyle.Right;
            this.gb_TargetForms.Location = new System.Drawing.Point(428, 0);
            this.gb_TargetForms.Margin = new System.Windows.Forms.Padding(0);
            this.gb_TargetForms.Name = "gb_TargetForms";
            this.gb_TargetForms.Size = new System.Drawing.Size(400, 529);
            this.gb_TargetForms.TabIndex = 7;
            this.gb_TargetForms.TabStop = false;
            this.gb_TargetForms.Text = "Forms in target";
            // 
            // lv_TargetForms
            // 
            this.lv_TargetForms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_TargetForms.FullRowSelect = true;
            this.lv_TargetForms.HideSelection = false;
            this.lv_TargetForms.Location = new System.Drawing.Point(3, 16);
            this.lv_TargetForms.Margin = new System.Windows.Forms.Padding(5);
            this.lv_TargetForms.Name = "lv_TargetForms";
            this.lv_TargetForms.Size = new System.Drawing.Size(394, 510);
            this.lv_TargetForms.TabIndex = 1;
            this.lv_TargetForms.UseCompatibleStateImageBehavior = false;
            this.lv_TargetForms.View = System.Windows.Forms.View.Details;
            this.lv_TargetForms.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lv_ItemSelectionChanged);
            // 
            // p_Status
            // 
            this.p_Status.Controls.Add(this.p_FormsSourceStatus);
            this.p_Status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p_Status.Location = new System.Drawing.Point(0, 529);
            this.p_Status.Name = "p_Status";
            this.p_Status.Size = new System.Drawing.Size(828, 31);
            this.p_Status.TabIndex = 9;
            // 
            // p_FormsSourceStatus
            // 
            this.p_FormsSourceStatus.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.p_FormsSourceStatus.Controls.Add(this.l_FormsSourceStatus);
            this.p_FormsSourceStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_FormsSourceStatus.Location = new System.Drawing.Point(0, 0);
            this.p_FormsSourceStatus.Name = "p_FormsSourceStatus";
            this.p_FormsSourceStatus.Size = new System.Drawing.Size(828, 31);
            this.p_FormsSourceStatus.TabIndex = 7;
            // 
            // l_FormsSourceStatus
            // 
            this.l_FormsSourceStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.l_FormsSourceStatus.AutoSize = true;
            this.l_FormsSourceStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_FormsSourceStatus.Location = new System.Drawing.Point(5, 7);
            this.l_FormsSourceStatus.Name = "l_FormsSourceStatus";
            this.l_FormsSourceStatus.Size = new System.Drawing.Size(137, 15);
            this.l_FormsSourceStatus.TabIndex = 0;
            this.l_FormsSourceStatus.Text = "Comparer status forms :";
            // 
            // p_settings
            // 
            this.p_settings.Controls.Add(this.gb_settings);
            this.p_settings.Controls.Add(this.gb_environments);
            this.p_settings.Dock = System.Windows.Forms.DockStyle.Top;
            this.p_settings.Location = new System.Drawing.Point(5, 5);
            this.p_settings.Name = "p_settings";
            this.p_settings.Size = new System.Drawing.Size(828, 77);
            this.p_settings.TabIndex = 5;
            // 
            // gb_settings
            // 
            this.gb_settings.Controls.Add(this.cb_TransfersFormsRedirect);
            this.gb_settings.Controls.Add(this.cb_TransfersFormsInDraft);
            this.gb_settings.Dock = System.Windows.Forms.DockStyle.Right;
            this.gb_settings.Location = new System.Drawing.Point(628, 0);
            this.gb_settings.Margin = new System.Windows.Forms.Padding(0);
            this.gb_settings.Name = "gb_settings";
            this.gb_settings.Size = new System.Drawing.Size(200, 77);
            this.gb_settings.TabIndex = 6;
            this.gb_settings.TabStop = false;
            this.gb_settings.Text = "Settings";
            // 
            // cb_TransfersFormsRedirect
            // 
            this.cb_TransfersFormsRedirect.AutoSize = true;
            this.cb_TransfersFormsRedirect.Dock = System.Windows.Forms.DockStyle.Top;
            this.cb_TransfersFormsRedirect.Location = new System.Drawing.Point(3, 43);
            this.cb_TransfersFormsRedirect.Name = "cb_TransfersFormsRedirect";
            this.cb_TransfersFormsRedirect.Padding = new System.Windows.Forms.Padding(5);
            this.cb_TransfersFormsRedirect.Size = new System.Drawing.Size(194, 27);
            this.cb_TransfersFormsRedirect.TabIndex = 1;
            this.cb_TransfersFormsRedirect.Text = "Transfers forms redirect";
            this.cb_TransfersFormsRedirect.UseVisualStyleBackColor = true;
            // 
            // cb_TransfersFormsInDraft
            // 
            this.cb_TransfersFormsInDraft.AutoSize = true;
            this.cb_TransfersFormsInDraft.Dock = System.Windows.Forms.DockStyle.Top;
            this.cb_TransfersFormsInDraft.Location = new System.Drawing.Point(3, 16);
            this.cb_TransfersFormsInDraft.Name = "cb_TransfersFormsInDraft";
            this.cb_TransfersFormsInDraft.Padding = new System.Windows.Forms.Padding(5);
            this.cb_TransfersFormsInDraft.Size = new System.Drawing.Size(194, 27);
            this.cb_TransfersFormsInDraft.TabIndex = 0;
            this.cb_TransfersFormsInDraft.Text = "Transfers forms in draft";
            this.cb_TransfersFormsInDraft.UseVisualStyleBackColor = true;
            // 
            // gb_environments
            // 
            this.gb_environments.Controls.Add(this.p_environmentSources);
            this.gb_environments.Dock = System.Windows.Forms.DockStyle.Left;
            this.gb_environments.Location = new System.Drawing.Point(0, 0);
            this.gb_environments.Margin = new System.Windows.Forms.Padding(0);
            this.gb_environments.Name = "gb_environments";
            this.gb_environments.Size = new System.Drawing.Size(200, 77);
            this.gb_environments.TabIndex = 0;
            this.gb_environments.TabStop = false;
            this.gb_environments.Text = "Environments";
            // 
            // p_environmentSources
            // 
            this.p_environmentSources.Controls.Add(this.l_environmentSourceValue);
            this.p_environmentSources.Controls.Add(this.l_environmentSource);
            this.p_environmentSources.Controls.Add(this.l_environmentTargetValue);
            this.p_environmentSources.Controls.Add(this.bt_SelectTarget);
            this.p_environmentSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_environmentSources.Location = new System.Drawing.Point(3, 16);
            this.p_environmentSources.Name = "p_environmentSources";
            this.p_environmentSources.Size = new System.Drawing.Size(194, 58);
            this.p_environmentSources.TabIndex = 6;
            // 
            // l_environmentSourceValue
            // 
            this.l_environmentSourceValue.AutoSize = true;
            this.l_environmentSourceValue.ForeColor = System.Drawing.Color.Green;
            this.l_environmentSourceValue.Location = new System.Drawing.Point(84, 0);
            this.l_environmentSourceValue.Name = "l_environmentSourceValue";
            this.l_environmentSourceValue.Size = new System.Drawing.Size(10, 13);
            this.l_environmentSourceValue.TabIndex = 6;
            this.l_environmentSourceValue.Text = "-";
            // 
            // l_environmentSource
            // 
            this.l_environmentSource.AutoSize = true;
            this.l_environmentSource.Location = new System.Drawing.Point(3, 0);
            this.l_environmentSource.Name = "l_environmentSource";
            this.l_environmentSource.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.l_environmentSource.Size = new System.Drawing.Size(44, 13);
            this.l_environmentSource.TabIndex = 0;
            this.l_environmentSource.Text = "Source";
            // 
            // l_environmentTargetValue
            // 
            this.l_environmentTargetValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.l_environmentTargetValue.AutoSize = true;
            this.l_environmentTargetValue.ForeColor = System.Drawing.Color.Red;
            this.l_environmentTargetValue.Location = new System.Drawing.Point(84, 23);
            this.l_environmentTargetValue.Name = "l_environmentTargetValue";
            this.l_environmentTargetValue.Size = new System.Drawing.Size(89, 13);
            this.l_environmentTargetValue.TabIndex = 7;
            this.l_environmentTargetValue.Text = "Pending selected";
            // 
            // bt_SelectTarget
            // 
            this.bt_SelectTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_SelectTarget.Location = new System.Drawing.Point(3, 18);
            this.bt_SelectTarget.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.bt_SelectTarget.Name = "bt_SelectTarget";
            this.bt_SelectTarget.Size = new System.Drawing.Size(75, 23);
            this.bt_SelectTarget.TabIndex = 6;
            this.bt_SelectTarget.Text = "Select target";
            this.bt_SelectTarget.UseVisualStyleBackColor = true;
            this.bt_SelectTarget.Click += new System.EventHandler(this.bt_SelectTarget_Click);
            // 
            // bt_LoadForms
            // 
            this.bt_LoadForms.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bt_LoadForms.Image = ((System.Drawing.Image)(resources.GetObject("bt_LoadForms.Image")));
            this.bt_LoadForms.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_LoadForms.Name = "bt_LoadForms";
            this.bt_LoadForms.Size = new System.Drawing.Size(71, 28);
            this.bt_LoadForms.Text = "Load forms";
            this.bt_LoadForms.Click += new System.EventHandler(this.bt_LoadForms_Click);
            // 
            // bt_TransferForms
            // 
            this.bt_TransferForms.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bt_TransferForms.Enabled = false;
            this.bt_TransferForms.Image = ((System.Drawing.Image)(resources.GetObject("bt_TransferForms.Image")));
            this.bt_TransferForms.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_TransferForms.Name = "bt_TransferForms";
            this.bt_TransferForms.Size = new System.Drawing.Size(138, 28);
            this.bt_TransferForms.Text = "Transfers forms selected";
            this.bt_TransferForms.Click += new System.EventHandler(this.bt_TransferForms_Click);
            // 
            // bt_Donate
            // 
            this.bt_Donate.Image = global::XrmToolBox.TransferFormsRTMEnvironments.Properties.Resources.paypal;
            this.bt_Donate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Donate.Name = "bt_Donate";
            this.bt_Donate.Size = new System.Drawing.Size(73, 28);
            this.bt_Donate.Text = "Donate";
            this.bt_Donate.Click += new System.EventHandler(this.bt_Donate_Click);
            // 
            // TransferRTMFormsBetweenEnvironmentsPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.p_control);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "TransferRTMFormsBetweenEnvironmentsPluginControl";
            this.Size = new System.Drawing.Size(838, 675);
            this.Load += new System.EventHandler(this.TransferFormsRTMEnvironmentsPluginControl_Load);
            this.Resize += new System.EventHandler(this.TransferRTMFormsBetweenEnvironmentsPluginControl_Resize);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.p_control.ResumeLayout(false);
            this.p_Forms.ResumeLayout(false);
            this.p_ListViewForms.ResumeLayout(false);
            this.gb_SourceForms.ResumeLayout(false);
            this.gb_TargetForms.ResumeLayout(false);
            this.p_Status.ResumeLayout(false);
            this.p_FormsSourceStatus.ResumeLayout(false);
            this.p_FormsSourceStatus.PerformLayout();
            this.p_settings.ResumeLayout(false);
            this.gb_settings.ResumeLayout(false);
            this.gb_settings.PerformLayout();
            this.gb_environments.ResumeLayout(false);
            this.p_environmentSources.ResumeLayout(false);
            this.p_environmentSources.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.Panel p_control;
        private System.Windows.Forms.ToolStripButton bt_LoadForms;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel p_settings;
        private System.Windows.Forms.GroupBox gb_settings;
        private System.Windows.Forms.CheckBox cb_TransfersFormsRedirect;
        private System.Windows.Forms.CheckBox cb_TransfersFormsInDraft;
        private System.Windows.Forms.GroupBox gb_environments;
        private System.Windows.Forms.Panel p_environmentSources;
        private System.Windows.Forms.Label l_environmentTargetValue;
        private System.Windows.Forms.Button bt_SelectTarget;
        private System.Windows.Forms.Label l_environmentSourceValue;
        private System.Windows.Forms.Label l_environmentSource;
        private System.Windows.Forms.ToolStripButton bt_TransferForms;
        private System.Windows.Forms.Panel p_Forms;
        private System.Windows.Forms.Panel p_ListViewForms;
        private System.Windows.Forms.GroupBox gb_SourceForms;
        private System.Windows.Forms.ListView lv_SourceForms;
        private System.Windows.Forms.GroupBox gb_TargetForms;
        private System.Windows.Forms.ListView lv_TargetForms;
        private System.Windows.Forms.Panel p_Status;
        private System.Windows.Forms.Panel p_FormsSourceStatus;
        private System.Windows.Forms.Label l_FormsSourceStatus;
        private System.Windows.Forms.ToolStripButton bt_Donate;
    }
}
