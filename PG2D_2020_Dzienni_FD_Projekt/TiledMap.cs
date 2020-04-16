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
        public int tileSize = 128;

        TmxMap tiledMap;
        List<int> firstGids = new List<int>();
        List<Texture2D> tilesets = new List<Texture2D>();

        List<TileLayer> tileLayers = new List<TileLayer>();
        List<Rectangle> collisionRectangles = new List<Rectangle>();

        int VResWidth, VResHeight;

        public TiledMap()
        {

        }

        public TiledMap(int screenWidth, int screenHeight)
        {
            VResWidth = screenWidth;
            VResHeight = screenHeight;
        }


        public void Load(ContentManager content, string filePath)
        {
            //Load tmx file
            //var tilemapsPathPrefix = @"Tilemaps/";
            tiledMap = new TmxMap(@content.RootDirectory + @"\" + filePath);

            foreach (var tileset in tiledMap.Tilesets)
            {
                tilesets.Add(content.Load<Texture2D>(System.IO.Path.GetDirectoryName(filePath) + @"\" + @tileset.Name.ToString()));
                firstGids.Add(tileset.FirstGid);
            }

            foreach (var layer in tiledMap.Layers)
            {
                TileLayer mapLayer = new TileLayer(VResWidth, VResHeight);

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

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            foreach (var layer in tileLayers)
            {
                layer.Update(gameTime, playerPosition);
            }
        }

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

        private List<Tile> drawableTiles = new List<Tile>();
        public float layerDepth = 1.0f;

        int VResWidth, VResHeight;
        Vector2 previousPlayerPosition = new Vector2(int.MinValue, int.MinValue);

        public TileLayer()
        {

        }

        public TileLayer(int screenWidth, int screenHeight)
        {
            VResWidth = screenWidth;
            VResHeight = screenHeight;
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (playerPosition != previousPlayerPosition)
            {
                drawableTiles.Clear();

                //choose tiles to be drawn
                foreach (var tile in tiles)
                {
                    if (tile.position.X > playerPosition.X - 2 * VResWidth && tile.position.X < playerPosition.X + 2 * VResWidth && tile.position.Y > playerPosition.Y - 2 * VResHeight && tile.position.Y < playerPosition.Y + 2 * VResHeight)
                    {
                        drawableTiles.Add(tile);
                    }
                }

            }

            previousPlayerPosition = playerPosition;

        }

        public void Draw(List<Texture2D> tilesets, SpriteBatch spriteBatch)
        {
            foreach (var tile in drawableTiles)
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
}
