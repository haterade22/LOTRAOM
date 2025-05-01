using System.Collections.Generic;


namespace LOTRAOM.Events
{
    public class IsengardTextEvent
    {
        public string Title { get; }
        public string Text { get; }
        public string ContinueText { get; }
        public float ThresholdToHappen { get; }
        public IsengardTextEvent(string title, string text, string continueText, float time)
        {
            Title = title;
            Text = text;
            ContinueText = continueText;
            ThresholdToHappen = time;
        }
        public static List<IsengardTextEvent> All = new()
        {
            new IsengardTextEvent("{CURRENT_DAY}", "Whispers from the Shire speak of old Gandalf the Grey visiting Hobbiton, his face grim with purpose. Strange business for a wizard in such quiet lands.", "continue", 0.9f),
            new IsengardTextEvent("{CURRENT_DAY}", "Rangers in the North speak of shadowed figures skulking near the Shire. The Dúnedain watch the roads with wary eyes.", "continue", 0.8f),
            new IsengardTextEvent("{CURRENT_DAY}", "Rumors drift from Isengard that the White Wizard has barred his gates. None have seen Gandalf since he rode south.", "continue", 0.7f),
            new IsengardTextEvent("{CURRENT_DAY}", "Tales from Bree tell of black-cloaked riders on dark steeds, asking after hobbits. Fear grips the hearts of men in the taverns", "continue", 0.6f),
            new IsengardTextEvent("{CURRENT_DAY}", "Horsemen of Rohan whisper of orc bands prowling the Westfold. The king’s hall is heavy with foreboding.", "continue", 0.5f),
            new IsengardTextEvent("{CURRENT_DAY}", "You hear rumblings that trees are falling in Fangorn Deep. The forest groans with an ancient anger.", "continue", 0.4f),
            new IsengardTextEvent("{CURRENT_DAY}", "The Fords of Isen have mysteriously dried up. Travelers speak of strange workings in the shadow of Isengard", "continue", 0.3f),
            new IsengardTextEvent("{CURRENT_DAY}", "The Wild men of Dunland are seen gathering near Isengard’s walls. Rohan’s riders sharpen their spears", "continue", 0.2f),
            new IsengardTextEvent("{CURRENT_DAY}", "In Gondor, the beacons stand ready to blaze. Men speak of a darkness gathering beyond the Anduin", "continue", 0.1f)
        };
    }
}