﻿using System;
using System.Windows.Forms;
using System.Drawing;

namespace AgOpenGPS
{
    public partial class FormFlags : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormFlags(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;

            //this.Text = gStr.gsSmoothABCurve;
        }
        private bool breit = false;
        private int color = 0;
        private double entfernung = 0;
        private vec2 flagPos = new vec2(0, 0);

        private void UpdateLabels()
        {
            lblLatStart.Text = mf.flagPts[mf.flagNumberPicked - 1].latitude.ToString("0.00000000");
            lblLonStart.Text = mf.flagPts[mf.flagNumberPicked - 1].longitude.ToString("0.00000000");
            lblEasting.Text = mf.flagPts[mf.flagNumberPicked - 1].easting.ToString("N2");
            lblNorthing.Text = mf.flagPts[mf.flagNumberPicked - 1].northing.ToString("N2");
            lblHeading.Text = glm.toDegrees(mf.flagPts[mf.flagNumberPicked - 1].heading).ToString("N2");
            lblFlagSelected.Text = mf.flagPts[mf.flagNumberPicked - 1].ID.ToString();
            tboxFlagNotes.Text = mf.flagPts[mf.flagNumberPicked - 1].notes;
            color = mf.flagPts[mf.flagNumberPicked - 1].color;
            ViewColor();
        }
        private void FormFlags_Load(object sender, EventArgs e)
        {
            this.Height = 350;
            this.Width = 316;
            UpdateLabels();
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            mf.flagNumberPicked++;
            if (mf.flagNumberPicked > mf.flagPts.Count) mf.flagNumberPicked = 1;
            UpdateLabels();
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            mf.flagNumberPicked--;
            if (mf.flagNumberPicked < 1) mf.flagNumberPicked = mf.flagPts.Count;
            UpdateLabels();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            mf.flagNumberPicked = 0;
            mf.FileSaveFlags();
            Close();
        }

        private void btnDeleteFlag_Click(object sender, EventArgs e)
        {
            int flag = mf.flagNumberPicked;
            if (mf.flagPts.Count > 0) mf.DeleteSelectedFlag();
            if (mf.flagPts.Count == 0)
            {
                mf.FileSaveFlags();
                Close();
                return;
            }
            if (flag > mf.flagPts.Count) mf.flagNumberPicked = mf.flagPts.Count;
            else mf.flagNumberPicked = flag;
            UpdateLabels();
        }

        private void tboxFlagNotes_Leave(object sender, EventArgs e)
        {
            mf.flagPts[mf.flagNumberPicked - 1].notes = tboxFlagNotes.Text;
        }

        private void tboxFlagNotes_TextChanged(object sender, EventArgs e)
        {

            //mf.flagPts[mf.flagNumberPicked - 1].notes = tboxFlagNotes.Text;
        }

        private void tboxFlagNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') e.Handled = true;
        }

        private void btnDriveToFlag_Click(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MakeDubinsLineFromPivotToFlag();
            vec3 steerAxlePosRP = mf.pivotAxlePos;
            if (mf.isMetric)
                lblDistanceToFlag.Text = glm.Distance(steerAxlePosRP,
                    mf.flagPts[mf.flagNumberPicked - 1].easting, mf.flagPts[mf.flagNumberPicked - 1].northing).ToString("N2") + " m";
            else lblDistanceToFlag.Text = (glm.Distance(steerAxlePosRP,
                mf.flagPts[mf.flagNumberPicked - 1].easting, mf.flagPts[mf.flagNumberPicked - 1].northing) * glm.m2ft).ToString("N2") + " m";

        }
        /*
        private void MakeDubinsLineFromPivotToFlag()
        {
            //if (mf.ABLine.isBtnABLineOn)
            //{
            //    mf.ABLine.isBtnABLineOn = false;
            //    mf.btnABLine.Image = Properties.Resources.ABLineOff;
            //}

            CDubins.turningRadius = mf.vehicle.minTurningRadius * 3.0;
            CDubins dubPath = new CDubins();

            // current psition
            vec3 steerAxlePosRP = mf.pivotAxlePos;

            //bump it back so you can line up to point
            vec3 goal = new vec3
            {
                easting = mf.flagPts[mf.flagNumberPicked - 1].easting - (Math.Sin(mf.flagPts[mf.flagNumberPicked - 1].heading) * 6),
                northing = mf.flagPts[mf.flagNumberPicked - 1].northing - (Math.Cos(mf.flagPts[mf.flagNumberPicked - 1].heading) * 6),
                heading = mf.flagPts[mf.flagNumberPicked - 1].heading
            };

            //bump it forward
            vec3 pt2 = new vec3
            {
                easting = steerAxlePosRP.easting + (Math.Sin(steerAxlePosRP.heading) * 6),
                northing = steerAxlePosRP.northing + (Math.Cos(steerAxlePosRP.heading) * 6),
                heading = steerAxlePosRP.heading
            };
        }
        */
        private void tboxFlagNotes_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnExit.Focus();
            }

        }

        private void btnBearbeiten_Click(object sender, EventArgs e)
        {
            breit = !breit;
            if (breit)
            {
                this.Width = 630;
            }
            else
            {
                this.Width = 316;
            }
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            color = hsbColor.Value;
            ViewColor();
            mf.flagPts[mf.flagNumberPicked - 1].color = color;
        }
        private void ViewColor()
        {
            switch (color)
                {
                case 0:
                    {
                        pbColor.BackColor = Color.Red;
                        break;
                    }
                case 1:
                    {
                        pbColor.BackColor = Color.Orange;
                        break;
                    }
                case 2:
                    {
                        pbColor.BackColor = Color.Yellow;
                        break;
                    }
                case 3:
                    {
                        pbColor.BackColor = Color.LightGreen;
                        break;
                    }
                case 4:
                    {
                        pbColor.BackColor = Color.Green;
                        break;
                    }
                case 5:
                    {
                        pbColor.BackColor = Color.Magenta;
                        break;
                    }
                default:
                    {
                        pbColor.BackColor = Color.Red;
                        break;
                    }
            }
        }

        private void hsbFlagAhead_Scroll(object sender, ScrollEventArgs e)
        {
            entfernung = (double)hsbFlagAhead.Value / 2;
            lblFlagAhead.Text = entfernung.ToString("0.0") + "m";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flagPos.easting = mf.pivotAxlePos.easting + (Math.Sin(mf.fixHeading) * entfernung);
            flagPos.northing = mf.pivotAxlePos.northing + (Math.Cos(mf.fixHeading) * entfernung);
            double lat = 0;
            double lon = 0;

            mf.pn.ConvertLocalToWGS84(flagPos.northing, flagPos.easting, out lat, out lon);

            mf.flagPts[mf.flagNumberPicked - 1].latitude = lat;
            mf.flagPts[mf.flagNumberPicked - 1].longitude = lon;
            mf.flagPts[mf.flagNumberPicked - 1].easting = flagPos.easting;
            mf.flagPts[mf.flagNumberPicked - 1].northing = flagPos.northing;
            UpdateLabels();
        }
    }
}