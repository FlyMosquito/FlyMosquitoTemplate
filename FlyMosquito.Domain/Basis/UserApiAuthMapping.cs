using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyMosquito.Domain.Basis
{
    public class UserApiAuthMapping :BaseModel<int>
    {
        public int UserId { get; set; }

        public int ApiAuthId { get; set; }

        public int IsAllowed { get; set; }
    }
}
