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

//public class Genome : IEnumerable,ICloneable
    //{
    //    public int[] genome;


    //    public List<int> genome { get; set; }

    //    public IEnumerator GetEnumerator()
    //    {
    //        foreach (var gene in genome) yield return gene;
    //    }

    //    /// <summary>
    //    /// Реализация IClonable для глубокого копирования
    //    /// </summary>
    //    /// <returns></returns>
    //    public object Clone()
    //    {
    //        return new List<int>(genome);
    //    }
    //    /// <summary>
    //    /// Клонирование генома в виде List<int>, чтобы не выполнялось клонирование по ссылке 
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<int> Clone(bool flag)
    //    {
    //        return new List<int>(genome); 
    //    }
    //}
