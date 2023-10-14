using MongoDB.Entities;
using SearchService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchService.Services
{
    public class AuctionSvcHttpClient
    {
        private readonly IConfiguration _config ;
        private readonly HttpClient _httpClient;

        public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _config = config;
            _httpClient = httpClient;
            
        }
        public async Task<List<Item>> GetItemsSearchDb()
        {
            var lastUpdated = await DB.Find<Item, string>()
                                .Sort(x => x.Descending(x =>x.UpdatedAt))
                                .Project(x => x.UpdatedAt.ToString())
                                .ExecuteFirstAsync();

            string url = $"{_config["AuctionServiceUrl"]}/api/auctions?date={lastUpdated}";
            return await _httpClient.GetFromJsonAsync<List<Item>>(url);
        }
    }
}