using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State.Adventure;
using UnityEngine;
using Assets.Scripts.Adventure.UI.Dialog;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Dialogs
{
    public class GreetingsInteractor : BaseDialogInteractor<ViewGreetings, DidViewGreetings>
    {
        private const int NUM_PAGES = 2;
        private static int page;

        public override void WaitForStart()
        {
            page = 1;

            string title = "Greetings!";
            string text = GetPageText(page);

            DialogUI.Instance.ShowDialogUI(title, text, null);

            GameStateManager.Instance().SetState(new DidViewGreetings());
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                DialogUI.Instance.UpdateTextMessage(GetPageText(++page));
            }

            if (page > NUM_PAGES)
            {
                DialogUI.Instance.HideDialog();

                GameStateManager.Instance().SetState(new Adventuring());
            }
        }

        private string GetPageText(int page)
        {
            string text = string.Empty;

            if (page == 1)
            {
                text += "\tKing Maximus suddenly summons you in his castle and says: \r\n";
                text += $"\t- My Dear {Player.Instance().GetFullTitle()}, many years have passed since that \r\n";
                text += "good old days when you helped me a lot... Ahh, that good old time... \r\n";
                text += "Long time ago the grass was greener, the sky was brighter and even my... \r\n";
                text += "hmm... Scepter was harder than now. \r\n";

                text += "\tWhen I realised I'm getting older, I opened my old wooden chest \r\n";
                text += "and found in rusted nails and foreign coins the old Diary \r\n";
                text += "of my grandfather, King Minimus.\r\n";

                text += "\tI spent many winter evenings reading this strange adagic folio \r\n";
                text += "and amidst my ancestor's wise words I read the story about \r\n";
                text += "legendary artifact - The Holy Grail.\r\n";

                text += "\tEternal glory awaits the one who finds it. \r\n";
                text += "\tAnd in these dark times, when there are plenty of consirators around and \r\n";
                text += "it's easy to confuse a finger with a horse radish or a sage with a quack, \r\n";
                text += "we could use some wisdom by drinking from this cup.\r\n";
            }

            if (page == 2)
            {
                text += "\tKing Maximus finished his quotation and returned to the throne with \r\n";
                text += "a short breath. After a few seconds he continues: \r\n";

                text += "\t- My Dear! The History repeats itself, first as tradegy, second as farce. \r\n";
                text += "It looks that I'm overlooked the conspiracy again. My past retinue laid a plot \r\n";
                text += "against me. The ancient Grail is lost, but many of my so called subjects wanted \r\n";
                text += "to find it first.\r\n";

                text += "\tIf that happens my kingdom will be ruined again. \r\n";

                text += $"\tHelp me, Obi-Wan... Hmm... {Player.Instance().GetFullTitle()}. You are my only hope.\r\n";
                text += "You have not so many days to recover the Lost Grail. Hurry up, my friend! \r\n";
            }

            return text;
        }
    }
}