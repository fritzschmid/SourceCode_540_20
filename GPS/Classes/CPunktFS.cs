using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS
{
    public class CPunktFS
    {
        public double Nr = 0;
        public double FeldEast = 0;
        public double FeldNorth = 0;
        public double UTMEast = 0;
        public double UTMNorth = 0;
        public string Name = "";
        public double Latidude = 0;
        public double Longitude = 0;
        public double Altitude = 0;
        public byte Art = 0;

        public List<CPunktFS> punktArr = new List<CPunktFS>();

        private readonly FormGPS mf;

        public CPunktFS(FormGPS _f)
        {
            mf = _f;

        }

        public CPunktFS()
        {
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        private const double sm_a = 6378137.0;

        private const double sm_b = 6356752.314;
        private const double UTMScaleFactor = 0.9996;
        //private double UTMScaleFactor2 = 1.0004001600640256102440976390556;
        public double zone=31;
        public double centralMeridian, convergenceAngle;
        //private double latitude, longitude;

        //public double latStart, lonStart;

        public double fPlatitude, fPlongitude;
        public double fPeasting, fPnorthing;
 
        public void LatLonToUTM()
        {
            //#region Convergence

            double[] xy = DecDeg2UTM(fPlatitude, fPlongitude);
            //keep a copy of actual easting and northings
            fPeasting = xy[0];
            fPnorthing = xy[1];


            //compensate for the fact the zones lines are a grid and the world is spheroid
            //fix.easting = (Math.Cos(-convergenceAngle) * east) - (Math.Sin(-convergenceAngle) * nort);
            //fix.northing = (Math.Sin(-convergenceAngle) * east) + (Math.Cos(-convergenceAngle) * nort);
            //#endregion Convergence
        }
        private double ArcLengthOfMeridian(double phi)
        {
            const double n = (sm_a - sm_b) / (sm_a + sm_b);
            double alpha = ((sm_a + sm_b) / 2.0) * (1.0 + (Math.Pow(n, 2.0) / 4.0) + (Math.Pow(n, 4.0) / 64.0));
            double beta = (-3.0 * n / 2.0) + (9.0 * Math.Pow(n, 3.0) * 0.0625) + (-3.0 * Math.Pow(n, 5.0) / 32.0);
            double gamma = (15.0 * Math.Pow(n, 2.0) * 0.0625) + (-15.0 * Math.Pow(n, 4.0) / 32.0);
            double delta = (-35.0 * Math.Pow(n, 3.0) / 48.0) + (105.0 * Math.Pow(n, 5.0) / 256.0);
            double epsilon = (315.0 * Math.Pow(n, 4.0) / 512.0);
            return alpha * (phi + (beta * Math.Sin(2.0 * phi))
                    + (gamma * Math.Sin(4.0 * phi))
                    + (delta * Math.Sin(6.0 * phi))
                    + (epsilon * Math.Sin(8.0 * phi)));
        }

        private double[] MapLatLonToXY(double phi, double lambda, double lambda0)
        {
            double[] xy = new double[2];
            double ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);
            double nu2 = ep2 * Math.Pow(Math.Cos(phi), 2.0);
            double n = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nu2));
            double t = Math.Tan(phi);
            double t2 = t * t;
            double l = lambda - lambda0;
            double l3Coef = 1.0 - t2 + nu2;
            double l4Coef = 5.0 - t2 + (9 * nu2) + (4.0 * (nu2 * nu2));
            double l5Coef = 5.0 - (18.0 * t2) + (t2 * t2) + (14.0 * nu2) - (58.0 * t2 * nu2);
            double l6Coef = 61.0 - (58.0 * t2) + (t2 * t2) + (270.0 * nu2) - (330.0 * t2 * nu2);
            double l7Coef = 61.0 - (479.0 * t2) + (179.0 * (t2 * t2)) - (t2 * t2 * t2);
            double l8Coef = 1385.0 - (3111.0 * t2) + (543.0 * (t2 * t2)) - (t2 * t2 * t2);

            /* Calculate easting (x) */
            xy[0] = (n * Math.Cos(phi) * l)
                + (n / 6.0 * Math.Pow(Math.Cos(phi), 3.0) * l3Coef * Math.Pow(l, 3.0))
                + (n / 120.0 * Math.Pow(Math.Cos(phi), 5.0) * l5Coef * Math.Pow(l, 5.0))
                + (n / 5040.0 * Math.Pow(Math.Cos(phi), 7.0) * l7Coef * Math.Pow(l, 7.0));

            /* Calculate northing (y) */
            xy[1] = ArcLengthOfMeridian(phi)
                + (t / 2.0 * n * Math.Pow(Math.Cos(phi), 2.0) * Math.Pow(l, 2.0))
                + (t / 24.0 * n * Math.Pow(Math.Cos(phi), 4.0) * l4Coef * Math.Pow(l, 4.0))
                + (t / 720.0 * n * Math.Pow(Math.Cos(phi), 6.0) * l6Coef * Math.Pow(l, 6.0))
                + (t / 40320.0 * n * Math.Pow(Math.Cos(phi), 8.0) * l8Coef * Math.Pow(l, 8.0));

            return xy;
        }

        public double[] DecDeg2UTM(double latitude, double longitude)
        {
            //only calculate the zone once!
            if (!mf.isFirstFixPositionSet) zone = Math.Floor((longitude + 180.0) * 0.16666666666666666666666666666667) + 1;

            double[] xy = MapLatLonToXY(latitude * 0.01745329251994329576923690766743,
                                        longitude * 0.01745329251994329576923690766743,
                                        (-183.0 + (zone * 6.0)) * 0.01745329251994329576923690766743);

            xy[0] = (xy[0] * UTMScaleFactor) + 500000.0;
            xy[1] *= UTMScaleFactor;
            if (xy[1] < 0.0)
                xy[1] += 10000000.0;
            return xy;
        }

    }
}
