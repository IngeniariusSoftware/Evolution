using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    /// <summary>
    /// Класс, в котором хранится коллекция жуков
    /// </summary>
    public class BugCollection
    {
        public BugCollection()
        {
            Bugs = new List<Bug>();
        }

        /// <summary>
        /// Конструктор выполняющий полное копирование прилетевших значений
        /// </summary>
        /// <param name="inBugs"></param>
        public BugCollection(IList<Bug> inBugs)
        {
            Bugs = inBugs.Select(x=>x).ToList();
        }
        public IList<Bug> Bugs { get; set; }
        /// <summary>
        /// Создает новое поколение жуков
        /// </summary>
        public void CreateNewGeneration()
        {

        }
        /// <summary>
        /// Начать действие над коллекцией жуков
        /// </summary>
        public void StartExecution()
        {
            foreach (var bug in Bugs)
            {
                bug.StartAction();
            }
        }
    }
}
