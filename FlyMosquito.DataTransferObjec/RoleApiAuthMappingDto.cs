#region using
using System.ComponentModel.DataAnnotations;
#endregion

namespace FlyMosquito.DataTransferObjec
{
    public class RoleApiAuthMappingDto
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "至少需要分配一个权限。")]
        public List<int> RoleApiAuthMappingIds { get; set; }
    }
}
