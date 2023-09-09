using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormPunkte : Form
    {
        private readonly FormGPS mf = null;
        double aktuellEast = 0;
        double aktuellNorth = 0;
        double gesucht = 0;
        double ergebnis = 0;
        double DistanzVonE = 0;
        double DistanzVonN = 0;
        double DistanzAB1E = 0;
        double DistanzAB1N = 0;
        double DistanzAB2E = 0;
        double DistanzAB2N = 0;
        private double easting, norting, latK, lonK;


        int SuchNr = -1;
        bool SuchDatei = false;
        int EinfuegeId = 0;

        string Textzeile;
        int iTrennZeichen;
        string sTrennZeichen = ";";
        string ZeilenKennung = "";
        int Lat;
        int Lon;
        double Latitude;
        double Longitude;
        string DateiPfad;
        public string Dateinamen;
        string Wert;


        private int A = 0, B = 0;
        //   private vec3[] arr;
        private CPunktFS Ap = new CPunktFS();
        private CPunktFS Bp = new CPunktFS();

        public FormPunkte(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }


        private void FormPunkte_Load(object sender, EventArgs e)
        {
            string dirField;
            if (mf.currentFieldDirectory.Length > 3)
            {
                dirField = mf.fieldsDirectory + mf.currentFieldDirectory + "\\";
            }
            else
            {
                dirField = mf.fieldsDirectory + "\\"; //fieldsDirectory Root
            }
            DateiPfad = dirField;
            lblDateinamen.Text = dirField + "GPSpositionen.txt";
            rbLL1.Checked = true;
            rbUTMNE1.Checked = false;
            //rbFutmNE1.Checked = false;
            lblLatNorthDiff1.Text = "0";
            lblLongEastDiff1.Text = "0";
            PunkteAuflisten();
            btnABerstellen1.Enabled = false;
            cbTrennZeichen1.Items.Add(";");
            cbTrennZeichen1.Items.Add("TAB");
            cbTrennZeichen1.Items.Add("Leerzeichen");
            cbTrennZeichen1.SelectedIndex = 0;
            nudX1.Value = 3;
            nudY1.Value = 4;
            SuchlDateilisteAnzeigen();


            lblZusatuDateiNamen1.Text = "";
        }
        private void SuchlDateilisteAnzeigen()
        {
            listBox1.Items.Clear();
            //txKennung1.Text = ZeilenKennung;
            //   listBox1.Items.AddRange(Directory.GetFiles(DateiPfad, "*.txt"));

            listBox1.Items.AddRange((from x in Directory.GetFiles(DateiPfad, "*.txt")//Alle Dateinamen ausfragen
                                     select //Nacheinander alle Dateinamen durchgehen
                                        new FileInfo(x).Name//Namen auswählen, es gibt natürlich noch alternativen

                                       ).ToArray());//In Array wandeln, damit AddRange funktioniert


        }
        private void ErstelleABaus2Punkte()
        {
            //calculate the AB Heading
            double abHead = Math.Atan2(Bp.FeldEast - Ap.FeldEast,
                Bp.FeldNorth - Ap.FeldNorth);
            if (abHead < 0) abHead += glm.twoPI;

            double offset = 0;// ((double)nudDistance.Value) / 200.0;

            double headingCalc = abHead + glm.PIBy2;

            mf.ABLine.lineArr.Add(new CABLines());
            mf.ABLine.numABLines = mf.ABLine.lineArr.Count;
            mf.ABLine.numABLineSelected = mf.ABLine.numABLines;

            int idx = mf.ABLine.numABLines - 1;

            mf.ABLine.lineArr[idx].heading = abHead;
            //calculate the new points for the reference line and points
            mf.ABLine.lineArr[idx].origin.easting = (Math.Sin(headingCalc) * Math.Abs(offset)) + Ap.FeldEast;
            mf.ABLine.lineArr[idx].origin.northing = (Math.Cos(headingCalc) * Math.Abs(offset)) + Ap.FeldNorth;

     
                headingCalc = abHead - glm.PIBy2;
                mf.ABLine.lineArr[idx].origin.easting = (Math.Sin(headingCalc) * Math.Abs(offset)) + Ap.FeldEast;
                mf.ABLine.lineArr[idx].origin.northing = (Math.Cos(headingCalc) * Math.Abs(offset)) + Ap.FeldNorth;
          
            //sin x cos z for endpoints, opposite for additional lines
           /* mf.ABLine.lineArr[idx].refPoint1.easting = mf.ABLine.lineArr[idx].origin.easting - (Math.Sin(mf.ABLine.lineArr[idx].heading) * 2000.0);
            mf.ABLine.lineArr[idx].refPoint1.northing = mf.ABLine.lineArr[idx].origin.northing - (Math.Cos(mf.ABLine.lineArr[idx].heading) * 2000.0);
            mf.ABLine.lineArr[idx].ref2.easting = mf.ABLine.lineArr[idx].origin.easting + (Math.Sin(mf.ABLine.lineArr[idx].heading) * 2000.0);
            mf.ABLine.lineArr[idx].ref2.northing = mf.ABLine.lineArr[idx].origin.northing + (Math.Cos(mf.ABLine.lineArr[idx].heading) * 2000.0);
            */
            //sin x cos z for endpoints, opposite for additional lines
            mf.ABLine.refABLineP1.easting = mf.ABLine.refPoint1.easting - (Math.Sin(abHead) * 2000);
            mf.ABLine.refABLineP1.northing = mf.ABLine.refPoint1.northing - (Math.Cos(abHead) * 2000);

            mf.ABLine.refABLineP2.easting = mf.ABLine.refPoint1.easting + (Math.Sin(abHead) * 2000);
            mf.ABLine.refABLineP2.northing = mf.ABLine.refPoint1.northing + (Math.Cos(abHead) * 2000);


            //create a name
            //mf.ABLine.lineArr[idx].Name = (Math.Round(glm.toDegrees(mf.ABLine.lineArr[idx].heading), 1)).ToString(CultureInfo.InvariantCulture)
            //      + "\u00B0" + mf.FindDirection(mf.ABLine.lineArr[idx].heading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
            mf.ABLine.lineArr[idx].Name = txtABnamen1.Text;

        }

        private void PunktVonAnwendungInPunktsuche()
        {
           /*
            txtLonEast1.Text = mf.pn.lonExtern.ToString();
            txtLatNorth1.Text = mf.pn.latExtern.ToString();
            mf.pn.neuExtern = false;
            rbLL1.Checked = true;
            */
        }
        private void PunkteAuflisten()
        {
            listPunkt1.Items.Clear();
            listAB1.Items.Clear();
            int i = 0;
            mf.FileLoadGPSpositionenFS();
            // ListViewItem itm;
            foreach (var item in mf.cPunktFs.punktArr)
            {
                //itm = new ListViewItem(item.Name);
                //listPunkt1.Items.Add(itm);
                if (rbLL1.Checked)
                {
                    listPunkt1.Items.Add(mf.cPunktFs.punktArr[i].Nr.ToString("000") + " " + mf.cPunktFs.punktArr[i].Longitude.ToString("###0.00000000") + " " + mf.cPunktFs.punktArr[i].Latidude.ToString("###0.00000000"));
                }
                if (rbUTMNE1.Checked)
                {
                    listPunkt1.Items.Add(mf.cPunktFs.punktArr[i].Nr.ToString("000") + " U " + mf.cPunktFs.punktArr[i].UTMEast.ToString("########0.00") + " " + mf.cPunktFs.punktArr[i].UTMNorth.ToString("#########0.00"));
                }
                //if (rbFutmNE1.Checked)
                //{
                //    listPunkt1.Items.Add(mf.cPunktFs.punktArr[i].Nr.ToString("000") + " F  " + mf.cPunktFs.punktArr[i].FeldEast.ToString("######0.00") + "  " + mf.cPunktFs.punktArr[i].FeldNorth.ToString("######0.00"));
                //}
                listAB1.Items.Add(mf.cPunktFs.punktArr[i].Nr.ToString("000") + " F  " + mf.cPunktFs.punktArr[i].FeldEast.ToString("######0.00") + "  " + mf.cPunktFs.punktArr[i].FeldNorth.ToString("######0.00"));

                i++;
            }
            lblAkoordinaten1.Text = "-";
            lblBkoordinaten1.Text = "-";
            A = 0;
            B = 0;

            if (SuchDatei)
            {
                listPunkt1.Items.Clear();
                i = 0;
                mf.FileLoadGPSpositionenSuche(0, (int)nudX1.Value, (int)nudY1.Value, (int)nudX1.Value, (int)nudY1.Value, 0);
                // ListViewItem itm;
                foreach (var item in mf.cPunktFSuche.punktArr)
                {
                    //itm = new ListViewItem(item.Name);
                    //listPunkt1.Items.Add(itm);
                    if (rbLL1.Checked)
                    {
                        listPunkt1.Items.Add(mf.cPunktFSuche.punktArr[i].Nr.ToString("000") + " " + mf.cPunktFSuche.punktArr[i].Longitude.ToString("###0.00000000") + " " + mf.cPunktFSuche.punktArr[i].Latidude.ToString("###0.00000000"));
                    }
                    if (rbUTMNE1.Checked)
                    {
                        listPunkt1.Items.Add(mf.cPunktFSuche.punktArr[i].Nr.ToString("000") + " U " + mf.cPunktFSuche.punktArr[i].UTMEast.ToString("########0.00") + " " + mf.cPunktFSuche.punktArr[i].UTMNorth.ToString("#########0.00"));
                    }

                    i++;
                }

            }
            BerechneFlaecheAusPunkte();
            // go to bottom of list - if there is a bottom
        }

        private void BerechneFlaecheAusPunkte()
        {
            int bp = 0;
            int fp = 0;
            int i = 0;
            double lonK = 0;
            double latK = 0;
            double norting = 0;
            double easting = 0;
            CBoundaryList NewF = new CBoundaryList();
            CBoundaryList NewB = new CBoundaryList();

            foreach (var item in mf.cPunktFs.punktArr)
            {
                //    listPunkt1.Items.Add(mf.cPunktFs.punktArr[i].Nr.ToString("000") + " " + mf.cPunktFs.punktArr[i].Longitude.ToString("###0.00000000") + " " + mf.cPunktFs.punktArr[i].Latidude.ToString("###0.00000000"));
                if (mf.cPunktFs.punktArr[i].Art == 1) { fp++; } // art 1 = aktives feld
                if (mf.cPunktFs.punktArr[i].Art == 1) { bp++; } // art 4 = brache
                i++;
            }

            if (fp > 2)
            {
                i = 0; // art 1 = aktives feld
                foreach (var item in mf.cPunktFs.punktArr)
                {

                    if (mf.cPunktFs.punktArr[i].Art == 1)
                    { // art 1 = aktives feld
                        lonK = mf.cPunktFs.punktArr[i].Longitude;
                        latK = mf.cPunktFs.punktArr[i].Latidude;
                        mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);
                        NewF.fenceLine.Add(new vec3(easting, norting, 0));
                    }
                    i++;
                }
                NewF.CalculateFenceArea(mf.bnd.bndList.Count);
                // mf.bnd.bndList.Add(NewF);
                lblFlaecheF1.Text = (NewF.area * 0.0001).ToString("0.0000") + " ha";
            }
            else
            {
                lblFlaecheF1.Text = "0.0000 ha";
            }


            if (bp > 2)
            {
                i = 0; // art 4 = aktives feld
                foreach (var item in mf.cPunktFs.punktArr)
                {

                    if (mf.cPunktFs.punktArr[i].Art == 4)
                    { // art 1 = aktives feld
                        lonK = mf.cPunktFs.punktArr[i].Longitude;
                        latK = mf.cPunktFs.punktArr[i].Latidude;
                        mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);
                        NewB.fenceLine.Add(new vec3(easting, norting, 0));
                    }
                    i++;
                }
                NewB.CalculateFenceArea(mf.bnd.bndList.Count);
                // mf.bnd.bndList.Add(NewB);
                lblFlaecheB1.Text = (NewB.area * 0.0001).ToString("0.0000") + " ha";
            }
            else
            {
                lblFlaecheB1.Text = "0.0000 ha";
            }

        }

        private double DistanzFS(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)); //Math.Sqrt(Math.Pow((Math.Abs(x2) - Math.Abs(x1)), 2) + Math.Pow((Math.Abs(y2) - Math.Abs(y1)), 2));
            //return Math.Sqrt(Math.Pow(((x2) - (x1)), 2) + Math.Pow(((y2) - (y1)), 2));
        }
        private double WinkelFS(double x1, double y1, double x2, double y2, double w1 = 0)
        {
            double ABHeading = Math.Atan2(y1 - y2, x1 - x2);//Math.Atan2(x1 -x2, y1 - y2);
            if (ABHeading < 0) ABHeading += glm.twoPI;
            if (ABHeading > glm.twoPI) ABHeading -= glm.twoPI;
            ABHeading /= glm.twoPI;
            ABHeading *= 360;
            w1 /= glm.twoPI;
            w1 *= 360;
            ABHeading = Math.Abs(ABHeading - w1);

            if (ABHeading > 180) ABHeading = 180-(ABHeading-180);// 180; // auf +-180 Grad beschränken
            if (ABHeading > 90) ABHeading = 90-(ABHeading-90); // 

            return ABHeading;
        }
        private double EntfernungAB1()
        {
            /*
             *   original Formel              //how far are we away from the reference line at 90 degrees
                distanceFromRefLine = ((dy * pivot.easting) - (dx * pivot.northing) + (refABLineP2.easting
                                        * refABLineP1.northing) - (refABLineP2.northing * refABLineP1.easting))
                                            / Math.Sqrt((dy * dy) + (dx * dx));
*/
            double entfernungAB = 0;
            //x2-x1
            double dx = mf.ABLine.refABLineP2.easting - mf.ABLine.refABLineP1.easting;
            //z2-z1
            double dy = mf.ABLine.refABLineP2.northing - mf.ABLine.refABLineP1.northing;

            double dd = Math.Sqrt((dy * dy) + (dx * dx));//diagonale


            //how far are we away from the reference line at 90 degrees
            entfernungAB = ((dy * mf.pn.fix.easting) - (dx * mf.pn.fix.northing) +
                              (mf.ABLine.refABLineP2.easting * mf.ABLine.refABLineP1.northing) - (mf.ABLine.refABLineP2.northing * mf.ABLine.refABLineP1.easting))
                                        / dd;
            return entfernungAB;
        }

        public double String2Double(string eingabestring)
        {
            if (eingabestring.Length == 0) { return 0; }
            //eingabestring.Replace(",", ".");// komma berichtigen
            double ausgabe = 0;
            string c = "";
            int i = 0;
            double x = 0;
            bool negativ = false;
            int stelle = 0;
            int komma = 0;

            while (i < eingabestring.Length)
            {
                c = eingabestring.Substring(i, 1);
                if ("-".Contains(c))
                {
                    negativ = true;
                }

                if (".,".Contains(c))
                {
                    komma = 1;
                    stelle = -1;
                }
                if ("0123456789".Contains(c))
                {
                    if (komma == 0)
                    {
                        x = Convert.ToDouble(c);
                        //x = x * Math.Pow(10, stelle);
                        ausgabe = (ausgabe * 10) + x;
                        stelle++;
                    }
                    else
                    {
                        x = Convert.ToDouble(c);
                        x = x * (Math.Pow(10, stelle));
                        ausgabe = ausgabe + x;
                        stelle--;
                    }
                }
                i++;
            }
            if (negativ) ausgabe *= -1;
            return ausgabe;
        }

    
        private void btnSpeichereGPSOB1_Click(object sender, EventArgs e)
        {
            if (cbPunktDieseZeile1.Checked)
            {
                EinfuegeId = listPunkt1.SelectedIndex;
                mf.GpsPositionenInsert(EinfuegeId, 2);
                PunkteAuflisten();
                listPunkt1.SelectedIndex = EinfuegeId + 1;
            }
            else
            {
                mf.FileCreateGPSpositionenFS(2); // auch unter AllePunkte speichern
                PunkteAuflisten();
            }
            mf.TimedMessageBox(1000, "Punkt angelegt", "Bearbeitung Punkt [" + (mf.PositionsNummer - 1).ToString() + "] aufgezeichnet");

        }

        private void btnspeichereGPSNB1_Click(object sender, EventArgs e)
        {
            if (cbPunktDieseZeile1.Checked)
            {
                EinfuegeId = listPunkt1.SelectedIndex;
                mf.GpsPositionenInsert(EinfuegeId, 4);
                PunkteAuflisten();
                listPunkt1.SelectedIndex = EinfuegeId + 1;
            }
            else
            {
                mf.FileCreateGPSpositionenFS(4); // auch unter AllePunkte speichern
                PunkteAuflisten();
            }
            mf.TimedMessageBox(1000, "Punkt angelegt", "Brache Punkt [" + (mf.PositionsNummer - 1).ToString() + "] aufgezeichnet");

        }

 

 
     
 

 
  
 
  
  


        private bool VerarbeiteTextzeile()
        {
            //string[] words = Textzeile.Split(cTrennZeichen);
            string Lats = "";
            string Lons = "";
            string s = "";
            string StartKennung = "";
            int ZeilenStart = 0;
            int mt = 0;
            switch (iTrennZeichen)
            {
                case 1:
                    sTrennZeichen = ";"; break;
                case 2:
                    sTrennZeichen = "\t"; break;
                case 3:
                    sTrennZeichen = " "; break;
            }
            //double.TryParse(words[Lat], out Latitude);
            //double.TryParse(words[Lat], out Longitude);
            int i = 0;
            int Trenner = 0;
            if (ZeilenKennung.Length > 0) ZeilenStart = 1;// startsequenz einleiten
            while (i < Textzeile.Length)
            {
                if (sTrennZeichen == Textzeile.Substring(i, 1) & ZeilenStart != 1)
                {
                    if (mt == 0 & sTrennZeichen == " ") //Bei Leerzeichen mehrfachtrenner als abstand herausfiltern
                    {
                        Trenner++; //1. spalte hat nr 1
                        if (Lat == Trenner) Lats = s;
                        if (Lon == Trenner) Lons = s;
                        s = "";
                        mt = 1;
                    }
                    if (mt == 0)// bei nicht leerzeichen
                    {
                        Trenner++; //1. spalte hat nr 1
                        if (Lat == Trenner) Lats = s;
                        if (Lon == Trenner) Lons = s;
                        s = "";
                    }
                }
                else
                {
                    mt = 0;// mehrfachtrennzeichen ausblenden
                    s = s + Textzeile.Substring(i, 1);
                    if ((ZeilenStart == 1) & (Trenner == 0))// Startkennung verarbeiten
                    {
                        if (StartKennung.Length < ZeilenKennung.Length)
                        {
                            StartKennung = s.ToUpper();// Startstring einlesen
                            if (StartKennung == ZeilenKennung) ZeilenStart = 2;
                        }
                    }

                }
                i++;
            }
            if (ZeilenStart != 1)
                Trenner++; //LETZTE spalte verarbeiten
            if (Lat == Trenner) Lats = s;
            if (Lon == Trenner) Lons = s;
            s = "";
            if (Lats.Length > 0 & Lons.Length > 0)
            {
                Latitude = String2Double(Lats); // Dezimalzeichen filtern
                Longitude = String2Double(Lons);// Dezimalzeichen filtern
                return true;

            }
            return false;
        }

        private bool LadeWert(string suchstring)
        {
            string s = "";
            Wert = "";
            int todo = 0;
            if (Textzeile.Contains(suchstring))
            {
                int i = 0;
                while (i < Textzeile.Length)
                {
                    s = Textzeile.Substring(i, 1);
                    if (s == "{")//beginn des Wertes
                    {
                        todo = 1;
                        s = "";
                        Wert = "";
                    }
                    if (s == "}")//ende des Wertes
                    {
                        todo = 3;
                        s = "";
                        return true;
                    }
                    if (todo == 1) Wert += s;
                    i++;
                }
            }
            return false;
        }

  

        private void FileLadeSucheTxt()
        {
            string Lat0 = "48.32";
            string Lon0 = "15.72";
            string Koordinaten;
            int PosZeilen = 0;
            lbSuche1.Items.Clear();
            //DateiPfad = mf.fieldsDirectory + mf.currentFieldDirectory + "\\";
            if (File.Exists(DateiPfad + Dateinamen + ".txt") == true)
            {

                File.OpenRead(DateiPfad + Dateinamen + ".txt");
                using (StreamReader reader = new StreamReader(DateiPfad + Dateinamen + ".txt"))

                // using (StreamWriter writer = new StreamWriter(DateiPfad + Dateinamen + "_GpsPositionenSuche.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        Textzeile = reader.ReadLine();
                        if (VerarbeiteTextzeile() == true)
                        {
                            PosZeilen++;
                            Lat0 = Latitude.ToString("###.0000000").Replace(",", ".");
                            Lon0 = Longitude.ToString("###.0000000").Replace(",", ".");
                            //Koordinaten = "Wgs84;" + Lon0 + ";" + Lat0 + ";" + PosZeilen.ToString("0000");

                            Koordinaten = PosZeilen.ToString("0000") + " " + Lon0 + " " + Lat0 + " ";
                            lbSuche1.Items.Add(Koordinaten);

                            //              writer.WriteLine(Koordinaten);

                        }

                    }

                }
            }
        }

  
   
        private void button2_Click(object sender, EventArgs e)
        {
            if (cbPunktDieseZeile1.Checked)
            {
                EinfuegeId = listPunkt1.SelectedIndex;
                mf.GpsPositionenInsert(EinfuegeId, 1);
                PunkteAuflisten();
                listPunkt1.SelectedIndex = EinfuegeId + 1;
            }
            else
            {
                mf.FileCreateGPSpositionenFS(1); // auch unter AllePunkte speichern
                PunkteAuflisten();
            }
            mf.TimedMessageBox(1000, "Punkt angelegt", "PositionsPunkt [" + (mf.PositionsNummer).ToString() + "] aufgezeichnet");

        }

 
 
        private void btnFS1_Click(object sender, EventArgs e)
        {
            //mf.FileLoadGPSpositionenAlt();
           // mf.FileSaveGPSpositionenFSvonAlt();
            PunkteAuflisten();
        }

 
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (mf.pn.neuExtern) { PunktVonAnwendungInPunktsuche(); }
            double N, E;
            mf.pn.ConvertWGS84ToLocal(mf.pn.latitude, mf.pn.longitude,  out N,  out E);
            //all the fixings and position
            //lblZone.Text = mf.Zone;
            double wL=0;
            if (mf.ABLine.isBtnABLineOn)
            {
                double wA = 0;
                wL = mf.ABLine.abHeading;
                wA = wL;
                if (wA < 0) wA += glm.twoPI;
                if (wA > glm.twoPI) wA -= glm.twoPI;
                wA /= glm.twoPI;
                wA *= 360;
                lblWLine.Text = wA.ToString("0.000000");
                //lblWPunkt.Text = Math.Round(mf.pn.fix.northing, 2).ToString("0.000000");
            }
            else
            {
                lblWLine.Text = 0.ToString("0.00");
                //lblWPunkt.Text = 0.ToString("0.00");
            }
            mf.pn.ConvertWGS84toMetersPerDegree(mf.pn.latitude, mf.pn.longitude, out N, out E);
            lblUTMeast1.Text = E.ToString("0.00");
            lblUTMnorth1.Text = N.ToString("0.00");

            lblLatitude.Text = mf.pn.latitude.ToString("0.00000000");
            lblLongitude.Text = mf.pn.longitude.ToString("0.00000000");
            //            lblLatitude.Text = mf.Latitude;
            //            lblLongitude.Text = mf.Longitude;
            lblAltitude.Text = mf.pn.altitude.ToString("0.00"); //mf.Altitude;
            int posnr = mf.PositionsNummer;
            lblPosNr1.Text = posnr.ToString("000");
            lblPSNr1.Text = posnr.ToString("000");


            double FPeast = String2Double(txtLonEast1.Text);
            double FPnorth = String2Double(txtLatNorth1.Text);
            double FPx;
            double FPy;
            mf.pn.ConvertWGS84ToLocal(FPnorth, FPeast, out FPy, out FPx);

            
            lblWPunkt.Text = WinkelFS(FPx, FPy, mf.pn.fix.easting, mf.pn.fix.northing, wL).ToString("0.000000");

            //lblDistanz1.Text = DistanzFS(FPx, FPy, mf.pn.fix.easting, mf.pn.fix.northing).ToString("0.00") + "m";
            //lblDistanz1.Text = DistanzFS(DistanzVonE, DistanzVonN, mf.pn.fix.easting, mf.pn.fix.northing).ToString("0.00") + " m";
            lblDistanz1.Text = Math.Sqrt(((FPx - mf.pn.fix.easting) * (FPx - mf.pn.fix.easting)) + ((FPy - mf.pn.fix.northing) * (FPy - mf.pn.fix.northing))).ToString("0.00");

            lblEastDiff1.Text = "E:" + (FPx - mf.pn.fix.easting).ToString("0.00") + "m";
            lblNorthDiff1.Text = "N:" + (FPy - mf.pn.fix.northing).ToString("0.00") + "m";


            if (rbLL1.Checked == true)
            {
                aktuellNorth = mf.pn.latitude;
                gesucht = String2Double(txtLatNorth1.Text);
                ergebnis = aktuellNorth - gesucht;
                lblLatNorthDiff1.Text = ergebnis.ToString("0.0000000");
                aktuellEast = mf.pn.longitude;
                gesucht = String2Double(txtLonEast1.Text);
                ergebnis = aktuellEast - gesucht;
                lblLongEastDiff1.Text = ergebnis.ToString("0.0000000");

             }
            if (rbUTMNE1.Checked == true)
            {
                aktuellNorth = mf.pn.north;
                gesucht = String2Double(txtLatNorth1.Text);
                ergebnis = aktuellNorth - gesucht;
                lblLatNorthDiff1.Text = ergebnis.ToString("0.00");
                aktuellEast = mf.pn.east;
                gesucht = String2Double(txtLonEast1.Text);
                ergebnis = aktuellEast - gesucht;
                lblLongEastDiff1.Text = ergebnis.ToString("0.00");
                lblDistanz1.Text = DistanzFS(DistanzVonE, DistanzVonN, mf.pn.fix.easting, mf.pn.fix.northing).ToString("0.00") + " m";
            }
            /*if (rbFutmNE1.Checked == true)
            {
                aktuellNorth = Math.Abs(mf.pn.fix.northing);
                gesucht = Math.Abs(String2Double(txtLatNorth1.Text));
                ergebnis = aktuellNorth - gesucht;
                lblLatNorthDiff1.Text = ergebnis.ToString("0.00");
                aktuellEast = Math.Abs(mf.pn.fix.easting);
                gesucht = Math.Abs(String2Double(txtLonEast1.Text));
                ergebnis = aktuellEast - gesucht;
                lblLongEastDiff1.Text = ergebnis.ToString("0.00");
                lblDistanz1.Text = DistanzFS(String2Double(txtLonEast1.Text), String2Double(txtLatNorth1.Text), mf.pn.fix.easting, mf.pn.fix.northing).ToString("0.00") + " m";
                lblDistanzFS2.Text = (EntfernungAB1()).ToString("0.00" + " m");
            }*/
            //SuchenTab
            aktuellEast = mf.pn.longitude;
            gesucht = String2Double(tbLonX1.Text);
            ergebnis = aktuellEast - gesucht;
            lblLonAW1.Text = ergebnis.ToString("0.0000000");
            aktuellNorth = mf.pn.latitude;
            gesucht = String2Double(tbLatY1.Text);
            ergebnis = aktuellNorth - gesucht;
            lblLatAW1.Text = ergebnis.ToString("0.0000000");



            //other sat and GPS info
            lblFixQuality.Text = mf.FixQuality;
            lblSatsTracked.Text = mf.SatsTracked;
            //lblStatus.Text = mf.pn.Status;
            lblHDOP.Text = mf.HDOP;
            /*
            tboxSerialFromRelay.Text = mf.mc.serialRecvRelayStr;
            tboxSerialToRelay.Text = mf.mc.relayData[0] + "," + mf.mc.relayData[1]
                 + "," + mf.mc.relayData[2] + "," + mf.mc.relayData[3] //relay and speed x 4
                 + "," + mf.mc.relayData[4] + "," + mf.mc.relayData[5] + "," + mf.mc.relayData[6]; //setpoint hi lo
            tboxNMEASerial.Text = mf.recvSentenceSettings;
            //tboxNMEASerial.Text = mainForm.pn.rawBuffer;

            tboxSerialFromAutoSteer.Text = mf.mc.serialRecvAutoSteerStr;
            tboxSerialToAutoSteer.Text = "32766, " + mf.mc.autoSteerData[mf.mc.sdRelayLo] + ", " + mf.mc.autoSteerData[mf.mc.sdSpeed]
                                    + ", " + mf.guidanceLineDistanceOff + ", " + mf.guidanceLineSteerAngle;
            */

        }

        private void btnDateiAuslesen1_Click_1(object sender, EventArgs e)
        {
            ZeilenKennung = txKennung1.Text;
            Lon = (int)nudX1.Value;
            Lat = (int)nudY1.Value;
            iTrennZeichen = cbTrennZeichen1.SelectedIndex + 1;
            //SichereEinstellungen();
            if (listBox1.SelectedIndex > -1)
            {
                Dateinamen = listBox1.SelectedItem.ToString();
                Dateinamen = Dateinamen.Substring(0, Dateinamen.IndexOf('.'));// ohne endung
                FileLadeSucheTxt();
                lblZusatuDateiNamen1.Text = Dateinamen;
                mf.TimedMessageBox(2000, "SuchDatei", "Suchdatei zu lesen versucht Ergebnis in Suchen->Liste");
            }
            else
            {
                lblZusatuDateiNamen1.Text = "";
            }

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (mf.Messpunkt)
            {
                mf.Messpunkt = false;
                mf.KompasAB = false;
                button2.BackColor = Color.Silver;
            }
            else
            {
                mf.Messpunkt = true;
                mf.KompasAB = false;
                button2.BackColor = Color.Blue;
            }
        }

  
        private void listPunkt1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i;
            i = listPunkt1.SelectedIndex;
            SuchNr = (int)mf.cPunktFs.punktArr[i].Nr;
            if (!cbPunktEingabefeld1.Checked) // wenn das feld gesetzt ist dann bei wiederholten anklichen der liste die eingabedaten nicht überschreiben
            {

                if (rbLL1.Checked)
                {
                    txtLonEast1.Text = mf.cPunktFs.punktArr[i].Longitude.ToString();
                    txtLatNorth1.Text = mf.cPunktFs.punktArr[i].Latidude.ToString();
                    double FPeast = mf.cPunktFs.punktArr[i].Longitude;
                    double FPnorth = mf.cPunktFs.punktArr[i].Latidude;
                    double N, E;
                    mf.pn.ConvertWGS84ToLocal(FPnorth, FPeast, out N, out E);
                    DistanzVonE = E;
                    DistanzVonN = N;

                }
                if (rbUTMNE1.Checked)
                {
                    txtLonEast1.Text = mf.cPunktFs.punktArr[i].UTMEast.ToString("0.00");
                    txtLatNorth1.Text = mf.cPunktFs.punktArr[i].UTMNorth.ToString("0.00");
                    DistanzVonE = mf.cPunktFs.punktArr[i].UTMEast;
                    DistanzVonN = mf.cPunktFs.punktArr[i].UTMNorth;

                }
                //if (rbFutmNE1.Checked)
                //{
                //    txtLonEast1.Text = mf.cPunktFs.punktArr[i].FeldEast.ToString("0.00");
                //    txtLatNorth1.Text = mf.cPunktFs.punktArr[i].FeldNorth.ToString("0.00");
                //}
                lblPSAltitude1.Text = mf.cPunktFs.punktArr[i].Altitude.ToString("0.00");
                //mf.PositionsNummer = (int) mf.cPunktFs.punktArr[i].Nr;
            }
        }

        private void label16_Click_1(object sender, EventArgs e)
        {
            DistanzVonE = mf.pn.fix.easting;
            DistanzVonN = mf.pn.fix.northing;
/*            if (rbLL1.Checked)
            {
                lblDistanz1.Text = DistanzFS(DistanzVonE, DistanzVonN, mf.pn.fix.easting, mf.pn.fix.northing).ToString("0.00") + " m";

            }
            if (rbUTMNE1.Checked)
            {
                 lblDistanz1.Text = DistanzFS(DistanzVonE, DistanzVonN, mf.pn.fix.easting, mf.pn.fix.northing).ToString("0.00") + " m";
            }
  */
        }

        private void btnPloeschen1_Click_1(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Punkt loeschen?",
gStr.gsArea,
MessageBoxButtons.YesNo,
MessageBoxIcon.Question,
MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {

                int x = -1;
                int i = 0;
                foreach (var item in mf.cPunktFs.punktArr)
                {
                    if (SuchNr == mf.cPunktFs.punktArr[i].Nr) { x = i; }
                    i++;
                }
                if (x >= 0)
                {

                    mf.cPunktFs.punktArr.RemoveAt(x);
                    mf.FileSaveGPSpositionenFS(); // auch unter AllePunkte speichern
                    mf.FileLoadGPSpositionenFS();
                    PunkteAuflisten();
                }
            }
        }

        private void btnSaveGPSposition1_Click(object sender, EventArgs e)
        {
            byte Art = 1; //1 = Aktives Feld, 2 = Sonstige Punkte , 4 = Brache
            double Lon = 0;
            double Lat = 0;
            if (cbPunktEingabefeld1.Checked)
            {
                Lon = String2Double(txtLonEast1.Text);
                Lat = String2Double(txtLatNorth1.Text); 
            }
            if (cbPunktDieseZeile1.Checked)
            {
                EinfuegeId = listPunkt1.SelectedIndex;
                mf.GpsPositionenInsert(EinfuegeId, Art, Lon, Lat);
                PunkteAuflisten();
                listPunkt1.SelectedIndex = EinfuegeId + 1;
            }
            else
            {
                    mf.FileCreateGPSpositionenFS(Art, Lon, Lat); // auch unter AllePunkte speichern
                    PunkteAuflisten();
            }
            mf.TimedMessageBox(1000, "Punkt angelegt", "PositionsPunkt [" + (mf.PositionsNummer).ToString() + "] aufgezeichnet");
        }

        private void btnCreateKml1_Click_1(object sender, EventArgs e)
        {
            mf.FileCreateAllGPSpositionenKML(Dateinamen);
            mf.TimedMessageBox(1000, "KML erstellen", "PositionsPunkte als KML erstellt");

        }

        private void btnLoeschePositionsdateien1_Click_1(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Punkt loeschen?",
 gStr.gsSetPoint,
 MessageBoxButtons.YesNo,
 MessageBoxIcon.Question,
 MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                mf.FileClearGPSpositionen();
                mf.TimedMessageBox(2000, "Punkt löschen", "PositionsPunkte gelöscht");
                mf.FileLoadGPSpositionenFS();
                PunkteAuflisten();

            }
        }

        private void rbLL1_CheckedChanged_1(object sender, EventArgs e)
        {
            PunkteAuflisten();
        }

        private void rbUTMNE1_CheckedChanged(object sender, EventArgs e)
        {
            PunkteAuflisten();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            byte Art = 2; //1 = Aktives Feld, 2 = Sonstige Punkte , 4 = Brache
            double Lon = 0;
            double Lat = 0;
            if (cbPunktEingabefeld1.Checked)
            {
                Lon = String2Double(txtLonEast1.Text);
                Lat = String2Double(txtLatNorth1.Text);
            }
            if (cbPunktDieseZeile1.Checked)
            {
                EinfuegeId = listPunkt1.SelectedIndex;
                mf.GpsPositionenInsert(EinfuegeId, Art, Lon, Lat);
                PunkteAuflisten();
                listPunkt1.SelectedIndex = EinfuegeId + 1;
            }
            else
            {
                mf.FileCreateGPSpositionenFS(Art, Lon, Lat); // auch unter AllePunkte speichern
                PunkteAuflisten();
            }
            mf.TimedMessageBox(1000, "Punkt angelegt", "PositionsPunkt [" + (mf.PositionsNummer).ToString() + "] aufgezeichnet");
        }

        private void btnFlag1_Click_1(object sender, EventArgs e)
        {
            int nextflag = mf.flagPts.Count + 1;
            CFlag flagPt = new CFlag(mf.pn.latitude, mf.pn.longitude, mf.pn.fix.easting, mf.pn.fix.northing, mf.fixHeading, mf.flagColor, nextflag, (nextflag).ToString());
            mf.flagPts.Add(flagPt);
            mf.FileSaveFlags();

        }

        private void btnPosSpeichern1_Click(object sender, EventArgs e)
        {
            if (cbPunktDieseZeile1.Checked)
            {
                EinfuegeId = listPunkt1.SelectedIndex;
                mf.GpsPositionenInsert(EinfuegeId, 1);
                PunkteAuflisten();
                listPunkt1.SelectedIndex = EinfuegeId + 1;
            }
            else
            {
                mf.FileCreateGPSpositionenFS(1); // auch unter AllePunkte speichern
                PunkteAuflisten();
            }
            mf.TimedMessageBox(1000, "Punkt angelegt", "PositionsPunkt [" + (mf.PositionsNummer).ToString() + "] aufgezeichnet");

        }

        private void btnAntenneA1_Click_1(object sender, EventArgs e)
        {
            A = 1;
            lblAkoordinaten1.Text = "Antenne" + "\n" + "E=" + mf.pn.fix.easting.ToString("0.00") + "\n" + "N=" + mf.pn.fix.northing.ToString("0.00");
            Ap.FeldEast = mf.pn.fix.easting;
            Ap.FeldNorth = mf.pn.fix.northing;
            btnABerstellenCheck();
            //für entfernungsmessung
            //rbFutmNE1.Checked = true;
            //txtLonEast1.Text = mf.pn.fix.easting.ToString("0.00");
            //txtLatNorth1.Text = mf.pn.fix.northing.ToString("0.00");
            DistanzAB1N = mf.pn.fix.northing;
            DistanzAB1E = mf.pn.fix.easting;

            lblAkoordinaten1.Text = "Antenne" + "\n" + "E=" + mf.pn.fix.easting.ToString("0.00") + "\n" + "N=" + mf.pn.fix.northing.ToString("0.00");
            btnABerstellenCheck();


        }

        private void btnAntenneB1_Click_1(object sender, EventArgs e)
        {
            B = 1;
            lblBkoordinaten1.Text = "Antenne" + "\n" + "E=" + mf.pn.fix.easting.ToString("0.00") + "\n" + "N=" + mf.pn.fix.northing.ToString("0.00");
            Bp.FeldEast = mf.pn.fix.easting;
            Bp.FeldNorth = mf.pn.fix.northing;
            DistanzAB2N = mf.pn.fix.northing;
            DistanzAB2E = mf.pn.fix.easting;

            lblBkoordinaten1.Text = "Antenne" + "\n" + "E=" + mf.pn.fix.easting.ToString("0.00") + "\n" + "N=" + mf.pn.fix.northing.ToString("0.00");
            btnABerstellenCheck();

        }

        private void label23_Click_1(object sender, EventArgs e)
        {
            lblAkoordinaten1.Text = "-";
            A = 0;
            DistanzAB1E = 0;
            DistanzAB1N = 0;
            btnABerstellenCheck();

        }

        private void label24_Click_1(object sender, EventArgs e)
        {
            lblBkoordinaten1.Text = "-";
            B = 0;
            DistanzAB2E = 0;
            DistanzAB2N = 0;
            btnABerstellenCheck();

        }

        private void listAB1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int verarbeiten = 1;
            int i = 0;
            string s = "";
            if (A == 0)
            {
                A = 1;
                //arr[A]
                i = listAB1.SelectedIndex;
                s = "Nr:" + mf.cPunktFs.punktArr[i].Nr.ToString() + "\n" + "E=" + mf.cPunktFs.punktArr[i].FeldEast.ToString("0.00") + "\n" + "N=" + mf.cPunktFs.punktArr[i].FeldNorth.ToString("0.00");
                lblAkoordinaten1.Text = s;
                verarbeiten = 0;
                Ap.FeldEast = mf.cPunktFs.punktArr[i].FeldEast;
                Ap.FeldNorth = mf.cPunktFs.punktArr[i].FeldNorth;
                // in Distanz AB laden
                DistanzAB1N = Ap.FeldNorth;
                DistanzAB1E = Ap.FeldEast;
            }
            if (B == 0 && verarbeiten == 1)
            {
                B = 1;
                //arr[A]
                i = listAB1.SelectedIndex;
                s = "Nr:" + mf.cPunktFs.punktArr[i].Nr.ToString() + "\n" + "E=" + mf.cPunktFs.punktArr[i].FeldEast.ToString("0.00") + "\n" + "N=" + mf.cPunktFs.punktArr[i].FeldNorth.ToString("0.00");
                lblBkoordinaten1.Text = s;
                verarbeiten = 0;
                Bp.FeldEast = mf.cPunktFs.punktArr[i].FeldEast;
                Bp.FeldNorth = mf.cPunktFs.punktArr[i].FeldNorth; 
                // In Distanz AB laden
                DistanzAB2N = Bp.FeldNorth;
                DistanzAB2E = Bp.FeldEast;
            }
            //für entfernungsmessung
            //rbFutmNE1.Checked = true;
            txtLonEast1.Text = mf.cPunktFs.punktArr[i].FeldEast.ToString();
            txtLatNorth1.Text = mf.cPunktFs.punktArr[i].FeldNorth.ToString();

            btnABerstellenCheck();

        }

        private void btnABerstellen1_Click_1(object sender, EventArgs e)
        {
            ErstelleABaus2Punkte();
            A = 0;
            B = 0;
            lblAkoordinaten1.Text = "-";
            lblBkoordinaten1.Text = "-";
            txtABnamen1.Text = "x";
            btnABerstellenCheck();
        }
        private void btnABerstellenCheck()
        {
            txtABnamen1.Text = DateTime.Now.ToString();
            if (A == 1 && B == 1)
            {
                btnABerstellen1.Enabled = true;
                lblDistanzAB1.Text = "Distanz: " + DistanzFS(DistanzAB1E , DistanzAB1N, DistanzAB2E , DistanzAB2N).ToString("0.00") + "m" ;
                //calculate the AB Heading
                double ABHeading = Math.Atan2(DistanzAB2E - DistanzAB1E, DistanzAB2N - DistanzAB1N);
                if (ABHeading < 0) ABHeading += glm.twoPI;
                ABHeading /= glm.twoPI;
                ABHeading *= 360;
                lblWinkelAB1.Text = "Winkel:" + ABHeading.ToString("0.000000") + "°";
            }
            else
            {
                btnABerstellen1.Enabled = false;
                lblDistanzAB1.Text = "Distanz: - m";
                lblWinkelAB1.Text = "Winkel: - °";
            }


        }

        private void btnSpeichereGPSOB1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnspeichereGPSNB1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSaveBrachePunkt1_Click(object sender, EventArgs e)
        {
            byte Art = 4; //1 = Aktives Feld, 2 = Sonstige Punkte , 4 = Brache
            double Lon = 0;
            double Lat = 0;
            if (cbPunktEingabefeld1.Checked)
            {
                Lon = String2Double(txtLonEast1.Text);
                Lat = String2Double(txtLatNorth1.Text);
            }
            if (cbPunktDieseZeile1.Checked)
            {
                EinfuegeId = listPunkt1.SelectedIndex;
                mf.GpsPositionenInsert(EinfuegeId, Art, Lon, Lat);
                PunkteAuflisten();
                listPunkt1.SelectedIndex = EinfuegeId + 1;
            }
            else
            {
                mf.FileCreateGPSpositionenFS(Art, Lon, Lat); // auch unter AllePunkte speichern
                PunkteAuflisten();
            }
            mf.TimedMessageBox(1000, "Punkt angelegt", "PositionsPunkt [" + (mf.PositionsNummer).ToString() + "] aufgezeichnet");
        }

        private void btnFS1_Click_1(object sender, EventArgs e)
        {

        }

        private void lblDistanz1_Click(object sender, EventArgs e)
        {

        }

        private void txtLonEast1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblUTMnorth1_Click(object sender, EventArgs e)
        {

        }

        private void lblUTMeast1_Click(object sender, EventArgs e)
        {

        }

        private void lblLongitude_Click(object sender, EventArgs e)
        {

        }

        private void lblLatitude_Click(object sender, EventArgs e)
        {

        }

        private void lblEasting_Click(object sender, EventArgs e)
        {

        }

        private void lblNorthing_Click(object sender, EventArgs e)
        {

        }

        private void btnGpsPunktUpdate1_Click(object sender, EventArgs e)
        {
            //byte Art = 1; //1 = Aktives Feld, 2 = Sonstige Punkte , 4 = Brache
            double Lon = 0;
            double Lat = 0;
            EinfuegeId = -1;
            if (cbPunktEingabefeld1.Checked)
            {
                Lon = String2Double(txtLonEast1.Text);
                Lat = String2Double(txtLatNorth1.Text);
            }
            EinfuegeId = listPunkt1.SelectedIndex;
            if (EinfuegeId > -1)
            { 
                mf.GpsPositionenUpdate(EinfuegeId, Lon, Lat);
                PunkteAuflisten();
                listPunkt1.SelectedIndex = EinfuegeId;
                mf.TimedMessageBox(1000, "Punkt Aktualisieren", "PositionsPunkt [" + (mf.PositionsNummer).ToString() + "] aktualisiert");
            }
        }

        private void btnKmlImport_Click(object sender, EventArgs e)
        {
            bool neu = false;
            if (mf.bnd.bndList.Count==0)
            {
                if (cbBoundary.Checked)
                {
                    DialogResult result3 = MessageBox.Show("Feldgrenze Neu importieren und \n ALLE DATEN Linien Kurven etc löschen ??? \n Position wird auf erste Kordinate gelegt!", //gStr.gsCompletelyDeleteBoundary,
                     "Feld NEU anlegen", //gStr.gsDeleteForSure,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                    if (result3 == DialogResult.Yes)
                    {
                        neu = true;
                    }
                }
            }
            mf.ImportFromKml(neu, cbBoundary.Checked, cbABLines.Checked, cbCurves.Checked, cbFlags.Checked);
        }

        private void btnKml2Txt_Click(object sender, EventArgs e)
        {
           
                string Lat0 = "48.32";
                string Lon0 = "15.72";
                string Koordinaten;
                string c, s;
                int PosZeilen = 0;
                int koord = 0;
                int i, lxa, lxe, la, le;
                int komma = 0;
                bool ke = false;
            Dateinamen = "";
            string schreibDatei = "";
            //string fileAndDirectory;

            //create the dialog instance
            OpenFileDialog ofd = new OpenFileDialog
            {
                //set the filter to text KML only
                Filter = "KML files (*.KML)|*.KML",

                //the initial directory, fields, for the open dialog
                InitialDirectory = mf.fieldsDirectory + mf.currentFieldDirectory
            };

            //was a file selected
            if (ofd.ShowDialog(this) == DialogResult.Cancel) return;
            else Dateinamen = ofd.FileName;
            try
            {


                Dateinamen = ofd.FileName;
                la = Dateinamen.LastIndexOf("\\") + 1;
                le = Dateinamen.LastIndexOf(".");

                schreibDatei = Dateinamen.Substring(la, le - la);

                if (File.Exists(Dateinamen) == true)
                {

                    File.OpenRead(Dateinamen);
                    using (StreamReader reader = new StreamReader(Dateinamen))

                    using (StreamWriter writer = new StreamWriter(mf.fieldsDirectory + mf.currentFieldDirectory + "\\" + schreibDatei + "_Kml_GpsPos_Suche.txt"))
                    {
                        while (!reader.EndOfStream)
                        {
                            Textzeile = reader.ReadLine();
                            s = "";
                            i = 0;
                            if (koord < 1)
                            {
                                i = Textzeile.IndexOf("<coordinates>");
                                if (i > -1)
                                {
                                    koord = 1; // Start gefunden
                                    i += 10; //zaehler nach koordinaten setzen
                                }
                            }
                            while (koord > 0 & i < Textzeile.Length)
                            {
                                c = Textzeile.Substring(i, 1);
                                if (c == " " | c == "<" | i == Textzeile.Length)
                                {
                                    ke = true; // kennune koordinaten trenner
                                    komma = 0;
                                    if (s.Length > 3)
                                    {
                                        Latitude = String2Double(s);
                                        s = "";
                                    }

                                }
                                else if (c == ",")
                                {
                                    if (komma == 0) //longitude fertig
                                    {
                                        Longitude = String2Double(s);
                                    }
                                    if (komma == 1)
                                    {
                                        Latitude = String2Double(s);
                                    }
                                    komma++;
                                    s = "";

                                }

                                else
                                {
                                    s += c; //zeichen summieren
                                }

                                if (ke & Longitude > 8 & Longitude < 25 & Latitude > 40 & Latitude < 55)
                                {
                                    //Sreibe Textzeile
                                    PosZeilen++;
                                    Lat0 = Latitude.ToString().Replace(",", ".");
                                    Lon0 = Longitude.ToString().Replace(",", ".");

                                    Koordinaten = "WGS84;" + PosZeilen.ToString("0000") + ";" + Lon0 + ";" + Lat0;
                                    writer.WriteLine(Koordinaten);
                                    ke = false;

                                }
                                i++;

                            }
                            if (PosZeilen > 0)
                            {
                                koord = 0;// bereits gelesen also fertig
                            }



                        }

                    }
                    if (PosZeilen > 0)
                    {
                        mf.TimedMessageBox(2000, "Erfolgreich", (PosZeilen.ToString() + " Positionszeilen in dieser Datei verarbeitet"));
                    }
                    else
                    {
                        mf.TimedMessageBox(2000, "Fehler", (PosZeilen.ToString() + " Datei nicht verarbeitet"));
                    }
                }
            }
            catch
            {
                mf.TimedMessageBox(2000, "Fehler"," Datei verarbeitungsfehler");
                return;
            }
            SuchlDateilisteAnzeigen();
        }

        private void btnSucheListeUebertragen1_Click(object sender, EventArgs e)
        {
            byte Art = 1; //1 = Aktives Feld, 2 = Sonstige Punkte , 4 = Brache
            double Lon = 0;
            double Lat = 0;
            int n = 0;
            int i = 0;

            //mf.FileClearGPSpositionen();
            mf.FileLoadGPSpositionenFS();
            mf.PositionsNummer = 0;
            if (lbSuche1.Items.Count > 0)
            {
                i = 0;

                while (i < lbSuche1.Items.Count)
                {
                    lbSuche1.SelectedIndex = i;
                    //lbSuche1.Item(i).ToString()
                    Lon = mf.String2Double(lbSuche1.SelectedItem.ToString().Substring(5, 10));
                    Lat = mf.String2Double(lbSuche1.SelectedItem.ToString().Substring(16, 10));
                    n = (int)mf.String2Double(lbSuche1.SelectedItem.ToString().Substring(1, 4));
                    //tbLonX1.Text = lbSuche1.SelectedItem.ToString().Substring(5, 10);
                    //tbLatY1.Text = lbSuche1.SelectedItem.ToString().Substring(16, 10);
                    mf.FileCreateGPSpositionenFS(Art, Lon, Lat); // als punkteFS speichern
                    i++;
                }


            }


            PunkteAuflisten();

            mf.TimedMessageBox(1000, "Punkt angelegt", "Feld-Punkte [" + (mf.PositionsNummer).ToString() + "] übertragen");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte Art = 2; //1 = Aktives Feld, 2 = Sonstige Punkte , 4 = Brache
            double Lon = 0;
            double Lat = 0;
            int n = 0;
            int i = 0;

            //mf.FileClearGPSpositionen();
            mf.FileLoadGPSpositionenFS();
            mf.PositionsNummer++;
            if (lbSuche1.Items.Count > 0)
            {
                i = 0;

                while (i < lbSuche1.Items.Count)
                {
                    lbSuche1.SelectedIndex = i;
                    //lbSuche1.Item(i).ToString()
                    Lon = mf.String2Double(lbSuche1.SelectedItem.ToString().Substring(5, 10));
                    Lat = mf.String2Double(lbSuche1.SelectedItem.ToString().Substring(16, 10));
                    n = (int)mf.String2Double(lbSuche1.SelectedItem.ToString().Substring(1, 4));
                    //tbLonX1.Text = lbSuche1.SelectedItem.ToString().Substring(5, 10);
                    //tbLatY1.Text = lbSuche1.SelectedItem.ToString().Substring(16, 10);
                    mf.FileCreateGPSpositionenFS(Art, Lon, Lat); // als punkteFS speichern
                    i++;
                }


            }


            PunkteAuflisten();

            mf.TimedMessageBox(1000, "Punkt angelegt", "Positions-Punkte [" + (mf.PositionsNummer).ToString() + "] übertragen");

        }

        private void btnKmlPosAllNew1_Click(object sender, EventArgs e)
        {

        }

        private void lbSuche1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lbSuche1.SelectedIndex > -1 & lbSuche1.SelectedItem.ToString().Length > 25)
            {
                tbLonX1.Text = lbSuche1.SelectedItem.ToString().Substring(5, 10);
                tbLatY1.Text = lbSuche1.SelectedItem.ToString().Substring(16, 10);

            }

        }

    

    }
}
