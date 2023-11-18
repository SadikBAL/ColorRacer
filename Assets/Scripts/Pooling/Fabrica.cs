using System.Collections;
using UnityEngine;

public class Fabrica : MonoBehaviour
{
    //Pooling InitDatas from ScriptableObject.
    public PoolDatas PoolDatas;
    private PrefabPool PlayerPool;
    private PrefabPool BlockPool;
    private PrefabPool BlowParticlePool;
    private PrefabPool RedBackgPool;
    private PrefabPool BlueBackgPool;
    private PrefabPool GreenBackPool;
    public void Start()
    {
        //Init for all createable game objects for pooling.
        PlayerPool = new PrefabPool(PoolDatas.Player.Prefab,PoolDatas.Player.InitSize);
        BlockPool = new PrefabPool(PoolDatas.Block.Prefab, PoolDatas.Block.InitSize);
        BlowParticlePool = new PrefabPool(PoolDatas.BlowParticle.Prefab, PoolDatas.BlowParticle.InitSize);
        RedBackgPool = new PrefabPool(PoolDatas.RedBackg.Prefab, PoolDatas.RedBackg.InitSize);
        BlueBackgPool = new PrefabPool(PoolDatas.BlueBackg.Prefab, PoolDatas.BlueBackg.InitSize);
        GreenBackPool = new PrefabPool(PoolDatas.GreenBack.Prefab, PoolDatas.GreenBack.InitSize);
    }
    //Pooling get metods.
    public GameObject GetPlayer()
    {
        GameObject Temp = PlayerPool.GetObject();
        Temp.transform.localScale = new Vector3(0.5f,0.5f,0.1f);
        Temp.GetComponent<PlayerController>().Init();
        return Temp;
    }
    public GameObject GetBlock()
    {
        return BlockPool.GetObject();
    }
    public GameObject GetBlowParticle()
    {
        return BlowParticlePool.GetObject();
    }
    public GameObject GetRedBackg()
    {
        return RedBackgPool.GetObject();
    }
    public GameObject GetBlueBackg()
    {
        return BlueBackgPool.GetObject();
    }
    public GameObject GetGreenBackg()
    {
        return GreenBackPool.GetObject();
    }
    public GameObject GetRandomBackground()
    {
        int Select = Random.Range(0, 3);
        switch (Select) 
        {
            case 0:
                return GetRedBackg();
            case 1: 
                return GetBlueBackg();
            case 2: 
                return GetGreenBackg();
        }
        return GetRedBackg();
    }
    public GameObject GetRandomBlock(int EmptyBlockPercantage)
    {
        int Percantage = Random.Range(0, 100);
        if(Percantage > EmptyBlockPercantage)
        {
            return GetBlock();
        }
        return null;
    }
    public IEnumerator BlowEffect(Vector3 Position)
    {
        GameObject Temp = GetBlowParticle();
        Temp.transform.position = Position;
        yield return new WaitForSeconds(1);
        Temp.GetComponent<PooledPrefab>().Pool.ReturnObject(Temp);
    }
}
