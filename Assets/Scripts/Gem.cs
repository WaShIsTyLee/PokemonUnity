using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, IItem
{
    public int worth = 5;

    public void Collect() {
        Destroy(gameObject);
    }
}