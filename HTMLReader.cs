using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace HTMLParser {
    public sealed class HTMLReader {

        public HTMLDocument ReadDocument (Stream stream) {
            var reader = new StreamReader(stream);
            return ReadDocument(reader);
        }

        /// <summary>
        /// Keeps information about html element and it's status (is it closed or not)
        /// </summary>
        private struct ElementInfo {
            public HTMLElement element { get; set; }
            public bool isClosed { get; set; }
            public ElementInfo (HTMLElement element, bool isClosed) {
                this.element = element;
                this.isClosed = isClosed;
            }
        }

        /// <summary>
        /// Reads an html documents and returns it's object-oriented representation as HTMLDocument instance
        /// </summary>
        /// <param name="reader">Opened Stream for a document</param>
        /// <returns></returns>
        public HTMLDocument ReadDocument (StreamReader reader) {
            string document = reader.ReadToEnd();

            HTMLDocument htmlDoc = new HTMLDocument();

            //  Represents tags nesting
            LinkedList<ElementInfo> waitingForParent = new LinkedList<ElementInfo>();

            List<HTMLElement> rootElements = new List<HTMLElement>();

            Regex endTagNameRegex = new Regex(@"(?<=<\/)[^\s>]+");

            //  Reading the document
            for (int i = 0; i < document.Length; i++) {
                if (document[i] == '<') {
                    //  Getting the text between angle brackets
                    string tagStr = ReadTag(document, i);
                    i += tagStr.Length; //  Skipping iterations to the current tag's end cause it was already read in ReadTag

                    //  If tag closed e.g. </div>
                    if (IsClosingTag(tagStr)) {
                        string endTagName = endTagNameRegex.Match(tagStr).ToString();
                        //  Info about element that was just closed

                        ElementInfo closedInfo;
                        try {
                            closedInfo = waitingForParent.First(x => x.element.Name == endTagName && !x.isClosed);
                        } catch (Exception) {
                            continue;
                        }

                        //  Breaking out if such tag was never opened
                        //  This may happen when </br> was used

                        //  Marking this tag as closed
                        closedInfo.isClosed = true;

                        //  Marking all inner elements as children
                        //  All children elements must have been added to stack AFTER the considered tag
                        HTMLElement child = null;
                        
                        while (waitingForParent.Count > 0 && waitingForParent.First.Value.element != closedInfo.element) {
                            child = waitingForParent.First.Value.element;

                            closedInfo.element.AddChild(child);
                            child.Parent = closedInfo.element;

                            waitingForParent.RemoveFirst();
                        }

                        //throw new FormatException("Missing closing tag for " + endTagName);

                    } else {
                        //  Parsing tag from text
                        HTMLElement tag = Tag.ParseTag(tagStr);
                        waitingForParent.AddFirst(new ElementInfo(tag, false));
                        rootElements.Add(tag);
                    }
                } else {

                    string textStr = ReadText(document, i);
                    i += textStr.Length - 1;
                    textStr = textStr.Trim();
                    if (textStr.Length > 0) {
                        HTMLElement element = Text.ParseText(textStr);
                        waitingForParent.AddFirst(new ElementInfo(element, true));
                    }
                }
            }
            htmlDoc.Add( = rootElements.Where(x => x.Parent == null).ToList();
            foreach (var root in rootElements) {
                Console.WriteLine(root);
            }
            return htmlDoc;
        }

        private bool IsClosingTag (string tagStr) {
            Regex endingTagRegex = new Regex(@"^</\s*");
            return endingTagRegex.IsMatch(tagStr);
        }

        /// <summary>
        /// Returns the text between angle brackets
        /// </summary>
        /// <param name="document">HTML document</param>
        /// <param name="startPos">Position of opening bracket in the document</param>
        /// <returns>Text between startPos and next closing angle bracket</returns>
        private string ReadTag (string document, int startPos) {
            if (document[startPos] != '<')
                throw new ArgumentException("Expecting startPos to reference at '<' symbol");

            for (int i = startPos; i < document.Length; i++) {
                //  If '>' symbol encountered returning tag string
                if (document[i] == '>') {
                    return document.Substring(startPos, i - startPos);
                }
            }

            throw new FormatException("Tag has no ending");
        }

        private string ReadText(string document, int startPos) {
            int i = 0;
            for (i = startPos; i < document.Length; i++) {
                //  If '<' symbol encountered returning text string
                if (document[i] == '<') {
                    //Console.WriteLine("--  " + document.Substring(startPos, i - startPos));
                    return document.Substring(startPos, (i - startPos));
                }
            }
            return document.Substring(startPos, document.Length - startPos);
        }


    }
}
