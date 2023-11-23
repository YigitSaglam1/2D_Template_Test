using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewAlly", menuName ="ScriptableObjects/Entities/Ally")]
public class SO_Ally : ScriptableObject
{
    public string nameAlly;
    public float health;
    public float runSpeed;
    public float damage;
    public float followDistance;
    public float dashingPower;
    public float dashingTime;
    public float dashingCooldown;
}
