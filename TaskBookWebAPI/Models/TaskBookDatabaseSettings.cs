namespace TaskBookWebAPI.Models
{
    public class TaskBookDatabaseSettings : ITaskBookDatabaseSettings
    {
        public string TaskListsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ITaskBookDatabaseSettings
    {
        string TaskListsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
