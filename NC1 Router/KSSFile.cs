using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC1_Router
{
    public class KSSFile
    {
        public string JobNumber;
        public string JobDescription;
        public Dictionary<string, Part> PartsDictionary;
        public string Heading;       

        public string FindJobDSTVDirectory()
        {
            try
            {
                string[] anyjobfolders = Directory.GetDirectories(@"Z:\Drawings in Process\2015 JOBS");
                var listfolders = anyjobfolders.ToList<string>();
                string rightjob = anyjobfolders.ToList<string>().Where(p => p.Contains(JobNumber)).First<string>();
                string[] rightjobfolders = Directory.GetDirectories(rightjob);
                listfolders = anyjobfolders.ToList<string>();
                return rightjobfolders.ToList<string>().Where(p => p.Contains("NC1")).First<string>();
            }
            catch (Exception)
            {
                try
                {
                    string[] anyjobfolders = Directory.GetDirectories(@"Z:\Drawings in Process\2016 JOBS");
                    var listfolders = anyjobfolders.ToList<string>();
                    string rightjob = anyjobfolders.ToList<string>().Where(p => p.Contains(JobNumber)).First<string>();
                    string[] rightjobfolders = Directory.GetDirectories(rightjob);
                    listfolders = anyjobfolders.ToList<string>();
                    return rightjobfolders.ToList<string>().Where(p => p.Contains("NC1")).First<string>();

                }
                catch (Exception)
                {

                    return "Job PDFs folder not found";
                }
            }


        }
    }
}
