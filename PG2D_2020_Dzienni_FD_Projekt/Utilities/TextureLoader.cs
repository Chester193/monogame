using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.Utilities
{
    public static class TextureLoader
    {
        const bool usingPipeline = true;

        public static Texture2D Load(string filePath, ContentManager content)
        {
            Texture2D image = content.Load<Texture2D>(filePath);

            if (usingPipeline == false)
                PremultiplyTexture(image);

            return image;
        }

        private static void PremultiplyTexture(Texture2D texture)
        {
            //This function pre multiplies the alpha of a texture, just like the XNA Content Pipeline does:
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }
    }
}
