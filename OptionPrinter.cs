using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OptionPrintService
{
    /// <summary>
    /// Class to create option usage text
    /// </summary>
    public class OptionPrinter
    {
        /// <summary>
        /// Left margin of each line
        /// </summary>
        public string Margin = "  ";

        /// <summary>
        /// Max length of each line
        /// </summary>
        public int LineLength = 80;

        // regex of line spliter characters
        private Regex splitRegex = new Regex(@"[ ()\[\]<>.,:;+*-/]", RegexOptions.RightToLeft);
        // regex of line break characters
        private Regex nlRegex = new Regex(@"\n|\r\n|\r", RegexOptions.RightToLeft);
        // length of left margin
        private const int LEFT_LENGTH = 4;

        /// <summary>
        /// Option informations
        /// </summary>
        private class Item
        {
            public string ShortName;
            public string LongName;
            public string Note;
        }

        private List<Item> items = new List<Item>();

        /// <summary>
        /// Add option information
        /// </summary>
        /// <param name="shortName">Short name of the option</param>
        /// <param name="longName">Long name of the option</param>
        /// <param name="note">Description of the option</param>
        public void Add(string shortName, string longName, string note)
        {
            items.Add(new Item() { ShortName = shortName, LongName = longName, Note = note });
        }

        /// <summary>
        /// Create texts for option usage
        /// </summary>
        /// <returns></returns>
        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            // margin of the center area
            int centerMargin = items.Select(x => x.LongName.Length).Max() + 3;
            // length of the note area
            int noteLength = LineLength - centerMargin - LEFT_LENGTH;

            string newLineMargin = "".PadRight(Margin.Length + LEFT_LENGTH + centerMargin);

            foreach (Item item in items)
            {
                // print short name
                sb.Append(Margin);
                if (string.IsNullOrEmpty(item.ShortName))
                {
                    sb.Append("    ");
                }
                else
                {
                    sb.Append(item.ShortName);
                    sb.Append(", ");
                }
                // print long name
                sb.Append(item.LongName.PadRight(centerMargin));

                // print note
                int noteIndex = 0;
                string note = item.Note;
                bool isFirst = true;
                while(true)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        // fill left and center area for a new line
                        sb.Append(newLineMargin);
                    }

                    // begin a new line if a break character was found
                    Match nlmatch = nlRegex.Match(note, Math.Min(noteIndex + noteLength, note.Length));
                    if (nlmatch.Success && noteIndex < nlmatch.Index)
                    {
                        sb.Append(note.Substring(noteIndex, nlmatch.Index - noteIndex));
                        sb.Append(Environment.NewLine);
                        noteIndex = nlmatch.Index + 1;
                        continue;
                    }

                    if (noteIndex + noteLength >= note.Length)
                    {
                        // print whole text if the note end up to line limit
                        sb.Append(note.Substring(noteIndex));
                        break;
                    }
                    // if break the line in the middle of note
                    Match m = splitRegex.Match(note, noteIndex + noteLength);
                    if (m.Success && noteIndex < m.Index)
                    {
                        // begin a new line before a spliter if found
                        sb.Append(note.Substring(noteIndex, m.Index - noteIndex + 1));
                        sb.Append(Environment.NewLine);
                        noteIndex = m.Index + 1;
                    }
                    else
                    {
                        // begin a new line at limit index if spliters were not found
                        sb.Append(note.Substring(noteIndex, noteLength));
                        sb.Append(Environment.NewLine);
                        noteIndex = noteIndex + noteLength;
                    }
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
