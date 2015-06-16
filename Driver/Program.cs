using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
   class Program
   {
      static void Main( string[] args )
      {
         string[] csvHeaders = {"buying", "maint", "doors", "persons", "lug_boot", "safety", "class"};

         List<string[]> allData = CSV.parseCSV( @"C:\Users\Jared Wadsworth\Documents\Visual Studio 2013\Projects\DataMining\DMF\Data\Car.csv", csvHeaders );

         List<string> headers = allData[0].ToList();
         allData.RemoveAt( 0 );

         DecisionTree tree = new DecisionTree( allData, headers );

         tree.buildTree();

         tree.printTree();

         Console.ReadKey();
      }
   }
}
