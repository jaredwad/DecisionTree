using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
   public enum AttributeType { Discrete, Continuous };

   public class VariableSet
   {
      public string Name { get; set; }
      public string MostCommon { get { return mostCommonItem; } }
      public AttributeType Type { get; set; }

      private string mostCommonItem;

      private List<string> set;
      private List<string> classes;

      private List<string> groups;

      private Dictionary<string, List<int>> splitMatrix;
      public Dictionary<string, List<int>> SplitMatrix { get { return splitMatrix; } }
      public List<string> Groups { get { return groups; } }

      double beginingEntropy;
      double total;

      public VariableSet(string pName, List<string> pData, List<string> pClasses, AttributeType pType = AttributeType.Discrete)
      {
         Name = pName;
         set = pData;
         classes = pClasses;
         Type = pType;

         mostCommonItem = set.GroupBy( x => x ).OrderByDescending( s => s.Count() ).First().Key;
      }

      public int classify(string item)
      {
         if( Type == AttributeType.Discrete )
            return classifyDiscrete( item );
         else
            return classifyContinuous( item );
      }

      private int classifyDiscrete(string item)
      {
         for(int i = 0; i < groups.Count; ++i){
            if( item == groups[i] )
               return i;
         }

         return -1; //ERROR
      }

      private int classifyContinuous(string item)
      {
         return 0;
      }

      public double getEntropy()
      {
         double entropy = 0;
         splitMatrix = split();

         groups = splitMatrix.Keys.ToList();

         foreach( string key in splitMatrix.Keys ) {
            List<string> newSet = new List<string>();
            foreach( int index in splitMatrix[key] ) {
               newSet.Add( set[index] );
            }

            Variable var = new Variable(newSet);

            double ent = var.getEntropy();
            if( ent <= 0 )
               continue;

            entropy -= ( (double)newSet.Count / total ) * ent;

         }

         return beginingEntropy - entropy;
      }

      public Dictionary<string, List<int>> split()
      {
         if( Type == AttributeType.Discrete )
            return splitDiscrete();
         else
            return splitContinuous();
      }

      private Dictionary<string, List<int>> splitDiscrete()
      {
         List<string> distinctSet = set.Distinct().ToList();
         
         Dictionary<string, List<int>> split = new Dictionary<string,List<int>>();
         foreach( string dis in distinctSet ) {
            split.Add( dis, new List<int>() );
         }

         for(int i = 0; i < set.Count; ++i) {
            split[set[i]].Add(i);
         }

         return split;
      }

      private Dictionary<string, List<int>> splitContinuous()
      {
         normalize();

         return null;
      }

      private void normalize()
      {

      }
   }

   internal class Variable
   {
      private double total;
      private Dictionary<string, int> classCount;

      public Variable( List<string> pClasses )
      {
         total = pClasses.Count;
         classCount = new Dictionary<string,int>();

         foreach( string item in pClasses.Distinct() ) {
            classCount.Add( item, 0 );
         }

         fillCount(pClasses);
      }

      private void fillCount(List<string> pClasses)
      {
         foreach( string item in pClasses ) {
            classCount[item]++;
         }
      }

      public double getEntropy()
      {
         double entropy = 0;
         foreach( string key in classCount.Keys ) {
            entropy -= getEntropy( classCount[key] );
         }
         return entropy;
      }

      private double getEntropy(double pCount)
      {
         return Math.Log(pCount / total);
      }
   }

}
