﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class Genome
{
    #region Constants

    /// <summary>
    ///     Количество ячеек в геноме жука
    /// </summary>
    public static readonly int LengthGenome = 64;

    #endregion
    
    private int _currentGenePosition;

    #region Properties

    public int CurrentGenePosition
    {
        get { return _currentGenePosition; }

        set { _currentGenePosition = value % Genome.LengthGenome; }
    }

    #endregion



    [DataMember]
    public int[] genome = new int[LengthGenome];

    public int[] GenomeMutate(int countMutation)
    {
        int[] mutateGenome = (int[])genome.Clone();

        for (int i = 0; i < countMutation; i++)
        {
            int newGen = Data.Rnd.Next(0, Bug.MasBugCommands.Length + 1);
            if (newGen == Bug.MasBugCommands.Length)
            {
                newGen = Data.Rnd.Next(Bug.MasBugCommands.Length, mutateGenome.Length);
            }

            mutateGenome[Data.Rnd.Next(0, mutateGenome.Length)] = newGen;
        }

        return mutateGenome;
    }

    public int NextGenePosition(int shift = 1)
    {
        return (CurrentGenePosition + shift) % LengthGenome;
    }

    #region Constructors
    public Genome()
    {
        CurrentGenePosition = 0;
        for (int i = 0; i < genome.Length; i++)
        {
            genome[i] = Data.Rnd.Next(0, LengthGenome);
        }
    }

    public Genome(int[] newGenome)
    {
        CurrentGenePosition = 0;
        genome = newGenome;
    }

    #endregion


}


