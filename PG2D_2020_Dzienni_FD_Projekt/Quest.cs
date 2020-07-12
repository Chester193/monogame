using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
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
        private List<Character> objectives;

        private int actionsCount = 0;
        private int prize, money;

        public static readonly string defaultDialog = "I'm busy, come back later";
        private string startDialog;
        private string endDialog;
        private string alternativeDialog;

        public Quest(List<Character> objectives, string startDialog, string endDialog, string alternativeDialog, int prize, int money)
        {
            this.objectives = objectives;
            this.startDialog = startDialog;
            this.endDialog = endDialog;
            this.alternativeDialog = alternativeDialog;
            this.prize = prize;
            this.money = money;

            foreach(Character item in objectives)
            {
                item.active = false;
            }
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

        public void Confirm(Player player)
        {
            if (State == QuestState.Available)
            {
                State = QuestState.InProgres;
                ActivateObjectives();
            }

            if (State == QuestState.CollectReward)
            {
                State = QuestState.Done;
                player.GainExperience(prize);
                player.EarnMoney(money);
            }
        }

        private void ActivateObjectives()
        {
            foreach(Character item in objectives)
            {
                item.active = true;
                item.Quest = this;
            }
        }
    }
}
