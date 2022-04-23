using Globals.Classes.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TiledEngine.Classes.Misc
{
    internal class GraphTraverser
    {
        public PortalGraph Graph { get; set; }
        public bool[] visited;
        public GraphTraverser(PortalGraph graph)
        {
            this.Graph = graph;
            visited = new bool[this.Graph.Size];
        }


        public void TraverseDFS(int v)

        {

            if (!visited[v])

            {

                Console.Write(v + " ");

                visited[v] = true;

                foreach (int child in this.Graph.GetSuccessors(v))

                {

                    TraverseDFS(child);

                }

            }

        }

        int nodeToReturn;
        public int GetNextNodeInPath(int nodeStart, int nodeEnd)
        {
            nodeToReturn = -1;
            bool[] isVisited = new bool[this.Graph.Size];
            List<int> pathList = new List<int>();

            // add source to path[]  
            pathList.Add(nodeStart);

            // Call recursive utility  
            GetNextNode(nodeStart, nodeEnd, isVisited, pathList);

           // Debug.Assert(nodeToReturn == 100, "Node equals 100");
            return nodeToReturn;
        }

        // A recursive function to print  
        // all paths from 'u' to 'd'.  
        // isVisited[] keeps track of  
        // vertices in current path.  
        // localPathList<> stores actual  
        // vertices in the current path  
        internal void GetNextNode(int nodeStart, int nodeEnd,
                                        bool[] isVisited,
                                List<int> localPathList)
        {

            // Mark the current node  
            isVisited[nodeStart] = true;

            if (nodeStart.Equals(nodeEnd))
            {
            //CommandConsole.Append(string.Join(" ", localPathList));

                //  Console.WriteLine(string.Join(" ", localPathList));
                nodeToReturn = localPathList[1];
                // if match found then no need  
                // to traverse more till depth  
                isVisited[nodeStart] = false;
                return;
            }

            // Recur for all the vertices  
            // adjacent to current vertex  
            foreach (int i in this.Graph.childNodes[nodeStart])
            {
                if (!isVisited[i])
                {
                    // store current node  
                    // in path[]  
                    localPathList.Add(i);
                    GetNextNode(i, nodeEnd, isVisited,
                                        localPathList);

                    // remove current node  
                    // in path[]  
                    localPathList.Remove(i);
                }
            }

            // Mark the current node  
            isVisited[nodeStart] = false;
        }
    }
}
