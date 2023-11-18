
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    [Tooltip("This value represents the overall game speed.")]
    [Range(1, 8)]
    public float SlideSpeed;
    [Tooltip("This value expresses the creation frequency of block objects.")]
    [Range(1, 100)]
    public float BlockFrequency;
    [Tooltip("This value expresses how fast the player scale while standing on the wrong color.")]
    [Range(0.2f, 1f)]
    public float PlayerScaleSpeed;
    [Tooltip("This value represents the time you have to endure.")]
    [Range(10, 100)]
    public float TargetTime;

}
