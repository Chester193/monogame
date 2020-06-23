using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.npc
{
    public enum NPCType
    {
        warlord,
        sage,
        jeweler,
        blacksmith
    }

    class NonplayableCharacter : Character
    {

        NPCType type;

        public NonplayableCharacter(Vector2 startingPosition, CharacterSettings settings, NPCType type)
        {
            this.position = startingPosition;
            this.type = type;
            applyGravity = false;

            base.SetCharacterSettings(settings);
        }

        public override void Initialize()
        {
            scale = 0.5f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {
            SpriteAtlasData atlas = null;
            switch (type)
            {
                case NPCType.blacksmith:
                    texture = TextureLoader.Load(@"characters/blacksmith", content);
                    atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/blacksmith.atlas", texture, content);
                    break;
                case NPCType.warlord:
                    texture = TextureLoader.Load(@"characters/warlord", content);
                    atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/warlord.atlas", texture, content);
                    break;
                case NPCType.sage:
                    texture = TextureLoader.Load(@"characters/sage", content);
                    atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/sage.atlas", texture, content);
                    break;
                case NPCType.jeweler:
                    texture = TextureLoader.Load(@"characters/jeweler", content);
                    atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/jeweler.atlas", texture, content);
                    break;
            }
            

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.Greeting);

            base.Load(content);

            boundingBoxOffset = new Vector2(50, 50);
            boundingBoxWidth = 50;
            boundingBoxHeight = 50;
        }
    }
}
