using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class Genome
{
    #region Constants

    /// <summary>
    /// Количество ячеек в геноме жука
    /// </summary>
    public static readonly int LengthGenome = 64;

    #endregion

    /// <summary>
    /// Текуая позиция в геноме
    /// </summary>
    private int _currentGenePosition;


    /// <summary>
    /// Показатель успешности данного генома
    /// </summary>
    private int _fitness;

    /// <summary>
    /// Координаты предыдущей мутации
    /// </summary>
    private Coordinates lastMutation;

    #region Properties

    public int CurrentGenePosition
    {
        get
        {
            return _currentGenePosition;
        }

        set
        {
            _currentGenePosition = value % LengthGenome;
        }
    }

    #endregion

    [DataMember]
    public int[] genome = new int[LengthGenome];

    public int[] GenomeMutate(int countMutations, int lastFitness, int currentFitness)
    {
        float fitnessSuccess = 1;
        if (_fitness != -1 && lastFitness >= 1)
        {
            fitnessSuccess = currentFitness / lastFitness;
        }

        int[] mutateGenome = (int[])genome.Clone();

        if (fitnessSuccess < 0.8f)
        {
            if (lastMutation != null)
            {
                mutateGenome[lastMutation.Y] = lastMutation.X;
            }
        }

        for (int i = 0; i < countMutations; i++)
        {
            int newGen = Data.Rnd.Next(0, Bug.MasBugCommands.Length + 1);
            if (newGen == Bug.MasBugCommands.Length)
            {
                newGen = Data.Rnd.Next(Bug.MasBugCommands.Length, mutateGenome.Length);
            }

            if (fitnessSuccess > 1.2f && lastMutation != null)
            {
                lastMutation.Y = Data.Rnd.Next(
                    Math.Max(0, lastMutation.Y - 6),
                    Math.Min(LengthGenome, lastMutation.Y + 6));
                mutateGenome[lastMutation.Y] = newGen;
                lastMutation.X = newGen;
            }
            else
            {
                lastMutation = new Coordinates();
                lastMutation.Y = Data.Rnd.Next(0, LengthGenome);
                mutateGenome[lastMutation.Y] = newGen;
                lastMutation.X = newGen;
            }
        }

        CurrentGenePosition = 0;
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

        _fitness = -1;
        lastMutation = null;
    }

    public Genome(Genome genePool, int currentFitness)
    {
        CurrentGenePosition = 0;
        genome = genePool.GenomeMutate(Data.Rnd.Next(0, 2), genePool._fitness, currentFitness);
        _fitness = currentFitness;
    }

    #endregion
}