using Epers.Models.PIP;

namespace EpersBackend.Services.PIP
{
	public interface IEfPIPRepository
	{
        PlanInbunatatirePerformante Get(int id);
        void Update(PlanInbunatatirePerformante pip);
        void Add(PlanInbunatatirePerformante pip);
    }
}

