using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using ResilientServices.Dtos;

namespace ResilientServices.Services
{
    [Headers("Accept: application/json")]
    public interface IPlaceholderConfApi
    {
	    [Get("/photos")]
	    Task<List<PhotoDto>> GetPhotos();

	    [Get("/photos/{id}")]
	    Task<PhotoDto> GetPhoto(int id);
    }
}