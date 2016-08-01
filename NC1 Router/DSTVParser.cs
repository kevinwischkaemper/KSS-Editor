using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NC1_Router
{
    public class DSTVParser
    {
        public List<HoleTooSmall> smallHoles = new List<HoleTooSmall>();
        public List<Part> plPartHalfTooLong = new List<Part>();
        public List<HeaderLengthChange> lengthchanges = new List<HeaderLengthChange>();

        public Part ParseDSTV(Part part, string searchfolder)
        {
            if (part.ShapeType.Contains("STUD") || part.RoutingCodes.Contains("bolt"))
            {
                return part;
            }
            
            string ncpath = Path.Combine(searchfolder, $"{part.PieceMark}.nc1");
            if (!File.Exists(ncpath))
            {
                part.RoutingCodes.Add("NO FILE");
                return part;
            }
            string text = File.ReadAllText(ncpath);
            bool islengthchange = false;
            #region Header Parsing            
            double webheadcut = 0;
            double webtailcut = 0;
            double flangeheadcut = 0;
            double flangetailcut = 0;
            double profileHeight = 0;
            double flangeWidth = 0;
            double flangeThickness = 0;
            double webThickness = 0;
            double headerlength = 0;
            string shape = "";
            string grade = "";
            var textlines = text.Split(new[] { '\n' });
            int skippedlines = 0;
            for (int i = 0; i < 23; i++)
            {
                textlines[i] = textlines[i].TrimEnd(new[] { '\r' });
                if (textlines[i].Contains("*"))
                {
                    skippedlines += 1;
                    continue;
                }
                if (i == 9 + skippedlines)
                    headerlength = Convert.ToDouble(textlines[i].TrimEnd(new[] { '\r' }));
                if (i == 5 + skippedlines)
                    grade = textlines[i];
                if (i == 7 + skippedlines)
                    shape = textlines[i];
                if (i == 10 + skippedlines)
                    profileHeight = Convert.ToDouble(textlines[i]);
                if (i == 11 + skippedlines)
                    flangeWidth = Convert.ToDouble(textlines[i]);
                if (i == 12 + skippedlines)
                    flangeThickness = Convert.ToDouble(textlines[i]);
                if (i == 13 + skippedlines)
                    webThickness = Convert.ToDouble(textlines[i].TrimEnd(new[] { '\r' }));
                if (i == 17 + skippedlines)
                    webheadcut = Convert.ToDouble(textlines[i]) * (Math.PI / 180);
                if (i == 18 + skippedlines)
                    webtailcut = Convert.ToDouble(textlines[i]) * (Math.PI / 180);
                if (i == 19 + skippedlines)
                    flangeheadcut = Convert.ToDouble(textlines[i]) * (Math.PI / 180);
                if (i == 20 + skippedlines)
                    flangetailcut = Convert.ToDouble(textlines[i]) * (Math.PI / 180);
                
            }
            var regexnumber = new Regex(@"\d");
            string shapeType = shape.Substring(0, regexnumber.Match(shape).Index).Trim();
            if (shapeType == "HSS")
                shapeType = "TS";
            string shapeDetails = shape.Substring(regexnumber.Match(shape).Index);
            #endregion

            #region Block Parsing
            var blocktitlematch = new Regex("\\n[A-Z][A-Z]\\r\\n");
            List<Block> blocklist = new List<Block>();
            foreach (Match blockmatch in blocktitlematch.Matches(text))
            {
                if (blockmatch.Value.Contains("ST"))
                    continue;
                if (blockmatch.Value.Contains("EN"))
                    break;
                Block block = new Block();
                block.Lines = new List<string>();
                var splitblock = text.Substring(blockmatch.Index, blockmatch.NextMatch().Index - blockmatch.Index).Split(new[] { '\r' });
                block.Type = splitblock[0].TrimStart(new[] { '\n' });
                for (int i = 1; i < splitblock.Count() - 1; i++)
                {
                    block.Lines.Add(splitblock[i].TrimStart(new[] { '\n' }).Trim());
                }
                blocklist.Add(block);
            }
            #endregion

            #region Block Logic and Routing
            bool punched = false;
            bool quadrilateral = true;
            bool miter = false;
            bool bent = false;
            bool stiffener = false;
            double maxLength = 0;
            double minLength = 2000;
            double miterXTolerance = 3.175;


            foreach (Block block in blocklist)
            {
                if (block.Type == "BO") // holes
                {
                    for (int i = 0; i < block.Lines.Count; i++)
                    {
                        string[] blocklinevalues = Regex.Split(block.Lines[i], @"\s+");
                        int first;
                        if (string.IsNullOrWhiteSpace(blocklinevalues[0]) || Regex.IsMatch(blocklinevalues[0], @"[a-z]"))
                            first = 1;
                        else
                            first = 0;
                        double holeDiameter = Convert.ToDouble(blocklinevalues[first + 2]);
                        //if (Regex.IsMatch(blocklinevalues[first], @"[a-z]"))
                        //{
                        //    blocklinevalues[first] = blocklinevalues[first].Substring(0, Regex.Match(blocklinevalues[first], @"[a-z]").Index);
                        //}
                        // use above if using first value
                        punched = true;
                        if (holeDiameter < webThickness)
                            if (!smallHoles.Any(x => x.Piecemark.Contains(part.PieceMark)))
                                smallHoles.Add(new HoleTooSmall { Piecemark = part.PieceMark, thickness = webThickness, HoleSize = UnitConverter.ConvertMilsToFracInches(holeDiameter)});
                        if (Regex.IsMatch(blocklinevalues[first + 1], @"[a-z]"))
                        {
                            if (Regex.Match(blocklinevalues[first + 1], @"[a-z]").Value == "m" && shapeType != "PL" && shapeType != "FB")
                                punched = false;
                            else
                                if (holeDiameter < webThickness)
                                    if (!smallHoles.Any(x => x.Piecemark.Contains(part.PieceMark)))
                                        smallHoles.Add(new HoleTooSmall { Piecemark = part.PieceMark, thickness = webThickness, HoleSize = UnitConverter.ConvertMilsToFracInches(holeDiameter)});
                        }
                    }
                }
                if (block.Type == "KA")
                    bent = true;
                if (block.Type == "AK")  // outline.  Max stiffener chamfer distance = 31.75 mm
                {
                    List<List<double>> blocklines = new List<List<double>>();                    

                    int first;
                    for (int i = 0; i < block.Lines.Count; i++)
                    {
                        List<double> blocklinedoubles = new List<double>();
                        string[] blocklinestrings = Regex.Split(block.Lines[i], @"\s+");
                        if (string.IsNullOrWhiteSpace(blocklinestrings[0]) || Regex.IsMatch(blocklinestrings[0], @"[a-z]"))
                            first = 1;
                        else
                            first = 0;
                        for (int p = first; p < blocklinestrings.Count(); p++)
                        {
                            blocklinedoubles.Add(Convert.ToDouble(Regex.Replace(blocklinestrings[p], "[a-z]", "").Trim()));
                        }
                        blocklines.Add(blocklinedoubles);
                        maxLength = Math.Max(maxLength, blocklinedoubles[0]);
                        minLength = Math.Min(minLength, blocklinedoubles[0]);
                    }
                    
                    int stiffenercount = 0;
                    if (shapeType == "PL" || shapeType == "FL" || shapeType == "L")
                    {
                        for (int i = 1; i < block.Lines.Count; i++)
                        {
                            double deltaX = blocklines[i][0] - blocklines[i - 1][0];
                            double deltaY = blocklines[i][1] - blocklines[i - 1][1];
                            double initialcurve = blocklines[i - 1][2];
                            if (Convert.ToInt32(Math.Abs(deltaX)) == Convert.ToInt32(Math.Abs(deltaY)) && initialcurve == 0.00 && Convert.ToInt32(Math.Abs(deltaX)) < 38.5)
                            {
                                stiffener = true;
                                stiffenercount += 1;
                            }
                        }
                    }
                    int btLineCount = block.Lines.Count - stiffenercount;
                    if (btLineCount != 5) // must ignore small discrepancies due to bevels ex -68 B13122
                        quadrilateral = false;
                }                    
            }
            double lengthFromAKBlocks = maxLength - minLength;
            if (maxLength - minLength != headerlength)
            {
                islengthchange = true;
                var change = new HeaderLengthChange();
                change.FileName = ncpath;
                change.InitialLength = headerlength.ToString();
                change.NewLength = lengthFromAKBlocks.ToString();
                if (headerlength - lengthFromAKBlocks >= 3.175)
                    change.OutOfTolerance = true;
                else
                    change.OutOfTolerance = false;
                lengthchanges.Add(change);
            }
            foreach (Block block in blocklist.Where(x => x.Type == "AK").ToList())
            {
                if (shapeType != "PL" && shapeType != "FL")
                {
                    List<List<double>> blocklines = new List<List<double>>();
                    int first;

                    foreach (string line in block.Lines)
                    {
                        List<double> blocklinedoubles = new List<double>();
                        string[] blocklinestrings = Regex.Split(line, @"\s+");
                        if (string.IsNullOrWhiteSpace(blocklinestrings[0]) || Regex.IsMatch(blocklinestrings[0], @"[a-z]"))
                            first = 1;
                        else
                            first = 0;
                        for (int p = first; p < blocklinestrings.Count(); p++)
                        {
                            blocklinedoubles.Add(Convert.ToDouble(Regex.Replace(blocklinestrings[p], "[a-z]", "").Trim()));
                        }
                        blocklines.Add(blocklinedoubles);
                    }
                    if (minLength > 1)
                    {
                        for (int i = 0; i < block.Lines.Count; i++)
                        {
                            for (int p = 0; p < blocklines[i].Count; p++)
                            {
                                blocklines[i][p] = blocklines[i][p] - minLength;
                            }
                        }
                    }
                    for (int i = 1; i < block.Lines.Count - 1; i++)
                    {
                        if ((blocklines[i][0] == maxLength || blocklines[i][0] == minLength) || (blocklines[i - 1][0] == maxLength || blocklines[i - 1][0] == minLength) || (blocklines[i + 1][0] == maxLength || blocklines[i + 1][0] == minLength))
                        {
                            double dXBack = Math.Abs(blocklines[i][0] - blocklines[i - 1][0]);
                            double dXForward = Math.Abs(blocklines[i][0] - blocklines[i + 1][0]);
                            if ((dXBack > miterXTolerance && Math.Abs(blocklines[i - 1][1] - blocklines[i][1]) > (flangeThickness + 2)) || (dXForward > miterXTolerance && Math.Abs(blocklines[i + 1][1] - blocklines[i][1]) > (flangeThickness + 2)))
                            {
                                miter = true;
                            }
                        }
                        if ((blocklines[0][0] == maxLength || blocklines[0][0] == minLength) || (blocklines[block.Lines.Count - 1][0] == maxLength || blocklines[block.Lines.Count - 1][0] == minLength))
                            if (Math.Abs((blocklines[0][0] - blocklines[block.Lines.Count - 1][0])) > miterXTolerance && blocklines[0][1] != blocklines[block.Lines.Count - 1][1])
                            {
                                miter = true;
                            }
                    }
                }
            }
            
            if (part.IsMain && part.AssemblyMarks.Count > 0)
                part.RoutingCodes.Add("F");
            else if (part.IsMain == false)
                part.RoutingCodes.Add("F");
            if (punched == true)
                part.RoutingCodes.Add("P");
            if (bent == true)
                part.RoutingCodes.Add("BNT");
            if (miter == true)
                part.RoutingCodes.Add("M");
            if (quadrilateral == false && shapeType != "PL" && shapeType != "FL")
                part.RoutingCodes.Add("CO");
            if (quadrilateral == false && (shapeType == "PL" || shapeType == "FL"))
                part.RoutingCodes.Add("BT");
            if (stiffener == true && (shapeType == "PL" || shapeType == "FL") && !part.IsMain)
                part.RoutingCodes.Add("ST");
            #endregion

            #region Plate Width parsing
            string adjustedDetails = "";
            if (shapeType == "PL" || shapeType == "FL")
            {
                if (Regex.IsMatch(shapeDetails, "X"))                
                    shapeDetails = shapeDetails.Substring(0, Regex.Match(shapeDetails, "X").Index);                
                double inches = profileHeight / 25.4000;
                int wholeInches = (int)inches;
                double remainder = inches - wholeInches;
                for (double i = 0.0000; i < 1.0625; i = i + 0.0625)
                {
                    if (i > remainder)
                    {
                        double high = i;
                        double low = i - 0.0625;
                        if ((high - remainder) > (remainder - low))
                            remainder = low;
                        else
                            remainder = high;
                        break;
                    }
                }
                var numerator = (int)(16.0000 / (1.0000 / remainder));
                string fractionalInches;
                if (numerator % 2 == 0)
                {
                    if (numerator % 4 == 0)
                    {
                        if (numerator % 8 == 0)
                            fractionalInches = $"{(numerator / 8).ToString()}/2";
                        else
                            fractionalInches = $"{(numerator / 4).ToString()}/4";
                    }
                    else
                        fractionalInches = $"{(numerator / 2).ToString()}/8";
                }  
                else
                    fractionalInches = $"{numerator.ToString()}/16";
                if (numerator == 0)
                    adjustedDetails = $"{shapeDetails}X{wholeInches.ToString()}";
                else
                adjustedDetails = $"{shapeDetails}X{wholeInches.ToString()}-{fractionalInches.ToString()}";
            }
            else
                adjustedDetails = shapeDetails;
            #endregion

            var newpart = new Part
            {
                IsMain = part.IsMain,
                AssemblyMarks = part.AssemblyMarks,
                DrawingNumber = part.DrawingNumber,
                Quantity = part.Quantity,
                Sequence = part.Sequence,
                PieceMark = part.PieceMark,
                RoutingCodes = part.RoutingCodes,
                Length = lengthFromAKBlocks,
                FlangeWidth = flangeWidth,
                ProfileHeight = profileHeight,
                Grade = grade.Trim(),            
                ShapeType = shapeType,
                ShapeDetails = adjustedDetails,
                MainMember = part.MainMember
            };
            if (lengthFromAKBlocks < 1)
                newpart.Length = headerlength;
            if (shapeType == "TS")
                newpart.ShapeDetails = shapeDetails;
            if (shapeType == "PL" && shapeDetails.Trim() == "1/2" && (lengthFromAKBlocks > 1523 || profileHeight > 1523))
                plPartHalfTooLong.Add(newpart);
            return newpart;
                
            }
        }
    
}
