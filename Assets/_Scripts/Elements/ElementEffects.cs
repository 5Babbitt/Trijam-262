using UnityEngine;

/// <summary>
/// ElementStats
/// </summary>
[CreateAssetMenu(menuName= "ElementEffects")]
public class ElementEffects : ScriptableObject
{
    [Header("Fire Settings")]
    public int burnDamage;
    public float burnTime;
    public float burnRate;

    [Header("Lightning Settings")]
    public int shockDamage;
    public float stunTime;
    public int maxChainTargets;

    [Header("Slime Settings")]
    public int slimeDamage;
    public float slimeTime;

    [Header("Wind Settings")]
    public int windDamage;
    public float windForce;

    public static void FireEffect()
    {

    }

    public static void LightningEffect()
    {

    }

    public static void SlimeEffect()
    {

    }

    public static void WindEffect()
    {

    }
}

public enum Elements
{
    none,
    fire,
    lightning,
    slime,
    wind
}
