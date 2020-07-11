using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    public class GameObject
    {
        protected Texture2D texture;
        public Vector2 position;
        public Vector2 originalPosition = new Vector2(-1, -1);
        protected Vector2 center; //origin of the sprite/texture
        public Color tintColor = Color.White;
        public float scale = 0.6f;
        public float rotation = 0.0f;
        //public float layerDepth = 0.5f;
        public float layerDepth = 0.1f;
        public bool active = true;

        public bool isCollidable = true;
        protected int boundingBoxWidth, boundingBoxHeight;
        protected Vector2 boundingBoxOffset = Vector2.Zero;
        Texture2D boundingBoxTexture;
        const bool drawBoundingBoxes = true;

        public Vector2 direction = new Vector2(1, 0);

        public Vector2 startPosition = new Vector2(-1, -1);

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)(position.X + boundingBoxOffset.X * scale), (int)(position.Y + boundingBoxOffset.Y * scale), (int)(boundingBoxWidth * scale), (int)(boundingBoxHeight * scale));
            }
        }

        public GameObject()
        {

        }

        public virtual void Initialize()
        {
            if (startPosition == new Vector2(-1, -1))
            {
                startPosition = position;
            }

        }

        public virtual void SetToDefaultPosition()
        {
            position = startPosition;
        }

        public virtual void Load(ContentManager content)
        {
            boundingBoxTexture = TextureLoader.Load(@"other/pixel", content);
            CalculateCenter();

            if (texture != null)
            {
                boundingBoxWidth = texture.Width;
                boundingBoxHeight = texture.Height;
            }
        }


        public virtual void Update(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {

        }

        public virtual bool CheckCollision(Rectangle input)
        {
            return BoundingBox.Intersects(input);
        }

        public virtual void BulletResponse(int damageTaken)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawBoundingBox(spriteBatch);


            if (texture != null && active == true)
            {
                spriteBatch.Draw(texture, position, null, tintColor, rotation, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
            }
        }

        protected void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            if (boundingBoxTexture != null && drawBoundingBoxes == true && active == true)
                spriteBatch.Draw(boundingBoxTexture, new Vector2(BoundingBox.X, BoundingBox.Y), BoundingBox, new Color(128, 128, 128, 128), rotation, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
        }

        private void CalculateCenter()
        {
            if (texture == null)
                return;
            center.X = texture.Width / 2;
            center.Y = texture.Height / 2;
        }

    }
}
