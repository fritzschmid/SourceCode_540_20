using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Xml;
using System.Text;

namespace AgOpenGPS
{
    
    public partial class FormGPS
    {
        //list of the list of patch data individual triangles for field sections
        public List<List<vec3>> patchSaveList = new List<List<vec3>>();

        //list of the list of patch data individual triangles for contour tracking
        public List<List<vec3>> contourSaveList = new List<List<vec3>>();

        public string Dateinamen = "";
        public double lonK, latK ,easting, northing;
        private vec2 Ap, Bp;
        private vec2 Cp;
        private double abHead;

        public void FileSaveCurveLines()
        {
            curve.moveDistance = 0;

            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            int cnt = curve.curveArr.Count;
            curve.numCurveLines = cnt;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    if (cnt > 0)
                    {
                        writer.WriteLine("$CurveLines");

                        for (int i = 0; i < cnt; i++)
                        {
                            //write out the Name
                            writer.WriteLine(curve.curveArr[i].Name);

                            //write out the aveheading
                            writer.WriteLine(curve.curveArr[i].aveHeading.ToString(CultureInfo.InvariantCulture));

                            //write out the points of ref line
                            int cnt2 = curve.curveArr[i].curvePts.Count;

                            writer.WriteLine(cnt2.ToString(CultureInfo.InvariantCulture));
                            if (curve.curveArr[i].curvePts.Count > 0)
                            {
                                for (int j = 0; j < cnt2; j++)
                                    writer.WriteLine(Math.Round(curve.curveArr[i].curvePts[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                        Math.Round(curve.curveArr[i].curvePts[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                            Math.Round(curve.curveArr[i].curvePts[j].heading, 5).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                    else
                    {
                        writer.WriteLine("$CurveLines");
                    }
                }
                catch (Exception er)
                {
                    WriteErrorLog("Saving Curve Line" + er.ToString());

                    return;
                }
            }

            if (curve.numCurveLines == 0) curve.numCurveLineSelected = 0;
            if (curve.numCurveLineSelected > curve.numCurveLines) curve.numCurveLineSelected = curve.numCurveLines;

        }

        public void FileLoadCurveLines()
        {
            curve.moveDistance = 0;

            curve.curveArr?.Clear();
            curve.numCurveLines = 0;

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("$CurveLines");
                }
            }

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, gStr.gsFileError, "Missing Curve File");
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;

                        //read header $CurveLine
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            curve.curveArr.Add(new CCurveLines());

                            //read header $CurveLine
                            curve.curveArr[curve.numCurveLines].Name = reader.ReadLine();
                            // get the average heading
                            line = reader.ReadLine();
                            curve.curveArr[curve.numCurveLines].aveHeading = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 1)
                            {
                                curve.curveArr[curve.numCurveLines].curvePts?.Clear();

                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec3 vecPt = new vec3(double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));
                                    curve.curveArr[curve.numCurveLines].curvePts.Add(vecPt);
                                }
                                curve.numCurveLines++;
                            }
                            else
                            {
                                if (curve.curveArr.Count > 0)
                                {
                                    curve.curveArr.RemoveAt(curve.numCurveLines);
                                }
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsCurveLineFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show(this);
                        WriteErrorLog("Load Curve Line" + er.ToString());
                    }
                }
            }

