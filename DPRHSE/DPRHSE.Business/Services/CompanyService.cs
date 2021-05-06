using AutoMapper;
using DPRHSE.Common.Models;
using DPRHSE.Common.Models.ViewModel;
using DPRHSE.WebAPI.Common;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPRHSE.Business.Services
{
    public interface ICompanyService
    {
        Task<Response<IReadOnlyList<CompanyViewModel>>> GetAllCompanies(int page, int size);
    }
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly AppSettings appSettings;
        private readonly IAuthService _authService;
        private readonly string[] allowedColumn;
        public CompanyService(IMapper mapper, AppSettings appSettings, IAuthService authService)
        {
            _mapper = mapper;
            this.appSettings = appSettings;
            allowedColumn = new string[] { "CustomerAccount", "OrganizationName", "NameAlias", "CustomerGroupId", "OgispNumber", "CompanyPrefix", "FourDigitName" };
            _authService = authService;
        }


        public async Task<Response<IReadOnlyList<CompanyViewModel>>> GetAllCompanies(int page, int size)
        {
            Response<IReadOnlyList<CompanyViewModel>> item;
            try
            {
                int skip = size * (page - 1);
                var authResponse = await _authService.GetToken();
                if (!authResponse.Status)
                {
                    item =  Response<IReadOnlyList<CompanyViewModel>>.TokenFailure();
                }

                string url = $"{appSettings.DynamicsBaseUrl}/CustomersV2?$count=true&$top={size}&$skip={skip}&$select = {string.Join(',', allowedColumn)}";
                var response = await url.WithOAuthBearerToken($"{authResponse.Data.AccessToken}")
                  .GetJsonAsync<BaseResponse<List<CompanyViewModel>>>();

                item =   Response<IReadOnlyList<CompanyViewModel>>.Success(response.Value, response.TotalCount);

            }
            catch (System.Exception ex)
            {
                item = Response<IReadOnlyList<CompanyViewModel>>.Failed(ex.Message);
            }

            return item;

        }

        
    }
}
