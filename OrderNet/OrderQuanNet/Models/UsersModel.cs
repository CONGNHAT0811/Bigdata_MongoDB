using OrderQuanNet.Services;

namespace OrderQuanNet.Models
{
    public class UsersModel
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? type { get; set; }
        public int? balance { get; set; }

        public void create()
        {
            this.id = null;
            UsersService usersService = new UsersService();
            usersService.Insert(this);
        }

        public void save()
        {
            UsersService usersService = new UsersService();
            usersService.Update(this);
        }

        public void delete()
        {
            UsersService usersService = new UsersService();
            usersService.Delete(this);
        }
    }
}
