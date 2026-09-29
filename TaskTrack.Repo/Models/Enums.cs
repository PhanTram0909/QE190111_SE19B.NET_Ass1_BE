namespace TaskTrack.Repo.Models
{
    public enum ProjectStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        OnHold = 3
    }

    public enum TaskStatusEnum
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2,
        Cancelled = 3
    }

    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }
}
