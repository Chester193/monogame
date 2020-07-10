using System;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class ScriptsController
    {
        private bool isFin = false;
        private bool activated = false;


        public delegate void GameScript();
        GameScript gScript;

        public ScriptsController(GameScript gameScript)
        {
            gScript = gameScript;
        }

        public void Activate()
        {
            //if (!activated)
           // {
               // activated = true;
                Execute();
              //  activated = false;
          //  }

        }

        public void Execute()
        {
            gScript();
            isFin = true;
        }
    }
}