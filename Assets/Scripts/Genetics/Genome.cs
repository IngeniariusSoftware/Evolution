namespace Genetics
{
    using System;

    /// <summary>
    /// Класс, реализующий геном решения
    /// </summary>
    public class Genome
    {
        /// <summary>
        /// Поле для получения случайного числа
        /// </summary>
        private static readonly Random RandomValue = new Random();

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
                Chromosomes[i] = (int)RandomValue.Next(minValue, maxValue + 1);
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
}