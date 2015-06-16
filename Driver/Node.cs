using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
   class Node
   {
      private Dictionary<string, List<string>> dataSet;
      private List<string> classes;
      private VariableSet set;
      private string value;
      private string rule;
      private int level;

      public bool IsLeaf { get; set; }
      public bool IsRoot { get; set; }
      public Node Parent { get; set; }
      public string Value { get { return value; } }
      public string Rule { get { return rule; } }
      public string Name { get; set; }

      private List<Node> branches;
      public List<Node> Branches { get { return branches; } }

      public Node(string pName, Dictionary<string, List<string>> pData, List<string> pClasses, int pLevel, bool pIsRoot = false)
      {
         Name = pName;
         dataSet = pData;
         classes = pClasses;
         level = pLevel;
         IsLeaf = false;
         IsRoot = pIsRoot;
         value = null;
      }

      public void build()
      {
         if( dataSet.Keys.Count == 1 || classes.Distinct().ToArray().Count() == 1 ) {
            IsLeaf = true;
            string temp = dataSet.Keys.First();
            rule = temp + " = " + dataSet[temp][0];
            value = classes.GroupBy( i => i ).OrderByDescending( grp => grp.Count() ).Select( grp => grp.Key ).First();
            return;
         }

         set = findBestSplit();

         dataSet.Remove( set.Name );//Remove the set from the split

         setBranches();

         foreach( Node branch in branches ) {
            branch.build();
         }
      }

      public void printNode()
      {
         for( int i = 0; i < level; ++i ) {
            Console.Write( "|\t" );
         }
         if( IsLeaf ) {
            Console.WriteLine( "<{0}> -- [{1}]",rule, value );
         } else {
            Console.WriteLine( "<{0} = {1} -- {2}>", set.Name, Name, set.MostCommon);
            Console.ReadKey();
            foreach( Node branch in branches ) {
               branch.printNode();
            }
         }
      }

      private VariableSet findBestSplit()
      {
         KeyValuePair<VariableSet, double> bestSet = new KeyValuePair<VariableSet,double>(null,2); //Anything is better than 2

         foreach( var key in dataSet.Keys ) {
            VariableSet set = new VariableSet( key, dataSet[key], classes );
            double entropy = set.getEntropy();
            if( entropy < bestSet.Value ) {
               bestSet = new KeyValuePair<VariableSet, double>( set, entropy );
            }
         }
         return bestSet.Key;
      }

      private void setBranches()
      {
         branches = new List<Node>();

         Dictionary<string, List<int>> splitMatrix = set.SplitMatrix;
         foreach( var temp in splitMatrix.Keys ) { //Each branch
            Dictionary<string, List<string>> newDataSet = new Dictionary<string, List<string>>();
            foreach( var col in dataSet.Keys ) { //Each Attribute
               List<string> items = new List<string>();
               foreach( int index in splitMatrix[temp] ) { //Get the items corresponding to this branch
                  items.Add( dataSet[col][index] );
               }

               newDataSet.Add( col, items ); // Add those items to the new data set
            }

            List<string> newClasses = new List<string>();
            foreach( int index in splitMatrix[temp] ) { //Get the classes corresponding to this branch
//               newClasses.Add( classes[index] );
               newClasses.Add( classes[index] );
            }

            branches.Add( new Node( temp, newDataSet, newClasses, level + 1 ) );
         }
      }
   }
}
