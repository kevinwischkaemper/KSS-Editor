using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NC1_Router
{
    public class KSSParser
    {
        public List<string> kssPLHalf = new List<string>();

        public KSSFile ParseKSS(string kssfilepath, bool multiplyBoltQTYs)
        {
            var lines = File.ReadAllLines(kssfilepath);
            int counter = 0;

            string jobNumber = "";
            string jobDescription = "";
            var partsDictionary = new Dictionary<string, Part>();
            string lastpart = "";

            foreach (string line in lines)
            {
                var splitline = line.Split(new[] { ',' });

                #region Header Line
                if (splitline[0].Contains("H"))
                {
                    jobNumber = Regex.Match(splitline[1], @"\d+").Value;
                    jobDescription = splitline[2];
                }
                #endregion

                #region Detail Line
                if (splitline[0].Contains("D"))
                {
                    var part = new Part() { AssemblyMarks = new List<Tuple<string, int>>(), IsMain = true, RoutingCodes = new List<string>() };
                    part.RoutingCodes.Add(" ");
                    part.DrawingNumber = splitline[1];
                    if (!part.DrawingNumber.Contains("FB") && !part.DrawingNumber.Contains("FD"))
                    {
                        var prefixmatch = Regex.Match(part.DrawingNumber, @"[A-Z]+");
                        part.DrawingNumber = part.DrawingNumber.Replace(prefixmatch.Value, "FB");
                    }
                    part.PieceMark = splitline[3];
                    if (splitline[4].TrimEnd(new[] { 'L', 'R' }) != splitline[3].TrimEnd(new[] { 'L', 'R' }))
                    {
                        part.PieceMark = splitline[4];
                        part.IsMain = false;
                        part.MainMember = splitline[3];
                    }
                    else if (!(partsDictionary.ContainsKey(splitline[3].TrimEnd(new[] { 'R', 'L' }) + "R") || partsDictionary.ContainsKey(splitline[3].TrimEnd(new[] { 'R', 'L' }) + "L")))
                    {
                        part.PieceMark = splitline[4];
                        part.IsMain = true;
                        part.MainMember = splitline[4];
                    }
                    else
                    {
                        part.PieceMark = splitline[4];
                        part.IsMain = false;
                        if (part.PieceMark.Contains("L"))
                            part.MainMember = $"{splitline[3]}R";
                        else
                            part.MainMember = $"{splitline[3]}L";
                    }
                    //if (!part.IsMain && !partsDictionary.ContainsKey(part.MainMember))
                    //{
                    //    part.IsMain = true;
                    //    part.PieceMark = part.MainMember;
                    //}

                    part.Quantity = Convert.ToInt32(splitline[5]);
                    part.ShapeType = splitline[6];
                    part.ShapeDetails = splitline[7].Replace('x', 'X');
                    part.Grade = splitline[8];
                    part.Length = Convert.ToDouble(splitline[9]);
                    part.Finish = splitline[10];
                    if ((part.ShapeType.Contains("PL") || part.ShapeType.Contains("FB")) && part.ShapeDetails.Contains("1/2") && part.Length > 1523)
                        kssPLHalf.Add(part.PieceMark.Trim());
                    if (part.ShapeType == "RB")
                        part.ShapeType = "BO";
                    if (part.ShapeType == "HS" || part.ShapeType == "MB")
                    {
                        if (Regex.IsMatch(splitline[11], @"(?i)f"))
                            continue;
                        var boltdetails = part.ShapeDetails.Split(new[] { 'X' });
                        if (!part.Grade.Contains("TC"))
                        {
                            if (boltdetails[0] == "3/4" && part.Length < 130) // 5
                            {
                                part.ShapeType = "BO";
                                part.ShapeDetails = $"3/4 A325 {boltdetails[1].Replace('-', ' ')}";
                            }
                            part.PieceMark = $"{counter}";
                            if (part.Length > 60.5 && part.Length < 66.5) // 2 1/2
                                part.PieceMark = "U0001";
                            if (part.Length > 73.2 && part.Length < 79.2) // 3
                                part.PieceMark = "U0002";
                            if (part.Length > 54.15 && part.Length < 60)// 2 1/4
                                part.PieceMark = "U0003";
                            if (part.Length > 47.8 && part.Length < 53.8)// 2
                                part.PieceMark = "U0009";
                            if (part.Length > 41.5 && part.Length < 47.4)// 1 3/4
                                part.PieceMark = "U0010";
                            if (part.Length > 124 && part.Length < 130) // 5
                                part.PieceMark = "U0014";
                            if (part.Length > 111.3 && part.Length < 117.3)// 4 1/2
                                part.PieceMark = "U0017";
                            if (part.Length > 35.1 && part.Length < 41.1)// 1 1/2
                                part.PieceMark = "U0019";
                            part.RoutingCodes.Add("bolt");
                        }
                        else
                        {
                            part.ShapeType = "TC";
                            part.ShapeDetails = $"{boltdetails[0]}X{boltdetails[1].Replace('-', ' ')}";
                            part.Grade = Regex.Replace(part.Grade, "TC", "").Trim();
                            if (part.ShapeDetails == "7/8X2 1/4") part.PieceMark = "TC10";
                            if (part.ShapeDetails == "7/8X6 1/2") part.PieceMark = "TC01";
                            if (part.ShapeDetails == "3/4X2 1/2") part.PieceMark = "TC02";
                            if (part.ShapeDetails == "3/4X2 1/4") part.PieceMark = "TC03";
                            if (part.ShapeDetails == "3/4X1 3/4") part.PieceMark = "TC04";
                            if (part.ShapeDetails == "3/4X2") part.PieceMark = "TC05";
                            if (part.ShapeDetails == "3/4X3") part.PieceMark = "TC06";
                            if (part.ShapeDetails == "3/4X3 1/4") part.PieceMark = "TC07";
                            if (part.ShapeDetails == "3/4X3 1/2") part.PieceMark = "TC08";
                            if (part.ShapeDetails == "3/4X2 3/4") part.PieceMark = "TC09";
                            part.RoutingCodes.Add("bolt");
                        }
                    }
                        if (!partsDictionary.ContainsKey(part.PieceMark))
                        {
                            partsDictionary.Add(part.PieceMark, part);
                            lastpart = part.PieceMark;
                        }

                        if (!part.IsMain)
                        {
                            {
                                string adjustedmain = "";
                                if (partsDictionary.ContainsKey(part.MainMember))
                                    adjustedmain = part.MainMember;
                                else if (partsDictionary.ContainsKey($"{part.MainMember}L"))
                                    adjustedmain = $"{part.MainMember}L";
                                else if (partsDictionary.ContainsKey($"{part.MainMember}R"))
                                    adjustedmain = $"{part.MainMember}R";
                                if (((part.ShapeType == "BO" || part.ShapeType == "TC") && part.ShapeDetails.Contains("/") ) && multiplyBoltQTYs)
                                {
                                    part.Quantity = part.Quantity * partsDictionary[adjustedmain].Quantity;
                                }
                                if (partsDictionary[adjustedmain].AssemblyMarks.Any(x => x.Item1 == part.PieceMark))
                                {
                                    int currenquantity = partsDictionary[adjustedmain].AssemblyMarks.Where(x => x.Item1 == part.PieceMark).First().Item2;
                                    currenquantity += part.Quantity;
                                    partsDictionary[adjustedmain].AssemblyMarks.RemoveAll(x => x.Item1 == part.PieceMark);
                                    partsDictionary[adjustedmain].AssemblyMarks.Add(new Tuple<string, int>(part.PieceMark, currenquantity));
                                }
                                else
                                {
                                    partsDictionary[adjustedmain].AssemblyMarks.Add(new Tuple<string, int>(part.PieceMark, part.Quantity));
                                    if ((part.ShapeType.Contains("STUD") || part.ShapeType.Contains("WS")) && !partsDictionary[adjustedmain].RoutingCodes.Contains("NS"))
                                    {
                                        partsDictionary[adjustedmain].RoutingCodes.Add("NS");
                                    }
                                }
                            }

                        }
                        if (part.IsMain && Regex.IsMatch(part.Finish, @"(?i)g"))
                            part.RoutingCodes.Add("GAL");
                    }
                    #endregion

                    #region Sequence Line
                    if (splitline[0].Contains("S") && !splitline[0].Contains("SS"))
                    {
                        partsDictionary[lastpart].Sequence = splitline[1];
                    }
                    #endregion

                    
                    counter += 1;
                }
         

            var kiss = new KSSFile
                {
                    JobNumber = jobNumber,
                    JobDescription = jobDescription,
                    PartsDictionary = partsDictionary,
                    Heading = lines[0]
                };
                return kiss;
            }
        }
    }

