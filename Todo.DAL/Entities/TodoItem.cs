namespace Todo.DAL.Entities
{
    public class TodoItem : EntityBase<long>
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public string Secret { get; set; }
    }
}
