using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser {
    /// <summary>
    /// Abstract class that defines common interface for all HTML elements like tag and text
    /// Implemented as a Composer pattern
    /// Presents html document as a tree where tags are branches and text sections are leaves
    /// </summary>
    public abstract class HTMLElement {

        /// <summary>
        /// May contain children
        /// </summary>
        public virtual bool isComposite {
            get {
                return false;
            }
        }

        /// <summary>
        /// Contains the html element's name e.g. body, div, span etc.
        /// </summary>
        public virtual string Name {
            get;
            protected set;
        }

        /// <summary>
        /// If element is composite returns it's children
        /// </summary>
        public virtual HTMLElement[] Children {
            get {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns parent html element
        /// </summary>
        public virtual HTMLElement Parent {
            get; internal set;
        }

        /// <summary>
        /// If element is composite marks specified html element as child
        /// </summary>
        /// <param name="element">HTML element to be marked as child</param>
        public virtual void AddChild (HTMLElement element) {
            throw new NotImplementedException();
        }

        public sealed override string ToString () {
            return ToString(0);
        }

        public abstract string ToString (int nestingLevel);
    }
}
