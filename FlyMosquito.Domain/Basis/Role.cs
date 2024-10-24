#region using
#endregion

namespace FlyMosquito.Domain
{
    public class Role : BaseModel<int>
    {
        public string RoleName { get; set; }
    }
}
