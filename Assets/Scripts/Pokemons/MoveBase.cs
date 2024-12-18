using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] PokemonBase.PokemonType type;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;
    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public PokemonBase.PokemonType Type
    {
        get { return type; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

    public int PP
    {
        get { return pp; }
    }
    public bool IsSpecial{
        get{
            if(type==PokemonBase.PokemonType.Fire||type==PokemonBase.PokemonType.Water
            ||type==PokemonBase.PokemonType.Grass||type==PokemonBase.PokemonType.Ice||type==PokemonBase.PokemonType.Electric||type==PokemonBase.PokemonType.Dragon ){
              return true;  
            }
            else{
                return false;
            }
        }
    }
}
