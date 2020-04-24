using System.Collections.Generic;
using System.Data.SqlClient;
using APBDTest1.Models;
using APBDTest1.Services;

namespace APBDTest1.DAL
{
    public class DbService : IDbService
    {
        private const string CONNECTION_STRING = "Data Source=db-mssql;Initial Catalog=s19135;Integrated Security=True";
        
        public TeamMemberData GetTeamMemberData(string index)
        {
            var data = new TeamMemberData();
            data.Tasks = new List<Task>();

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();

                    command.CommandText = "SELECT IdTeamMember, FirstName, LastName, Email" +
                        " FROM teammember WHERE IdTeamMember = @Id";
                    command.Parameters.AddWithValue("@Id", index);

                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        return null;
                    }
                    data.IdTeamMember = reader["IdTeamMember"].ToString();
                    data.FirstName = reader["FirstName"].ToString();
                    data.LastName = reader["LastName"].ToString();
                    data.Email = reader["Email"].ToString();

                    command.CommandText = "SELECT t.Idtask, t.Name, t.Description, t.Deadline, tm.LastName " +
                                          "FROM Task t " +
                                          "JOIN TeamMemeber tm ON t.IdAssignedTo = tm.IdTeamMember " +
                                          "WHERE IdTeamMember = @IdTeamMember" +
                                          "ORDER BY 4 DESC;";
                    command.Parameters.AddWithValue("@IdTeamMember", index);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        data.Tasks.Add(new Task(
                            reader["IdTask"].ToString(),
                            reader["Name"].ToString(),
                            reader["Description"].ToString(),
                            reader["Deadline"].ToString(),
                            reader["LastName"].ToString()
                        ));
                    }
                }
            }
            return data;
        }
        
        public bool DeleteProject(string index)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand())
                {
                    var transaction = connection.BeginTransaction();
                    try
                    {
                        command.Connection = connection;
                        connection.Open();

                        command.CommandText = "SELECT IdProject " +
                                              "FROM Project " + 
                                              "WHERE IdTeam = @IdTeam";
                        command.Parameters.AddWithValue("@IdTeam", index);
                        var reader = command.ExecuteReader();
                        if (!reader.Read()) return false;
                        reader.Close();
                        
                        command.Transaction = transaction;

                    
                        command.CommandText = "Delete From Task " +
                                              "Where IdTask IN ("+
                                              "Select IdTask " +
                                              "where IdTeam = @IdTeam)";
                        command.Parameters.AddWithValue("@IdTeam", index);
                        command.ExecuteNonQuery();

                    
                        command.CommandText = "Delete From Project " +
                                              "Where IdTeam = @IdTeam";
                        command.Parameters.AddWithValue("@IdTeam", index);
                        command.ExecuteNonQuery();
                    
                        transaction.Commit();
                    }
                    catch (SqlException e)
                    {
                        transaction.Rollback();
                        return false;
                    }
                   
                }
            }
            return true;
        }
    }
}
