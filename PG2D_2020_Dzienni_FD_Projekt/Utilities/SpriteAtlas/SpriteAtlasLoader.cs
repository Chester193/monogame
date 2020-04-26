using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas
{
    public static class SpriteAtlasLoader
    {
        public static SpriteAtlasData ParseSpriteAtlas(string path, Texture2D spriteSheet, ContentManager content, bool premultiplyAlpha = false)
        {

            string filepath = @content.RootDirectory + @"\" + path;

            var spriteAtlasData = ParseSpriteAtlasData(filepath);
            return spriteAtlasData;
        }

        internal static List<int> GetNumbersFromString(String line)
        {
            string[] numbersString = Regex.Split(line, @"\D+");
            List<int> numbers = new List<int>();

            foreach (string value in numbersString)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    numbers.Add(System.Int32.Parse(value));
                }
            }
            return numbers;
        }

        internal static SpriteAtlasData ParseSpriteAtlasData(string dataFile, bool leaveOriginsRelative = false)
        {
            var spriteAtlas = new SpriteAtlasData();

            var parsingSprites = true;
            var commaSplitter = new char[] { ',' };

            string line = null;
            using (var streamFile = File.OpenRead(dataFile))
            {
                using (var stream = new StreamReader(streamFile))
                {
                    while ((line = stream.ReadLine()) != null)
                    {
                        // Parse the pages
                        // Page information starts after blank line
                        // WARNING - This code assumes single page (single sprite texture)
                        if (parsingSprites && string.IsNullOrWhiteSpace(line))
                        {

                            //parse file name
                            line = stream.ReadLine();
                            var filename = line;
                            spriteAtlas.PageNames.Add(line);
                            //parse dimensions
                            line = stream.ReadLine();
                            var dimmensions = GetNumbersFromString(line);
                            var width = dimmensions[0];
                            var height = dimmensions[1];
                            //skip over format, filter and repeat
                            for (int i = 1; i <= 3; i++)
                                line = stream.ReadLine();

                            parsingSprites = true;
                            continue;
                        }

                        // start parsing sprite data (regions)
                        if (parsingSprites)
                        {

                            //spriteAtlas.Names.Add(line);

                            // Animation name
                            var animName = line;
                            if (!spriteAtlas.AnimationNames.Contains(animName))
                            {
                                spriteAtlas.AnimationNames.Add(animName);
                                spriteAtlas.SourceRects[animName] = new List<Rectangle>();
                            }

                            // is the sprite rotated - NOT SUPPORTED
                            line = stream.ReadLine();

                            // rectangle dimmensions and addig to list
                            var rectPos = GetNumbersFromString(stream.ReadLine());
                            var rectDimmensions = GetNumbersFromString(stream.ReadLine());
                            spriteAtlas.SourceRects[animName].Add(new Rectangle(rectPos[0], rectPos[1], rectDimmensions[0], rectDimmensions[1]));

                            //original size 
                            line = stream.ReadLine();
                            //offset
                            line = stream.ReadLine();
                            //animation Index - NOT SUPPORTED, only sequential (grid) layout is implemented
                            line = stream.ReadLine();

                        }
                        else
                        {
                            // catch the case of a newline at the end of the file
                            if (string.IsNullOrWhiteSpace(line))
                                break;
                        }
                    }
                }
            }
            return spriteAtlas;
        }
    }
}
