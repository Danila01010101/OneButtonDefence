using System;
using System.Collections.Generic;

public class GroundBlocksFactory
{
    private List<Ground> blockPrefabs;

    public void Initialize(List<Ground> blocks)
    {
        blockPrefabs = blocks;
    }

    public Ground GetBlockByType(Ground.GroundBlockType type)
    {
        switch (type)
        {
            case Ground.GroundBlockType.MainBuilding:
                return GetBlock<MainBuildingGroundBlock>();
            case Ground.GroundBlockType.Ordinary:
                return GetBlock<Ground>();
            case Ground.GroundBlockType.CampBlock:
                return GetBlock<CampBlock>();
            case Ground.GroundBlockType.WaterBlock:
                return GetBlock<WaterBlock>();
            default:
                throw new NotImplementedException();
        }
    }

    public T GetBlock<T>() where T : Ground
    {
        foreach (var blockPrefab in blockPrefabs)
        {
            if (blockPrefab is T && !blockPrefab.GetType().IsSubclassOf(typeof(T)))
            {
                return blockPrefab as T;
            }
        }

        throw new ArgumentException("Invalid type of ground block or blocks list is incorrect");
    }
}