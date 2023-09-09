namespace AgOpenGPS
{
    partial class FormABDraw
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormABDraw));
            this.oglSelf = new OpenTK.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblNumCu = new System.Windows.Forms.Label();
            this.lblNumAB = new System.Windows.Forms.Label();
            this.lblABSelected = new System.Windows.Forms.Label();
            this.lblCurveSelected = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudDistance = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tboxNameCurve = new System.Windows.Forms.TextBox();
            this.tboxNameLine = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnFlipOffset = new System.Windows.Forms.Button();
            this.btnMakeBoundaryCurve = new System.Windows.Forms.Button();
            this.btnDrawSections = new System.Windows.Forms.Button();
            this.btnCancelTouch = new System.Windows.Forms.Button();
            this.btnDeleteABLine = new System.Windows.Forms.Button();
            this.btnDeleteCurve = new System.Windows.Forms.Button();
            this.btnSelectABLine = new System.Windows.Forms.Button();
            this.btnSelectCurve = new System.Windows.Forms.Button();
            this.btnMakeCurve = new System.Windows.Forms.Button();
            this.btnMakeABLine = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblCmInch = new System.Windows.Forms.Label();
            this.btnCurVor1 = new System.Windows.Forms.Button();
            this.btnKurNach1 = new System.Windows.Forms.Button();
            this.btnAbVor1 = new System.Windows.Forms.Button();
            this.btnAbRueck1 = new System.Windows.Forms.Button();
            this.btnSave1 = new System.Windows.Forms.Button();
            this.btnZweitGrenzenAktiv1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // oglSelf
            // 
            this.oglSelf.BackColor = System.Drawing.Color.Black;
            this.oglSelf.Cursor = System.Windows.Forms.Cursors.Cross;
            this.oglSelf.Location = new System.Drawing.Point(5, 7);
            this.oglSelf.Margin = new System.Windows.Forms.Padding(0);
            this.oglSelf.Name = "oglSelf";
            this.oglSelf.Size = new System.Drawing.Size(700, 700);
            this.oglSelf.TabIndex = 183;
            this.oglSelf.VSync = false;
            this.oglSelf.Load += new System.EventHandler(this.oglSelf_Load);
            this.oglSelf.Paint += new System.Windows.Forms.PaintEventHandler(this.oglSelf_Paint);
            this.oglSelf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.oglSelf_MouseDown);
            this.oglSelf.Resize += new System.EventHandler(this.oglSelf_Resize);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblNumCu
            // 
            this.lblNumCu.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumCu.ForeColor = System.Drawing.Color.White;
            this.lblNumCu.Location = new System.Drawing.Point(963, 296);
            this.lblNumCu.Margin = new System.Windows.Forms.Padding(0);
            this.lblNumCu.Name = "lblNumCu";
            this.lblNumCu.Size = new System.Drawing.Size(35, 26);
            this.lblNumCu.TabIndex = 327;
            this.lblNumCu.Text = "1";
            this.lblNumCu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNumAB
            // 
            this.lblNumAB.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumAB.ForeColor = System.Drawing.Color.White;
            this.lblNumAB.Location = new System.Drawing.Point(964, 469);
            this.lblNumAB.Margin = new System.Windows.Forms.Padding(0);
            this.lblNumAB.Name = "lblNumAB";
            this.lblNumAB.Size = new System.Drawing.Size(35, 26);
            this.lblNumAB.TabIndex = 328;
            this.lblNumAB.Text = "2";
            this.lblNumAB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblABSelected
            // 
            this.lblABSelected.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblABSelected.ForeColor = System.Drawing.Color.White;
            this.lblABSelected.Location = new System.Drawing.Point(885, 469);
            this.lblABSelected.Margin = new System.Windows.Forms.Padding(0);
            this.lblABSelected.Name = "lblABSelected";
            this.lblABSelected.Size = new System.Drawing.Size(35, 26);
            this.lblABSelected.TabIndex = 330;
            this.lblABSelected.Text = "1";
            this.lblABSelected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurveSelected
            // 
            this.lblCurveSelected.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurveSelected.ForeColor = System.Drawing.Color.White;
            this.lblCurveSelected.Location = new System.Drawing.Point(884, 297);
            this.lblCurveSelected.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurveSelected.Name = "lblCurveSelected";
            this.lblCurveSelected.Size = new System.Drawing.Size(35, 26);
            this.lblCurveSelected.TabIndex = 329;
            this.lblCurveSelected.Text = "1";
            this.lblCurveSelected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(926, 298);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 23);
            this.label1.TabIndex = 332;
            this.label1.Text = "of";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(926, 471);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 23);
            this.label2.TabIndex = 333;
            this.label2.Text = "of";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudDistance
            // 
            this.nudDistance.BackColor = System.Drawing.Color.AliceBlue;
            this.nudDistance.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDistance.Location = new System.Drawing.Point(860, 41);
            this.nudDistance.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudDistance.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.nudDistance.Name = "nudDistance";
            this.nudDistance.ReadOnly = true;
            this.nudDistance.Size = new System.Drawing.Size(132, 52);
            this.nudDistance.TabIndex = 16;
            this.nudDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDistance.Click += new System.EventHandler(this.nudDistance_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(712, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 32);
            this.label5.TabIndex = 15;
            this.label5.Text = "Tool Width";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(901, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 25);
            this.label6.TabIndex = 341;
            this.label6.Text = "2";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tboxNameCurve
            // 
            this.tboxNameCurve.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tboxNameCurve.CausesValidation = false;
            this.tboxNameCurve.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxNameCurve.Location = new System.Drawing.Point(712, 404);
            this.tboxNameCurve.Margin = new System.Windows.Forms.Padding(0);
            this.tboxNameCurve.MaxLength = 100;
            this.tboxNameCurve.Name = "tboxNameCurve";
            this.tboxNameCurve.Size = new System.Drawing.Size(283, 27);
            this.tboxNameCurve.TabIndex = 10;
            this.tboxNameCurve.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tboxNameCurve.Enter += new System.EventHandler(this.tboxNameCurve_Enter);
            this.tboxNameCurve.Leave += new System.EventHandler(this.tboxNameCurve_Leave);
            // 
            // tboxNameLine
            // 
            this.tboxNameLine.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tboxNameLine.CausesValidation = false;
            this.tboxNameLine.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxNameLine.Location = new System.Drawing.Point(712, 568);
            this.tboxNameLine.Margin = new System.Windows.Forms.Padding(0);
            this.tboxNameLine.MaxLength = 100;
            this.tboxNameLine.Name = "tboxNameLine";
            this.tboxNameLine.Size = new System.Drawing.Size(283, 27);
            this.tboxNameLine.TabIndex = 9;
            this.tboxNameLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tboxNameLine.Enter += new System.EventHandler(this.tboxNameLine_Enter);
            this.tboxNameLine.Leave += new System.EventHandler(this.tboxNameLine_Leave);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(805, 173);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 27);
            this.label4.TabIndex = 12;
            this.label4.Text = "Boundary Curve";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(728, 309);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 348;
            this.label3.Text = "Curve";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(728, 479);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 20);
            this.label7.TabIndex = 349;
            this.label7.Text = "Line";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(728, 608);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 20);
            this.label8.TabIndex = 13;
            this.label8.Text = "Mapping";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFlipOffset
            // 
            this.btnFlipOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFlipOffset.BackColor = System.Drawing.Color.Transparent;
            this.btnFlipOffset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFlipOffset.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnFlipOffset.FlatAppearance.BorderSize = 0;
            this.btnFlipOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlipOffset.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFlipOffset.Image = global::AgOpenGPS.Properties.Resources.ABSwapPoints;
            this.btnFlipOffset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFlipOffset.Location = new System.Drawing.Point(875, 99);
            this.btnFlipOffset.Name = "btnFlipOffset";
            this.btnFlipOffset.Size = new System.Drawing.Size(80, 71);
            this.btnFlipOffset.TabIndex = 14;
            this.btnFlipOffset.UseVisualStyleBackColor = false;
            this.btnFlipOffset.Click += new System.EventHandler(this.btnFlipOffset_Click);
            // 
            // btnMakeBoundaryCurve
            // 
            this.btnMakeBoundaryCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMakeBoundaryCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnMakeBoundaryCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMakeBoundaryCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnMakeBoundaryCurve.FlatAppearance.BorderSize = 0;
            this.btnMakeBoundaryCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeBoundaryCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeBoundaryCurve.Image = global::AgOpenGPS.Properties.Resources.BoundaryCurveLine;
            this.btnMakeBoundaryCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeBoundaryCurve.Location = new System.Drawing.Point(716, 129);
            this.btnMakeBoundaryCurve.Name = "btnMakeBoundaryCurve";
            this.btnMakeBoundaryCurve.Size = new System.Drawing.Size(80, 71);
            this.btnMakeBoundaryCurve.TabIndex = 4;
            this.btnMakeBoundaryCurve.UseVisualStyleBackColor = false;
            this.btnMakeBoundaryCurve.Click += new System.EventHandler(this.btnMakeBoundaryCurve_Click);
            // 
            // btnDrawSections
            // 
            this.btnDrawSections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDrawSections.BackColor = System.Drawing.Color.Transparent;
            this.btnDrawSections.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDrawSections.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnDrawSections.FlatAppearance.BorderSize = 0;
            this.btnDrawSections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrawSections.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDrawSections.Image = global::AgOpenGPS.Properties.Resources.MappingOff;
            this.btnDrawSections.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDrawSections.Location = new System.Drawing.Point(731, 621);
            this.btnDrawSections.Name = "btnDrawSections";
            this.btnDrawSections.Size = new System.Drawing.Size(89, 63);
            this.btnDrawSections.TabIndex = 11;
            this.btnDrawSections.UseVisualStyleBackColor = false;
            this.btnDrawSections.Click += new System.EventHandler(this.btnDrawSections_Click);
            // 
            // btnCancelTouch
            // 
            this.btnCancelTouch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelTouch.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelTouch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancelTouch.Enabled = false;
            this.btnCancelTouch.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancelTouch.FlatAppearance.BorderSize = 0;
            this.btnCancelTouch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTouch.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancelTouch.Image = global::AgOpenGPS.Properties.Resources.HeadlandDeletePoints;
            this.btnCancelTouch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelTouch.Location = new System.Drawing.Point(722, 35);
            this.btnCancelTouch.Name = "btnCancelTouch";
            this.btnCancelTouch.Size = new System.Drawing.Size(64, 63);
            this.btnCancelTouch.TabIndex = 1;
            this.btnCancelTouch.UseVisualStyleBackColor = false;
            this.btnCancelTouch.Click += new System.EventHandler(this.btnCancelTouch_Click);
            // 
            // btnDeleteABLine
            // 
            this.btnDeleteABLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteABLine.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteABLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteABLine.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnDeleteABLine.FlatAppearance.BorderSize = 0;
            this.btnDeleteABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteABLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineDelete;
            this.btnDeleteABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteABLine.Location = new System.Drawing.Point(934, 497);
            this.btnDeleteABLine.Name = "btnDeleteABLine";
            this.btnDeleteABLine.Size = new System.Drawing.Size(70, 68);
            this.btnDeleteABLine.TabIndex = 7;
            this.btnDeleteABLine.UseVisualStyleBackColor = false;
            this.btnDeleteABLine.Click += new System.EventHandler(this.btnDeleteABLine_Click);
            // 
            // btnDeleteCurve
            // 
            this.btnDeleteCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnDeleteCurve.FlatAppearance.BorderSize = 0;
            this.btnDeleteCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteCurve.Image = global::AgOpenGPS.Properties.Resources.HideContour;
            this.btnDeleteCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteCurve.Location = new System.Drawing.Point(934, 327);
            this.btnDeleteCurve.Name = "btnDeleteCurve";
            this.btnDeleteCurve.Size = new System.Drawing.Size(70, 68);
            this.btnDeleteCurve.TabIndex = 6;
            this.btnDeleteCurve.UseVisualStyleBackColor = false;
            this.btnDeleteCurve.Click += new System.EventHandler(this.btnDeleteCurve_Click);
            // 
            // btnSelectABLine
            // 
            this.btnSelectABLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectABLine.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectABLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSelectABLine.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSelectABLine.FlatAppearance.BorderSize = 0;
            this.btnSelectABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectABLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSelectABLine.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectABLine.Image")));
            this.btnSelectABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectABLine.Location = new System.Drawing.Point(826, 496);
            this.btnSelectABLine.Name = "btnSelectABLine";
            this.btnSelectABLine.Size = new System.Drawing.Size(70, 68);
            this.btnSelectABLine.TabIndex = 8;
            this.btnSelectABLine.UseVisualStyleBackColor = false;
            this.btnSelectABLine.Click += new System.EventHandler(this.btnSelectABLine_Click);
            // 
            // btnSelectCurve
            // 
            this.btnSelectCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSelectCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSelectCurve.FlatAppearance.BorderSize = 0;
            this.btnSelectCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSelectCurve.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectCurve.Image")));
            this.btnSelectCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectCurve.Location = new System.Drawing.Point(826, 327);
            this.btnSelectCurve.Name = "btnSelectCurve";
            this.btnSelectCurve.Size = new System.Drawing.Size(70, 68);
            this.btnSelectCurve.TabIndex = 5;
            this.btnSelectCurve.UseVisualStyleBackColor = false;
            this.btnSelectCurve.Click += new System.EventHandler(this.btnSelectCurve_Click);
            // 
            // btnMakeCurve
            // 
            this.btnMakeCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMakeCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnMakeCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMakeCurve.Enabled = false;
            this.btnMakeCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnMakeCurve.FlatAppearance.BorderSize = 0;
            this.btnMakeCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeCurve.Image = global::AgOpenGPS.Properties.Resources.CurveOn;
            this.btnMakeCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeCurve.Location = new System.Drawing.Point(716, 327);
            this.btnMakeCurve.Name = "btnMakeCurve";
            this.btnMakeCurve.Size = new System.Drawing.Size(70, 68);
            this.btnMakeCurve.TabIndex = 2;
            this.btnMakeCurve.UseVisualStyleBackColor = false;
            this.btnMakeCurve.Click += new System.EventHandler(this.BtnMakeCurve_Click);
            // 
            // btnMakeABLine
            // 
            this.btnMakeABLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMakeABLine.BackColor = System.Drawing.Color.Transparent;
            this.btnMakeABLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMakeABLine.Enabled = false;
            this.btnMakeABLine.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnMakeABLine.FlatAppearance.BorderSize = 0;
            this.btnMakeABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeABLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineOn;
            this.btnMakeABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeABLine.Location = new System.Drawing.Point(716, 497);
            this.btnMakeABLine.Name = "btnMakeABLine";
            this.btnMakeABLine.Size = new System.Drawing.Size(70, 68);
            this.btnMakeABLine.TabIndex = 3;
            this.btnMakeABLine.UseVisualStyleBackColor = false;
            this.btnMakeABLine.Click += new System.EventHandler(this.BtnMakeABLine_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.OliveDrab;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.Location = new System.Drawing.Point(914, 214);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(78, 79);
            this.btnExit.TabIndex = 0;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblCmInch
            // 
            this.lblCmInch.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCmInch.ForeColor = System.Drawing.Color.White;
            this.lblCmInch.Location = new System.Drawing.Point(963, 0);
            this.lblCmInch.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmInch.Name = "lblCmInch";
            this.lblCmInch.Size = new System.Drawing.Size(39, 32);
            this.lblCmInch.TabIndex = 350;
            this.lblCmInch.Text = "cm";
            this.lblCmInch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCurVor1
            // 
            this.btnCurVor1.BackColor = System.Drawing.Color.YellowGreen;
            this.btnCurVor1.Location = new System.Drawing.Point(792, 334);
            this.btnCurVor1.Name = "btnCurVor1";
            this.btnCurVor1.Size = new System.Drawing.Size(35, 55);
            this.btnCurVor1.TabIndex = 351;
            this.btnCurVor1.Text = "Vor <";
            this.btnCurVor1.UseVisualStyleBackColor = false;
            this.btnCurVor1.Click += new System.EventHandler(this.btnCurVor1_Click);
            // 
            // btnKurNach1
            // 
            this.btnKurNach1.BackColor = System.Drawing.Color.YellowGreen;
            this.btnKurNach1.Location = new System.Drawing.Point(899, 334);
            this.btnKurNach1.Name = "btnKurNach1";
            this.btnKurNach1.Size = new System.Drawing.Size(35, 55);
            this.btnKurNach1.TabIndex = 352;
            this.btnKurNach1.Text = "Rueck >";
            this.btnKurNach1.UseVisualStyleBackColor = false;
            this.btnKurNach1.Click += new System.EventHandler(this.btnKurNach1_Click);
            // 
            // btnAbVor1
            // 
            this.btnAbVor1.BackColor = System.Drawing.Color.Salmon;
            this.btnAbVor1.Location = new System.Drawing.Point(792, 504);
            this.btnAbVor1.Name = "btnAbVor1";
            this.btnAbVor1.Size = new System.Drawing.Size(35, 54);
            this.btnAbVor1.TabIndex = 353;
            this.btnAbVor1.Text = "Vor <";
            this.btnAbVor1.UseVisualStyleBackColor = false;
            this.btnAbVor1.Click += new System.EventHandler(this.btnAbVor1_Click);
            // 
            // btnAbRueck1
            // 
            this.btnAbRueck1.BackColor = System.Drawing.Color.Salmon;
            this.btnAbRueck1.Location = new System.Drawing.Point(899, 504);
            this.btnAbRueck1.Name = "btnAbRueck1";
            this.btnAbRueck1.Size = new System.Drawing.Size(35, 54);
            this.btnAbRueck1.TabIndex = 354;
            this.btnAbRueck1.Text = "Rueck >";
            this.btnAbRueck1.UseVisualStyleBackColor = false;
            this.btnAbRueck1.Click += new System.EventHandler(this.btnAbRueck1_Click);
            // 
            // btnSave1
            // 
            this.btnSave1.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.btnSave1.Location = new System.Drawing.Point(844, 228);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(64, 65);
            this.btnSave1.TabIndex = 355;
            this.btnSave1.Text = "Namen speichern";
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new System.EventHandler(this.btnSave1_Click);
            // 
            // btnZweitGrenzenAktiv1
            // 
            this.btnZweitGrenzenAktiv1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZweitGrenzenAktiv1.BackColor = System.Drawing.Color.Gray;
            this.btnZweitGrenzenAktiv1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZweitGrenzenAktiv1.Location = new System.Drawing.Point(722, 238);
            this.btnZweitGrenzenAktiv1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnZweitGrenzenAktiv1.Name = "btnZweitGrenzenAktiv1";
            this.btnZweitGrenzenAktiv1.Size = new System.Drawing.Size(105, 55);
            this.btnZweitGrenzenAktiv1.TabIndex = 356;
            this.btnZweitGrenzenAktiv1.Text = "Auch 2.Grenzen aktivieren";
            this.btnZweitGrenzenAktiv1.UseVisualStyleBackColor = false;
            this.btnZweitGrenzenAktiv1.Click += new System.EventHandler(this.btnZweitGrenzenAktiv1_Click);
            // 
            // FormABDraw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(1004, 697);
            this.ControlBox = false;
            this.Controls.Add(this.btnZweitGrenzenAktiv1);
            this.Controls.Add(this.btnSave1);
            this.Controls.Add(this.btnAbRueck1);
            this.Controls.Add(this.btnAbVor1);
            this.Controls.Add(this.btnKurNach1);
            this.Controls.Add(this.btnCurVor1);
            this.Controls.Add(this.btnFlipOffset);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tboxNameLine);
            this.Controls.Add(this.tboxNameCurve);
            this.Controls.Add(this.btnMakeBoundaryCurve);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnDrawSections);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudDistance);
            this.Controls.Add(this.btnCancelTouch);
            this.Controls.Add(this.lblABSelected);
            this.Controls.Add(this.lblCurveSelected);
            this.Controls.Add(this.lblNumAB);
            this.Controls.Add(this.lblNumCu);
            this.Controls.Add(this.btnDeleteABLine);
            this.Controls.Add(this.btnDeleteCurve);
            this.Controls.Add(this.btnSelectABLine);
            this.Controls.Add(this.btnSelectCurve);
            this.Controls.Add(this.btnMakeCurve);
            this.Controls.Add(this.btnMakeABLine);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.oglSelf);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCmInch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormABDraw";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Click 2 points on the Boundary to Begin";
            this.Load += new System.EventHandler(this.FormABDraw_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMakeABLine;
        private System.Windows.Forms.Button btnMakeCurve;
        private System.Windows.Forms.Button btnSelectCurve;
        private System.Windows.Forms.Button btnSelectABLine;
        private System.Windows.Forms.Button btnDeleteCurve;
        private System.Windows.Forms.Button btnDeleteABLine;
        private System.Windows.Forms.Label lblNumCu;
        private System.Windows.Forms.Label lblNumAB;
        private System.Windows.Forms.Label lblABSelected;
        private System.Windows.Forms.Label lblCurveSelected;
        private System.Windows.Forms.Button btnCancelTouch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudDistance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnDrawSections;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnMakeBoundaryCurve;
        private System.Windows.Forms.TextBox tboxNameCurve;
        private System.Windows.Forms.TextBox tboxNameLine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnFlipOffset;
        private System.Windows.Forms.Label lblCmInch;
        private System.Windows.Forms.Button btnCurVor1;
        private System.Windows.Forms.Button btnKurNach1;
        private System.Windows.Forms.Button btnAbVor1;
        private System.Windows.Forms.Button btnAbRueck1;
        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.Button btnZweitGrenzenAktiv1;
    }
}