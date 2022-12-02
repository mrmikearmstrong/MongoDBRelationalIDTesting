using Microsoft.VisualBasic;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("MongoDB Relational Testing");

        // connect
        var client = new MongoClient("mongodb://localhost:27018");

        // get DB
        Console.WriteLine("Getting DB");
        var chronosDB = client.GetDatabase("ChronosDatabase");

        // get collections
        Console.WriteLine("Getting Collections");
        var projectsCol = chronosDB.GetCollection<DBPrj>("projects");
        var tasksCol = chronosDB.GetCollection<DBTsk>("tasks");


        /*
        // add some tasks to the task collection and add them to the project
        for (int i = 1; i <= 10; i++) {

            // create a task and add it to the task collection
            Console.WriteLine("Creating a task and adding it to the task collection...");

            // creates new task
            DBTsk task = new();
            task.id = ObjectId.GenerateNewId();     // here generate a new ID for the object
            task.Name = "Task" + i.ToString();
            await tasksCol.InsertOneAsync(task);
            Console.WriteLine(task.Name + " has ID: " + task.id.ToString());

            // take the id of the created task and add it to the associatedTasks array of project0
            var filter = Builders<DBPrj>.Filter.Eq(e => e.Name, "Project0");
            var update = Builders<DBPrj>.Update.Push<ObjectId>(e => e.associatedTasks, task.id);
            await projectsCol.FindOneAndUpdateAsync(filter, update);

            Console.WriteLine("");

        }
        */

        // read all the tasks associated with a project
        var filter = Builders<DBPrj>.Filter.Eq("Name", "Project0");
        var project0 = projectsCol.Find(filter).FirstOrDefault();

        // print the collection
        Console.WriteLine(project0.ToJson().ToString());

    }
}

public class DBPrj {

    // id
    [BsonId]
    public ObjectId id { get; set; }

    // info
    public string Name { get; set; } = "Project0";
    public List<ObjectId> associatedTasks { get; set; } = new List<ObjectId>();

}

public class DBTsk {

    // id
    [BsonId]
    public ObjectId id { get; set; }

    // info
    public string Name { get; set; } = "Task0";

}