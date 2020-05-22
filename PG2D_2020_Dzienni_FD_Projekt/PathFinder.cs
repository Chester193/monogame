using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class PathFinder
    {
        public static List<Vector2> test;
        public static List<Node> test2;
        public static List<Vector2> FindPath(TiledMap map, Vector2 startPoint, Vector2 endPoint)
        {
            List<Node> available = new List<Node>();
            List<Vector2> visited = new List<Vector2>();
            test = visited;
            test2 = available;
            int tileSize = map.tileSize;

            Vector2 fixedStartPoint = AlignToGrid(startPoint, tileSize);

            Node root = new Node(null, fixedStartPoint, 0, Vector2.Distance(fixedStartPoint, endPoint));
            available.Add(root);

            while(available.Count > 0)
            {
                Node current = available[available.Count - 1];

                if (current.DistanceTo < tileSize)
                {
                    List<Vector2> result = new List<Vector2>();

                    while(current.Previous != null)
                    {
                        result.Add(current.Position);
                        current = current.Previous;
                    }
                    result.Reverse();
                    return result;
                }

                available.RemoveAt(available.Count - 1);
                visited.Add(current.Position);

                for(int i = -tileSize; i <= tileSize; i += tileSize)
                {
                    for (int j = -tileSize; j <= tileSize; j += tileSize)
                    {
                        bool flag = true;
                        Vector2 next = new Vector2(current.Position.X + i, current.Position.Y + j);

                        foreach(Vector2 item in visited)
                        {
                            if(Math.Abs(next.X - item.X) < 1 && Math.Abs(next.Y - item.Y) < 1)
                            {
                                flag = false;
                            }
                        }

                        foreach (Node item in available)
                        {
                            if (Math.Abs(next.X - item.Position.X) < 1 && Math.Abs(next.Y - item.Position.Y) < 1)
                            {
                                flag = false;
                                float distanceFrom = current.DistanceFrom + Vector2.Distance(current.Position, next);
                                float distanceTo = Vector2.Distance(next, endPoint);
                                if (distanceFrom + distanceTo < item.Value)
                                {
                                    item.Previous = current;
                                    item.DistanceFrom = distanceFrom;
                                    item.DistanceTo = distanceTo;
                                }
                            }
                        }

                        Rectangle wallCollision = map.CheckCollision(new Rectangle((int)next.X, (int)next.Y, tileSize, tileSize));

                        if (wallCollision == Rectangle.Empty && flag)
                        {
                            float distanceFrom = current.DistanceFrom + Vector2.Distance(current.Position, next);
                            float distanceTo = Vector2.Distance(next, endPoint);
                            available.Add(new Node(current, next, distanceFrom, distanceTo));
                        }
                    }
                }
                available.Sort((a, b) => b.Value.CompareTo(a.Value));
            }

            return new List<Vector2>();
        }

        private static Vector2 AlignToGrid(Vector2 coordinate, int tileSize)
        {
            return new Vector2(RoundToTiles(coordinate.X, tileSize), RoundToTiles(coordinate.Y, tileSize));
        }

        private static float RoundToTiles(float coordinate, int tileSize)
        {
            return (float)(Math.Round(coordinate/tileSize) * tileSize);
        }
    }

    public class Node
    {
        Node previous;
        Vector2 position;
        float distanceFrom;
        float distanceTo;

        public Node Previous
        {
            get => previous;
            set => previous = value;
        }

        public float DistanceTo
        {
            get => distanceTo;
            set => distanceTo = value;
        }

        public float DistanceFrom
        {
            get => distanceFrom;
            set => distanceFrom = value;
        }

        public Vector2 Position
        {
            get => position;
        }

        public float Value
        {
            get => distanceFrom + distanceTo;
        }

        public Node(Node previous, Vector2 position, float distanceFrom, float distanceTo)
        {
            this.previous = previous;
            this.position = position;
            this.distanceFrom = distanceFrom;
            this.distanceTo = distanceTo;
        }
    }
}
