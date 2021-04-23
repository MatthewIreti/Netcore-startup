using AutoMapper;

namespace DPRHSE.Business.Services
{
    public interface IAccountService
    {
        string TestMethod();
    }
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        public AccountService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string TestMethod()
        {
            return "Hello world";
        }
    }
}
