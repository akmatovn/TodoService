namespace Todo.DAL.Entities
{
    public interface IEntityBase<TKey> : IEntityBase
    {
        TKey Id { get; set; }
    }

    public interface IEntityBase { }
}
