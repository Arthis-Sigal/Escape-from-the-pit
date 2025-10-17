using System;
using System.Collections.Generic;

[Serializable]
public class LevelStarData
{
    public int level;
    public int stars;
}

[Serializable]
public class SaveDataSerializable
{
    public int lastUnlockedLevel;
    public List<LevelStarData> levelStars = new List<LevelStarData>();
}
