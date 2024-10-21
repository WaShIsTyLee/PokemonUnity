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
        //                   NOR  FIR  WAT  ELE  GRA  ICE  FIG  POI
        /*NOR*/ new float[] { 1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f },
        /*FIR*/ new float[] { 1f,  0.5f, 0.5f, 1f,  2f,  2f,  1f,  1f },
        /*WAT*/ new float[] { 1f,  2f,  0.5f, 2f,  0.5f, 1f,  1f,  1f },
        /*ELE*/ new float[] { 1f,  1f,  2f,  0.5f, 0.5f, 1f,  1f,  1f },
        /*GRS*/ new float[] { 1f,  0.5f, 2f,  0.5f, 0.5f, 2f,  1f,  0.5f },
        /*POI*/ new float[] { 1f,  1f,  1f,  1f,  2f,  1f,  1f,  1f }
    };

        // El método que accede a chart debe seguir siendo estático
        public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
        {
            if (attackType == PokemonType.None || defenseType == PokemonType.None)
                return 1;

            int row = (int)attackType - 1;
            int col = (int)defenseType - 1;

            return chart[row][col];  // Ahora chart es estático, no dará error
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
