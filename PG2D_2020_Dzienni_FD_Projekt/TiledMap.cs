using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;



namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class TiledMap
    {
        //public List<Decoration> decorations = new List<Decoration>();
        //public List<Wall> walls = new List<Wall>();
        //Texture2D wallTexture;

        public int mapWidth = 15;
        public int mapHeight = 9;

        public int tileSize = 128;

        TmxMap tiledMap;
        List<int> firstGids = new List<int>();
        List<Texture2D> tilesets = new List<Texture2D>();

        List<TileLayer> tileLayers = new List<TileLayer>();
        List<Rectangle> collisionRectangles = new List<Rectangle>();
        private IEnumerable<object> walls;

        public TiledMap()
        {

        }


        public void Load(ContentManager content, string filePath)
        {
            //Load tmx file
            //var tilemapsPathPrefix = @"Tilemaps/";
            tiledMap = new TmxMap(@content.RootDirectory + @"\" + filePath);

            foreach (var tileset in tiledMap.Tilesets)
            {
                Console.WriteLine(@content.RootDirectory + @"\" + filePath + " " + System.IO.Path.GetDirectoryName(filePath));
                tilesets.Add(content.Load<Texture2D>(System.IO.Path.GetDirectoryName(filePath) + @"\" + @tileset.Name.ToString()));
                firstGids.Add(tileset.FirstGid);
            }

            foreach (var layer in tiledMap.Layers)
            {
                TileLayer mapLayer = new TileLayer();

                if (float.TryParse(layer.Properties["layerDepth"], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float result))
                {
                    mapLayer.layerDepth = result;
                }

                int i = 0;
                foreach (var tile in layer.Tiles)
                {

                    int gid = tile.Gid;

                    // Empty tile, do nothing
                    if (gid == 0)
                    {
                    }
                    else
                    {
                        var index = firstGids.IndexOf(firstGids.Where(n => n <= gid).Max());
                        int tileWidth = tiledMap.Tilesets[index].TileWidth;
                        int tileHeight = tiledMap.Tilesets[index].TileHeight;
                        int tilesetTilesWide = tilesets[index].Width / tileWidth;
                        //int tilesetTilesHigh = tilesets[index].Height / tileHeight;


                        int tileFrame = gid - firstGids[index];
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                        float x = (i % tiledMap.Width) * tiledMap.TileWidth;
                        float y = (float)Math.Floor(i / (double)tiledMap.Width) * tiledMap.TileHeight;

                        Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                        Tile mapTile = new Tile(new Rectangle((int)x, (int)y, tileWidth, tileHeight), index, tilesetRec);
                        mapLayer.tiles.Add(mapTile);
                    }
                    i++;
                }

                tileLayers.Add(mapLayer);

            }

            foreach (var objectLayer in tiledMap.ObjectGroups)
            {
                foreach (var rect in objectLayer.Objects)
                {
                    collisionRectangles.Add(new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
                }
            }

        }

        public Rectangle CheckCollision(Rectangle input)
        {
            foreach (var rectangle in collisionRectangles)
            {
                if (rectangle != null && rectangle.Intersects(input) == true)
                {
                    return rectangle;
                }
            }

            return Rectangle.Empty;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var layer in tileLayers)
            {
                layer.Draw(tilesets, spriteBatch);
            }
        }

        //public void DrawWalls(SpriteBatch spriteBatch)
        //{
        //    foreach (var wall in walls)
        //    {
        //        if (wall != null && wall.active == true)
        //        {
        //            spriteBatch.Draw(wallTexture, new Vector2(wall.boundingBox.X, wall.boundingBox.Y), Color.White);
        //            //spriteBatch.Draw(wallTexture, new Vector2(wall.boundingBox.X, wall.boundingBox.Y), wall.boundingBox, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.7f);
        //        }
        //    }
        //}

        //public Point GetTileIndex(Vector2 inputPosition)
        //{
        //    if (inputPosition == new Vector2(-1, -1))
        //    {
        //        return new Point(-1, -1);
        //    }

        //    return new Point((int)inputPosition.X / tileSize, (int)inputPosition.Y / tileSize);
        //}

    }

    public class TileLayer
    {
        public string name;
        public List<Tile> tiles = new List<Tile>();
        public float layerDepth = 1.0f;

        public TileLayer()
        {

        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Draw(List<Texture2D> tilesets, SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
            {
                spriteBatch.Draw(tilesets[tile.tilesetIndex], tile.position, tile.sourceRect, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            }
        }
    }

    public class Tile
    {
        public int tilesetIndex;
        public Rectangle sourceRect;
        public Rectangle position;

        public Tile()
        { }

        public Tile(Rectangle position, int tilesetIndex, Rectangle sourceRect)
        {
            this.position = position;
            this.tilesetIndex = tilesetIndex;
            this.sourceRect = sourceRect;
        }

    }
    //public class Wall
    //{
    //    public Rectangle boundingBox;
    //    public bool active = true;

    //    public Wall()
    //    {

    //    }

    //    public Wall(Rectangle rect)
    //    {
    //        boundingBox = rect;
    //    }
    //}

    //public class Decoration : GameObject
    //{
    //    public string imagePath;
    //    public Rectangle sourceRect;

    //    public string Name
    //    {
    //        get { return imagePath; }
    //    }


    //    public Decoration()
    //    {
    //        isCollidable = false;
    //    }

    //    public Decoration(Vector2 position, string imagePath, float layerDepth)
    //    {
    //        this.position = position;
    //        this.imagePath = imagePath;
    //        this.layerDepth = layerDepth;
    //    }

    //    public virtual void Load(ContentManager content, string assetPath)
    //    {
    //        texture = TextureLoader.Load(assetPath, content);
    //        texture.Name = assetPath;
    //        //imagePath = assetPath;

    //        boundingBoxWidth = texture.Width;
    //        boundingBoxHeight = texture.Height;

    //        if (sourceRect == Rectangle.Empty)
    //        {
    //            sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
    //        }

    //    }

    //    public void SetImage(Texture2D inputTexture, string assetPath)
    //    {
    //        texture = inputTexture;
    //        imagePath = assetPath;
    //    }

    //    public override void Draw(SpriteBatch spriteBatch)
    //    {
    //        if (texture != null && active == true)
    //        {
    //            spriteBatch.Draw(texture, position, sourceRect, tintColor, rotation, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
    //        }
    //    }

    //}
}
