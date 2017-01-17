namespace ProjectSS.Web.Models.admin
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
