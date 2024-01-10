using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkintechRestApiDemo.Business;
using WorkintechRestApiDemo.Domain;
using WorkintechRestApiDemo.Domain.ApiLayer;
using WorkintechRestApiDemo.Domain.Exceptions;

namespace WorkintechRestApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        protected readonly ICurrencyService currencyService;

        public CurrencyController(ICurrencyService _currencyService)
        {
            currencyService = _currencyService;
        }

        [Authorize(Roles ="admin,user")]
        [HttpGet(Name = "GetCurrency")]
        public async Task<CurrencyResponse> Get()
        {

           return await currencyService.GetCurrency();
        }

        [Authorize(Roles="user")]
        [HttpPost(Name ="GetCurrencyFromApiLayer")]
        public async Task<object> Post(string startDate, string endDate)
        {
            ApiLayerResponse response =  await currencyService.PostCurrencyToApiLayer(startDate, endDate);
            if(!response.success)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest("Hata oluştu");
            }
            return response;
        }
    }
}
