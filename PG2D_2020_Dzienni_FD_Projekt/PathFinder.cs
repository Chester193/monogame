using Microsoft.Xna.Framework;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class PathFinder
    {
        //public List<Node> available_test = new List<Node>();
        //public List<Point> visited_test = new List<Point>();

        //private Point fixedEndPoint = new Point(-1, -1);

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

        public bool FindPath(TiledMap map, List<GameObject> gameObjects, Vector2 startPoint, Vector2 endPoint)
        {
            List<Node> available = new List<Node>();
            List<Point> visited = new List<Point>();

            int tileSize = map.tileSize;

            // Remove first step and exit if endPoint not changed
            //if(fixedEndPoint.X == (int)endPoint.X && fixedEndPoint.Y == endPoint.Y)
            //{
            //    if(Path.Count > 0)
            //        Path.RemoveAt(Path.Count - 1);
            //
            //    return true;
            //}    

            //available_test = available;
            //visited_test = visited;

            Point fixedStartPoint = AlignToGrid(startPoint, tileSize);
            Point fixedEndPoint = AlignToGrid(endPoint, tileSize);

            //Prepare root node and push it into queue
            Node root = new Node(null, fixedStartPoint, 0, fixedStartPoint.DistanceTo(fixedEndPoint));
            available.Add(root);

            while(available.Count > 0 && visited.Count < 100)
            {
                //Pop lowest value node from queue and add to visited list
                Node current = available[available.Count - 1];
                available.RemoveAt(available.Count - 1);
                visited.Add(current.Position);

                //If shortest path was found, save it and exit.
                if (current.DistanceTo < tileSize + tileSize / 2)
                {
                    SavePath(current, endPoint);
                    return true;
                }
                //Find all adjacent fields and check their availability
                for(int i = -tileSize; i <= tileSize; i += tileSize)
                {
                    for (int j = -tileSize; j <= tileSize; j += tileSize)
                    {
                        Point neighbour_position = new Point(current.Position.X + i, current.Position.Y + j);
                        Node  neighbour = Node.GetNode(current, neighbour_position, fixedEndPoint);
                        Rectangle box = new Rectangle(neighbour_position.X - (tileSize / 2), neighbour_position.Y - (tileSize / 2), tileSize, tileSize);

                        if (!IsAlreadyAvailable(neighbour, available) && !IsVisited(neighbour_position, visited) && !IsCollision(box, map) && !IsGameObjectCollision(box, gameObjects))
                        {
                            available.Add(neighbour);
                        }
                    }
                }

                //sort queue
                available.Sort((a, b) => b.Value.CompareTo(a.Value));
            }
            return false;
        }

        private Point AlignToGrid(Vector2 coordinate, int tileSize)
        {
            return new Point(RoundToTileCenter(coordinate.X, tileSize), RoundToTileCenter(coordinate.Y, tileSize));
        }

        private int RoundToTileCenter(float coordinate, int tileSize)
        {
            return (int)(coordinate - (coordinate % tileSize) + (tileSize / 2));
        }

        private void SavePath(Node current, Vector2 endPoint)
        {
            Path = new List<Vector2>();
            Path.Add(endPoint);

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

        private bool IsCollision(Rectangle box, TiledMap map)
        {
            Rectangle wallCollision = map.CheckCollision(box);

            if (wallCollision == Rectangle.Empty)
                return false;

            return true;
        }

        private bool IsGameObjectCollision(Rectangle box, List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.active == true && gameObject.isCollidable == true && gameObject.CheckCollision(box) == true)
                {
                    return true;
                }
            }
            return false;
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
