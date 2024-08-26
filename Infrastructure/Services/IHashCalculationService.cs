namespace Infrastructure.Services;

public interface IHashCalculationService
{
    string Hash(string toHash);
}