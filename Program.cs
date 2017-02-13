using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser {
    class Program {
        static void Main (string[] args) {
            
            using (StreamReader reader = new StreamReader(File.Open("index.html", FileMode.Open))) {
                var htmlReader = new HTMLReader();
                htmlReader.ReadDocument(reader);
            }
            Console.ReadKey();
        }
    }
}
