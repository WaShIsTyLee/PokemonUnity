using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new Pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] private string name;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private PokemonType type1;
    [SerializeField] private PokemonType type2;

    // Base Stats
    [SerializeField] private int maxHp;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int spAttack;
    [SerializeField] private int spDefense;
    [SerializeField] private int speed;

    [SerializeField] List<LearnableMove> learnableMoves;
    [System.Serializable]
    public class LearnableMove
    {
        [SerializeField] MoveBase moveBase;
        [SerializeField] int level;

        public MoveBase Base
        {
            get { return moveBase; }
        }
        public int Level
        {
            get { return level; }
        }

    }
    public enum PokemonType
    {
        None,
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Ice,
        Fighting,
        Poison,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon
    }

    public enum Stat{
        Attack,
        Defense,
        SpAttack,
        SpDefense,
        Speed
    }
    public class TypeChart
    {
         static float[][] chart =
    {
        //                      NOR  FIR  WAT  ELE  GRS  ICE  FIG  POI  GRO  FLY  PSY  BUG  ROC  GHO  DRA  STL 
        /*NOR*/ new float[]    { 1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  0.5f, 0f,  1f,  0.5f },
        /*FIR*/ new float[]    { 1f,  0.5f, 0.5f, 1f,  2f,  2f,  1f,  1f,  1f,  1f,  1f,  2f,  0.5f, 1f,  0.5f, 2f },
        /*WAT*/ new float[]    { 1f,  2f,  0.5f, 2f,  0.5f, 1f,  1f,  1f,  2f,  1f,  1f,  1f,  2f,  1f,  0.5f, 1f },
        /*ELE*/ new float[]    { 1f,  1f,  2f,  0.5f, 0.5f, 1f,  1f,  1f,  0f,  2f,  1f,  1f,  1f,  1f,  0.5f, 1f },
        /*GRS*/ new float[]    { 1f,  0.5f, 2f,  0.5f, 0.5f, 2f,  1f,  0.5f, 2f,  0.5f, 1f,  0.5f, 2f,  1f,  0.5f, 0.5f },
        /*ICE*/ new float[]    { 1f,  0.5f, 0.5f, 1f,  1f,  0.5f, 1f,  1f,  2f,  2f,  1f,  1f,  1f,  1f,  2f,  0.5f },
        /*FIG*/ new float[]    { 2f,  1f,  1f,  1f,  1f,  2f,  1f,  0.5f, 1f,  0.5f, 0.5f, 0.5f, 1f,  0f,  1f,  2f },
        /*POI*/ new float[]    { 1f,  1f,  1f,  1f,  2f,  1f,  1f,  0.5f, 0.5f, 1f,  1f,  1f,  0.5f, 0.5f, 1f,  0f },
        /*GRO*/ new float[]    { 1f,  2f,  1f,  2f,  0.5f, 1f,  1f,  2f,  1f,  0f,  1f,  0.5f, 2f,  1f,  1f,  2f },
        /*FLY*/ new float[]    { 1f,  1f,  1f,  0.5f, 2f,  1f,  2f,  1f,  1f,  1f,  1f,  2f,  0.5f, 1f,  1f,  0.5f },
        /*PSY*/ new float[]    { 1f,  1f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,  1f,  0.5f, 1f,  1f,  1f,  1f,  0.5f },
        /*BUG*/ new float[]    { 1f,  0.5f, 1f,  1f,  2f,  1f,  0.5f, 0.5f, 1f,  0.5f, 2f,  1f,  1f,  0.5f, 1f,  0.5f },
        /*ROC*/ new float[]    { 1f,  2f,  1f,  1f,  1f,  2f,  0.5f, 1f,  0.5f, 2f,  1f,  1f,  1f,  1f,  1f,  0.5f },
        /*GHO*/ new float[]    { 0f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,  2f,  1f,  0.5f },
        /*DRA*/ new float[]    { 1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  2f,  0.5f },
        /*STL*/ new float[]    { 1f,  0.5f, 0.5f, 0.5f, 1f,  2f,  1f,  1f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,  0.5f }
    };
        // El método que accede a chart debe seguir siendo estático
        public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
{
    if (attackType == PokemonType.None || defenseType == PokemonType.None)
        return 1;

    int row = (int)attackType - 1;
    int col = (int)defenseType - 1;

    // Validación de los límites del array para evitar IndexOutOfRangeException
    if (row < 0 || row >= chart.Length || col < 0 || col >= chart[row].Length)
    {
        Debug.LogError($"Índices fuera de rango: attackType={attackType}, defenseType={defenseType}");
        return 1;
    }

    return chart[row][col];
}

    }




    // Properties
    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public PokemonType Type1
    {
        get { return type1; }
    }

    public PokemonType Type2
    {
        get { return type2; }
    }

    // Base Stats Properties
    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }
    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
}
