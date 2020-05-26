using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class PathFinder
    {
        //public List<Node> available_test = new List<Node>();
        //public List<Point> visited_test = new List<Point>();

        private Point fixedEndPoint = new Point(-1, -1);

        public List<Vector2> Path { get; private set; } = new List<Vector2>();

        public bool TryGetFirstStep(out Vector2 firstStep)
        {
            if (Path.Count > 0)
            {
                firstStep = Path[Path.Count - 1];
                return true;
            }

            firstStep = Vector2.Zero;
            return false;

        }

        public void FindPath(TiledMap map, Vector2 startPoint, Vector2 endPoint)
        {
            List<Node> available = new List<Node>();
            List<Point> visited = new List<Point>();

            int tileSize = map.tileSize;

            // Remove first step and exit if endPoint not changed
            if(fixedEndPoint.X == (int)endPoint.X && fixedEndPoint.Y == endPoint.Y)
            {
                if(Path.Count > 0)
                    Path.RemoveAt(Path.Count - 1);

                return;
            }    

            //available_test = available;
            //visited_test = visited;

            Point fixedStartPoint = AlignToGrid(startPoint, tileSize);
            fixedEndPoint = new Point((int)endPoint.X, (int)endPoint.Y);

            //Prepare root node and push it into queue
            Node root = new Node(null, fixedStartPoint, 0, fixedStartPoint.DistanceTo(fixedEndPoint));
            available.Add(root);

            while(available.Count > 0)
            {
                //Pop lowest value node from queue and add to visited list
                Node current = available[available.Count - 1];
                available.RemoveAt(available.Count - 1);
                visited.Add(current.Position);

                //If shortest path was found, save it and exit.
                if (current.DistanceTo < tileSize)
                {
                    SavePath(current);
                    return;
                }
                //Find all adjacent fields and check their availability
                for(int i = -tileSize; i <= tileSize; i += tileSize)
                {
                    for (int j = -tileSize; j <= tileSize; j += tileSize)
                    {
                        Point neighbour_position = new Point(current.Position.X + i, current.Position.Y + j);
                        Node  neighbour = Node.GetNode(current, neighbour_position, fixedEndPoint);

                        if (!IsAlreadyAvailable(neighbour, available) && !IsVisited(neighbour_position, visited) && !IsCollision(neighbour_position, map))
                        {
                            available.Add(neighbour);
                        }
                    }
                }

                //sort queue
                available.Sort((a, b) => b.Value.CompareTo(a.Value));
            }
        }

        private Point AlignToGrid(Vector2 coordinate, int tileSize)
        {
            return new Point(RoundToTiles(coordinate.X, tileSize), RoundToTiles(coordinate.Y, tileSize));
        }

        private int RoundToTiles(float coordinate, int tileSize)
        {
            return (int)Math.Round(coordinate/tileSize) * tileSize;
        }

        private void SavePath(Node current)
        {
            Path = new List<Vector2>();

            while (current.Previous != null)
            {
                Path.Add(new Vector2(current.Position.X, current.Position.Y));
                current = current.Previous;
            }
        }

        private bool IsVisited(Point point, List<Point> visited)
        {
            foreach (Point item in visited)
            {
                if (point.X == item.X && point.Y == item.Y)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsAlreadyAvailable(Node node, List<Node> available)
        {
            foreach (Node item in available)
            {
                if (node.Position.X == item.Position.X && node.Position.Y == item.Position.Y)
                {
                    // Update node if lower value was found for this position
                    if (node.Value < item.Value)
                    {
                        item.Previous = node.Previous;
                        item.DistanceFrom = node.DistanceFrom;
                        item.DistanceTo = node.DistanceTo;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool IsCollision(Point position, TiledMap map)
        {
            Rectangle wallCollision = map.CheckCollision(new Rectangle(position.X, position.Y, map.tileSize, map.tileSize));

            if (wallCollision == Rectangle.Empty)
                return false;

            return true;
        }
    }

    public class Node
    {
        public Node Previous { get; set; }

        public double DistanceTo { get; set; }

        public double DistanceFrom { get; set; }

        public Point Position { get; }

        public double Value
        {
            get => DistanceFrom + DistanceTo;
        }

        public Node(Node previous, Point position, double distanceFrom, double distanceTo)
        {
            this.Previous = previous;
            this.Position = position;
            this.DistanceFrom = distanceFrom;
            this.DistanceTo = distanceTo;
        }

        public static Node GetNode(Node previous, Point position, Point endPoint)
        {
            double distanceFrom = previous.DistanceFrom + previous.Position.DistanceTo(position);
            double distanceTo = position.DistanceTo(endPoint);
            return new Node(previous, position, distanceFrom, distanceTo);
        }
    }

    public class Point
    {
        public int X { get; }

        public int Y { get; }

        public double DistanceTo(Point dest)
        {
            return Math.Sqrt(Math.Pow(this.X - dest.X, 2) + Math.Pow(this.Y -dest.Y, 2));
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
