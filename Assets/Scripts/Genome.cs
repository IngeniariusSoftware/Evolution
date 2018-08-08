using System;

public class Genome
{
    public int[] genome = new int[Data.LengthGenome];

    public int[] GenomeMutate(int countMutation)
    {
        int[] mutateGenome = (int[])genome.Clone();

        for (int i = 0; i < countMutation; i++)
        {
            mutateGenome[Data.Rnd.Next(0, mutateGenome.Length)] = Data.Rnd.Next(0, Data.LengthGenome);
        }

        return mutateGenome;
    }

    public Genome()
    {
        for (int i = 0; i < genome.Length; i++)
        {
            genome[i] = Data.Rnd.Next(0, Data.LengthGenome);
        }
    }

    public Genome(int[] newGenome)
    {
        genome = newGenome;
    }
}


