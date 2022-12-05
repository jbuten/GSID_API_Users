
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Users
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    public interface IUsersContext
    {
        Task<User> GetUser(string id);
        Task<bool> CheckApp(string app);
        Task<List<UserListItem>> ListUsersFilter(string company, string like);
        Task<User> UserUpdate(User usr);
    }

    public partial class UsersContext : IUsersContext
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<User> _Users;

        public UsersContext(IUsersSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase("atlas");
            _Users = database.GetCollection<User>("users");
        }

        public async Task<User> GetUser(string id)
        {
            string json = "{ $or: [ {\"Username\": \"" + id + "\"} ,{  \"SAMAccountName\" : /^" + id + "/i } , {PostOfficeBox : \"" + id + "\"}   ] }";
            Console.WriteLine(json);
            return await _Users.Find(json).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckApp(string app) {
            
            var cursor = await database.GetCollection<dynamic>("applications").
                    FindAsync(Builders<dynamic>.Filter.Eq("_id", app));

            return cursor.Any();

        }

        public async Task<List<UserListItem>> ListUsersFilter(string company, string like)
        {
            List<UserListItem> data = new List<UserListItem>();

            List<string> par = new List<string>();

            par.Add($" 'Company': '{company}' ");
            par.Add(" 'CustomerName': /" + like + "/i ");
            par.Add($" 'Enabled': true ");

            string json = "{" + string.Join(",", par) + "}";

            System.Console.WriteLine(json);

            BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(json);

            data = await _Users.Find(document)
                .Sort("{DisplayName: 1}")
                .Project(x => new
                UserListItem
                {
                    UserName = x.Username,
                    DisplayName = x.DisplayName.Trim(),
                    Title = x.Title,
                    PhotoPath = x.PhotoPath
                }).ToListAsync();

            return data;
        }

        public async Task<User> UserUpdate(User usr)
        {
            var filter = Builders<User>.Filter.Eq(a => a.Username, usr.Username);

            var reg = await _Users.Find(filter).FirstOrDefaultAsync();

            if (reg != null) { usr.Id = reg.Id; usr.Rols = reg.Rols; }

            await _Users.ReplaceOneAsync(filter, usr, new ReplaceOptions { IsUpsert = true });

            return usr;
        }

    }

}
