//Please, if you use this, share the improvements

using System;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;

namespace AgOpenGPS
{
    public partial class FormABLine : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private int originalLine = 0;

        public FormABLine(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            this.Text = gStr.gsABline;
        }

        private void FormABLine_Load(object sender, EventArgs e)
        {
            //tboxABLineName.Enabled = false;
            //btnAddToFile.Enabled = false;
            //btnAddAndGo.Enabled = false;
            //btnAPoint.Enabled = false;
            //btnBPoint.Enabled = false;
            //cboxHeading.Enabled = false;
            //tboxHeading.Enabled = false;
            //tboxABLineName.Text = "";
            //tboxABLineName.Enabled = false;

            //small window
            //ShowFullPanel(true);

            panelPick.Top = 3;
            panelPick.Left = 3;
            panelAPlus.Top = 3;
            panelAPlus.Left = 3;
            panelName.Top = 3;
            panelName.Left = 3;

            panelEditName.Top = 3;
            panelEditName.Left = 3;

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;
            panelEditName.Visible = false;

            //this.Size = new System.Drawing.Size(470, 360);
            this.Size = new System.Drawing.Size(470, 600);

            originalLine = mf.ABLine.numABLineSelected;

            mf.ABLine.isABLineBeingSet = false;
            UpdateLineList();
            OriginaleLinie();
            BerechnungsLinienAnzeigen();
         }

        private void OriginaleLinie()
        {
            if (lvLines.Items.Count > 0 && originalLine > 0)
            {
                lvLines.Items[originalLine - 1].EnsureVisible();
                lvLines.Items[originalLine - 1].Selected = true;
                lvLines.Select();
            }

        }
        private void BerechnungsLinienAnzeigen()
        {
            if (mf.distanzABLine1.isABLineSet)
            {
                btnLine1.Text = mf.distanzABLine1.desName;
                btnLine1.BackColor = mf.colorBerechnungslinie1;
            }
            else
            {
                btnLine1.Text = "Berechnungslinie1 ?";
                btnLine1.BackColor = Color.Gray;
            }
            if (mf.distanzABLine2.isABLineSet)
            {
                btnLine2.Text = mf.distanzABLine2.desName;
                btnLine2.BackColor = mf.colorBerechnungslinie2;
            }
            else
            {
                btnLine2.Text = "Berechnungslinie2 ?";
                btnLine2.BackColor = Color.Gray;
            }

        }

        private void UpdateLineList()
        {
            lvLines.Clear();
            ListViewItem itm;

            foreach (CABLines item in mf.ABLine.lineArr)
            {
                itm = new ListViewItem(item.Name);
                lvLines.Items.Add(itm);
            }

            // go to bottom of list - if there is a bottom
            if (lvLines.Items.Count > 0)
            {
                lvLines.Items[lvLines.Items.Count - 1].EnsureVisible();
                lvLines.Items[lvLines.Items.Count - 1].Selected = true;
                lvLines.Select();
            }
        }
        private void btnCancel_APlus_Click(object sender, EventArgs e)
        {
            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelEditName.Visible = false;

            textBox1.Enter -= textBox1_Enter;
            panelName.Visible = false;
            textBox1.Enter += textBox1_Enter;

            this.Size = new System.Drawing.Size(470,600);

            UpdateLineList();
            mf.ABLine.isABLineBeingSet = false;
            btnBPoint.BackColor = System.Drawing.Color.Transparent;
        }

        private void btnAPoint_Click(object sender, EventArgs e)
        {
            vec3 fix = new vec3(mf.pivotAxlePos);

            mf.ABLine.desPoint1.easting = fix.easting + Math.Cos(fix.heading) * mf.tool.toolOffset;
            mf.ABLine.desPoint1.northing = fix.northing - Math.Sin(fix.heading) * mf.tool.toolOffset;
            mf.ABLine.desHeading = fix.heading;

            mf.ABLine.desPoint2.easting = 99999;
            mf.ABLine.desPoint2.northing = 99999;

            nudHeading.Enabled = true;
            nudHeading.Value = (decimal)(glm.toDegrees(mf.ABLine.desHeading));

            BuildDesLine();

            btnBPoint.Enabled = true;
            btnAPoint.Enabled = false;

            btnEnter_APlus.Enabled = true;
            mf.ABLine.isABLineBeingSet = true;
        }

