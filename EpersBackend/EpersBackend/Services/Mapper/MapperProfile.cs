using AutoMapper;
using Epers.Models.Obiectiv;
using Epers.Models.PIP;
using Epers.Models.Users;

namespace EpersBackend.Services.Mapper
{
	public class MapperProfile: Profile
	{
		public MapperProfile()
		{
			CreateMap<UserCreateModel, User>();
            CreateMap<UserEditModel, User>();
            CreateMap<User, UserEditModel>();

            CreateMap<ObiectivTemplate, Obiective>();

            CreateMap<PipDisplayAddEditModel, PlanInbunatatirePerformante>();
            CreateMap<PlanInbunatatirePerformante, PipDisplayAddEditModel>();

        }

    }
}

