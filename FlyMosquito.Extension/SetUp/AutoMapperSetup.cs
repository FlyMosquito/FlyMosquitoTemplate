#region using
using AutoMapper;
using FlyMosquito.DataTransferObjec;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Extension.SetUp
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
