#region using
#endregion

namespace FlyMosquito.Domain
{
    public class UserRoleMapping : BaseModel<int>
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
