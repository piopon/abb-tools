using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace abbTools
{
    public enum logType
    {
        info = 0,
        warning,
        error
    };

    class loggerABB
    {
        //private components
        private Control parent;
        private string parentClass;
        private bool checkTags;
        //for append line
        private string lastLine;
        private logType lastType;

        public loggerABB()
        {
            checkTags = false;
            parentClass = "";
            lastLine = "";
        }

        public loggerABB(Control textDest, bool textFormatted)
        {
            checkTags = textFormatted;
            parent = textDest;
            parentClass = textDest.GetType().Name;
            lastLine = textDest.Text;
        }

        ~loggerABB()
        {
            parent = null;
            parentClass = "";
        }

        public void setParent(Control textDest)
        {
            checkTags = false;
            parent = textDest;
            parentClass = textDest.GetType().Name;
            lastLine = textDest.Text;
        }

        public void setParent(Control textDest, bool textFormatted)
        {
            checkTags = textFormatted;
            parent = textDest;
            parentClass = textDest.GetType().Name;
            lastLine = textDest.Text;
        }

        public void writeLog(logType type,string text)
        {
            if (parent != null) {
                //check if we want to input formatted text
                if (checkTags) {
                    //find HTML-like format tags positions
                    int[] tagPos = findFormatTags(text);
                    int tagCount = tagPos != null ? tagPos.Count() : 0, tagWidth;
                    //formatted string is supported by RichTextBox (parent)
                    if (parentClass == "RichTextBox" && tagCount > 0) {
                        //check tags and remeber them if ok
                        string[] tags = getCharTags(text, tagPos);
                        if (tags != null && tags.Count() > 0) {
                            //cast to rich text box 
                            RichTextBox owner = (RichTextBox)parent;
                            //remove tags from text and implement format
                            for (int i = 0; i < tagCount; i++) {
                                tagWidth = i < tagCount / 2 ? tags[i].Length+2 : tags[i].Length+3;
                                text = text.Remove(tagPos[i], tagWidth);
                                for (int j = i + 1; j < tagCount; j++) tagPos[j] -= tagWidth;
                            }
                            //na koniec wypisz ostateczny tekst
                            owner.Text = text;
                            //apply format from tags
                            for (int i = 0; i < tagCount/2; i++) {
                                owner.Select(tagPos[i], tagPos[i+1] - tagPos[i]);
                                owner.SelectionFont = new Font(owner.Font, stringToFontStyle(tags[i]));
                            }
                        } else {
                            //formatting is wrong
                            parent.Text = text + "   [ !!! ERROR: bad tags !!! ] ";
                        }
                    } else {
                        //parent is incompatible to format text
                        parent.Text = text + "   [ !!! ERROR: parent can't format !!! ] ";
                    }
                } else {
                    parent.Text = text;
                }
                //update last line
                lastLine = text;
                lastType = type;
            }
        }

        public void appendLog(string text)
        {
            if (parent != null) {
                writeLog(lastType, lastLine + text);
                lastLine = parent.Text;
            }
        }

        private int[] findFormatTags(string txt)
        {
            //check number of tags
            int tagsNo = txt.Count(x => x == '<');
            if (tagsNo == 0 || tagsNo % 2 != 0) {
                return null;
            }
            //create result table
            int[] result = new int[tagsNo];
            //fill result with opening tags positions
            result[0] = txt.IndexOf("<", 0);
            for (int i = 1; i < tagsNo / 2; i++) {
                result[i] = txt.IndexOf("<", result[i-1]+1);
            }
            //search for closing tags positions
            result[tagsNo/2] = txt.IndexOf("</", 0);
            for (int i = tagsNo/2; i < tagsNo; i++)
            {
                result[i] = txt.IndexOf("</", result[i - 1] + 1);
            }

            return result;
        }

        private string[] getCharTags(string txt, int[] tagPos)
        {
            int tagsNo = tagPos.Count(), offset;
            string[] result = new string[tagsNo];
            //get all tags from text
            for (int i = 0; i < tagsNo; i++) {
                offset = i < tagsNo / 2 ? 1 : 2;
                result[i] = txt.Substring(tagPos[i]+offset,txt.IndexOf('>',tagPos[i]) - tagPos[i] - offset);
            }
            //check if tags are correct
            for (int check = 0; check < tagsNo/2; check++) {
                if (result[check] != result[tagsNo - 1 - check]) {
                    result = null;
                    break;
                }
            }

            return result;
        }

        private FontStyle stringToFontStyle(string tagStr)
        {
            FontStyle result = FontStyle.Regular;

            for (int tagNo = 0; tagNo < tagStr.Length; tagNo++) {
                char tag = tagStr[tagNo];
                switch (tag) {
                    case 'b':
                        result |= FontStyle.Bold;
                        break;
                    case 'i':
                        result |= FontStyle.Italic;
                        break;
                    case 'u':
                        result |= FontStyle.Underline;
                        break;
                    case 's':
                        result |= FontStyle.Strikeout;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}
