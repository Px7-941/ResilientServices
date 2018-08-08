using System.Collections.Generic;
using System.Threading.Tasks;
using Fusillade;
using ResilientServices.Dtos;

namespace ResilientServices.Services
{
    public interface IPhotosService
	{
		Task<List<PhotoDto>> GetPhotos(Priority priority);
		Task<PhotoDto> GetPhoto(Priority priority, int id);
	}
}