        private void btnBPoint_Click(object sender, EventArgs e)
        {
            vec3 fix = new vec3(mf.pivotAxlePos);

            btnBPoint.BackColor = System.Drawing.Color.Teal;

            mf.ABLine.desPoint2.easting = fix.easting + Math.Cos(fix.heading) * mf.tool.toolOffset;
            mf.ABLine.desPoint2.northing = fix.northing - Math.Sin(fix.heading) * mf.tool.toolOffset;

            // heading based on AB points
            mf.ABLine.desHeading = Math.Atan2(mf.ABLine.desPoint2.easting - mf.ABLine.desPoint1.easting,
                mf.ABLine.desPoint2.northing - mf.ABLine.desPoint1.northing);
            if (mf.ABLine.desHeading < 0) mf.ABLine.desHeading += glm.twoPI;

            nudHeading.Value = (decimal)(glm.toDegrees(mf.ABLine.desHeading));

            BuildDesLine();
        }

        private void nudHeading_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                BuildDesLine();
            }
        }

        private void BuildDesLine()
        {
            mf.ABLine.desHeading = glm.toRadians((double)nudHeading.Value);

            //sin x cos z for endpoints, opposite for additional lines
            mf.ABLine.desP1.easting = mf.ABLine.desPoint1.easting - (Math.Sin(mf.ABLine.desHeading) * mf.ABLine.abLength);
            mf.ABLine.desP1.northing = mf.ABLine.desPoint1.northing - (Math.Cos(mf.ABLine.desHeading) * mf.ABLine.abLength);
            mf.ABLine.desP2.easting = mf.ABLine.desPoint1.easting + (Math.Sin(mf.ABLine.desHeading) * mf.ABLine.abLength);
            mf.ABLine.desP2.northing = mf.ABLine.desPoint1.northing + (Math.Cos(mf.ABLine.desHeading) * mf.ABLine.abLength);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnAdd.Focus();
            }
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {

            textBox1.Text += DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
            mf.ABLine.desName = textBox1.Text;
        }

        private void btnEnter_APlus_Click(object sender, EventArgs e)
        {
            panelAPlus.Visible = false;
            panelName.Visible = true;

            mf.ABLine.desName = "AB " +
                (Math.Round(glm.toDegrees(mf.ABLine.desHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                "\u00B0 " + mf.FindDirection(mf.ABLine.desHeading);

            textBox1.Text = mf.ABLine.desName;

        }

        private void BtnNewABLine_Click(object sender, EventArgs e)
        {
            lvLines.SelectedItems.Clear();
            panelPick.Visible = false;
            panelAPlus.Visible = true;
            panelName.Visible = false;

            btnAPoint.Enabled = true;
            btnBPoint.Enabled = false;
            nudHeading.Enabled = false;

            btnEnter_APlus.Enabled = false;

            this.Size = new System.Drawing.Size(270, 360);

        }
        private void btnEditName_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];
                textBox2.Text = mf.ABLine.lineArr[idx].Name;

                panelPick.Visible = false;
                panelEditName.Visible = true;
                this.Size = new System.Drawing.Size(270, 360);
            }
        }

        private void btnSaveEditName_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "") textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            int idx = lvLines.SelectedIndices[0];

            textBox2.Enter -= textBox2_Enter;
            panelEditName.Visible = false;
            textBox2.Enter += textBox2_Enter;

            panelPick.Visible = true;

            mf.ABLine.lineArr[idx].Name = textBox2.Text.Trim();
            mf.FileSaveABLines();

            this.Size = new System.Drawing.Size(470,600);

            UpdateLineList();
            lvLines.Focus();
            mf.ABLine.isABLineBeingSet = false;

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            mf.ABLine.lineArr.Add(new CABLines());
            mf.ABLine.numABLines = mf.ABLine.lineArr.Count;
            mf.ABLine.numABLineSelected = mf.ABLine.numABLines;

            //index to last one.    
            int idx = mf.ABLine.lineArr.Count - 1;

            mf.ABLine.lineArr[idx].heading = mf.ABLine.desHeading;
            //calculate the new points for the reference line and points
            mf.ABLine.lineArr[idx].origin.easting = mf.ABLine.desPoint1.easting;
            mf.ABLine.lineArr[idx].origin.northing = mf.ABLine.desPoint1.northing;

            //name
            if (textBox2.Text.Trim() == "") textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            mf.ABLine.lineArr[idx].Name = textBox1.Text.Trim();

            mf.FileSaveABLines();

            panelPick.Visible = true;
            panelAPlus.Visible = false;

            textBox1.Enter -= textBox1_Enter;
            panelName.Visible = false;
            textBox1.Enter += textBox1_Enter;

            this.Size = new System.Drawing.Size(470, 600);

            UpdateLineList();
            lvLines.Focus();
            mf.ABLine.isABLineBeingSet = false;
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];


                panelPick.Visible = false;
                panelName.Visible = true;
                this.Size = new System.Drawing.Size(270, 360);

                panelAPlus.Visible = false;
                panelName.Visible = true;

                mf.ABLine.desHeading = mf.ABLine.lineArr[idx].heading;

                //calculate the new points for the reference line and points                
                mf.ABLine.desPoint1.easting = mf.ABLine.lineArr[idx].origin.easting;
                mf.ABLine.desPoint1.northing = mf.ABLine.lineArr[idx].origin.northing;

                mf.ABLine.desName = mf.ABLine.lineArr[idx].Name + " Copy";

                textBox1.Text = mf.ABLine.desName;
            }

        }


        private void btnListUse_Click(object sender, EventArgs e)
        {
            mf.ABLine.moveDistance = 0;
            //reset to generate new reference
            mf.ABLine.isABValid = false;

            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];
                mf.ABLine.numABLineSelected = idx + 1;

                mf.ABLine.abHeading = mf.ABLine.lineArr[idx].heading;
                mf.ABLine.refPoint1 = mf.ABLine.lineArr[idx].origin;

                mf.ABLine.SetABLineByHeading();

                mf.EnableYouTurnButtons();

                //Go back with Line enabled
                Close();
            }

            //no item selected
            else
            {
                mf.btnABLine.Image = Properties.Resources.ABLineOff;
                mf.ABLine.isBtnABLineOn = false;
                mf.ABLine.isABLineSet = false;
                mf.ABLine.isABLineLoaded = false;
                mf.ABLine.numABLineSelected = 0;
                mf.DisableYouTurnButtons();
                if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                Close();
            }
        }
        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                mf.ABLine.isABValid = false;
                int idx = lvLines.SelectedIndices[0];


                mf.ABLine.lineArr[idx].heading += Math.PI;
                if (mf.ABLine.lineArr[idx].heading > glm.twoPI) mf.ABLine.lineArr[idx].heading -= glm.twoPI;


                mf.FileSaveABLines();

                UpdateLineList();
                lvLines.Focus();
            }
        }

        private void btnListDelete_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int num = lvLines.SelectedIndices[0];
                mf.ABLine.lineArr.RemoveAt(num);
                lvLines.SelectedItems[0].Remove();

                mf.ABLine.numABLines = mf.ABLine.lineArr.Count;
                if (mf.ABLine.numABLineSelected > mf.ABLine.numABLines) mf.ABLine.numABLineSelected = mf.ABLine.numABLines;

                if (mf.ABLine.numABLines == 0)
                {
                    mf.ABLine.DeleteAB();
                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                }
                mf.FileSaveABLines();
            }
            else
            {
                if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
            }
            UpdateLineList();
            lvLines.Focus();

        }

        private void FormABLine_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //mf.ABLine.numABLines = mf.ABLine.lineArr.Count;
            //if (mf.ABLine.numABLineSelected > mf.ABLine.numABLines) mf.ABLine.numABLineSelected = mf.ABLine.numABLines;

            //if (mf.ABLine.numABLines < originalSelected) mf.ABLine.numABLineSelected = 0;
            //else mf.ABLine.numABLineSelected = originalSelected;

            //if (mf.ABLine.numABLineSelected > 0)
            //{
            //    mf.ABLine.abHeading = mf.ABLine.lineArr[mf.ABLine.numABLineSelected - 1].heading;
            //    mf.ABLine.refPoint1 = mf.ABLine.lineArr[mf.ABLine.numABLineSelected - 1].origin;
            //    mf.ABLine.SetABLineByHeading();
            //    Close();
            //}
            //else
            {
                //mf.ABLine.tramPassEvery = 0;
                //mf.ABLine.tramBasedOn = 0;
                mf.btnABLine.Image = Properties.Resources.ABLineOff;
                mf.ABLine.isBtnABLineOn = false;
                mf.ABLine.isABLineSet = false;
                mf.ABLine.isABLineLoaded = false;
                mf.ABLine.numABLineSelected = 0;
                mf.DisableYouTurnButtons();
                if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                Close();
                mf.ABLine.isABValid = false;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSaveEditName.Focus();
            }
        }

        private void btnAddTimeEdit_Click(object sender, EventArgs e)
        {
            textBox2.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            using (var form = new FormEnterAB(mf))
            {
                if (form.ShowDialog(this) == DialogResult.OK) 
                {
                    panelAPlus.Visible = false;
                    panelName.Visible = true;

                    mf.ABLine.desName = "AB m " +
                        (Math.Round(glm.toDegrees(mf.ABLine.desHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                        "\u00B0 " + mf.FindDirection(mf.ABLine.desHeading);

                    textBox1.Text = mf.ABLine.desName;

                    //sin x cos z for endpoints, opposite for additional lines
                    mf.ABLine.desP1.easting = mf.ABLine.desPoint1.easting - (Math.Sin(mf.ABLine.desHeading) * mf.ABLine.abLength);
                    mf.ABLine.desP1.northing = mf.ABLine.desPoint1.northing - (Math.Cos(mf.ABLine.desHeading) * mf.ABLine.abLength);
                    mf.ABLine.desP2.easting = mf.ABLine.desPoint1.easting + (Math.Sin(mf.ABLine.desHeading) * mf.ABLine.abLength);
                    mf.ABLine.desP2.northing = mf.ABLine.desPoint1.northing + (Math.Cos(mf.ABLine.desHeading) * mf.ABLine.abLength);
                }
                else
                {
                    btnCancel_APlus.PerformClick();
                }
            }
        }

        private void panelPick_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLine1_Click(object sender, EventArgs e)
        {
            mf.distanzABLine1.moveDistance = 0;
            //reset to generate new reference
            mf.distanzABLine1.isABValid = false;

            if (lvLines.SelectedItems.Count > 0 & !mf.distanzABLine1.isABLineSet)
            {
                int idx = lvLines.SelectedIndices[0];
                mf.distanzABLine1.isABLineSet = true;
                
                mf.distanzABLine1.desHeading = mf.ABLine.lineArr[idx].heading;
                mf.distanzABLine1.abHeading = mf.ABLine.lineArr[idx].heading;
                mf.distanzABLine1.desPoint1.easting = mf.ABLine.lineArr[idx].origin.easting;
                mf.distanzABLine1.desPoint1.northing = mf.ABLine.lineArr[idx].origin.northing;
                mf.distanzABLine1.desName = mf.ABLine.lineArr[idx].Name;
            //sin x cos z for endpoints, opposite for additional lines
                mf.distanzABLine1.desP1.easting = mf.ABLine.lineArr[idx].origin.easting - (Math.Sin(mf.ABLine.lineArr[idx].heading) * 1000);
                mf.distanzABLine1.desP1.northing = mf.ABLine.lineArr[idx].origin.northing - (Math.Cos(mf.ABLine.lineArr[idx].heading) * 1000);
                mf.distanzABLine1.desP2.easting = mf.ABLine.lineArr[idx].origin.easting + (Math.Sin(mf.ABLine.lineArr[idx].heading) * 1000);
                mf.distanzABLine1.desP2.northing = mf.ABLine.lineArr[idx].origin.northing + (Math.Cos(mf.ABLine.lineArr[idx].heading) * 1000);            

            }
            else
            { 
                mf.distanzABLine1.isABLineSet = false;
            }
            BerechnungsLinienAnzeigen();
            OriginaleLinie();
        }

        private void btnLine2_Click(object sender, EventArgs e)
        {
            mf.distanzABLine2.moveDistance = 0;
            //reset to generate new reference
            mf.distanzABLine2.isABValid = false;

            if (lvLines.SelectedItems.Count > 0 & !mf.distanzABLine2.isABLineSet)
            {
                int idx = lvLines.SelectedIndices[0];
                mf.distanzABLine2.isABLineSet = true;

                mf.distanzABLine2.desHeading = mf.ABLine.lineArr[idx].heading;
                mf.distanzABLine2.abHeading = mf.ABLine.lineArr[idx].heading;
                mf.distanzABLine2.desPoint1.easting = mf.ABLine.lineArr[idx].origin.easting;
                mf.distanzABLine2.desPoint1.northing = mf.ABLine.lineArr[idx].origin.northing;
                mf.distanzABLine2.desName = mf.ABLine.lineArr[idx].Name;
                //sin x cos z for endpoints, opposite for additional lines
                mf.distanzABLine2.desP1.easting = mf.ABLine.lineArr[idx].origin.easting - (Math.Sin(mf.ABLine.lineArr[idx].heading) * 1000);
                mf.distanzABLine2.desP1.northing = mf.ABLine.lineArr[idx].origin.northing - (Math.Cos(mf.ABLine.lineArr[idx].heading) * 1000);
                mf.distanzABLine2.desP2.easting = mf.ABLine.lineArr[idx].origin.easting + (Math.Sin(mf.ABLine.lineArr[idx].heading) * 1000);
                mf.distanzABLine2.desP2.northing = mf.ABLine.lineArr[idx].origin.northing + (Math.Cos(mf.ABLine.lineArr[idx].heading) * 1000);

            }
            else
            {
                mf.distanzABLine2.isABLineSet = false;
            }
            BerechnungsLinienAnzeigen();
            OriginaleLinie();
        }

        private void lvLines_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
