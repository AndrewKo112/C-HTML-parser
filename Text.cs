using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser {
    class Text : HTMLElement {

        public static HTMLElement ParseText (string html) {
            return new Text(html);
        }

        private string _text;

        public override string Name {
            get {
                return "Text";
            }
        }

        public Text (string text) {
            _text = text;
        }

        public override string ToString(int nestingLevel) {
            string tabulation = new string(' ', nestingLevel);
            return tabulation + _text + "\n";
            //return _text.Replace("\n", "\n" + tabulation) + '\n';
        }
    }
}