            if (curve.numCurveLines == 0) curve.numCurveLineSelected = 0;
            if (curve.numCurveLineSelected > curve.numCurveLines) curve.numCurveLineSelected = curve.numCurveLines;
        }

        public void FileSaveABLines()
        {
            ABLine.moveDistance = 0;

            //make sure at least a global blank AB Line file exists
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\ABLines.txt";
            int cnt = ABLine.lineArr.Count;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                if (cnt > 0)
                {
                    foreach (var item in ABLine.lineArr)
                    {
                        //make it culture invariant
                        string line = item.Name
                            + ',' + (Math.Round(glm.toDegrees(item.heading), 8)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.origin.easting, 3)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.origin.northing, 3)).ToString(CultureInfo.InvariantCulture);

                        //write out to file
                        writer.WriteLine(line);
                    }
                }
            }

            if (ABLine.numABLines == 0) ABLine.numABLineSelected = 0;
            if (ABLine.numABLineSelected > ABLine.numABLines) ABLine.numABLineSelected = ABLine.numABLines;
        }

        public void FileLoadABLines()
        {
            ABLine.moveDistance = 0;

            //make sure at least a global blank AB Line file exists
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\ABLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                }
            }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, gStr.gsFileError, gStr.gsMissingABLinesFile);
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;
                        ABLine.numABLines = 0;
                        ABLine.numABLineSelected = 0;
                        ABLine.lineArr?.Clear();

                        //read all the lines
                        for (int i = 0; !reader.EndOfStream; i++)
                        {

                            line = reader.ReadLine();
                            string[] words = line.Split(',');

                            if (words.Length != 4) break;

                            ABLine.lineArr.Add(new CABLines());

                            ABLine.lineArr[i].Name = words[0];


                            ABLine.lineArr[i].heading = glm.toRadians(double.Parse(words[1], CultureInfo.InvariantCulture));
                            ABLine.lineArr[i].origin.easting = double.Parse(words[2], CultureInfo.InvariantCulture);
                            ABLine.lineArr[i].origin.northing = double.Parse(words[3], CultureInfo.InvariantCulture);
                            ABLine.numABLines++;
                        }
                    }
                    catch (Exception er)
                    {
                        var form = new FormTimedMessage(2000, "AB Line Corrupt", "Please delete it!!!");
                        form.Show(this);
                        WriteErrorLog("FieldOpen, Loading ABLine, Corrupt ABLine File" + er);
                    }
                }
            }

            if (ABLine.numABLines == 0) ABLine.numABLineSelected = 0;
            if (ABLine.numABLineSelected > ABLine.numABLines) ABLine.numABLineSelected = ABLine.numABLines;
        }

        //function to open a previously saved field, resume, open exisiting, open named field
        public void FileOpenField(string _openType)
        {
            string fileAndDirectory = "";
            if (_openType.Contains("Field.txt"))
            {
                fileAndDirectory = _openType;
                _openType = "Load";
            }

            else fileAndDirectory = "Cancel";

            //get the directory where the fields are stored
            //string directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\fields\\";
            switch (_openType)
            {
                case "Resume":
                    {
                        //Either exit or update running save
                        fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Field.txt";
                        if (!File.Exists(fileAndDirectory)) fileAndDirectory = "Cancel";
                        break;
                    }

                case "Open":
                    {
                        //create the dialog instance
                        OpenFileDialog ofd = new OpenFileDialog();

                        //the initial directory, fields, for the open dialog
                        ofd.InitialDirectory = fieldsDirectory;

                        //When leaving dialog put windows back where it was
                        ofd.RestoreDirectory = true;

                        //set the filter to text files only
                        ofd.Filter = "Field files (Field.txt)|Field.txt";

                        //was a file selected
                        if (ofd.ShowDialog(this) == DialogResult.Cancel) fileAndDirectory = "Cancel";
                        else fileAndDirectory = ofd.FileName;
                        break;
                    }
            }

            if (fileAndDirectory == "Cancel") return;

            //close the existing job and reset everything
            this.JobClose();

            //and open a new job
            this.JobNew();

            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //start to read the file
            string line;
            using (StreamReader reader = new StreamReader(fileAndDirectory))
            {
                try
                {
                    //Date time line
                    line = reader.ReadLine();

                    //dir header $FieldDir
                    line = reader.ReadLine();

                    //read field directory
                    line = reader.ReadLine();

                    currentFieldDirectory = line.Trim();

                    displayFieldName = currentFieldDirectory;

                    //Offset header
                    line = reader.ReadLine();

                    //read the Offsets 
                    line = reader.ReadLine();
                    string[] offs = line.Split(',');

                    //convergence angle update
                    if (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                    }

                    //start positions
                    if (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        offs = line.Split(',');

                        pn.latStart = double.Parse(offs[0], CultureInfo.InvariantCulture);
                        pn.lonStart = double.Parse(offs[1], CultureInfo.InvariantCulture);

                        if (timerSim.Enabled)
                        {
                            pn.latitude = pn.latStart;
                            pn.longitude = pn.lonStart;

                            sim.latitude = pn.latStart;  // Properties.Settings.Default.setGPS_SimLatitude = pn.latitude;
                            sim.longitude = pn.lonStart; // Properties.Settings.Default.setGPS_SimLongitude = pn.longitude;
                            Properties.Settings.Default.Save();
                        }

                        pn.SetLocalMetersPerDegree();
                    }
                }

                catch (Exception e)
                {
                    WriteErrorLog("While Opening Field" + e.ToString());

                    var form = new FormTimedMessage(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);

                    form.Show(this);
                    JobClose();
                    return;
                }
            }

            // ABLine -------------------------------------------------------------------------------------------------
            FileLoadABLines();

            if (ABLine.lineArr.Count > 0)
            {
                ABLine.numABLineSelected = 1;
                ABLine.refPoint1 = ABLine.lineArr[ABLine.numABLineSelected - 1].origin;
                //ABLine.refPoint2 = ABLine.lineArr[ABLine.numABLineSelected - 1].ref2;
                ABLine.abHeading = ABLine.lineArr[ABLine.numABLineSelected - 1].heading;
                ABLine.SetABLineByHeading();
                ABLine.isABLineSet = false;
                ABLine.isABLineLoaded = true;
            }
            else
            {
                ABLine.isABLineSet = false;
                ABLine.isABLineLoaded = false;
            }


            //CurveLines
            FileLoadCurveLines();
            if (curve.curveArr.Count > 0)
            {
                curve.numCurveLineSelected = 1;
                int idx = curve.numCurveLineSelected - 1;
                curve.aveLineHeading = curve.curveArr[idx].aveHeading;

                curve.refList?.Clear();
                for (int i = 0; i < curve.curveArr[idx].curvePts.Count; i++)
                {
                    curve.refList.Add(curve.curveArr[idx].curvePts[i]);
                }
                curve.isCurveSet = true;
            }
            else
            {
                curve.isCurveSet = false;
                curve.refList?.Clear();
            }
            
            //section patches
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Sections.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(2000, gStr.gsMissingSectionFile, gStr.gsButFieldIsLoaded);
                form.Show(this);
                //return;
            }
            else
            {
                bool isv3 = false;
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        fd.workedAreaTotal = 0;
                        fd.distanceUser = 0;
                        vec3 vecFix = new vec3();

                        //read header
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            if (line.Contains("ect"))
                            {
                                isv3 = true;
                                break;
                            }
                            int verts = int.Parse(line);

                            section[0].triangleList = new List<vec3>();
                            section[0].triangleList.Capacity = verts + 1;

                            section[0].patchList.Add(section[0].triangleList);


                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                vecFix.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                vecFix.heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                section[0].triangleList.Add(vecFix);
                            }

                            //calculate area of this patch - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                            verts -= 2;
                            if (verts >= 2)
                            {
                                for (int j = 1; j < verts; j++)
                                {
                                    double temp = 0;
                                    temp = section[0].triangleList[j].easting * (section[0].triangleList[j + 1].northing - section[0].triangleList[j + 2].northing) +
                                              section[0].triangleList[j + 1].easting * (section[0].triangleList[j + 2].northing - section[0].triangleList[j].northing) +
                                                  section[0].triangleList[j + 2].easting * (section[0].triangleList[j].northing - section[0].triangleList[j + 1].northing);

                                    fd.workedAreaTotal += Math.Abs((temp * 0.5));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WriteErrorLog("Section file" + e.ToString());

                        var form = new FormTimedMessage(2000, "Section File is Corrupt", gStr.gsButFieldIsLoaded);
                        form.Show(this);
                    }

                }

                //was old version prior to v4
                if (isv3)
                {
                        //Append the current list to the field file
                        using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Sections.txt"), false))
                        {
                        }
                }
            }

            // Contour points ----------------------------------------------------------------------------

            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Contour.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(2000, gStr.gsMissingContourFile, gStr.gsButFieldIsLoaded);
                form.Show(this);
                //return;
            }
            
            //Points in Patch followed by easting, heading, northing, altitude
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            //read how many vertices in the following patch
                            line = reader.ReadLine();
                            int verts = int.Parse(line);

                            vec3 vecFix = new vec3(0, 0, 0);

                            ct.ptList = new List<vec3>();
                            ct.ptList.Capacity = verts + 1;
                            ct.stripList.Add(ct.ptList);

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                vecFix.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                vecFix.heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                ct.ptList.Add(vecFix);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WriteErrorLog("Loading Contour file" + e.ToString());

                        var form = new FormTimedMessage(2000, gStr.gsContourFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show(this);
                    }
                }
            }


            // Flags -------------------------------------------------------------------------------------------------

            //Either exit or update running save
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Flags.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(2000, gStr.gsMissingFlagsFile, gStr.gsButFieldIsLoaded);
                form.Show(this);
            }

            else
            {
                flagPts?.Clear();
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();

                        //number of flags
                        line = reader.ReadLine();
                        int points = int.Parse(line);

                        if (points > 0)
                        {
                            double lat;
                            double longi;
                            double east;
                            double nort;
                            double head;
                            int color, ID;
                            string notes;

                            for (int v = 0; v < points; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');

                                if (words.Length == 8)
                                {
                                    lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                    longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                    east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                    nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                    head = double.Parse(words[4], CultureInfo.InvariantCulture);
                                    color = int.Parse(words[5]);
                                    ID = int.Parse(words[6]);
                                    notes = words[7].Trim();
                                }
                                else
                                {
                                    lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                    longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                    east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                    nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                    head = 0;
                                    color = int.Parse(words[4]);
                                    ID = int.Parse(words[5]);
                                    notes = "";
                                }

                                CFlag flagPt = new CFlag(lat, longi, east, nort, head, color, ID, notes);
                                flagPts.Add(flagPt);
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsFlagFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show(this);
                        WriteErrorLog("FieldOpen, Loading Flags, Corrupt Flag File" + e.ToString());
                    }
                }
            }

            //Boundaries
            //Either exit or update running save
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Boundary.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(2000, gStr.gsMissingBoundaryFile, gStr.gsButFieldIsLoaded);
                form.Show(this);
            }
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {

                        //read header
                        line = reader.ReadLine();//Boundary

                        for (int k = 0; true; k++)
                        {
                            if (reader.EndOfStream) break;

                            CBoundaryList New = new CBoundaryList();

                            //True or False OR points from older boundary files
                            line = reader.ReadLine();

                            //Check for older boundary files, then above line string is num of points
                            if (line == "True" || line == "False")
                            {
                                New.isDriveThru = bool.Parse(line);
                                line = reader.ReadLine(); //number of points
                            }

                            //Check for latest boundary files, then above line string is num of points
                            if (line == "True" || line == "False")
                            {
                                line = reader.ReadLine(); //number of points
                            }

                            int numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                //load the line
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec3 vecPt = new vec3(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture),
                                    double.Parse(words[2], CultureInfo.InvariantCulture));

                                    New.fenceLine.Add(vecPt);
                                }

                                New.CalculateFenceArea(k);

                                double delta = 0;
                                New.fenceLineEar?.Clear();

                                for (int i = 0; i < New.fenceLine.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        New.fenceLineEar.Add(new vec2(New.fenceLine[i].easting, New.fenceLine[i].northing));
                                        continue;
                                    }
                                    delta += (New.fenceLine[i - 1].heading - New.fenceLine[i].heading);
                                    if (Math.Abs(delta) > 0.04)
                                    {
                                        New.fenceLineEar.Add(new vec2(New.fenceLine[i].easting, New.fenceLine[i].northing));
                                        delta = 0;
                                    }
                                }

                                bnd.bndList.Add(New);
                            }
                        }

                        CalculateMinMax();
                        bnd.BuildTurnLines();
                        if (bnd.bndList.Count > 0) btnMakeLinesFromBoundary.Visible = true;
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsBoundaryLineFilesAreCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show(this);
                        WriteErrorLog("Load Boundary Line" + e.ToString());
                    }
                }
            }

            // Headland  -------------------------------------------------------------------------------------------------
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Headland.txt";

            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();

                        for (int k = 0; true; k++)
                        {
                            if (reader.EndOfStream) break;

                            if (bnd.bndList.Count > k)
                            {
                                bnd.bndList[k].hdLine.Clear();

                                //read the number of points
                                line = reader.ReadLine();
                                int numPoints = int.Parse(line);

                                if (numPoints > 0)
                                {
                                    //load the line
                                    for (int i = 0; i < numPoints; i++)
                                    {
                                        line = reader.ReadLine();
                                        string[] words = line.Split(',');
                                        vec3 vecPt = new vec3(
                                            double.Parse(words[0], CultureInfo.InvariantCulture),
                                            double.Parse(words[1], CultureInfo.InvariantCulture),
                                            double.Parse(words[2], CultureInfo.InvariantCulture));
                                        bnd.bndList[k].hdLine.Add(vecPt);
                                    }
                                }
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(2000, "Headland File is Corrupt", "But Field is Loaded");
                        form.Show(this);
                        WriteErrorLog("Load Headland Loop" + e.ToString());
                    }
                }
            }

            if (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
            {
                bnd.isHeadlandOn = true;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                btnHeadlandOnOff.Visible = true;
                btnHydLift.Visible = true;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;

            }
            else
            {
                bnd.isHeadlandOn = false;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
                btnHeadlandOnOff.Visible = false;
                btnHydLift.Visible = false;
            }

            //trams ---------------------------------------------------------------------------------
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Tram.txt";

            tram.tramBndOuterArr?.Clear();
            tram.tramBndInnerArr?.Clear();
            tram.tramList?.Clear();
            tram.displayMode = 0;
            btnTramDisplayMode.Visible = false;

            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();//$Tram

                        //outer track of boundary tram
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            int numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                //load the line
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec2 vecPt = new vec2(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture));

                                    tram.tramBndOuterArr.Add(vecPt);
                                }
                                tram.displayMode = 1;
                            }

                            //inner track of boundary tram
                            line = reader.ReadLine();
                            numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                //load the line
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec2 vecPt = new vec2(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture));

                                    tram.tramBndInnerArr.Add(vecPt);
                                }
                            }

                            if (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                int numLines = int.Parse(line);

                                for (int k = 0; k < numLines; k++)
                                {
                                    line = reader.ReadLine();
                                    numPoints = int.Parse(line);

                                    tram.tramArr = new List<vec2>();
                                    tram.tramList.Add(tram.tramArr);

                                    for (int i = 0; i < numPoints; i++)
                                    {
                                        line = reader.ReadLine();
                                        string[] words = line.Split(',');
                                        vec2 vecPt = new vec2(
                                        double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture));

                                        tram.tramArr.Add(vecPt);
                                    }
                                }
                            }
                        }

                        FixTramModeButton();
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(2000, "Tram is corrupt", gStr.gsButFieldIsLoaded);
                        form.Show(this);
                        WriteErrorLog("Load Boundary Line" + e.ToString());
                    }
                }
            }
            FileLoadGPSpositionenFS();

            FixPanelsAndMenus(true);
            SetZoom();

            //Recorded Path
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\RecPath.txt";
            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        int numPoints = int.Parse(line);
                        recPath.recList.Clear();

                        while (!reader.EndOfStream)
                        {
                            for (int v = 0; v < numPoints; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                CRecPathPt point = new CRecPathPt(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture),
                                    double.Parse(words[2], CultureInfo.InvariantCulture),
                                    double.Parse(words[3], CultureInfo.InvariantCulture),
                                    bool.Parse(words[4]));

                                //add the point
                                recPath.recList.Add(point);
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsRecordedPathFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show(this);
                        WriteErrorLog("Load Recorded Path" + e.ToString());
                    }
                }
            }


        }//end of open file

        //creates the field file when starting new field
        public void FileCreateField()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            if (!isJobStarted)
            {
                using (var form = new FormTimedMessage(3000, gStr.gsFieldNotOpen, gStr.gsCreateNewField))
                { form.Show(this); }
                return;
            }
            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Field.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir");
                writer.WriteLine(currentFieldDirectory.ToString(CultureInfo.InvariantCulture));

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");

                writer.WriteLine("Convergence");
                writer.WriteLine("0");

                writer.WriteLine("StartFix");
                writer.WriteLine(pn.latitude.ToString(CultureInfo.InvariantCulture) + "," + pn.longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void FileCreateElevation()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //if (!isJobStarted)
            //{
            //    using (var form = new FormTimedMessage(3000, "Ooops, Job Not Started", "Start a Job First"))
            //    { form.Show(this); }
            //    return;
            //}

            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Elevation.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir");
                writer.WriteLine(currentFieldDirectory.ToString(CultureInfo.InvariantCulture));

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");

                writer.WriteLine("Convergence");
                writer.WriteLine("0");

                writer.WriteLine("StartFix");
                writer.WriteLine(pn.latitude.ToString(CultureInfo.InvariantCulture) + "," + pn.longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        //save field Patches
        public void FileSaveSections()
        {
            //make sure there is something to save
            if (patchSaveList.Count() > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Sections.txt"), true))
                {
                    //for each patch, write out the list of triangles to the file
                    foreach (var triList in patchSaveList)
                    {
                        int count2 = triList.Count();
                        writer.WriteLine(count2.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < count2; i++)
                            writer.WriteLine((Math.Round(triList[i].easting,3)).ToString(CultureInfo.InvariantCulture) +
                                "," + (Math.Round(triList[i].northing,3)).ToString(CultureInfo.InvariantCulture) +
                                 "," + (Math.Round(triList[i].heading, 3)).ToString(CultureInfo.InvariantCulture));
                    }
                }

                //clear out that patchList and begin adding new ones for next save
                patchSaveList.Clear();
            }
        }

        //Create contour file
        public void FileCreateSections()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Sections.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //write paths # of sections
                //writer.WriteLine("$Sectionsv4");
            }
        }

        //Create Flag file
        public void FileCreateFlags()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Flags.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //write paths # of sections
                //writer.WriteLine("$Sectionsv4");
            }
        }

        //Create contour file
        public void FileCreateContour()
        {
            //12  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Contour.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                writer.WriteLine("$Contour");
            }
        }

        //save the contour points which include elevation values
        public void FileSaveContour()
        {
            //1  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //make sure there is something to save
            if (contourSaveList.Count() > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Contour.txt"), true))
                {

                    //for every new chunk of patch in the whole section
                    foreach (var triList in contourSaveList)
                    {
                        int count2 = triList.Count;

                        writer.WriteLine(count2.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < count2; i++)
                        {
                            writer.WriteLine(Math.Round((triList[i].easting), 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(triList[i].northing, 3).ToString(CultureInfo.InvariantCulture)+ "," +
                                Math.Round(triList[i].heading, 3).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }

                contourSaveList.Clear();

            }
        }

        //save the boundary
        public void FileSaveBoundary()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "Boundary.Txt"))
            {
                writer.WriteLine("$Boundary");
                for (int i = 0; i < bnd.bndList.Count; i++)
                {
                    writer.WriteLine(bnd.bndList[i].isDriveThru);
                    //writer.WriteLine(bnd.bndList[i].isDriveAround);

                    writer.WriteLine(bnd.bndList[i].fenceLine.Count.ToString(CultureInfo.InvariantCulture));
                    if (bnd.bndList[i].fenceLine.Count > 0)
                    {
                        for (int j = 0; j < bnd.bndList[i].fenceLine.Count; j++)
                            writer.WriteLine(Math.Round(bnd.bndList[i].fenceLine[j].easting,3).ToString(CultureInfo.InvariantCulture) + "," +
                                                Math.Round(bnd.bndList[i].fenceLine[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                    Math.Round(bnd.bndList[i].fenceLine[j].heading,5).ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        //save tram
        public void FileSaveTram()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "Tram.Txt"))
            {
                writer.WriteLine("$Tram");

                if (tram.tramBndOuterArr.Count > 0)
                {
                    //outer track of outer boundary tram
                    writer.WriteLine(tram.tramBndOuterArr.Count.ToString(CultureInfo.InvariantCulture));

                    for (int i = 0; i < tram.tramBndOuterArr.Count; i++)
                    {
                        writer.WriteLine(Math.Round(tram.tramBndOuterArr[i].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(tram.tramBndOuterArr[i].northing, 3).ToString(CultureInfo.InvariantCulture));
                    }

                    //inner track of outer boundary tram
                    writer.WriteLine(tram.tramBndInnerArr.Count.ToString(CultureInfo.InvariantCulture));

                    for (int i = 0; i < tram.tramBndInnerArr.Count; i++)
                    {
                        writer.WriteLine(Math.Round(tram.tramBndInnerArr[i].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(tram.tramBndInnerArr[i].northing, 3).ToString(CultureInfo.InvariantCulture));
                    }
                }

                if (tram.tramList.Count > 0)
                {
                    writer.WriteLine(tram.tramList.Count.ToString(CultureInfo.InvariantCulture));
                    for (int i = 0; i < tram.tramList.Count; i++)
                    {
                        writer.WriteLine(tram.tramList[i].Count.ToString(CultureInfo.InvariantCulture));
                        for (int h = 0; h < tram.tramList[i].Count; h++)
                        {
                            writer.WriteLine(Math.Round(tram.tramList[i][h].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(tram.tramList[i][h].northing, 3).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
        }


        //save the headland
        public void FileSaveHeadland()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            try
            {


                string directoryName = Path.GetDirectoryName(dirField);
                if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
                { Directory.CreateDirectory(directoryName); }

                //write out the file
                using (StreamWriter writer = new StreamWriter(dirField + "Headland.Txt"))
                {
                    writer.WriteLine("$Headland");

                    if (bnd.bndList[0].hdLine.Count > 0)
                    {
                        for (int i = 0; i < bnd.bndList.Count; i++)
                        {
                            writer.WriteLine(bnd.bndList[i].hdLine.Count.ToString(CultureInfo.InvariantCulture));
                            if (bnd.bndList[0].hdLine.Count > 0)
                            {
                                for (int j = 0; j < bnd.bndList[i].hdLine.Count; j++)
                                    writer.WriteLine(Math.Round(bnd.bndList[i].hdLine[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                     Math.Round(bnd.bndList[i].hdLine[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                     Math.Round(bnd.bndList[i].hdLine[j].heading, 3).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

        //Create contour file
        public void FileCreateRecPath()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "RecPath.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //write paths # of sections
                writer.WriteLine("$RecPath");
                writer.WriteLine("0");
            }
        }

        //save the recorded path
        public void FileSaveRecPath()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //string fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\RecPath.txt";
            //if (!File.Exists(fileAndDirectory)) FileCreateRecPath();

            //write out the file
            using (StreamWriter writer = new StreamWriter((dirField + "RecPath.Txt")))
            {
                writer.WriteLine("$RecPath");
                writer.WriteLine(recPath.recList.Count.ToString(CultureInfo.InvariantCulture));
                if (recPath.recList.Count > 0)
                {
                    for (int j = 0; j < recPath.recList.Count; j++)
                        writer.WriteLine(
                            Math.Round(recPath.recList[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].heading, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].speed, 1).ToString(CultureInfo.InvariantCulture) + "," +
                            (recPath.recList[j].autoBtnState).ToString());

                    //Clear list
                    //recPath.recList.Clear();
                }
            }
        }

        //save all the flag markers
        public void FileSaveFlags()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FlagsDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //use Streamwriter to create and overwrite existing flag file
            using (StreamWriter writer = new StreamWriter(dirField + "Flags.txt"))
            {
                try
                {
                    writer.WriteLine("$Flags");

                    int count2 = flagPts.Count;
                    writer.WriteLine(count2);

                    for (int i = 0; i < count2; i++)
                    {
                        writer.WriteLine(
                            flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].easting.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].northing.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].heading.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].color.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].ID.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].notes);
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n Cannot write to file.");
                    WriteErrorLog("Saving Flags" + e.ToString());
                    return;
                }
            }
        }

        //save all the flag markers
        //public void FileSaveABLine()
        //{
        //    //Saturday, February 11, 2017  -->  7:26:52 AM

        //    //get the directory and make sure it exists, create if not
        //    string dirField = fieldsDirectory + currentFieldDirectory + "\\";

        //    string directoryName = Path.GetDirectoryName(dirField);
        //    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
        //    { Directory.CreateDirectory(directoryName); }

        //    //use Streamwriter to create and overwrite existing ABLine file
        //    using (StreamWriter writer = new StreamWriter(dirField + "ABLine.txt"))
        //    {
        //        try
        //        {
        //            //write out the ABLine
        //            writer.WriteLine("$ABLine");

        //            //true or false if ABLine is set
        //            if (ABLine.isABLineSet) writer.WriteLine(true);
        //            else writer.WriteLine(false);

        //            writer.WriteLine(ABLine.abHeading.ToString(CultureInfo.InvariantCulture));
        //            writer.WriteLine(ABLine.refPoint1.easting.ToString(CultureInfo.InvariantCulture) + "," + ABLine.refPoint1.northing.ToString(CultureInfo.InvariantCulture));
        //            writer.WriteLine(ABLine.refPoint2.easting.ToString(CultureInfo.InvariantCulture) + "," + ABLine.refPoint2.northing.ToString(CultureInfo.InvariantCulture));
        //            writer.WriteLine(ABLine.tramPassEvery.ToString(CultureInfo.InvariantCulture) + "," + ABLine.passBasedOn.ToString(CultureInfo.InvariantCulture));
        //        }

        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message + "\n Cannot write to file.");
        //            WriteErrorLog("Saving AB Line" + e.ToString());

        //            return;
        //        }

        //    }
        //}

        //save all the flag markers
        //public void FileSaveCurveLine()
        //{
        //    //Saturday, February 11, 2017  -->  7:26:52 AM

        //    //get the directory and make sure it exists, create if not
        //    string dirField = fieldsDirectory + currentFieldDirectory + "\\";

        //    string directoryName = Path.GetDirectoryName(dirField);
        //    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
        //    { Directory.CreateDirectory(directoryName); }

        //    //use Streamwriter to create and overwrite existing ABLine file
        //    using (StreamWriter writer = new StreamWriter(dirField + "CurveLine.txt"))
        //    {
        //        try
        //        {
        //            //write out the ABLine
        //            writer.WriteLine("$CurveLine");

        //            //write out the aveheading
        //            writer.WriteLine(curve.aveLineHeading.ToString(CultureInfo.InvariantCulture));

        //            //write out the points of ref line
        //            writer.WriteLine(curve.refList.Count.ToString(CultureInfo.InvariantCulture));
        //            if (curve.refList.Count > 0)
        //            {
        //                for (int j = 0; j < curve.refList.Count; j++)
        //                    writer.WriteLine(Math.Round(curve.refList[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
        //                                        Math.Round(curve.refList[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
        //                                            Math.Round(curve.refList[j].heading, 5).ToString(CultureInfo.InvariantCulture));
        //            }
        //        }

        //        catch (Exception e)
        //        {
        //            WriteErrorLog("Saving Curve Line" + e.ToString());

        //            return;
        //        }

        //    }
        //}

        //save nmea sentences
        public void FileSaveNMEA()
        {
            using (StreamWriter writer = new StreamWriter("zAOG_log.txt", true))
            {
                writer.Write(pn.logNMEASentence.ToString());
            }
            pn.logNMEASentence.Clear();
        }

        //save nmea sentences
        public void FileSaveElevation()
        {
            using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Elevation.txt"), true))
            {
                writer.Write(sbFix.ToString());
            }
            sbFix.Clear();
        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML2(int flagNumber)
        {
            double lat = 0;
            double lon = 0;

            pn.ConvertLocalToWGS84(flagPts[flagNumber - 1].northing, flagPts[flagNumber - 1].easting, out lat, out lon);

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //match new fix to current position


                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                    writer.WriteLine(@"<color>ff4400ff</color>");
                if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                    writer.WriteLine(@"<color>ff44ff00</color>");
                if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                    writer.WriteLine(@"<color>ff44ffff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                lon.ToString(CultureInfo.InvariantCulture) + "," + lat.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }
                                   
        //generate KML file from flag
        public void FileSaveSingleFlagKML(int flagNumber)
        {

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //match new fix to current position

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                    writer.WriteLine(@"  <Placemark>                                  ");
                    writer.WriteLine(@"<Style> <IconStyle>");
                    if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                        writer.WriteLine(@"<color>ff4400ff</color>");
                    if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                        writer.WriteLine(@"<color>ff44ff00</color>");
                    if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                        writer.WriteLine(@"<color>ff44ffff</color>");
                    writer.WriteLine(@"</IconStyle> </Style>");
                    writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                    writer.WriteLine(@"<Point><coordinates> " +
                                    flagPts[flagNumber-1].longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[flagNumber-1].latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                    @"</coordinates> </Point> ");
                    writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flag
        public void FileMakeKMLFromCurrentPosition(double lat, double lon)
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }


            using (StreamWriter writer = new StreamWriter(dirField + "CurrentPosition.kml"))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                writer.WriteLine(@"<color>ff4400ff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> Your Current Position </name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                lon.ToString(CultureInfo.InvariantCulture) + "," + lat.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flags
        public void FileSaveFieldKML()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Field.kml";

            XmlTextWriter kml = new XmlTextWriter(dirField + myFileName, Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            //Boundary  ----------------------------------------------------------------------
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Boundaries");

            for (int i = 0; i < bnd.bndList.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                if (i == 0) kml.WriteElementString("name", currentFieldDirectory);

                //lineStyle
                kml.WriteStartElement("Style");
                kml.WriteStartElement("LineStyle");
                if (i == 0) kml.WriteElementString("color", "ffdd00dd");
                else kml.WriteElementString("color", "ff4d3ffd");
                kml.WriteElementString("width", "4");
                kml.WriteEndElement(); // <LineStyle>

                kml.WriteStartElement("PolyStyle");
                if (i == 0) kml.WriteElementString("color", "407f3f55");
                else kml.WriteElementString("color", "703f38f1");
                kml.WriteEndElement(); // <PloyStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("Polygon");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("outerBoundaryIs");
                kml.WriteStartElement("LinearRing");

                //coords
                kml.WriteStartElement("coordinates");
                string bndPts = "";
                if (bnd.bndList[i].fenceLine.Count > 3)
                    bndPts = GetBoundaryPointsLatLon(i);
                kml.WriteRaw(bndPts);
                kml.WriteEndElement(); // <coordinates>

                kml.WriteEndElement(); // <Linear>
                kml.WriteEndElement(); // <OuterBoundary>
                kml.WriteEndElement(); // <Polygon>
                kml.WriteEndElement(); // <Placemark>
            }

            kml.WriteEndElement(); // <Folder>  
            //End of Boundary

            //guidance lines AB
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "AB_Lines");
            kml.WriteElementString("visibility", "0");

            string linePts = "";

            for (int i = 0; i < ABLine.lineArr.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", ABLine.lineArr[i].Name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff0000ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                linePts = pn.GetLocalToWSG84_KML(ABLine.lineArr[i].origin.easting - (Math.Sin(ABLine.lineArr[i].heading) * ABLine.abLength),
                    ABLine.lineArr[i].origin.northing - (Math.Cos(ABLine.lineArr[i].heading) * ABLine.abLength));
                linePts += pn.GetLocalToWSG84_KML(ABLine.lineArr[i].origin.easting + (Math.Sin(ABLine.lineArr[i].heading) * ABLine.abLength),
                    ABLine.lineArr[i].origin.northing + (Math.Cos(ABLine.lineArr[i].heading) * ABLine.abLength));
                kml.WriteRaw(linePts);

                kml.WriteEndElement(); // <coordinates>
                kml.WriteEndElement(); // <LineString>

                kml.WriteEndElement(); // <Placemark>

            }
            kml.WriteEndElement(); // <Folder>   

            //guidance lines Curve
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Curve_Lines");
            kml.WriteElementString("visibility", "0");

            for (int i = 0; i < curve.curveArr.Count; i++)
            {
                linePts = "";
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", curve.curveArr[i].Name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff6699ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                for (int j = 0; j < curve.curveArr[i].curvePts.Count; j++)
                {
                    linePts += pn.GetLocalToWSG84_KML(curve.curveArr[i].curvePts[j].easting, curve.curveArr[i].curvePts[j].northing);
                }
                kml.WriteRaw(linePts);

                kml.WriteEndElement(); // <coordinates>
                kml.WriteEndElement(); // <LineString>

                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   

            //Recorded Path
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Recorded Path");
            kml.WriteElementString("visibility", "1");

            linePts = "";
            kml.WriteStartElement("Placemark");
            kml.WriteElementString("visibility", "1");

            kml.WriteElementString("name", "Path " + 1);
            kml.WriteStartElement("Style");

            kml.WriteStartElement("LineStyle");
            kml.WriteElementString("color", "ff44ffff");
            kml.WriteElementString("width", "2");
            kml.WriteEndElement(); // <LineStyle>
            kml.WriteEndElement(); //Style

            kml.WriteStartElement("LineString");
            kml.WriteElementString("tessellate", "1");
            kml.WriteStartElement("coordinates");

            for (int j = 0; j < recPath.recList.Count; j++)
            {
                linePts += pn.GetLocalToWSG84_KML(recPath.recList[j].easting, recPath.recList[j].northing);
            }
            kml.WriteRaw(linePts);

            kml.WriteEndElement(); // <coordinates>
            kml.WriteEndElement(); // <LineString>

            kml.WriteEndElement(); // <Placemark>
            kml.WriteEndElement(); // <Folder>

            //flags  *************************************************************************
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Flags");

            for (int i = 0; i < flagPts.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("name", "Flag_"+ i.ToString());

                kml.WriteStartElement("Style");
                kml.WriteStartElement("IconStyle");

                if (flagPts[i].color == 0)  //red - xbgr
                    kml.WriteElementString("color", "ff4400ff");
                if (flagPts[i].color == 1)  //grn - xbgr
                    kml.WriteElementString("color", "ff44ff00");
                if (flagPts[i].color == 2)  //yel - xbgr
                    kml.WriteElementString("color", "ff44ffff");

                kml.WriteEndElement(); //IconStyle
                kml.WriteEndElement(); //Style

                kml.WriteElementString("name", ((i + 1).ToString() + " " + flagPts[i].notes));
                kml.WriteStartElement("Point");
                kml.WriteElementString("coordinates", flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) +
                    "," + flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + ",0");
                kml.WriteEndElement(); //Point
                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   
            //End of Flags

            //Sections  ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Sections");

            string secPts = "";
            int cntr = 0;

            for (int j = 0; j < tool.numSuperSection; j++)
            {
                int patches = section[j].patchList.Count;

                if (patches > 0)
                {
                    //for every new chunk of patch
                    foreach (var triList in section[j].patchList)
                    {
                        if (triList.Count > 0)
                        {
                            kml.WriteStartElement("Placemark");
                            kml.WriteElementString("name", "Sections_" + cntr.ToString());
                            cntr++;

                            string collor = "F0" + ((byte)(triList[0].heading)).ToString("X2") +
                                ((byte)(triList[0].northing)).ToString("X2") + ((byte)(triList[0].easting)).ToString("X2");

                            //lineStyle
                            kml.WriteStartElement("Style");

                            kml.WriteStartElement("LineStyle");
                            kml.WriteElementString("color", collor);
                            //kml.WriteElementString("width", "6");
                            kml.WriteEndElement(); // <LineStyle>
                            
                            kml.WriteStartElement("PolyStyle");
                            kml.WriteElementString("color", collor);
                            kml.WriteEndElement(); // <PloyStyle>
                            kml.WriteEndElement(); //Style

                            kml.WriteStartElement("Polygon");
                            kml.WriteElementString("tessellate", "1");
                            kml.WriteStartElement("outerBoundaryIs");
                            kml.WriteStartElement("LinearRing");
                            
                            //coords
                            kml.WriteStartElement("coordinates");
                            secPts = "";
                            for (int i = 1; i < triList.Count; i += 2)
                            {
                                secPts += pn.GetLocalToWSG84_KML(triList[i].easting, triList[i].northing);
                            }
                            for (int i = triList.Count - 1; i > 1; i -= 2)
                            {
                                secPts += pn.GetLocalToWSG84_KML(triList[i].easting, triList[i].northing);
                            }
                            secPts += pn.GetLocalToWSG84_KML(triList[1].easting, triList[1].northing);

                            kml.WriteRaw(secPts);
                            kml.WriteEndElement(); // <coordinates>

                            kml.WriteEndElement(); // <LinearRing>
                            kml.WriteEndElement(); // <outerBoundaryIs>
                            kml.WriteEndElement(); // <Polygon>

                            kml.WriteEndElement(); // <Placemark>
                        }
                    }
                }
            }
            kml.WriteEndElement(); // <Folder>
            //End of sections

            //end of document
            kml.WriteEndElement(); // <Document>
            kml.WriteEndElement(); // <kml>

            //The end
            kml.WriteEndDocument();

            kml.Flush();

            //Write the XML to file and close the kml
            kml.Close();
        }

        public string GetBoundaryPointsLatLon(int bndNum)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bnd.bndList[bndNum].fenceLine.Count; i++)
            {
                double lat = 0;
                double lon = 0;

                pn.ConvertLocalToWGS84(bnd.bndList[bndNum].fenceLine[i].northing, bnd.bndList[bndNum].fenceLine[i].easting, out lat, out lon);

                sb.Append(lon.ToString("N7", CultureInfo.InvariantCulture) + ',' + lat.ToString("N7", CultureInfo.InvariantCulture) + ",0 ");
            }
            return sb.ToString();
        }

        // FRitz Import aus KML
        public void ImportFromKml(bool neu = false, bool boundary=false, bool ablines=false, bool curves=false, bool flags=false )
        {
            //string fileAndDirectory;

            //create the dialog instance
            OpenFileDialog ofd = new OpenFileDialog
            {
                //set the filter to text KML only
                Filter = "KML files (*.KML)|*.KML",

                //the initial directory, fields, for the open dialog
                InitialDirectory = fieldsDirectory + currentFieldDirectory
            };

            //was a file selected
            if (ofd.ShowDialog(this) == DialogResult.Cancel) return;
            else Dateinamen = ofd.FileName;

            if (boundary)ImportBoundaryFromKml(neu);
            if (ablines)ImportLinesFromKml();
            if (curves)ImportCurvesFromKml(); 
            if (flags) ImportFlagsFromKml();
            Dateinamen = "";
        }

        private void ImportBoundaryFromKml(bool neu = false)
        {
            string coordinates = null;
            int startIndex =-1;
            int endIndex = -1;
            int toDo = 0;
            int ersteKoordinaten = 0;
            if (neu)
            {
                bnd.isOkToAddPoints = false;
                ABLine.isABLineSet = false;
                ABLine.isABLineBeingSet = false;
                curve.isCurveSet = false;
                curve.isBtnCurveOn = false;
                bnd.bndList?.Clear();
                ABLine.lineArr?.Clear();
                curve.curveArr?.Clear();
                //bnd.bndList[0].hdLine.Clear();
                tram.tramArr?.Clear();
                tram.tramList?.Clear();
                tram.tramBndOuterArr?.Clear();
                tram.tramBndInnerArr?.Clear();
                flagPts?.Clear();
                recPath.pathCount = 0;
                fd.UpdateFieldBoundaryGUIAreas();

                FileSaveHeadland();
                FileSaveABLines();
                FileSaveCurveLines();
                FileSaveTram();
                FixTramModeButton();
                FileSaveFlags();
                FileSaveRecPath();

            }

            using (StreamReader reader = new StreamReader(Dateinamen))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        //start to read the file
                        string line = reader.ReadLine(); //</outerBoundaryIs>
                        if (toDo > 2 & line.IndexOf("</Placemark>") > -1) // fertig mit boundary
                        {
                            toDo = -1;
                        }
                        if (toDo == 0)
                        {
                            if (line.IndexOf("<Placemark>") > -1)
                            {
                                toDo = 1;
                            }
                        }
                        if (toDo == 1)
                        {
                            if (line.IndexOf("<outerBoundaryIs>") > -1)
                            {
                                toDo = 2;
                            }
                        }
                        if (toDo == 1)
                        {
                            if (line.IndexOf("<styleUrl>BoundaryStyle") > -1) //fendt
                            {
                                toDo = 2;
                            }
                        }
                        {
                            if ((line.IndexOf("<LineString>") > -1) & toDo==2)
                            {
                                toDo = 3;
                            }
                        }
                        {
                            if ((line.IndexOf("<LinearRing>") > -1) & toDo > 1)
                            {
                                toDo = 3;
                            }
                        }

                        if (toDo == 3)
                        {
                            startIndex = line.IndexOf("<coordinates>");
                        }
                        
                        if (startIndex > -1)
                        {
                            while (true)
                            {
                                endIndex = line.IndexOf("</coordinates>");

                                if (endIndex == -1)
                                {
                                    //just add the line
                                    if (startIndex == -1) coordinates += line.Substring(0);
                                    else coordinates += line.Substring(startIndex + 13);
                                    coordinates += ", ";
                                }
                                else
                                {
                                    if (startIndex == -1) coordinates += line.Substring(0, endIndex) + " ";
                                    else coordinates += line.Substring(startIndex + 13, endIndex - (startIndex + 13));
                                    break;
                                }
                                line = reader.ReadLine();
                                line = line.Trim();
                                startIndex = -1;
                                toDo = 3;// fertig
                            }

                            line = coordinates;
                            char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                            string[] numberSets = line.Split(delimiterChars);

                            //at least 3 points
                            if (numberSets.Length > 2)
                            {
                                CBoundaryList New = new CBoundaryList();

                                foreach (string item in numberSets)
                                {
                                    if (item.Length > 7)
                                    {
                                    
                                        string[] fix = item.Split(',');
                                        double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                        double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);

                                        if (neu & (ersteKoordinaten == 0))
                                        {
                                            //position fuer feld anagen
                                            ersteKoordinaten = 1;
                                            pn.latStart = latK;
                                            pn.lonStart = lonK;

                                            pn.latitude = latK;
                                            pn.longitude = lonK;
                                            pn.SetLocalMetersPerDegree();
                                            FileCreateField();
                                            FileSaveElevation();

                                            //mf.pn.ConvertWGS84toMetersPerDegree(latK, lonK, out norting, out easting);
                                            if (timerSim.Enabled)
                                            {
                                                //pn.latitude = pn.latStart;
                                                //pn.longitude = pn.lonStart;

                                                sim.latitude = latK;  // Properties.Settings.Default.setGPS_SimLatitude = pn.latitude;
                                                sim.longitude = lonK; // Properties.Settings.Default.setGPS_SimLongitude = pn.longitude;
                                                Properties.Settings.Default.Save();
                                            }
                                        }
                                        pn.ConvertWGS84ToLocal(latK, lonK, out northing, out easting);
                                        //add the point to boundary
                                        New.fenceLine.Add(new vec3(easting, northing, 0));
                                    }
                                }

                                New.CalculateFenceArea(bnd.bndList.Count);
                                New.FixFenceLine(bnd.bndList.Count);

                                bnd.bndList.Add(New);

                                btnMakeLinesFromBoundary.Visible = true;

                                coordinates = "";
                                toDo = -1;

                            }
                            else
                            {
                                TimedMessageBox(2000, gStr.gsErrorreadingKML, gStr.gsChooseBuildDifferentone);
                            }
                            
                        }
                        
                    }
                    FileSaveBoundary();
                    bnd.BuildTurnLines();
                    btnMakeLinesFromBoundary.Visible = true;
                 
                }

                catch (Exception)
                {
                    return;
                }
            }
    }
        public void ImportLinesFromKml()
        {
            string coordinates = null;
            string name = "";
            int startIndex = -1;
            int endIndex = -1;
            int toDo = 0;
            int startN = -1;
            int stoppN = -1;
            bool fendt = false;

            using (StreamReader reader = new StreamReader(Dateinamen))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        //start to read the file
                        string line = reader.ReadLine();

                        if (toDo > 1 & line.IndexOf("</Folder>") > -1) // fertig mit linien
                        {
                            toDo = 0;
                        }
                    //    if (fendt & toDo > 1 & line.IndexOf("</Placemark>") > -1) // fertig mit linien Fendt
                    //    {
                    //        toDo = -1;
                    //    }
                        //start
                        if (toDo == 0 & (line.IndexOf("Folder") > -1))
                        {
                            toDo = 1;
                        }
                        if (toDo == 0 & (line.IndexOf("Placemark") > -1)) 
                        {
                            toDo = 1;
                        }


                        if (toDo == 1 & (line.IndexOf("<name>") > -1))
                        {
                            toDo = 2;
                            startN = line.IndexOf("<name>") + 6;
                            stoppN = line.IndexOf("</name>");
                            name = line.Substring(startN, stoppN - startN);
                            if (name== "AB_Lines")// bei AOG
                            {
                                toDo = 3;
                            }
                        }
                        if (toDo == 3 & (line.IndexOf("<Placemark>") > -1))
                        {
                            toDo = 4;
                        }

                        if (toDo == 2 & (line.IndexOf("<styleUrl>WaylineStyle</styleUrl>") > -1))// fendt
                        {
                            toDo = 3;
                            fendt = true;
                        }

                        if (fendt & (line.IndexOf("WaylineType") > -1)) // 0 ist AB 3 ist kurve
                        {
                            toDo = 4;
                        }
                        if (fendt & toDo == 4 & (line.IndexOf("<value>") > -1)) // Linienart 0 AB 3 curve
                        {
                            startN = line.IndexOf("<value>") + 7;
                            stoppN = line.IndexOf("</value>");
                            string v = line.Substring(startN, stoppN - startN);
                            if (v == "0") { toDo = 5; } 
                            else { fendt = false; toDo = 0; } // Keine ABlinie
                        }


                        if (!fendt & toDo == 4 & (line.IndexOf("<name>") > -1))
                        {
                             toDo = 5;
                                startN = line.IndexOf("<name>")+6;
                                stoppN = line.IndexOf("</name>");
                                name= line.Substring(startN, stoppN-startN);
                        }



                        if (toDo == 5)
                        {
                            if (line.IndexOf("<LineString>") > -1)
                            {
                                toDo = 6;
                            }
                        }
                        if (toDo == 6)
                        {
                            startIndex = line.IndexOf("<coordinates>");
                        }

                        if (startIndex > -1)
                        {
                            while (true)
                            {
                                endIndex = line.IndexOf("</coordinates>");

                                if (endIndex == -1)
                                {
                                    //just add the line
                                    if (startIndex == -1) coordinates += line.Substring(0);
                                    else coordinates += line.Substring(startIndex + 13);
                                    coordinates += ", ";
                                }
                                else
                                {
                                    if (startIndex == -1) coordinates += line.Substring(0, endIndex) + " ";
                                    else coordinates += line.Substring(startIndex + 13, endIndex - (startIndex + 13));
                                    break;
                                }
                                line = reader.ReadLine();
                                line = line.Trim();
                                startIndex = -1;
                                toDo = 5;// fertig
                            }
                            toDo = 8;// fertig
                            line = coordinates;
                            char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                            string[] numberSets = line.Split(delimiterChars);

                            //at least 3 points
                            if (numberSets.Length > 2)
                            {

                                
                                foreach (string item in numberSets)
                                {
                                    if (item.Length > 7)
                                    {

                                        string[] fix = item.Split(',');
                                        double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                        double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);

                                        pn.ConvertWGS84ToLocal(latK, lonK, out northing, out easting);
                                        if (toDo < 11)  toDo++;
                                        //add the point to boundary
                                        if (toDo == 9) // ersten koordinaten A
                                        {
                                            Ap.easting = easting;
                                            Ap.northing = northing;
                                        }
                                        if (toDo == 10)// zweiten koordinaten B
                                        {
                                            Bp.easting = easting;
                                            Bp.northing = northing;
                                            toDo = 11;
                                        }
                                    }
                                }

                                if (toDo == 11)// AB Anlegen
                                {
                                    //calculate the AB Heading
                                    double abHead = Math.Atan2(Bp.easting - Ap.easting,
                                        Bp.northing - Ap.northing);
                                    if (abHead < 0) abHead += glm.twoPI;

                                    ABLine.lineArr.Add(new CABLines());
                                    ABLine.numABLines = ABLine.lineArr.Count;
                                    ABLine.numABLineSelected = ABLine.numABLines;

                                    int idx = ABLine.numABLines - 1;

                                    ABLine.lineArr[idx].heading = abHead;
                                    //calculate the new points for the reference line and points
                                    ABLine.lineArr[idx].origin.easting = (Ap.easting + Bp.easting)/2;
                                    ABLine.lineArr[idx].origin.northing = (Ap.northing + Bp.northing)/2;
                        
                                    ABLine.lineArr[idx].Name = name;
                                }
                                if (fendt)
                                {
                                    toDo = 0; 
                                }
                                else
                                {
                                    toDo = 3;// fertig mit dieser linie
                                }
                                coordinates = "";
                                startIndex = -1;
                                endIndex = -1;
                            }
                            else
                            {
                                TimedMessageBox(2000, gStr.gsErrorreadingKML, gStr.gsChooseBuildDifferentone);
                            }
                            
                          

                        }


                    }
                    FileSaveABLines();
                    return;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public void ImportCurvesFromKml()
        {
            string coordinates = null;
            string name = "";
            int startIndex = -1;
            int endIndex = -1;
            int toDo = 0;
            int startN = -1;
            int stoppN = -1;
            bool fendt = false;

            using (StreamReader reader = new StreamReader(Dateinamen))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        //start to read the file
                        string line = reader.ReadLine();

                        if (toDo > 1 & line.IndexOf("</Folder>") > -1) // fertig mit linien
                        {
                            toDo = 0;
                        }
                        if (toDo > 1 & line.IndexOf("</Polygon>") > -1) // fertig mit polygone
                        {
                            toDo = 0;
                        }
                        //    if (fendt & toDo > 1 & line.IndexOf("</Placemark>") > -1) // fertig mit linien Fendt
                        //    {
                        //        toDo = -1;
                        //    }
                        //start
                        if (toDo == 0 & (line.IndexOf("Folder") > -1))
                        {
                            toDo = 1;
                        }
                        if (toDo == 0 & (line.IndexOf("Placemark") > -1))
                        {
                            toDo = 1;
                        }
                        if (toDo == 1 & (line.IndexOf("<name>") > -1))
                        {
                            toDo = 2;
                            startN = line.IndexOf("<name>") + 6;
                            stoppN = line.IndexOf("</name>");
                            name = line.Substring(startN, stoppN - startN);
                            if (name == "Curve_Lines")// bei AOG
                            {
                                toDo = 3;
                            }
                        }
                        if (toDo == 3 & (line.IndexOf("<Placemark>") > -1))
                        {
                            toDo = 4;
                        }

                        if (toDo == 2 & (line.IndexOf("<styleUrl>WaylineStyle</styleUrl>") > -1))// fendt
                        {
                            toDo = 3;
                            fendt = true;
                        }

                        if (fendt & (line.IndexOf("WaylineType") > -1)) // 0 ist AB 3 ist kurve
                        {
                            toDo = 4;
                        }
                        if (fendt & toDo == 4 & (line.IndexOf("<value>") > -1)) // Linienart 0 AB 3 curve
                        {
                            startN = line.IndexOf("<value>") + 7;
                            stoppN = line.IndexOf("</value>");
                            string v = line.Substring(startN, stoppN - startN);
                            if (v == "3") { toDo = 5; }
                            else { fendt = false; toDo = 0; } // Keine ABlinie
                        }


                        if (!fendt & toDo == 4 & (line.IndexOf("<name>") > -1))
                        {
                            toDo = 5;
                            startN = line.IndexOf("<name>") + 6;
                            stoppN = line.IndexOf("</name>");
                            name = line.Substring(startN, stoppN - startN);
                        }



                        if (toDo == 5)
                        {
                            if (line.IndexOf("<LineString>") > -1)
                            {
                                toDo = 6;
                            }
                        }

                        if (toDo == 6)
                        {
                            startIndex = line.IndexOf("<coordinates>");
                        }

                        if (startIndex != -1)
                        {
                            while (true)
                            {
                                endIndex = line.IndexOf("</coordinates>");

                                if (endIndex == -1)
                                {
                                    //just add the line
                                    if (startIndex == -1) coordinates += line.Substring(0);
                                    else coordinates += line.Substring(startIndex + 13);
                                    coordinates += ", ";
                                }
                                else
                                {
                                    if (startIndex == -1) coordinates += line.Substring(0, endIndex) + " ";
                                    else coordinates += line.Substring(startIndex + 13, endIndex - (startIndex + 13));
                                    break;
                                }
                                line = reader.ReadLine();
                                line = line.Trim();
                                startIndex = -1;
                                toDo = 8;// fertig
                            }
                            toDo = 8;// fertig
                            line = coordinates;
                            char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                            string[] numberSets = line.Split(delimiterChars);

                            //at least 3 points
                            if (numberSets.Length > 2)
                            {

                                //calculate the AB Heading
                                abHead = Math.Atan2(Bp.easting - Ap.easting,
                                    Bp.northing - Ap.northing);
                                if (abHead < 0) abHead += glm.twoPI;

                                curve.curveArr.Add(new CCurveLines());

                                //read header $CurveLine
                                curve.curveArr[curve.numCurveLines].Name = name;
                                // get the average heading
                                curve.curveArr[curve.numCurveLines].aveHeading = 0;

                                foreach (string item in numberSets)
                                {
                                    if (item.Length > 7)
                                    {

                                        string[] fix = item.Split(',');
                                        double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                        double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);

                                        pn.ConvertWGS84ToLocal(latK, lonK, out northing, out easting);
                                        
                            
                                        if (toDo > 8) // nächsten
                                        {
                                            Bp.easting = easting;
                                            Bp.northing = northing;
                                            abHead = Math.Atan2(Bp.easting - Ap.easting, Bp.northing - Ap.northing); //if (abHead < 0) abHead += glm.twoPI;
                                            vec3 vecPt = new vec3(easting, northing, abHead);
                                        
                                            curve.curveArr[curve.numCurveLines].curvePts.Add(vecPt); // in punktrichtungsliste hinzufügen
                                            Ap = Bp; // neuen punkt merken
                                        }

                                        if (toDo == 8) // ersten koordinaten A
                                        {
                                            Ap.easting = easting;
                                            Ap.northing = northing;
                                            toDo = 9;
                                            Cp = Ap;
                                        }
                                    }
                                }

                                if (toDo == 9)// AB Anlegen
                                {
                                    abHead = Math.Atan2(Cp.easting - Bp.easting, Cp.northing - Ap.northing); //if (abHead < 0) abHead += glm.twoPI;
                                    curve.curveArr[curve.numCurveLines].aveHeading = 0; 
                                    curve.numCurveLines++;
                                }
                                if (fendt)
                                {
                                    toDo = 0;
                                   
                                }
                                else
                                {
                                    toDo = 3;// fertig mit dieser linie
                                }
                                coordinates = "";
                                startIndex = -1;
                                endIndex = -1;
                            }
                            else
                            {
                                TimedMessageBox(2000, gStr.gsErrorreadingKML, gStr.gsChooseBuildDifferentone);
                            }



                        }


                    }
                    FileSaveCurveLines();
                    return;
                }
                catch (Exception)
                {
                    return;
                }
            }
            // original Load curves
            /*            curve.moveDistance = 0;

            curve.curveArr?.Clear();
            curve.numCurveLines = 0;

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("$CurveLines");
                }
            }

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, gStr.gsFileError, "Missing Curve File");
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;

                        //read header $CurveLine
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            curve.curveArr.Add(new CCurveLines());

                            //read header $CurveLine
                            curve.curveArr[curve.numCurveLines].Name = reader.ReadLine();
                            // get the average heading
                            line = reader.ReadLine();
                            curve.curveArr[curve.numCurveLines].aveHeading = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 1)
                            {
                                curve.curveArr[curve.numCurveLines].curvePts?.Clear();

                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec3 vecPt = new vec3(double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));
                                    curve.curveArr[curve.numCurveLines].curvePts.Add(vecPt);
                                }
                                curve.numCurveLines++;
                            }
                            else
                            {
                                if (curve.curveArr.Count > 0)
                                {
                                    curve.curveArr.RemoveAt(curve.numCurveLines);
                                }
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsCurveLineFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show(this);
                        WriteErrorLog("Load Curve Line" + er.ToString());
                    }
                }
            }

            if (curve.numCurveLines == 0) curve.numCurveLineSelected = 0;
            if (curve.numCurveLineSelected > curve.numCurveLines) curve.numCurveLineSelected = curve.numCurveLines;
            */
        }

        public void ImportFlagsFromKml()
        {
            string coordinates = null;
            string name = "";
            string bezeichnung ="";
            int startIndex = -1;
            int endIndex = -1;
            int toDo = 0;
            int ID = 0;
            int startN = -1;
            int stoppN = -1;
            bool flags = false;

            using (StreamReader reader = new StreamReader(Dateinamen))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        //start to read the file
                        string line = reader.ReadLine();

                        if (flags & line.IndexOf("</Folder>") > -1) // fertig mit linien
                        {
                            toDo = -1;
                        }
                        if (toDo == 0 & (line.IndexOf("Folder") > -1))
                        {
                            toDo = 1;
                        }
                        if (toDo == 1 & (line.IndexOf("Placemark") > -1))
                        {
                            toDo = 2;
                        }
                       if (toDo == 3)
                        {
                            if (line.IndexOf("<name>") > 1) // Name Flag
                            {
                                toDo = 4;
                                startN = line.IndexOf("<name>") + 6;
                                stoppN = line.IndexOf("</name>");
                                name = line.Substring(startN, stoppN - startN);
                            }
                        }
                        if (toDo == 2 & (line.IndexOf("<name>Flag") > -1))
                        {
                            toDo = 3;
                        }
                        if (toDo == 4)
                        {
                            if (line.IndexOf("<Point>") > -1)
                            {
                                toDo = 6;
                            }
                        }

                        if (toDo == 6)
                        {
                            startIndex = line.IndexOf("<coordinates>");
                        }

                        if (startIndex != -1)
                        {
                            while (true)
                            {
                                endIndex = line.IndexOf("</coordinates>");

                                if (endIndex == -1)
                                {
                                    //just add the line
                                    if (startIndex == -1) coordinates += line.Substring(0);
                                    else coordinates += line.Substring(startIndex + 13);
                                    coordinates += ", ";
                                }
                                else
                                {
                                    if (startIndex == -1) coordinates += line.Substring(0, endIndex) + " ";
                                    else coordinates += line.Substring(startIndex + 13, endIndex - (startIndex + 13));
                                    break;
                                }
                                line = reader.ReadLine();
                                line = line.Trim();
                                startIndex = -1;
                                toDo = 7;// fertig
                            }
                            toDo = 7;// fertig
                            line = coordinates;
                            char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                            string[] numberSets = line.Split(delimiterChars);

                            //at least 3 points
                            if (numberSets.Length > 0)
                            {


                                foreach (string item in numberSets)
                                {
                                    if (item.Length > 7)
                                    {

                                        string[] fix = item.Split(',');
                                        double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                        double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);

                                        pn.ConvertWGS84ToLocal(latK, lonK, out northing, out easting);
                                        toDo++;
                                        
                                        
                                    }
                                }

                                if (toDo>6)// flag Anlegen
                                {
                                    ID = flagPts.Count;
                                    ID++;
                                    CFlag flagPt = new CFlag(latK, lonK, easting, northing, 0, 0, ID, name);
                                    flagPts.Add(flagPt);
                                }
                                toDo = 1; // fertig mit dieser linie
                                coordinates = "";
                                startIndex = -1;
                                endIndex = -1;
                                flags = true;
                            }
                            else
                            {
                                TimedMessageBox(2000, gStr.gsFlagFileIsCorrupt, gStr.gsChooseBuildDifferentone);
                            }



                        }


                    }
                    FileSaveFlags();
                    return;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        //************************************************************************************************************************************************
        // FRitzpunkte mit kml aus punkten

        public void FileCreateAllGPSpositionenKML(string DN = "")
        {
            //Dateinamen = "GPSpositionenX";
            //FileCreateGPSpositionenKML();
            //Dateinamen = "GPSpositionenFS";
            //FileCreateKMLpositionenFS();
            //Dateinamen = "GPSpositionenFS";
            //FileCreateKMLpositionenFS(0);
            Dateinamen = "GPSpositionenNurBrache";
            FileCreateKMLpositionenFS(4);
            // ART 2 sind sonstige Punkte
            //Dateinamen = "GPSpositionenOhneBrache";
            //FileCreateKMLpositionenFS(2);
            Dateinamen = "GPSpositionenAcker";
            FileCreateKMLpositionenFS(1);
            Dateinamen = "GpsPositionen_" + displayFieldName;
            FileCreateKMLpositionenFS(1);

        }
        private void FileCreateKMLpositionenFS(byte Art = 0)
        {
            //string Pos;
            string Koordinaten;
            string dirField;
            string Lat1 = "0";
            string Lon1 = "0";
            string Lat0 = "48.32";
            string Lon0 = "15.72";
            int erstezeile = 0;

            if (currentFieldDirectory.Length > 3 & isJobStarted == true)
            {
                dirField = fieldsDirectory + currentFieldDirectory + "\\";
            }
            else
            {
                dirField = fieldsDirectory + "\\"; //fieldsDirectory Root
            }

            using (StreamWriter writer = new StreamWriter(dirField + Dateinamen + ".kml"))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"" xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom"">");

                writer.WriteLine(@"<Document>");
                writer.WriteLine(@"<name>" + currentFieldDirectory + "</name>");// feldnamen

                writer.WriteLine(@"<Style id=""sh_ylw-pushpin"">");
                writer.WriteLine(@"<IconStyle>");
                writer.WriteLine(@"<scale> 1.3</scale>");
                writer.WriteLine(@"<Icon>");
                writer.WriteLine(@"<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>");
                writer.WriteLine(@"</Icon>");
                writer.WriteLine(@"<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
                writer.WriteLine(@"</IconStyle>");
                writer.WriteLine(@"<BalloonStyle>");
                writer.WriteLine(@"</BalloonStyle>");
                writer.WriteLine(@"<PolyStyle>");
                writer.WriteLine(@"<color>66ffaaff</color>");
                writer.WriteLine(@"</PolyStyle>");
                writer.WriteLine(@"</Style>");

                writer.WriteLine(@"<Style id=""sn_ylw-pushpin"">");
                writer.WriteLine(@"<IconStyle>");
                writer.WriteLine(@"<scale> 1.1 </scale>");
                writer.WriteLine(@"<Icon>");
                writer.WriteLine(@"<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>");
                writer.WriteLine(@"</Icon>");
                writer.WriteLine(@"<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
                writer.WriteLine(@"</IconStyle>");
                writer.WriteLine(@"<BalloonStyle>");
                writer.WriteLine(@"</BalloonStyle>");
                writer.WriteLine(@"<PolyStyle>");
                writer.WriteLine(@"<color>66ffaaff</color>");
                writer.WriteLine(@"</PolyStyle>");
                writer.WriteLine(@"</Style>");

                writer.WriteLine(@"<StyleMap id=""msn_ylw-pushpin"">");
                writer.WriteLine(@"<Pair>");
                writer.WriteLine(@"<key>normal</key>");
                writer.WriteLine(@"<styleUrl>#sn_ylw-pushpin</styleUrl>");
                writer.WriteLine(@"</Pair>");

                writer.WriteLine(@"<Pair>");
                writer.WriteLine(@"<key>highlight</key>");
                writer.WriteLine(@"<styleUrl>#sh_ylw-pushpin</styleUrl>");
                writer.WriteLine(@"</Pair>");
                writer.WriteLine(@"</StyleMap>");

                writer.WriteLine(@"<Placemark>");
                writer.WriteLine(@"<name>" + currentFieldDirectory + "</name>");
                writer.WriteLine(@"<styleUrl>#msn_ylw-pushpin</styleUrl>");
                writer.WriteLine(@"<Polygon>");
                writer.WriteLine(@"<tessellate>1</tessellate>");
                writer.WriteLine(@"<outerBoundaryIs>");
                writer.WriteLine(@"<LinearRing>");
                writer.WriteLine(@"<coordinates>");

                int i = 0;
                foreach (var item in cPunktFs.punktArr)
                {
                    //$Positions
                    //
                    //WGS84;15.72315678901;48.172803131734;utmEastNorth;555555;5555555;feldEastNorth;1234.123;1234.123;Altitude;321.123;Nr;001;Art;0;Name;FeldDateinamen
                    //WGS84; X->Longitude ; Y->Latitude   ;utmEastNorth;UTM->X;UTM->Y ;feldEastNorth;F.utm->X;F.utm->Y;Altitude;mmm.ddd;Nr;001;Art,3; FeldDateinamen
                    if ((Art == cPunktFs.punktArr[i].Art))
                    {



                        Lat0 = cPunktFs.punktArr[i].Latidude.ToString("0.000000000000").Replace(",", ".");
                        Lon0 = cPunktFs.punktArr[i].Longitude.ToString("0.000000000000").Replace(",", ".");
                        if (erstezeile == 0)
                        {
                            Lat1 = Lat0;
                            Lon1 = Lon0;
                            erstezeile = 1;
                        }
                        Koordinaten = Lon0 + "," + Lat0 + ",0 ";
                        writer.Write(Koordinaten);
                    }
                    i++;
                }
                // zum schluss nochmals startkoordinaten um feld fertig einzuranden
                Koordinaten = Lon1 + "," + Lat1 + ",0 ";
                writer.Write(Koordinaten);
                writer.WriteLine("");// nur zeilenumbruch

                writer.WriteLine(@"</coordinates>");
                writer.WriteLine(@"</LinearRing>");
                writer.WriteLine(@"</outerBoundaryIs>");
                writer.WriteLine(@"</Polygon>");
                writer.WriteLine(@"</Placemark>");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>");


            }
        }



        public void FileSaveGPSpositionenFS()
        {
            string Pos = "";
            string dirField;
            string myFileName = "GPSpositionenFS.txt";
            if (currentFieldDirectory.Length > 3 & isJobStarted == true)
            {
                dirField = fieldsDirectory + currentFieldDirectory + "\\";
            }
            else
            {
                dirField = fieldsDirectory + "\\"; //fieldsDirectory Root
            }
            File.WriteAllText(dirField + myFileName, Pos);// Leere Datei erstellen

            /*
            string dirField = fieldsDirectory + "\\"; //fieldsDirectory Root
            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }
            //string DateiName = txtDateiname1.Text;
            */

            int i = 0;
            foreach (var item in cPunktFs.punktArr)
            {
                //$Positions
                //
                //WGS84;15.72315678901;48.172803131734;utmEastNorth;555555;5555555;feldEastNorth;1234.123;1234.123;Altitude;321.123;Nr;001;Art;0;Name;FeldDateinamen
                //WGS84; X->Longitude ; Y->Latitude   ;utmEastNorth;UTM->X;UTM->Y ;feldEastNorth;F.utm->X;F.utm->Y;Altitude;mmm.ddd;Nr;001;Art,3; FeldDateinamen


                Pos = "WGS84" +
                      ";" + cPunktFs.punktArr[i].Longitude.ToString("0.00000000000") +
                      ";" + cPunktFs.punktArr[i].Latidude.ToString("0.00000000000") +
                      ";utmEN" +
                      ";" + cPunktFs.punktArr[i].UTMEast.ToString("0.000") +
                      ";" + cPunktFs.punktArr[i].UTMNorth.ToString("0.000") +
                      ";FeldEN" +
                      ";" + cPunktFs.punktArr[i].FeldEast.ToString("0.000") +
                      ";" + cPunktFs.punktArr[i].FeldNorth.ToString("0.000") +
                      ";Hoehe" +
                      ";" + cPunktFs.punktArr[i].Altitude.ToString("0.000") +
                      ";Nr" +
                      ";" + cPunktFs.punktArr[i].Nr.ToString("0000") +
                      ";Art" +
                      ";" + cPunktFs.punktArr[i].Art.ToString("00") +
                      ";" + Convert.ToString(cPunktFs.punktArr[i].Art, 2).PadLeft(8, '0') +
                      ";Name" +
                      ";" + currentFieldDirectory + "\r\n";

                File.AppendAllText(dirField + myFileName, Pos);


                i++;
            }


        }

        public void FileLoadGPSpositionenFS()
        {
            PositionsNummer = 1;
            int Nr = 0;
            //make sure at least a global blank AB Line file exists
            string dirField;
            if (currentFieldDirectory.Length > 3 & isJobStarted == true)
            {
                dirField = fieldsDirectory + currentFieldDirectory + "\\";
            }
            else
            {
                dirField = fieldsDirectory + "\\"; //fieldsDirectory Root
            }
            string myFileName = "GPSpositionenFS.txt";

            if (!File.Exists(dirField + myFileName))
            {
                FileClearGPSpositionen();
                TimedMessageBox(2000, gStr.gsFileError, "Datei [" + dirField + myFileName + "] Neu angelegt");
            }
            else
            {
                using (StreamReader reader = new StreamReader(dirField + myFileName))
                {
                    try
                    {
                        cPunktFs.punktArr.Clear(); // loesche gelesenes array
                        string line;
                        //read all the lines
                        for (int i = 0; !reader.EndOfStream; i++)
                        {

                            line = reader.ReadLine();
                            string[] words = line.Split(';');

                            if (words.Length < 9) break;

                            cPunktFs.punktArr.Add(new CPunktFS());
                            //WGS84
                            cPunktFs.punktArr[i].Longitude = String2Double(words[1]);
                            cPunktFs.punktArr[i].Latidude = String2Double(words[2]);
                            //UTM
                            //cPunktFs.punktArr[i].UTMEast = uEg = String2Double(words[4]);
                            //cPunktFs.punktArr[i].UTMNorth = uNg = String2Double(words[5]);
                            //FeldUTM
                            //cPunktFs.punktArr[i].FeldEast = String2Double(words[7]);
                            //cPunktFs.punktArr[i].FeldNorth = String2Double(words[8]);
                            //Utm2FUtm();
                            //cPunktFs.punktArr[i].FeldEast = uEf;
                            //cPunktFs.punktArr[i].FeldNorth = uNf;
                            //Altitude
                            cPunktFs.punktArr[i].Altitude = String2Double(words[10]);
                            //Punktnummer
                            Nr = (int)(String2Double(words[12]));
                            cPunktFs.punktArr[i].Nr = Nr;
                            //PunktArt
                            cPunktFs.punktArr[i].Art = (byte)(String2Double(words[14]));
                            //Name
                            cPunktFs.punktArr[i].Name = words[17];
                            if (Nr > PositionsNummer) PositionsNummer = Nr;// hoechste nummer merken
                            pn.ConvertWGS84ToLocal(cPunktFs.punktArr[i].Latidude, cPunktFs.punktArr[i].Longitude, out double northing, out double easting);
                            cPunktFs.punktArr[i].FeldEast = easting;
                            cPunktFs.punktArr[i].FeldNorth = northing;
                            cPunktFs.punktArr[i].UTMEast = easting;
                            cPunktFs.punktArr[i].UTMNorth = northing;
                        }
                    }
                    catch (Exception er)
                    {
                        var form = new FormTimedMessage(2000, "Datei [GPSpositionenFS.txt] beschaedigt", "Datei bitte loeschen!!!");
                        form.Show();
                        WriteErrorLog("FieldOpen, Loading ABLine, Corrupt ABLine File" + er);
                    }
                }
            }

        }
        public void FileCreateGPSpositionenFS(byte punktArt = 0, double Lon = 0, double Lat = 0)
        {
            //$Positions
            //
            //WGS84;15.72315678901;48.172803131734;utmEastNorth;555555;5555555;feldEastNorth;1234.123;1234.123;Altitude;321.123;Nr;001;Art;0;Name;FeldDateinamen
            //WGS84; X->Longitude ; Y->Latitude   ;utmEastNorth;UTM->X;UTM->Y ;feldEastNorth;F.utm->X;F.utm->Y;Altitude;mmm.ddd;Nr;001;Art,3; FeldDateinamen

            string Pos;
            string dirField;
            if (currentFieldDirectory.Length > 3 & isJobStarted == true)
            {
                dirField = fieldsDirectory + currentFieldDirectory + "\\";
            }
            else
            {
                dirField = fieldsDirectory + "\\"; //fieldsDirectory Root
            }
            /*
            string dirField = fieldsDirectory + "\\"; //fieldsDirectory Root
            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }
            //string DateiName = txtDateiname1.Text;
            */
            PositionsNummer++; // alle Positionen speichern aber nicht erhöhen

            if (Lon==0 | Lat == 0)//Keine Parameter uebergeben dann die Antenne Nehmen
            {
                Lon = pn.longitude;
                Lat = pn.latitude;
            }
            string myFileName = "GPSpositionenFS.txt";
            Pos = "WGS84" +
                  ";" + Lon.ToString("0.00000000000") +
                  ";" + Lat.ToString("0.00000000000") +
                  ";utmEN" +
                  ";" + 0.ToString("0.000") +
                  ";" + 0.ToString("0.000") +
                  ";FeldEN" +
                  ";" + 0.ToString("0.000") +
                  ";" + 0.ToString("0.000") +
                  ";Hoehe" +
                  ";" + pn.altitude.ToString("0.000") +
                  ";Nr" +
                  ";" + PositionsNummer.ToString("0000") +
                  ";Art" +
                  ";" + punktArt.ToString("00") +
                  ";" + Convert.ToString(punktArt, 2).PadLeft(8, '0') +
                  ";Name" +
                  ";" + currentFieldDirectory + "\r\n";

            File.AppendAllText(dirField + myFileName, Pos);
        }

        public void FileClearGPSpositionen()
        {
            string Pos;
            string dirField;
            if (currentFieldDirectory.Length > 3 & isJobStarted == true)
            {
                dirField = fieldsDirectory + currentFieldDirectory + "\\";
            }
            else
            {
                dirField = fieldsDirectory + "\\"; //fieldsDirectory Root
            }
            Pos = "";
            string myFileName = "GPSpositionenFS.txt";
            /*
            File.WriteAllText(dirField + myFileName, Pos);
            myFileName = "GPSpositionenOB.txt";
            File.WriteAllText(dirField + myFileName, Pos);
            myFileName = "GPSpositionenNB.txt";
            File.WriteAllText(dirField + myFileName, Pos);
            myFileName = "GPSpositionenNP.txt";
            File.WriteAllText(dirField + myFileName, Pos);
            myFileName = "GPSpositionenAP.txt";
            File.WriteAllText(dirField + myFileName, Pos);
            */
            //myFileName = "GPSpositionenFS.txt";
            File.WriteAllText(dirField + myFileName, Pos);
            PositionsNummer = 0;
            TimedMessageBox(3000, "GpsPunkte geleert", "GpsPunkt = " + PositionsNummer.ToString());


        }

        public void GpsPositionenInsert(int Position = -1, byte punktArt = 0, double Lon = 0, double Lat = 0)
        {
            int i = Position;
            if (Position >= 0)
            {
                cPunktFs.punktArr.Insert(Position, new CPunktFS());
            }
            else
            {
                cPunktFs.punktArr.Add(new CPunktFS());
                i = cPunktFs.punktArr.Count();
            }
            if (Lon == 0 | Lat == 0)//Keine Parameter uebergeben dann die Antenne Nehmen
            {
                Lon = pn.longitude;
                Lat = pn.latitude;
            }

            //WGS84
            cPunktFs.punktArr[i].Longitude = Lon;
            cPunktFs.punktArr[i].Latidude = Lat;
            //UTM
            cPunktFs.punktArr[i].UTMEast =0;
            cPunktFs.punktArr[i].UTMNorth =0;
            //FeldUTM
            cPunktFs.punktArr[i].FeldEast = 0;
            cPunktFs.punktArr[i].FeldNorth = 0;
            //Altitude
            cPunktFs.punktArr[i].Altitude = pn.altitude;
            //Punktnummer
            PositionsNummer++;
            cPunktFs.punktArr[i].Nr = PositionsNummer;
            //PunktArt
            cPunktFs.punktArr[i].Art = punktArt;
            //Name
            cPunktFs.punktArr[i].Name = currentFieldDirectory;

            FileSaveGPSpositionenFS();
        }
        public void GpsPositionenUpdate(int Position = -1,  double Lon = 0, double Lat = 0)
        {
            // Update einer vorhandener position
            int i = Position;
            if (Position >= 0) // position muss angegeben werden
            {
 
                if (Lon == 0 | Lat == 0)//Keine Parameter uebergeben dann die Antenne Nehmen
                {
                    Lon = pn.longitude;
                    Lat = pn.latitude;
                }

                //WGS84
                cPunktFs.punktArr[i].Longitude = Lon;
                cPunktFs.punktArr[i].Latidude = Lat;
                //UTM
                cPunktFs.punktArr[i].UTMEast = 0;
                cPunktFs.punktArr[i].UTMNorth = 0;
                //FeldUTM
                cPunktFs.punktArr[i].FeldEast = 0;
                cPunktFs.punktArr[i].FeldNorth = 0;
                //Altitude
                cPunktFs.punktArr[i].Altitude = pn.altitude;
                //Punktnummer
                //cPunktFs.punktArr[i].Nr = Position+1;//+1
                //PunktArt
                //cPunktFs.punktArr[i].Art = punktArt;
                //Name
                //cPunktFs.punktArr[i].Name = currentFieldDirectory;

                FileSaveGPSpositionenFS();
            }
        }

        public void FileLoadGPSpositionenSuche(int WgsUtm, int WgsLonId, int WgsLatId, int UtmEastId, int UtmNorthId, int NrId)
        {
            //PositionsNummer = 1;
            int Nr = 0;
            //make sure at least a global blank AB Line file exists
            string dirField;
            if (currentFieldDirectory.Length > 3 & isJobStarted == true)
            {
                dirField = fieldsDirectory + currentFieldDirectory + "\\";
            }
            else
            {
                dirField = fieldsDirectory + "\\"; //fieldsDirectory Root
            }
            string myFileName = "GPSpositionenSuche.txt";

            if (!File.Exists(dirField + myFileName))
            {

                TimedMessageBox(2000, gStr.gsFileError, "suchDatei [" + dirField + myFileName + "] nicht vorhanden");
            }
            else
            {
                using (StreamReader reader = new StreamReader(dirField + myFileName))
                {
                    try
                    {
                        cPunktFSuche.punktArr.Clear(); // loesche gelesenes array
                        string line;
                        //read all the lines
                        for (int i = 0; !reader.EndOfStream; i++)
                        {

                            line = reader.ReadLine();
                            string[] words = line.Split(';');

                            if (words.Length < 9) break;

                            cPunktFSuche.punktArr.Add(new CPunktFS());
                            //WGS84
                            cPunktFSuche.punktArr[i].Longitude = String2Double(words[WgsLonId]);
                            cPunktFSuche.punktArr[i].Latidude = String2Double(words[WgsLatId]);
                            //UTM
                            cPunktFSuche.punktArr[i].UTMEast = String2Double(words[UtmEastId]);
                            cPunktFSuche.punktArr[i].UTMNorth = String2Double(words[UtmNorthId]);
                            //FeldUTM
                            //cPunktFs.punktArr[i].FeldEast = String2Double(words[7]);
                            //cPunktFs.punktArr[i].FeldNorth = String2Double(words[8]);
                            //Altitude
                            //cPunktFs.punktArr[i].Altitude = String2Double(words[10]);
                            //Punktnummer
                            Nr = (int)(String2Double(words[NrId]));
                            if (NrId == 0)
                            {
                                Nr++;
                            }
                            cPunktFSuche.punktArr[i].Nr = Nr;
                            //PunktArt
                            //cPunktFs.punktArr[i].Art = (byte)(String2Double(words[14]));
                            //Name
                            //cPunktFs.punktArr[i].Name = words[17];

                        }
                    }
                    catch (Exception er)
                    {
                        var form = new FormTimedMessage(2000, "Datei [GPSpositionenSuche.txt] beschaedigt", "Datei bitte kontrollieren!!!");
                        form.Show();
                        WriteErrorLog("GpsPositionenSuche.txt Dateifehler" + er);
                    }
                }
            }

        }


    }
}