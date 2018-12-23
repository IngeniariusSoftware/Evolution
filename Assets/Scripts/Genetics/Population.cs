namespace Genetics
{
    using System.Collections.Generic;

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
        public int SelectionLimit { get; }

        /// <summary>
        /// Количество детей у особи
        /// </summary>
        public int ChildrenCount { get; }

        /// <summary>
        /// Создание популяции
        /// </summary>
        /// <param name="selectionLimit"> Количество особей для дальнейшего отбора </param>
        /// <param name="childrenCount"> Количество детей у особи </param>
        /// <param name="genome"> Геном, по которому будут создаваться особи </param>
        public Population(int selectionLimit, int childrenCount, Genome genome = null)
        {
            Creatures = new List<Genome>(selectionLimit * childrenCount);
            SelectionLimit = selectionLimit;
            ChildrenCount = childrenCount;
            if (genome != null)
            {
                for (int i = 0; i < selectionLimit * childrenCount; i++)
                {
                    Creatures.Add(new Genome(genome.Length, genome.ValueRange.min, genome.ValueRange.max));
                }
            }
        }
    }
}