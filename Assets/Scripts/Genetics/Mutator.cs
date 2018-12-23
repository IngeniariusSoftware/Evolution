namespace Genetics
{
    using System;

    /// <summary>
    /// Класс для создания следующих поколений
    /// </summary>
    public class Mutator
    {
        /// <summary>
        /// Поле для получения случайного числа
        /// </summary>
        private static readonly Random RandomValue = new Random();

        /// <summary>
        /// Создание нового экземпляра мутатора для генерации новых поколений
        /// </summary>
        /// <param name="crossoverChance"> Шанс спаривания особей, иначе происходит клонирование</param>
        /// <param name="mutationChance"> Шанс возникновения мутаций в популяции </param>
        /// <param name="inversionChance"> Шанс возникновения инверсий в популяции </param>
        /// <remarks>
        /// Шансы традиционно указываются от 0 (0%) до 1 (100%)
        /// Однако, для шанса мутаций и инверсий можно установить сколь уголно большой шанс
        /// Например, шанс мутаций в 2 (200%) означает,  что в среднем каждая особь популяции получит две мутации
        /// </remarks>
        public Mutator(float crossoverChance, float mutationChance, float inversionChance)
        {
            CrossoverChance = crossoverChance <= 1 ? crossoverChance / 2.0f : 1;
            MutationChance = mutationChance >= 0 ? mutationChance : 0;
            InversionChance = inversionChance >= 0 ? inversionChance : 0;
        }

        /// <summary>
        /// Шанс мутациии генома
        /// </summary>
        public float MutationChance { get; }

        /// <summary>
        /// Шанс инверсии генома
        /// </summary>
        public float InversionChance { get; }

        /// <summary>
        /// Шанс спаривания особей
        /// </summary>
        public float CrossoverChance { get; }

        /// <summary>
        /// Сгенерировать следующее поколение c использованием кроссинговера, мутаций, инверсий и клонирования
        /// </summary>
        public void Evaluate(Population parents)
        {
            var children = new Population(parents.SelectionLimit, parents.ChildrenCount);
            WeedOut(parents);
            PopulationCrossover(parents, children);
            PopulationClone(parents, children);
            MutatePopulation(children);
            InversePopulation(children);
            parents.Creatures.Clear();
            parents.Creatures.AddRange(children.Creatures);
        }

        /// <summary>
        /// Отсеивает плохие решения, оставляя лишь лучшую часть популяции
        /// </summary>
        /// <param name="population"> Популяция, в которой будет проиходить отсеивание </param>
        public void WeedOut(Population population)
        {
            population.Creatures.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
            population.Creatures.RemoveRange(0, population.Count - population.SelectionLimit);
        }

        /// <summary>
        /// Клонирование родительских особей в следующую популяцию
        /// </summary>
        /// <param name="parents"> Предыдущая популяция (родители), из которой будем клонировать решения </param>
        /// <param name="children"> Следующая популяция (дети), в которую будем клонировать предыдущие решения </param>
        private void PopulationClone(Population parents, Population children)
        {
            int cloneCount = children.Creatures.Capacity - children.Creatures.Count;
            for (int i = 0; i < cloneCount; i++)
            {
                children.Creatures.Add(new Genome(parents.Creatures[RandomValue.Next(0, parents.Count)]));
            }
        }

        /// <summary>
        /// Метод для добавления в следующую популяцию особей, полученных методом кроссинговера
        /// </summary>
        /// <param name="parents"> Предыдущая популяция (родители), из которой генерируем детей </param>
        /// <param name="children"> Следующая популяция (дети) </param>
        private void PopulationCrossover(Population parents, Population children)
        {
            for (int i = 0; i < children.Creatures.Capacity * CrossoverChance; i++)
            {
                // Получаем двух потомков от кроссинговера двух случайных родителей
                var descendents = GeneCrossover(
                    parents.Creatures[RandomValue.Next(0, parents.Count)],
                    parents.Creatures[RandomValue.Next(0, parents.Count)]);
                children.Creatures.Add(descendents.children1);
                children.Creatures.Add(descendents.children2);
            }
        }

        /// <summary>
        /// Метод, реализующий скрещивание геномов с помощью кроссинговера
        /// </summary>
        /// <param name="partner1"> Геном первого партнера </param>
        /// <param name="partner2"> Геном второго партнера </param>
        /// <returns></returns>
        private (Genome children1, Genome children2) GeneCrossover(Genome partner1, Genome partner2)
        {
            Genome children1 = new Genome(partner1);
            Genome children2 = new Genome(partner2);
            int separatorPosition = RandomValue.Next(1, children1.Length);
            for (int i = separatorPosition; i < children1.Length; i++)
            {
                int shelfGene = children1.Chromosomes[i];
                children1.Chromosomes[i] = children2.Chromosomes[i];
                children2.Chromosomes[i] = shelfGene;
            }

            return (children1, children2);
        }

        /// <summary>
        /// Метод для внесения мутаций в популяцию
        /// </summary>
        /// <param name="population"> Популяция, которая подвергается мутаций </param>
        private void MutatePopulation(Population population)
        {
            for (int i = 0; i < population.Count * MutationChance; i++)
            {
                MutateGenome(population.Creatures[RandomValue.Next(0, population.Count)]);
            }
        }

        /// <summary>
        /// Метод для внесения мутаций в геном
        /// </summary>
        /// <param name="mutationGenome"> Геном, который подвергается мутации </param>
        private void MutateGenome(Genome mutationGenome)
        {
            mutationGenome.Chromosomes[RandomValue.Next(0, mutationGenome.Length)] = RandomValue.Next(
                mutationGenome.ValueRange.min,
                mutationGenome.ValueRange.max + 1);
        }

        /// <summary>
        /// Метод для внесения инверсий в популяцию
        /// </summary>
        /// <param name="population"> Популяция, в которую вносится изменения </param>
        private void InversePopulation(Population population)
        {
            for (int i = 0; i < population.Count * InversionChance; i++)
            {
                InverseGenome(population.Creatures[RandomValue.Next(0, population.Count)]);
            }
        }

        /// <summary>
        /// Метод для внесения инверсии в геном
        /// </summary>
        /// <param name="inversionGenome"> Геном, который подвергается инверсии </param>
        private void InverseGenome(Genome inversionGenome)
        {
            int inversionStartPosition = RandomValue.Next(0, inversionGenome.Length - 1);
            int inversionEndPosition = RandomValue.Next(inversionStartPosition + 1, inversionGenome.Length);
            for (int i = inversionStartPosition, j = inversionEndPosition; i < j; i++, j--)
            {
                int geneShelf = inversionGenome.Chromosomes[i];
                inversionGenome.Chromosomes[i] = inversionGenome.Chromosomes[j];
                inversionGenome.Chromosomes[j] = geneShelf;
            }
        }
    }
}