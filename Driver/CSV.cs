using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
   class CSV
   {
      public static List<string[]> parseCSV( string filename, char delim = ',' )
      {
         string[] lines = readFile( filename );

         List<string[]> splitLines = new List<string[]>( lines.Length );

         foreach( var line in lines ) {
            splitLines.Add( line.Split( new char[] { delim } ) );
         }

         return splitLines;
      }

      public static List<string[]> parseCSV( string filename, string[] headers, char delim = ',' )
      {
         List<string[]> splitLines = parseCSV( filename, delim );

         splitLines.Insert( 0, headers );

         return splitLines;
      }

      private static string[] readFile( string filename )
      {
         return System.IO.File.ReadAllLines( filename );
      }
   }
}
