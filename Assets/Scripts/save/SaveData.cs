using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int lastUnlockedLevel = 1; // dernier niveau débloqué
    public Dictionary<int, int> levelStars = new Dictionary<int, int>(); // niveau → étoiles (0 à 3)
}
