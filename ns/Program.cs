using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;
using System.Globalization;


namespace ns
{
    class Program
    {
        static void Main(string[] args)
        {
            

            // Global vars
            string url = "http://aerospace.illinois.edu/m-selig/ads/coord_database.html";
            string datFilesPath = @"C:\Users\Panayotis\Desktop\nsFiles\total\";

            // Get all links and descriptions
            // FTS must be enabled on DB: http://msdn.microsoft.com/en-us/library/ms187787(v=sql.120).aspx
            GetLinks(url);

            // Populate database using file records
            GetAllData(datFilesPath);

            Console.WriteLine("END");
            Console.ReadLine();
        }

        private static void SavePoint(string recordId, float x, float y)
        {
            using (AirFoilDBEntities db = new AirFoilDBEntities())
            {
                Coordinates newPoint = new Coordinates();
                newPoint.DatId = recordId;
                newPoint.Latitude = x;
                newPoint.Longitude = y;
                db.Coordinates.Add(newPoint);
                db.SaveChanges();
            }
        }

        private static void SaveDat(string recordId, string recordPath, string recordDescription)
        {
            using (AirFoilDBEntities db = new AirFoilDBEntities())
            {
                Dat record = new Dat();
                record.Id = recordId;
                record.Path = recordPath;
                record.Description = recordDescription;
                db.Dat.Add(record);
                db.SaveChanges();
            }
        }

        private static void GetAllData(string filepath)
        {
            string[] fileEntries = Directory.GetFiles(filepath);

            string pointPattern = @"([-]?[01]?.?[0-9]+)\s+([-]?[01]?.?[0-9]+)";

            Regex pointRE = new Regex(pointPattern, RegexOptions.IgnoreCase);


            foreach (string filename in fileEntries)
            {
                string file = Path.GetFileName(filename);

                string[] lines = File.ReadAllLines(filename);

                foreach (string line in lines.Skip(1))
                {
                    if (pointRE.IsMatch(line))
                    {
                        Console.WriteLine("File: {0} has {1}.", file, line);

                        float x = float.Parse(pointRE.Match(line).Groups[1].Value, CultureInfo.GetCultureInfo("en-GB"));

                        float y = float.Parse(pointRE.Match(line).Groups[2].Value, CultureInfo.GetCultureInfo("en-GB"));

                        SavePoint(file, x, y);
                    }
                }
            }
        }

        private static void GetLinks(string url)
        {
            string datLinksUrl = url;

            WebClient client = new WebClient();

            string htmlText = client.DownloadString(datLinksUrl);

            string datPattern = @"<a\shref=""(coord.*)\/(.*.dat)"">.*.dat<\/a>\s\\\s(.*)<br>";

            Regex datRE = new Regex(datPattern, RegexOptions.IgnoreCase);

            bool matchFlag = Regex.IsMatch(htmlText, datPattern, RegexOptions.IgnoreCase);

                if (matchFlag)
                {
                    foreach (Match match in datRE.Matches(htmlText))
                    {
                        if (match.Groups[2].Value == "goe765.dat" || match.Groups[2].Value == "sc20714.dat") { Console.WriteLine("Duplicates Found!"); }
                        else
                        SaveDat(match.Groups[2].Value, match.Groups[1].Value, match.Groups[3].Value);
                    }
                }
             
        }
    }
}
