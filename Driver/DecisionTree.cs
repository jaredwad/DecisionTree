using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
   class DecisionTree
   {
      private Dictionary<string, List<string>> fullDataSet;
      private List<string> classes;
      private Node root;

      public DecisionTree(List<string[]> pData, List<string> pHeaders)
      {
         organizeData( pData, pHeaders );
      }

      public void buildTree()
      {
         root = new Node("all", fullDataSet, classes, 0, true);
         root.build();
      }

      public void printTree()
      {
         root.printNode();
      }

      private void organizeData(List<string[]> pData, List<string> pHeaders)
      {
         fullDataSet = new Dictionary<string, List<string>>();

         for( int i = 0; i < pHeaders.Count; ++i ) {
            fullDataSet.Add( pHeaders[i], new List<string>() );
         }

            foreach( string[] row in pData ) {
               for( int i = 0; i < pHeaders.Count; ++i ) {
                  fullDataSet[pHeaders[i]].Add( row[i] );
               }
            }

         classes = fullDataSet[pHeaders[pHeaders.Count - 1]].ToList(); //get the classes for each row
         fullDataSet.Remove( pHeaders[pHeaders.Count - 1] );
      }
   }
}
