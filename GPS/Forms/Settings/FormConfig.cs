﻿//Please, if you use this, share the improvements

using AgOpenGPS.Properties;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormConfig : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        //brand variables
        TBrand brand;
        HBrand brandH;
        WDBrand brand4WD;

        //constructor
        public FormConfig(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();


            //Brand constructor
            brand = Settings.Default.setBrand_TBrand;

            if (brand == TBrand.Case)
                rbtnBrandTCase.Checked = true;
            else if (brand == TBrand.Claas)
                rbtnBrandTClaas.Checked = true;
            else if (brand == TBrand.Deutz)
                rbtnBrandTDeutz.Checked = true;
            else if (brand == TBrand.Fendt)
                rbtnBrandTFendt.Checked = true;
            else if (brand == TBrand.JDeere)
                rbtnBrandTJDeere.Checked = true;
            else if (brand == TBrand.Kubota)
                rbtnBrandTKubota.Checked = true;
            else if (brand == TBrand.Massey)
                rbtnBrandTMassey.Checked = true;
            else if (brand == TBrand.NewHolland)
                rbtnBrandTNH.Checked = true;
            else if (brand == TBrand.Same)
                rbtnBrandTSame.Checked = true;
            else if (brand == TBrand.Steyr)
                rbtnBrandTSteyr.Checked = true;
            else if (brand == TBrand.Ursus)
                rbtnBrandTUrsus.Checked = true;
            else if (brand == TBrand.Valtra)
                rbtnBrandTValtra.Checked = true;
            else
                rbtnBrandTAoG.Checked = true;


            brandH = Settings.Default.setBrand_HBrand;


            if (brandH == HBrand.Case)
                rbtnBrandHCase.Checked = true;
            else if (brandH == HBrand.Claas)
                rbtnBrandHClaas.Checked = true;
            else if (brandH == HBrand.JDeere)
                rbtnBrandHJDeere.Checked = true;
            else if (brandH == HBrand.NewHolland)
                rbtnBrandHNH.Checked = true;
            else
                rbtnBrandHAoG.Checked = true;


            brand4WD = Settings.Default.setBrand_WDBrand;


            if (brand4WD == WDBrand.Case)
                rbtnBrand4WDCase.Checked = true;
            else if (brand4WD == WDBrand.Challenger)
                rbtnBrand4WDChallenger.Checked = true;
            else if (brand4WD == WDBrand.JDeere)
                rbtnBrand4WDJDeere.Checked = true;
            else if (brand4WD == WDBrand.NewHolland)
                rbtnBrand4WDNH.Checked = true;
            else
                rbtnBrand4WDAoG.Checked = true;


            tab1.Appearance = TabAppearance.FlatButtons;
            tab1.ItemSize = new Size(0, 1);
            tab1.SizeMode = TabSizeMode.Fixed;

            HideSubMenu();

            nudTrailingHitchLength.Controls[0].Enabled = false;
            nudDrawbarLength.Controls[0].Enabled = false;
            nudTankHitch.Controls[0].Enabled = false;


            nudLookAhead.Controls[0].Enabled = false;
            nudLookAheadOff.Controls[0].Enabled = false;
            nudTurnOffDelay.Controls[0].Enabled = false;
            nudOffset.Controls[0].Enabled = false;
            nudOverlap.Controls[0].Enabled = false;
            nudCutoffSpeed.Controls[0].Enabled = false;


            nudMinTurnRadius.Controls[0].Enabled = false;
            nudAntennaHeight.Controls[0].Enabled = false;
            nudAntennaOffset.Controls[0].Enabled = false;
            nudAntennaPivot.Controls[0].Enabled = false;
            nudLightbarCmPerPixel.Controls[0].Enabled = false;
            nudVehicleTrack.Controls[0].Enabled = false;
            nudSnapDistance.Controls[0].Enabled = false;
            nudABLength.Controls[0].Enabled = false;
            nudWheelbase.Controls[0].Enabled = false;
            nudLineWidth.Controls[0].Enabled = false;

            nudMinCoverage.Controls[0].Enabled = false;
            nudDefaultSectionWidth.Controls[0].Enabled = false;

            nudSection1.Controls[0].Enabled = false;
            nudSection2.Controls[0].Enabled = false;
            nudSection3.Controls[0].Enabled = false;
            nudSection4.Controls[0].Enabled = false;
            nudSection5.Controls[0].Enabled = false;
            nudSection6.Controls[0].Enabled = false;
            nudSection7.Controls[0].Enabled = false;
            nudSection8.Controls[0].Enabled = false;
            nudSection9.Controls[0].Enabled = false;
            nudSection10.Controls[0].Enabled = false;
            nudSection11.Controls[0].Enabled = false;
            nudSection12.Controls[0].Enabled = false;
            nudSection13.Controls[0].Enabled = false;
            nudSection14.Controls[0].Enabled = false;
            nudSection15.Controls[0].Enabled = false;
            nudSection16.Controls[0].Enabled = false;

            nudMinFixStepDistance.Controls[0].Enabled = false;
            nudStartSpeed.Controls[0].Enabled = false;

            nudForwardComp.Controls[0].Enabled = false;
            nudReverseComp.Controls[0].Enabled = false;

            nudAgeAlarm.Controls[0].Enabled = false;

            nudMaxCounts.Controls[0].Enabled = false;
            nudRaiseTime.Controls[0].Enabled = false;
            nudLowerTime.Controls[0].Enabled = false;

            nudTramWidth.Controls[0].Enabled = false;
            nudMenusOnTime.Controls[0].Enabled = false;

            nudGuidanceLookAhead.Controls[0].Enabled = false;

            //nudTramBoundaryAdd1.Enabled = false;
            //nudTramCount1.Enabled = false;

        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            //seince we rest, save current state
            mf.SaveFormGPSWindowSettings();

            if (mf.isMetric)
            {
                lblInchesCm.Text = gStr.gsCentimeters;
                lblFeetMeters.Text = gStr.gsMeters;
                lblSecTotalWidthFeet.Visible = false;
                lblSecTotalWidthInches.Visible = false;
                lblSecTotalWidthMeters.Visible = true;
            }
            else
            {
                lblInchesCm.Text = gStr.gsInches;
                lblFeetMeters.Text = "Feet";
                lblSecTotalWidthFeet.Visible = true;
                lblSecTotalWidthInches.Visible = true;
                lblSecTotalWidthMeters.Visible = false;
            }

            //update the first child form summary data items
            UpdateSummary();

            //metric or imp on spinners min/maxes
            FixMinMaxSpinners();

            //the pick a saved vehicle box
            UpdateVehicleListView();

            //the tool size in bottom panel
            if (mf.isMetric)
            {
                lblSecTotalWidthMeters.Text = (mf.tool.toolWidth * 100) + " cm";
            }
            else
            {
                double toFeet = mf.tool.toolWidth * 100 * 0.0328084;
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';
            }

            chkDisplaySky.Checked = mf.isSkyOn;
            chkDisplayFloor.Checked = mf.isTextureOn;
            chkDisplayGrid.Checked = mf.isGridOn;
            chkDisplaySpeedo.Checked = mf.isSpeedoOn;
            chkDisplayDayNight.Checked = mf.isAutoDayNight;
            chkDisplayStartFullScreen.Checked = Properties.Settings.Default.setDisplay_isStartFullScreen;
            chkDisplayExtraGuides.Checked = mf.isSideGuideLines;
            chkDisplayLogNMEA.Checked = mf.isLogNMEA;
            chkDisplayPolygons.Checked = mf.isDrawPolygons;
            chkDisplayLightbar.Checked = mf.isLightbarOn;
            chkDisplayKeyboard.Checked = mf.isKeyboardOn;

            if (mf.isMetric) rbtnDisplayMetric.Checked = true;
            else rbtnDisplayImperial.Checked = true;

            //nudMenusOnTime.Value = mf.timeToShowMenus;

            nudTramBoundaryAdd1.Value = (decimal)Properties.Settings.Default.setTram_BoundaryAdd;
            nudTramCount1.Value = (int)Properties.Settings.Default.setTram_Counter;

            tab1.SelectedTab = tabSummary;
            tboxVehicleNameSave.Focus();
            if (Properties.Settings.Default.setUturnAutoRadius)
            {
                btnUturnAutoRadius1.BackColor = Color.LightGreen;
            }
            else
            {
                btnUturnAutoRadius1.BackColor = Color.Tomato;
            }
            //hsbHeadingFilter1.Value = Settings.Default.setHeadingFilter;
            //LblHeadingFilter1.Text = Settings.Default.setHeadingFilter.ToString();

            mf.CloseTopMosts();

        }

        private void FormConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDisplaySettings();

            //reload all the settings from default and user.config
            mf.LoadSettings();

            //save current vehicle
            SettingsIO.ExportAll(mf.vehiclesDirectory + mf.vehicleFileName + ".XML");

        }

        private void FixMinMaxSpinners()
        {
            if (!mf.isMetric)
            {
                nudTankHitch.Maximum = (Math.Round(nudTankHitch.Maximum / 2.54M));
                nudTankHitch.Minimum = Math.Round(nudTankHitch.Minimum / 2.54M);

                nudDrawbarLength.Maximum = Math.Round(nudDrawbarLength.Maximum / 2.54M);
                nudDrawbarLength.Minimum = Math.Round(nudDrawbarLength.Minimum / 2.54M);

                nudTrailingHitchLength.Maximum = Math.Round(nudTrailingHitchLength.Maximum / 2.54M);
                nudTrailingHitchLength.Minimum = Math.Round(nudTrailingHitchLength.Minimum / 2.54M);

                nudSnapDistance.Maximum = Math.Round(nudSnapDistance.Maximum / 2.54M);
                nudSnapDistance.Minimum = Math.Round(nudSnapDistance.Minimum / 2.54M);

                nudLightbarCmPerPixel.Maximum = Math.Round(nudLightbarCmPerPixel.Maximum / 2.54M);
                nudLightbarCmPerPixel.Minimum = Math.Round(nudLightbarCmPerPixel.Minimum / 2.54M);

                nudVehicleTrack.Maximum = Math.Round(nudVehicleTrack.Maximum / 2.54M);
                nudVehicleTrack.Minimum = Math.Round(nudVehicleTrack.Minimum / 2.54M);

                nudWheelbase.Maximum = Math.Round(nudWheelbase.Maximum / 2.54M);
                nudWheelbase.Minimum = Math.Round(nudWheelbase.Minimum / 2.54M);

                nudMinTurnRadius.Maximum = Math.Round(nudMinTurnRadius.Maximum / 2.54M);
                nudMinTurnRadius.Minimum = Math.Round(nudMinTurnRadius.Minimum / 2.54M);

                //.Maximum = Math.Round(/2.54M);
                //.Minimum = Math.Round(/2.54M);

                nudOverlap.Maximum = Math.Round(nudOverlap.Maximum / 2.54M);
                nudOverlap.Minimum = Math.Round(nudOverlap.Minimum / 2.54M);

                nudOffset.Maximum = Math.Round(nudOffset.Maximum / 2.54M);
                nudOffset.Minimum = Math.Round(nudOffset.Minimum / 2.54M);

                nudCutoffSpeed.Maximum = Math.Round(nudCutoffSpeed.Maximum / 1.60934M);
                nudCutoffSpeed.Minimum = Math.Round(nudCutoffSpeed.Minimum / 1.60934M);

                nudDefaultSectionWidth.Maximum = Math.Round(nudDefaultSectionWidth.Maximum / 2.54M);
                nudDefaultSectionWidth.Minimum = Math.Round(nudDefaultSectionWidth.Minimum / 2.54M);

                nudSection1.Maximum = Math.Round(nudSection1.Maximum / 2.54M);
                nudSection1.Minimum = Math.Round(nudSection1.Minimum / 2.54M);
                nudSection2.Maximum = Math.Round(nudSection2.Maximum / 2.54M);
                nudSection2.Minimum = Math.Round(nudSection2.Minimum / 2.54M);
                nudSection3.Maximum = Math.Round(nudSection3.Maximum / 2.54M);
                nudSection3.Minimum = Math.Round(nudSection3.Minimum / 2.54M);
                nudSection4.Maximum = Math.Round(nudSection4.Maximum / 2.54M);
                nudSection4.Minimum = Math.Round(nudSection4.Minimum / 2.54M);
                nudSection5.Maximum = Math.Round(nudSection5.Maximum / 2.54M);
                nudSection5.Minimum = Math.Round(nudSection5.Minimum / 2.54M);
                nudSection6.Maximum = Math.Round(nudSection6.Maximum / 2.54M);
                nudSection6.Minimum = Math.Round(nudSection6.Minimum / 2.54M);
                nudSection7.Maximum = Math.Round(nudSection7.Maximum / 2.54M);
                nudSection7.Minimum = Math.Round(nudSection7.Minimum / 2.54M);
                nudSection8.Maximum = Math.Round(nudSection8.Maximum / 2.54M);
                nudSection8.Minimum = Math.Round(nudSection8.Minimum / 2.54M);
                nudSection9.Maximum = Math.Round(nudSection9.Maximum / 2.54M);
                nudSection9.Minimum = Math.Round(nudSection9.Minimum / 2.54M);
                nudSection10.Maximum = Math.Round(nudSection10.Maximum / 2.54M);
                nudSection10.Minimum = Math.Round(nudSection10.Minimum / 2.54M);
                nudSection11.Maximum = Math.Round(nudSection11.Maximum / 2.54M);
                nudSection11.Minimum = Math.Round(nudSection11.Minimum / 2.54M);
                nudSection12.Maximum = Math.Round(nudSection12.Maximum / 2.54M);
                nudSection12.Minimum = Math.Round(nudSection12.Minimum / 2.54M);
                nudSection13.Maximum = Math.Round(nudSection13.Maximum / 2.54M);
                nudSection13.Minimum = Math.Round(nudSection13.Minimum / 2.54M);
                nudSection14.Maximum = Math.Round(nudSection14.Maximum / 2.54M);
                nudSection14.Minimum = Math.Round(nudSection14.Minimum / 2.54M);
                nudSection15.Maximum = Math.Round(nudSection15.Maximum / 2.54M);
                nudSection15.Minimum = Math.Round(nudSection15.Minimum / 2.54M);
                nudSection16.Maximum = Math.Round(nudSection16.Maximum / 2.54M);
                nudSection16.Minimum = Math.Round(nudSection16.Minimum / 2.54M);

                nudTramWidth.Minimum = Math.Round(nudTramWidth.Minimum / 2.54M);
                nudTramWidth.Maximum = Math.Round(nudTramWidth.Maximum / 2.54M);

                nudSnapDistance.Minimum = Math.Round(nudSnapDistance.Minimum / 2.54M);
                nudSnapDistance.Maximum = Math.Round(nudSnapDistance.Maximum / 2.54M);

                nudLightbarCmPerPixel.Minimum = Math.Round(nudLightbarCmPerPixel.Minimum / 2.54M);
                nudLightbarCmPerPixel.Maximum = Math.Round(nudLightbarCmPerPixel.Maximum / 2.54M);

                //Meters to feet
                nudTurnDistanceFromBoundary.Minimum = Math.Round(nudTurnDistanceFromBoundary.Minimum * 3.28M);
                nudTurnDistanceFromBoundary.Maximum = Math.Round(nudTurnDistanceFromBoundary.Maximum * 3.28M);

                nudABLength.Minimum = Math.Round(nudABLength.Minimum * 3.28M);
                nudABLength.Maximum = Math.Round(nudABLength.Maximum * 3.28M);

                lblTurnOffBelowUnits.Text = gStr.gsMPH;
                //cutoffMetricImperial = 1.60934;

            }
            else
            {
                lblTurnOffBelowUnits.Text = gStr.gsKMH;
                //cutoffMetricImperial = 1;
            }
            hsbHeadingFilter1.Value = (int)(mf.HeadingFilter);
            LblHeadingFilter1.Text = mf.HeadingFilter.ToString();
            hsbSteerOnDist.Value = Settings.Default.setSteerOnDist;
            lblSteerOnDist.Text = (mf.vehicle.steerOnDist).ToString("0.0") + "m";
            hsbSteerOffDist.Value = Settings.Default.setSteerOffDist;
            lblSteerOffDist.Text = (mf.vehicle.steerOffDist).ToString("0.0") + "m";


        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setTram_BoundaryAdd = (double)nudTramBoundaryAdd1.Value;
            Properties.Settings.Default.setTram_Counter = (int)nudTramCount1.Value;

            Close();
        }

        private void tabSummary_Enter(object sender, EventArgs e)
        {
            chkDisplaySky.Checked = mf.isSkyOn;
            chkDisplayFloor.Checked = mf.isTextureOn;
            chkDisplayGrid.Checked = mf.isGridOn;
            chkDisplaySpeedo.Checked = mf.isSpeedoOn;
            chkDisplayDayNight.Checked = mf.isAutoDayNight;
            chkDisplayStartFullScreen.Checked = Properties.Settings.Default.setDisplay_isStartFullScreen;
            chkDisplayExtraGuides.Checked = mf.isSideGuideLines;
            chkDisplayLogNMEA.Checked = mf.isLogNMEA;
            chkDisplayPolygons.Checked = mf.isDrawPolygons;
            chkDisplayLightbar.Checked = mf.isLightbarOn;
            chkDisplayKeyboard.Checked = mf.isKeyboardOn;
            //nudMenusOnTime.Value = mf.timeToShowMenus;

            if (mf.isMetric) rbtnDisplayMetric.Checked = true;
            else rbtnDisplayImperial.Checked = true;
        }

        private void tabSummary_Leave(object sender, EventArgs e)
        {
            SaveDisplaySettings();
        }

        //Check Brand is changed
        private void rbtnBrandTAoG_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.AGOpenGPS;
        }

        private void rbtnBrandTCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Case;
        }

        private void rbtnBrandTClaas_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Claas;
        }

        private void rbtnBrandTDeutz_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Deutz;
        }

        private void rbtnBrandTFendt_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Fendt;
        }

        private void rbtnBrandTJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.JDeere;
        }

        private void rbtnBrandTKubota_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Kubota;
        }

        private void rbtnBrandTMassey_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Massey;
        }

        private void rbtnBrandTNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.NewHolland;
        }

        private void rbtnBrandTSame_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Same;
        }

        private void rbtnBrandTSteyr_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Steyr;
        }

        private void rbtnBrandTUrsus_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Ursus;
        }

        private void rbtnBrandTValtra_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand = TBrand.Valtra;
        }

        private void rbtnBrandHAoG_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brandH = HBrand.AGOpenGPS;
        }

        private void rbtnBrandHCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brandH = HBrand.Case;
        }

        private void rbtnBrandHClaas_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brandH = HBrand.Claas;
        }

        private void rbtnBrandHJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brandH = HBrand.JDeere;
        }
        private void rbtnBrandHNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brandH = HBrand.NewHolland;
        }
        private void rbtnBrand4WDAoG_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand4WD = WDBrand.AGOpenGPS;
        }

        private void rbtnBrand4WDCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand4WD = WDBrand.Case;
        }

        private void rbtnBrand4WDChallenger_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand4WD = WDBrand.Challenger;
        }

        private void rbtnBrand4WDJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand4WD = WDBrand.JDeere;
        }
        private void rbtnBrand4WDNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                brand4WD = WDBrand.NewHolland;
        }

        private void tabVBrand_Leave(object sender, EventArgs e)
        {
            if (rbtnTractor.Checked == true)
            {
                Settings.Default.setBrand_TBrand = brand;

                Bitmap bitmap = mf.GetTractorBrand(brand);


                //GL.GenTextures(1, out mf.texture[13]);//Already done on startup
                //Draw vehicle by brand
                GL.BindTexture(TextureTarget.Texture2D, mf.texture[13]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtnHarvester.Checked == true)

            {
                Settings.Default.setBrand_HBrand = brandH;
                Bitmap bitmap = mf.GetHarvesterBrand(brandH);


                GL.BindTexture(TextureTarget.Texture2D, mf.texture[18]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtn4WD.Checked == true)

            {
                Settings.Default.setBrand_WDBrand = brand4WD;
                Bitmap bitmap = mf.Get4WDBrandFront(brand4WD);


                GL.BindTexture(TextureTarget.Texture2D, mf.texture[16]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }

            if (rbtn4WD.Checked == true)

            {
                Settings.Default.setBrand_WDBrand = brand4WD;
                Bitmap bitmap = mf.Get4WDBrandRear(brand4WD);


                GL.BindTexture(TextureTarget.Texture2D, mf.texture[17]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }
        }

 
        private void nudTramBoundaryAdd1_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnOK.Focus();
        }

         private void nudTramCount1_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnOK.Focus();
        }

        private void btnUturnAutoRadius1_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setUturnAutoRadius)
            {
                Properties.Settings.Default.setUturnAutoRadius = false;
                btnUturnAutoRadius1.BackColor = Color.Tomato;
            }
            else
            {
                Properties.Settings.Default.setUturnAutoRadius = true;
                btnUturnAutoRadius1.BackColor = Color.LightGreen; 
            }

        }

        private void hsbHeadingFilter1_Scroll(object sender, ScrollEventArgs e)
        {
            Settings.Default.setHeadingFilter = hsbHeadingFilter1.Value;
            LblHeadingFilter1.Text = Settings.Default.setHeadingFilter.ToString();
            mf.HeadingFilter= hsbHeadingFilter1.Value;
        }

        private void hsbarFusion_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void hsbSteerOnDist_Scroll(object sender, ScrollEventArgs e)
        {
            Settings.Default.setSteerOnDist = hsbSteerOnDist.Value;
            mf.vehicle.steerOnDist = (double)Settings.Default.setSteerOnDist / 2;
            lblSteerOnDist.Text = (mf.vehicle.steerOnDist).ToString("0.0") + "m";
        }

        private void hsbSteerOffDist_Scroll(object sender, ScrollEventArgs e)
        {
            Settings.Default.setSteerOffDist = hsbSteerOffDist.Value;
            mf.vehicle.steerOffDist = (double)Settings.Default.setSteerOffDist / 2;
            lblSteerOffDist.Text = (mf.vehicle.steerOffDist).ToString("0.0") + "m";
        }
    }
}

