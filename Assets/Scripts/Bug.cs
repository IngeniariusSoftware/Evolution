using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Bug : IComparable<Bug>
{
    public Coordinates Coordinate { get; set; }

    public int Health { get; set; }

    public int GenerationNumber { get; set; }

    public Genome Gene{ get; set; }

    public int CurrentGenePosition { get; set; }

    public int Direction { get; set; }

    public void StartAction()
    {

    }

    public int NextGenePosition(int shift)
    {
        return (CurrentGenePosition + shift) % 64;
    }

    public Bug(Coordinates coordinate, Genome gene, int health = 50, int generationNumber = 0)
    {
        Coordinate.Y = coordinate.Y;
        Coordinate.X = coordinate.X;
        Gene = gene;
        Health = health;
        GenerationNumber = generationNumber;
    }

    /// <summary>
    /// Сортировка жуков для формирования генома
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Bug other)
    {
        if (this.Health > other.Health)
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

