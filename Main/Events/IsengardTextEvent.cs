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
            new IsengardTextEvent("{CURRENT_DAY}", "Gossip of Gandalf the Grey visiting Bag End spreads across the Shire. His presence is welcomed, but curiosity of the friendly wizard’s reasons for making his way to Hobbiton lingers.", "Unexpected Company in Bag End", 0.9f),
            //new IsengardTextEvent("{CURRENT_DAY}", "The Rangers of the North, led by Strider, confirm sightings of hooded, shadowy figures stalking the outskirts of the Shire. With a growing sense of worry and unease, the Rangers continue to keep a close eye on the unfamiliar visitors.", "Strider’s Reports", 0.8f),
            //new IsengardTextEvent("{CURRENT_DAY}", "Accounts from travelers detail Saruman the White sealing the gates of Isengard not long after Gandalf the Grey was seen heading toward the fortress. The two events having correlation is unknown, but the travelers emphasize the oddity.", "Rumors from Isengard", 0.7f),
            //new IsengardTextEvent("{CURRENT_DAY}", "An assistant at the Prancing Pony in Bree recalls the terrifying moment two figures dressed in all black raided the rooms assigned to a group of hobbits staying at the inn. Luckily, the party was not present during the attack. Unsure of its importance, the assistant notes that the same figures had come to the inn just days prior asking for a “Baggins”. He turned them away due to an uneasiness he felt about them.", "Raid at the Prancing Pony", 0.6f),
            //new IsengardTextEvent("{CURRENT_DAY}", "The horsemen of Rohan exclaim their concern over the bands of orc prowling the Westfold leaving their king, Théoden, and his court distressed.", "A Warning from Rohan", 0.5f),
           //new IsengardTextEvent("{CURRENT_DAY}", "A low, groaning wail from deep in Fangorn Forest accompanies the sounds of trees falling. While listening more intently, the noises seem to be those of ancient agony over the deforestation that is occurring.", "The Wailing in the Wind", 0.4f),
            new IsengardTextEvent("{CURRENT_DAY}", "Those using the Fords of Isen for travel report on its sudden decrease in water levels and flow. Upon further investigation, the disturbance seems to come from the north near Isengard.", "Disturbance to the River Isen", 0.3f),
            //new IsengardTextEvent("{CURRENT_DAY}", "Riders from Rohan are alarmed to find the Dunlendings accumulating outside of Isengard. With such lines being drawn and war growing closer, Théodred orders the preparation of weapons and forces.", "The Threat Grows", 0.2f),
            new IsengardTextEvent("{CURRENT_DAY}", "With beacons made ready to be set ablaze, the watchful eyes in Gondor notice a large gathering just over the Great River. The stark realization that the time of war has come begins to set in.With beacons made ready to be set ablaze, the watchful eyes in Gondor notice a large gathering just over the Great River. The stark realization that the time of war has come begins to set in.", "A Darkness Beyond the Anduin", 0.1f)
        };
    }
}