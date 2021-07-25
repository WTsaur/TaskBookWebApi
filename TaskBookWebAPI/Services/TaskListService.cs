using System;
using System.Collections.Generic;
using MongoDB.Driver;
using TaskBook.Models;
using TaskBookWebAPI.Models;
using System.Linq;

namespace TaskBookWebAPI.Services
{
    public class TaskListService
    {
        private readonly IMongoCollection<TaskList> _tasklists;

        public TaskListService(ITaskBookDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _tasklists = database.GetCollection<TaskList>(settings.TaskListsCollectionName);
        }

        public List<TaskList> Get() =>
            _tasklists.Find(t => true).ToList();

        public TaskList Get(Guid id) =>
            _tasklists.Find<TaskList>(tasklist => tasklist.Id == id).FirstOrDefault();

        public TaskList AddOrUpdate(TaskList tasklistIn)
        {
            var tasklist = _tasklists.Find<TaskList>(t => t.Id == tasklistIn.Id).FirstOrDefault();
            if (tasklist != default(TaskList))
            {
                _tasklists.ReplaceOne(t => t.Id == tasklistIn.Id, tasklistIn);
            }
            else
            {
                _tasklists.InsertOne(tasklistIn);
            }
            return tasklist;
        }

        public Task AddOrUpdateTask(Guid id, Task taskIn)
        {
            var tasklist = _tasklists.Find<TaskList>(t => t.Id == id).FirstOrDefault();
            if (tasklist != default(TaskList))
            {
                var filter = Builders<TaskList>.Filter.Where(list => list.Id == id);
                UpdateDefinition<TaskList> update;
                Task task = tasklist.Tasks.Where(t => t.Id == taskIn.Id).FirstOrDefault();
                int idx = tasklist.Tasks.IndexOf(task);
                if (task != default(Task))
                {
                    update = Builders<TaskList>.Update.Set(list => list.Tasks.ElementAt(idx), taskIn);
                }
                else
                {
                    update = Builders<TaskList>.Update.Push(list => list.Tasks, taskIn);
                }
                _tasklists.UpdateOneAsync(filter, update);
            }
            return taskIn;
        }

        public Appointment AddOrUpdateAppointment(Guid id, Appointment apptIn)
        {
            var tasklist = _tasklists.Find<TaskList>(t => t.Id == id).FirstOrDefault();
            if (tasklist != default(TaskList))
            {
                var filter = Builders<TaskList>.Filter.Where(list => list.Id == id);
                UpdateDefinition<TaskList> update;
                Appointment appt = tasklist.Appointments.Where(t => t.Id == apptIn.Id).FirstOrDefault();
                int idx = tasklist.Appointments.IndexOf(appt);
                if (appt != default(Appointment))
                {
                    update = Builders<TaskList>.Update.Set(list => list.Appointments.ElementAt(idx), apptIn);
                }
                else
                {
                    update = Builders<TaskList>.Update.Push(list => list.Appointments, apptIn);
                }
                _tasklists.UpdateOneAsync(filter, update);
            }
            return apptIn;
        }

        public Task RemoveTask(Guid id, Task taskIn)
        {
            var filter = Builders<TaskList>.Filter.Where(list => list.Id == id);
            var update = Builders<TaskList>.Update.PullFilter(list => list.Tasks, Builders<Task>.Filter.Where(task => task.Id == taskIn.Id));
            _tasklists.UpdateOneAsync(filter, update);
            return taskIn;
        }

        public Appointment RemoveAppointment(Guid id, Appointment apptIn)
        {
            var filter = Builders<TaskList>.Filter.Where(list => list.Id == id);
            UpdateDefinition<TaskList> update;
            update = Builders<TaskList>.Update.Pull(list => list.Appointments, apptIn);
            _tasklists.UpdateOneAsync(filter, update);
            return apptIn;
        }

        public DeleteResult Remove(TaskList tasklistIn) =>
            _tasklists.DeleteOne(tasklist => tasklist.Id == tasklistIn.Id);

        public DeleteResult Remove(Guid Id) =>
            _tasklists.DeleteOne(tasklist => tasklist.Id == Id);

        public List<TaskList> Search(string name) =>
            _tasklists.Find(tasklist => tasklist.Name.Contains(name)).ToList();
    }
}
