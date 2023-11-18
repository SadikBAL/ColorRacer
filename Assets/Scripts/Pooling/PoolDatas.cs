
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PoolDatas", order = 1)]
public class PoolDatas : ScriptableObject
{
    public PrefabPoolData Player;
    public PrefabPoolData Block;
    public PrefabPoolData BlowParticle;
    public PrefabPoolData RedBackg;
    public PrefabPoolData BlueBackg;
    public PrefabPoolData GreenBack;

}
