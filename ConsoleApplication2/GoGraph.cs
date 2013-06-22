﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class GoGraph<TVertex, TEdge, TData, TWeight>
    {
        public Graph<TVertex, TEdge, TData, TWeight> G;
      

        public GoGraph()
        {
            G = new LGraph<TVertex, TEdge, TData, TWeight>();
        }

        public GoGraph(int V, bool D, bool F)
        {
            G = Graph<TVertex, TEdge, TData, TWeight>.MakeGraph(V, D, F);
        }

        public GoGraph(int V, int E, bool D, bool F)
        {
            G = Graph<TVertex, TEdge, TData, TWeight>.MakeGraph(V, E, D, F);
        }

        public Edge<TEdge, TWeight, TVertex> GetEdge(Vertex<TVertex> v1, Vertex<TVertex> v2)
        {
            if (G.Type == true)
            {
                int i, j;
                for (i = 0; i < G.Vertexes.Count; i++)
                {
                    if (G.Vertexes[i].Index == v1.Index)
                        break;
                }

                for (j = 0; j < G.Vertexes.Count; j++)
                {
                    if (G.Vertexes[j].Index == v2.Index)
                        break;
                }

                if (i >= G.Vertexes.Count || j >= G.Vertexes.Count)
                    return null;

                return G.Matrix[i, j];
            }
            else
            {
                for (int i = 0; i < G.Vertexes.Count; i++)
                {
                    if (G.Adj[i].AdjIndex == v1.Index)
                    {
                        for (var q = G.Adj[i].Head; q != null; q = q.Next)
                        {
                            if (q == null)
                                return null;
                            if (q.AdjEdge.Vertex1.Index == v1.Index && q.AdjEdge.Vertex2.Index == v2.Index)
                                return q.AdjEdge;
                            if (q.AdjEdge.Vertex1.Index == v2.Index && q.AdjEdge.Vertex2.Index == v1.Index)
                                return q.AdjEdge;
                        }
                    }
                }
                return null;
            }
        }

        public void Reverse()
        {
           // var g = G;
            if (G.Type == true)//если M, то в L
            {
                G.Adj = new List<AdjList<TVertex, TEdge, TData, TWeight>>();
                for (int i = 0; i < G.Vertexes.Count; i++)//пополняем вектор смежности
                {
                    G.Adj.Add(new AdjList<TVertex, TEdge, TData, TWeight>());
                    G.Adj[i].AdjIndex = G.Vertexes[i].Index;
                }
                
                for (int i = 0; i < G.Vertexes.Count; i++)
                {
                    for (int j = 0; j < G.Vertexes.Count; j++)
                    {
                        if (G.Matrix[i, j] != null)
                        {
                            G.Adj[i].AddNode(G.Matrix[i, j]);
                        }
                    }
                }

                G.Type = false;
                G.Matrix = null;
                // g = null;
            }

            else//из L в M
            {
                G.Matrix = new Edge<TEdge, TWeight, TVertex>[G.Vertexes.Count,G.Vertexes.Count];
                for (int i = 0; i < G.Vertexes.Count; i++)
                {
                    for (var q = G.Adj[i].Head; q != null; q = q.Next)
                    {
                        G.Matrix[q.AdjEdge.Vertex1.Index, q.AdjEdge.Vertex2.Index] = q.AdjEdge;
                        if(!G.Oriented)
                            G.Matrix[q.AdjEdge.Vertex2.Index, q.AdjEdge.Vertex1.Index] = q.AdjEdge;
                    }
                }
                G.Adj = null;
                G.Type = true;
            }
        }
        

        public void Print()
        {
            string str = "";
            if (G.Type)
            {
                string numb1 = "  ";
                for (int y = 0; y < G.Vertexes.Count; y++)
                {
                    numb1 = numb1 + y.ToString();
                }
                Console.WriteLine(numb1);
                for (int i = 0; i < G.Vertexes.Count; i++)
                {
                    str = str + i.ToString() + " ";
                    for (int j = 0; j < G.Vertexes.Count; j++)
                    {
                        if(G.Matrix[i, j] != null)
                            str = str + G.Matrix[i, j].Weight.ToString();
                        else
                         str = str + "-";
                    }
                    Console.WriteLine(str);
                    str = "";
                }
            }
            else
            {
               

                    for (int i = 0; i < G.Adj.Count; i++)
                    {
                        Console.WriteLine(G.Adj[i].AdjIndex.ToString());
                        var q = G.Adj[i].Head;
                        if (q != null && q.AdjEdge != null)
                        {
                            for (q = G.Adj[i].Head; (q != null); q = q.Next)
                            {
                                if (G.Oriented == false)
                                {
                                    if (G.Adj[i].AdjIndex == q.AdjEdge.Vertex1.Index)
                                    {
                                        str = str + " " + q.AdjEdge.Vertex2.Index.ToString();
                                    }
                                    else
                                    {
                                        str = str + " " + q.AdjEdge.Vertex1.Index.ToString();
                                    }
                                }

                                else
                                {
                                    str = str + " " + q.AdjEdge.Vertex2.Index.ToString();
                                }
                            }
                            Console.WriteLine(str);
                        }
                        str = "";
                    }
            }
        }
    }
}
