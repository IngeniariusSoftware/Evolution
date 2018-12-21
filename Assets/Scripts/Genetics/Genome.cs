/// <summary>
/// Класс для работы с генетическими алгоритмами
/// </summary>
namespace Genetics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Класс для работы с популяцией решений
    /// </summary>
    public class Population
    {
        /// <summary>
        /// Текущая популяция особей
        /// </summary>
        public List<Genome> Creatures { get; }

        /// <summary>
        /// Текущее размер популяции
        /// </summary>
        public int Count => Creatures.Count;

        /// <summary>
        /// Количество особей для дальнейшего отбора
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Количество детей у особи
        /// </summary>
        public int ChildrenCount { get; }
    }

    /// <summary>
    /// Класс, реализующий геном решения
    /// </summary>
    public class Genome
    {
        /// <summary>
        /// Хромосомы решения, каждая содержит параметр для оптимизации решения
        /// </summary>
        public int[] Chromosomes { get; }

        /// <summary>
        /// Диапозон значений параметров решения
        /// </summary>
        public (int min, int max) ValueRange { get; }

        /// <summary>
        /// Количество хромосом в гене
        /// </summary>
        public int Length => Chromosomes.Length;

        /// <summary>
        /// Успешность генома
        /// </summary>
        public int Fitness { get; set; }

        /// <summary>
        /// Создание генома
        /// </summary>
        /// <param name="length"> Количество хромосом генома </param>
        /// <param name="minValue"> Минимальное значение хромосомы </param>
        /// <param name="maxValue"> Максимальное значение хромосомы </param>
        public Genome(int length, int minValue, int maxValue)
        {
            Chromosomes = new int[length > 0 ? length : 1];
            ValueRange = (minValue, maxValue);
            Fitness = 0;
            for (int i = 0; i < length; i++)
            {
                Chromosomes[i] = (int)Random.Range(minValue, maxValue);
            }
        }

        /// <summary>
        /// Создание клона решения
        /// </summary>
        /// <param name="genome"> Геном, который необходимо клонировать </param>
        public Genome(Genome genome)
        {
            Chromosomes = (int[])genome.Chromosomes.Clone();
            ValueRange = genome.ValueRange;
            Fitness = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Mutator
    {

    }
}
