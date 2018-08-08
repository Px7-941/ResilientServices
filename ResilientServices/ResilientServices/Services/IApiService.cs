namespace ResilientServices.Services
{
	public interface IApiService
	{
		IPlaceholderConfApi Speculative { get; }
		IPlaceholderConfApi UserInitiated { get; }
		IPlaceholderConfApi Background { get; }
	}
}