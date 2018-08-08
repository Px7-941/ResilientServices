using System;
using System.Net.Http;
using Fusillade;
using Refit;

namespace ResilientServices.Services
{
    public class ApiService : IApiService
    {
	    public const string ApiBaseAddress = "https://jsonplaceholder.typicode.com";

	    public ApiService(string apiBaseAddress = null)
	    {
            IPlaceholderConfApi createClient(HttpMessageHandler messageHandler)
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress ?? ApiBaseAddress)
                };
                return RestService.For<IPlaceholderConfApi>(client);
            }

            _background = new Lazy<IPlaceholderConfApi>(() => createClient(new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.Background)));

            _userInitiated = new Lazy<IPlaceholderConfApi>(() => createClient(new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.UserInitiated)));

            _speculative = new Lazy<IPlaceholderConfApi>(() => createClient(new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.Speculative)));
        }

        private readonly Lazy<IPlaceholderConfApi> _background;
	    private readonly Lazy<IPlaceholderConfApi> _userInitiated;
	    private readonly Lazy<IPlaceholderConfApi> _speculative;

	    public IPlaceholderConfApi Background
	    {
		    get { return _background.Value; }
	    }

	    public IPlaceholderConfApi UserInitiated
	    {
		    get { return _userInitiated.Value; }
	    }

	    public IPlaceholderConfApi Speculative
	    {
		    get { return _speculative.Value; }
	    }
    }
}