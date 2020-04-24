namespace APBDTest1.Services
{
    public interface IDbService
    {
        public TeamMemberData GetTeamMemberData(string index);

        public bool DeleteProject(string index);
    }
}