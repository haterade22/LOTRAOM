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
            new IsengardTextEvent("{CURRENT_DAY}", "Whispers from the Shire speak of old Gandalf the Grey visiting Hobbiton, his face grim with purpose. Strange business for a wizard in such quiet lands.", "Continue", 0.9f),
            //new IsengardTextEvent("{CURRENT_DAY}", "Rangers in the North speak of shadowed figures skulking near the Shire. The Dúnedain watch the roads with wary eyes.", "Continue", 0.8f),
            //new IsengardTextEvent("{CURRENT_DAY}", "Rumors drift from Isengard that the White Wizard has barred his gates. None have seen Gandalf since he rode south.", "Continue", 0.7f),
            //new IsengardTextEvent("{CURRENT_DAY}", "Tales from Bree tell of black-cloaked riders on dark steeds, asking after hobbits. Fear grips the hearts of men in the taverns", "Continue", 0.6f),
            //new IsengardTextEvent("{CURRENT_DAY}", "Horsemen of Rohan whisper of orc bands prowling the Westfold. The king’s hall is heavy with foreboding.", "Continue", 0.5f),
           //new IsengardTextEvent("{CURRENT_DAY}", "Deep in Fangorn’s ancient heart, the felling of trees echoes like a wound. The forest stirs, its wrath kindling as if roused by some unseen hand.", "Continue", 0.4f),
            new IsengardTextEvent("{CURRENT_DAY}", "The Fords of Isen lie parched, their waters vanished under Isengard’s gaze. Wayfarers murmur of foul craft at work in the shadow of the White Wizard’s tower.", "Continue", 0.3f),
            //new IsengardTextEvent("{CURRENT_DAY}", "The Wild men of Dunland are seen gathering near Isengard’s walls. Rohan’s riders sharpen their spears", "Continue", 0.2f),
            new IsengardTextEvent("{CURRENT_DAY}", "In Gondor, the beacons stand poised to blaze, their keepers vigilant. Across the Anduin, a nameless darkness gathers, and the hearts of Men steel for war.", "Continue", 0.1f)
        };
    }
}