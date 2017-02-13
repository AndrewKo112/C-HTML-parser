using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser {
    public class HTMLDocument : IEnumerable<HTMLElement> {
        private List<HTMLElement> _rootElements = new List<HTMLElement>();

        IEnumerator<HTMLElement> IEnumerable<HTMLElement>.GetEnumerator () {
            return _rootElements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator () {
            return _rootElements.GetEnumerator();
        }

        public void Add (HTMLElement rootElement) {
            _rootElements.Add(rootElement);
        }

        public void AddRange (IEnumerable<HTMLElement> rootElements) {
            _rootElements.AddRange(rootElements);
        }

        public HTMLElement this[int index] {
            get {
                return _rootElements[index];
            }
        }

        public int Count {
            get {
                return _rootElements.Count;
            }
        }
    }
}
