using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Trigger : GameObject
    {
        private bool isCircel = false;
        private List<ScriptsController> scripts;
        private int scriptId;

        private ScriptsController lastScript;

        public Trigger(Vector2 location, Vector2 size, int scriptID, List<ScriptsController> scriptsList)
        {
            position = location;
            boundingBoxHeight = (int)size.Y;
            boundingBoxWidth = (int)size.X;
            isCollidable = false;
            scripts = scriptsList;
            scriptId = scriptID;
        }

        public Trigger(Vector2 positon, int radius, int scriptID, List<ScriptsController> scripts)
        {
            isCircel = true;


        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            if (CheckCollision(gameObjects[0].BoundingBox))
            {
                ActivScript();
            }
            else
            {

            }
        }

        private void ActivScript()
        {
            ScriptsController script = scripts[scriptId];
            script.Activate();
        }
    }
}
