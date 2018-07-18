using BareboneUi.Pages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BareboneUi.Common
{
    public class ModelRepository : ILoadModel, ISaveModel
    {
        private readonly IConfiguration _configuration;
        protected IApiClient ApiClient { get; }

        public ModelRepository(IApiClient apiClient, IConfiguration configuration)
        {
            ApiClient = apiClient;
            _configuration = configuration;
        }

        public async Task<TModel> Load<TResource, TModel>(string url)
        {
            var resource = await ApiClient.GetAsync<TResource>(url);

            var parameters = new List<object> {resource};

            if (typeof(TModel).GetConstructor(new [] {typeof(Resource), typeof(IConfiguration)}) != null)
            {
                parameters.Add(_configuration);
            }

            return (TModel)Activator.CreateInstance(typeof(TModel), parameters.ToArray());
        }

        public async Task<IResponse> Save(ISaveable model)
        {
            var resource = model.Resource;
            return new Response(await ApiClient.SendAsync(resource));
        }
    }
}