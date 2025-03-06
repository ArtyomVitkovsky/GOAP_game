using System;

namespace Game.NpcSystem.Components
{
    public enum NpcMood
    {
        Aggressive = 0,
        Bad = 1,
        Wary = 2,
        Neutral = 3,
        Normal = 4,
        Good = 5,
        Friendly = 6
    }

    public class NpcMoodComponent
    {
        public NpcMood CurrentMood { get; private set; }

        public Action<NpcMood> OnMoodChanged;

        public void UpdateCurrentMood(NpcMood targetMood)
        {
            CurrentMood = targetMood;
            OnMoodChanged?.Invoke(CurrentMood);
        }
    }
}