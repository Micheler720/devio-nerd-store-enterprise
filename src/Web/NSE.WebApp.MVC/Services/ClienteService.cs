using Microsoft.Extensions.Options;
using NSE.Core.Comunication;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;
        public ClienteService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ClientesUrl);
        }

        public async Task<EnderecoViewModel> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("cliente/endereco");

            if(response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<EnderecoViewModel>(response);
        }
        public async Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco)
        {
            var response = await _httpClient.PostAsync("cliente/endereco", ObterConteudo(endereco));
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }
    }
}
