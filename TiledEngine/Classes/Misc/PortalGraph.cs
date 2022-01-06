using System;
using System.Collections.Generic;
using System.Text;

namespace TiledEngine.Classes.Misc
{
    internal class PortalGraph
    {

            public List<int>[] childNodes;
            public int Size { get { return childNodes.Length; } }
            public PortalGraph(int size)
            {
                childNodes = new List<int>[size];
                for (int i = 0; i < size; i++)
                {
                    childNodes[i] = new List<int>();
                }
            }

            public PortalGraph(List<int>[] childNodes)
            {
                this.childNodes = childNodes;
            
            }

            public void AddEdge(int baseNode, int nodeToConnect)
            {
                childNodes[baseNode].Add(nodeToConnect);
            }

            public void RemoveEdge(int baseNode, int nodeToConnect)
            {
                childNodes[baseNode].Remove(nodeToConnect);
            }

            public bool HasEdge(int baseNode, int nodeToConnect)
            {
                bool hasEdge = childNodes[baseNode].Contains(nodeToConnect);
                return hasEdge;
            }

            public IList<int> GetSuccessors(int v)

            {
                return childNodes[v];
            }


        
    }
}
