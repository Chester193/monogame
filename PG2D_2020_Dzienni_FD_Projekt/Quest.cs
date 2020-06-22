using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.SpecialEnemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum QuestState
{
    Available,
    InProgres,
    CollectReward,
    Done
}

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class Quest
    {
        public QuestState State { get; private set; } = QuestState.Available;
        private List<SpecialEnemy> objectives;

        private int actionsCount = 0;
        private int prize;

        public static readonly string defaultDialog = "I'm busy, come back later";
        private string startDialog;
        private string endDialog;
        private string alternativeDialog;

        public Quest(List<SpecialEnemy> objectives, string startDialog, string endDialog, string alternativeDialog, int prize)
        {
            this.objectives = objectives;
            this.startDialog = startDialog;
            this.endDialog = endDialog;
            this.alternativeDialog = alternativeDialog;
            this.prize = prize;
        }

        public string getDialog()
        {
            if (State == QuestState.Available)
            {
                return startDialog;
            }
            else if(State == QuestState.InProgres)
            {
                return alternativeDialog;
            }
            else if(State == QuestState.CollectReward)
            {
                return endDialog;
            }
            return defaultDialog;
        }

        public void Action()
        {
            actionsCount++;
        }

        public void Update()
        {
            if (State == QuestState.InProgres && actionsCount >= objectives.Count)
            {
                State = QuestState.CollectReward;
            }
        }

        public void Confirm()
        {
            if (State == QuestState.Available)
            {
                State = QuestState.InProgres;
                ActivateObjectives();
            }

            if (State == QuestState.CollectReward)
            {
                State = QuestState.Done;
                //TODO: GIVE EXP TO PLAYER
            }
        }

        private void ActivateObjectives()
        {
            foreach(SpecialEnemy item in objectives)
            {
                item.active = true;
                item.Quest = this;
            }
        }
    }
}
