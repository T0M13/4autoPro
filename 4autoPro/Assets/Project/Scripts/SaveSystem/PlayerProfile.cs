using System;
using System.Collections;
using UnityEngine;

namespace tomi.SaveSystem
{
    [System.Serializable]
    public class PlayerProfile
    {
        public int coins;
        public float score;

        public int masterVolume;
        public int musicVolume;
        public int effectsVolume;
    }
}