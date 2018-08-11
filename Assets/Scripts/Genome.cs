using System;

public class Genome
{
    #region Constants

    /// <summary>
    ///     Количество ячеек в геноме жука
    /// </summary>
    public static readonly int LengthGenome = 128;

    #endregion

    private int _currentGenePosition;

    public int CurrentGenePosition
    {
        get { return _currentGenePosition; }

        set { _currentGenePosition = value % Genome.LengthGenome; }
    }

    public int[] genome = new int[LengthGenome];

    public int[] GenomeMutate(int countMutation)
    {
        int[] mutateGenome = (int[])genome.Clone();

        for (int i = 0; i < countMutation; i++)
        {
            mutateGenome[Data.Rnd.Next(0, mutateGenome.Length)] = Data.Rnd.Next(0, LengthGenome);
        }

        return mutateGenome;
    }

    public int NextGenePosition(int shift = 1)
    {
        return (CurrentGenePosition + shift) % LengthGenome;
    }

    public Genome()
    {
        for (int i = 0; i < genome.Length; i++)
        {
            genome[i] = Data.Rnd.Next(0, LengthGenome);
        }
    }

    public Genome(int[] newGenome)
    {
        genome = newGenome;
    }
}


