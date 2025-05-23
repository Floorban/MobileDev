using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/New Wave")]
public class WaveStats : ScriptableObject
{
    public List<Enemy> enemyPoll;
    public float waveDuration;
    public int waveValue = 1;
    public int maxNumOfEnemies;
}
