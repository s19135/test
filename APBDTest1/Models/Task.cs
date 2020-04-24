namespace APBDTest1.Models
{
    public class Task
    {
        public string IdTask { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeadLine { get; set; }
        public string TeamMemberName { get; set; }

        public Task(string idTask, string name, string description, string deadLine, string teamMemberName)
        {
            IdTask = idTask;
            Name = name;
            Description = description;
            DeadLine = deadLine;
            TeamMemberName = teamMemberName;
        }

    }
}