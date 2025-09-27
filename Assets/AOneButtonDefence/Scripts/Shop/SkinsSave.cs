using System;
using System.Collections.Generic;

[Serializable]
public class SkinsSave
{
    public List<SkinSaveData> Skins = new List<SkinSaveData>();

    public bool IsUnlocked(string skinId)
    {
        var skin = Skins.Find(s => s.SkinId == skinId);
        return skin != null && skin.Unlocked;
    }

    public void Unlock(string skinId)
    {
        var skin = Skins.Find(s => s.SkinId == skinId);
        if (skin == null)
        {
            skin = new SkinSaveData { SkinId = skinId, Unlocked = true };
            Skins.Add(skin);
        }
        else
        {
            skin.Unlocked = true;
        }
    }
}
    
[Serializable]
public class SkinSaveData
{
    public string SkinId;
    public bool Unlocked;
}