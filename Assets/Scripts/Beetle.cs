using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Beetle:IComparable<Beetle>
{
    public int X { get; set; }

    public int Y { get; set; }

    public int Health { get; set; }

    public int Generation { get; set; }

    public Genom Gen { get; set; }

    public void StartAction()
    {
    }

    public Beetle(int x, int y, Genom gen, int health = 50, int generation = 0)
    {
        X = x;
        Y = y;
        Gen = gen;
        Health = health;
        Generation = generation;
    }
    /// <summary>
    /// Сортировка жуков для формирования генома
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Beetle other)
    {
        if (this.Health>other.Health)
        {
            return -1;
        }

        if (this.Health == other.Health)
        {
            return 0;
        }

        return 1;
    }
}

