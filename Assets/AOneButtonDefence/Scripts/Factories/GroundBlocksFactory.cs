using System;
using System.Collections.Generic;

public class GroundBlocksFactory
{
    private WorldGenerationData data;
    private List<Ground> blockPrefabs;

    public GroundBlocksFactory(WorldGenerationData data)
    {
        this.data = data;
        blockPrefabs = data.EarthBlocks;
    }

    public Ground GetDefaultBlock() => data.EmptyBlock;

    public Ground GetCentralBlock() => data.CentralBlock;

    public Ground GetEmptyBlock() => data.EmptyBlock;

    public Ground GetRandomEarthBlock()
    {
        int blockIndex = UnityEngine.Random.Range(0, blockPrefabs.Count);

        return blockPrefabs[blockIndex];
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
            case Ground.GroundBlockType.Empty:
                return GetEmptyBlock();
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