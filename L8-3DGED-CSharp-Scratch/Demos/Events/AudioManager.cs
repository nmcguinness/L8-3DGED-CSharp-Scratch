using System;
using System.Collections.Generic;

namespace L8_3DGED_CSharp_Scratch.Demos     
{
    public class AudioManager
    {
        private Dictionary<PickupType, string> pickupCueMap;

        public AudioManager()
        {
            pickupCueMap = new Dictionary<PickupType, string>();
        }

        public void AddPickupCue(PickupType pickup, string audioFileName)
        {
            // add audio file name if not in map
            if (!pickupCueMap.ContainsKey(pickup))
                pickupCueMap.Add(pickup, audioFileName);
        }

        public void PlayPickupCue(int amount, PickupType pickup)
        {
            if (pickupCueMap.ContainsKey(pickup))
            {
                string audioFileName = pickupCueMap[pickup];
                // play the audio file
                Console.WriteLine($"Playing audio cue: {audioFileName}");
            }
            else
            {
                Console.WriteLine($"No audio cue found for pickup type: {pickup}");
                Console.WriteLine($"Playing generic audio cue");
            }
        }
    }
}
