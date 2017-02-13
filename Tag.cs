using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLParser {
    /// <summary>
    /// Represents HTML tag as a tree branch
    /// </summary>
    class Tag : HTMLElement {

        /// <summary>
        /// Converts a string into a Tag element
        /// </summary>
        /// <param name="html">HTML text</param>
        /// <returns></returns>
        public static HTMLElement ParseTag (string html) {
            return new Tag(html);
        }

        /// <summary>
        /// For all tags returns true
        /// </summary>
        public override bool isComposite {
            get {
                return true;
            }
        }

        /// <summary>
        /// Returns an array of children HTML Elements
        /// </summary>
        public override HTMLElement[] Children {
            get {
                return _children.ToArray();
            }
        }

        /// <summary>
        /// Returns array of all tag's attributes
        /// </summary>
        public string[] Attributes {
            get {
                return _attributes.Keys.ToArray();
            }
        }
        
        /// <summary>
        /// Contains keys and values of this element's attributes
        /// </summary>
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();

        /// <summary>
        /// Contains this element's children tags
        /// </summary>
        private ICollection<HTMLElement> _children = new List<HTMLElement>();

        /// <summary>
        /// Returns the attribute value
        /// </summary>
        /// <param name="attribute">Attribute name</param>
        /// <returns></returns>
        public string this[string attribute] {
            get {
                return _attributes[attribute];
            }
        }

        private Tag (string html) {
            ParseFromString(html);
        }

        /// <summary>
        /// Parses data about tag from string
        /// </summary>
        /// <param name="rawHTML"></param>
        private void ParseFromString (string rawHTML) {
            //  Trimming and removing the '<>' symbols
            rawHTML = rawHTML.Trim();

            //  Acquiring tag name
            Regex tagRegex = new Regex(@"^<\s*[^\s]+");
            Name = tagRegex.Match(rawHTML).ToString().Trim(new char[] { ' ', '\t', '\r', '\n', '<' });
            //  Acquiring attributes
            Regex attrRegex = new Regex("\\w+\\s*=\\s*[\\w\"]+");

            foreach(var attr in attrRegex.Matches(rawHTML)) {
                //  Splitting every attribute string by '=' to divide key from value
                var keyValueArr = attr.ToString().Split('=');
                //  and saving them
                _attributes.Add(keyValueArr[0].Trim(), keyValueArr[1].Trim().Trim('"'));
            }
        }

        /// <summary>
        /// Converts HTML tag to string
        /// </summary>
        /// <param name="nestingLevel">Level of nesting to tabulate html correctrly</param>
        /// <returns>Returns the html of this element and all it's children</returns>
        public override string ToString (int nestingLevel) {
            //  Creating tabulation for specified 
            string tabulation = new string(' ', nestingLevel);
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append(tabulation).Append("<").Append(Name);
            foreach (var attribute in _attributes) {
                sBuilder.Append(" ").Append(attribute.Key).Append("=").Append("\"").Append(attribute.Value).Append("\"");
            }

            sBuilder.Append(">\n");

            if (_children.Count > 0) {
                foreach (var child in _children) {
                    sBuilder.Append(child.ToString(nestingLevel + 1));
                }
                sBuilder.Append(tabulation).Append("</").Append(Name).Append(">\n");
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// Returns true if this tag has the specified attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>True if tag has a specified attribute, false otherwise</returns>
        public bool HasAttribute (string attribute) {
            return _attributes.ContainsKey(attribute);
        }

        /// <summary>
        /// Marks specified HTMLElement as child
        /// </summary>
        /// <param name="element">Child element</param>
        public override void AddChild (HTMLElement element) {
            _children.Add(element);
        }
    }
}
