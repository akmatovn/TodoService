namespace Todo.BLL.Models
{
    public class ToDoItemBusinessModel
    {
        /// <summary>
        /// Идентификационный номер
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Отметка: исполнено или нет
        /// </summary>
        public bool IsComplete { get; set; }
        public string Secret { get; set; }
    }
}
