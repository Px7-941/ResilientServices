using System;
using System.Net.Http;
using Fusillade;
using Refit;
using Splat;

namespace ResilientServices.Services
{
    public class ApiService : IApiService
    {
        public const string ApiBaseAddress = "https://jsonplaceholder.typicode.com";

        public ApiService(string apiBaseAddress = null, HttpMessageHandler handler = null)
        {
            handler = handler ?? Locator.Current.GetService<HttpMessageHandler>();
            IPlaceholderConfApi createClient(HttpMessageHandler messageHandler)
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress ?? ApiBaseAddress)
                };
                return RestService.For<IPlaceholderConfApi>(client);
            }

            _background = new Lazy<IPlaceholderConfApi>(() => createClient(new RateLimitedHttpMessageHandler(handler, Priority.Background)));
            _userInitiated = new Lazy<IPlaceholderConfApi>(() => createClient(new RateLimitedHttpMessageHandler(handler, Priority.UserInitiated)));
            _speculative = new Lazy<IPlaceholderConfApi>(() => createClient(new RateLimitedHttpMessageHandler(handler, Priority.Speculative)));
        }

        private readonly Lazy<IPlaceholderConfApi> _background;
        private readonly Lazy<IPlaceholderConfApi> _userInitiated;
        private readonly Lazy<IPlaceholderConfApi> _speculative;

        public IPlaceholderConfApi Background => _background.Value;
        public IPlaceholderConfApi UserInitiated => _userInitiated.Value;
        public IPlaceholderConfApi Speculative => _speculative.Value;
    }
}