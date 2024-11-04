using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    public PokemonBase Base
    {
        get
        {
            return _base;

        }
    }
    public int Level
    {
        get
        {
            return level;
        }
    }
    public int HP { get; set; }
    public Move CurrentMove{ get; set; }

    public List<Move> Moves { get; set; }

    public Dictionary<PokemonBase.Stat, int> Stats { get; private set; }


    public void Init()
    {

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));
            if (Moves.Count >= 4)
                break;
        }
        CalculateStats();
        HP = MaxHp;

    }
    void CalculateStats()
    {
        Stats = new Dictionary<PokemonBase.Stat, int>();

        Stats.Add(PokemonBase.Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(PokemonBase.Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5);
        Stats.Add(PokemonBase.Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
        Stats.Add(PokemonBase.Stat.SpDefense, Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5);
        Stats.Add(PokemonBase.Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);

        MaxHp = Mathf.FloorToInt((Base.Speed * Level) / 100f) + 10;
    }

    int GetStat(PokemonBase.Stat stat)
    {
        int statVal = Stats[stat];
        return statVal;
    }



    public int Attack
    {
        get { return GetStat(PokemonBase.Stat.Attack); }
    }
    public int Defense
    {
        get { return GetStat(PokemonBase.Stat.Defense); }
    }
    public int SpAttack
    {
        get { return GetStat(PokemonBase.Stat.SpAttack); }
    }
    public int SpDefense
    {
        get { return GetStat(PokemonBase.Stat.SpDefense); }
    }
    public int Speed
    {
        get { return GetStat(PokemonBase.Stat.Speed); }
    }
    public int MaxHp
    {
        get; private set;
    }
    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if (UnityEngine.Random.value * 100f <= 6.25f)
            critical = 2f;
        float type = PokemonBase.TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * PokemonBase.TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false

        };
        float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.IsSpecial) ? SpDefense : Defense;
        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = UnityEngine.Random.Range(0, Moves.Count);
        return Moves[r];
    }
    public class DamageDetails
    {
        public bool Fainted { get; set; }
        public float Critical { get; set; }
        public float TypeEffectiveness { get; set; }


    }

}
