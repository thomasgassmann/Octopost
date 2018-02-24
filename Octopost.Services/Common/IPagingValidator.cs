namespace Octopost.Services.Common
{
    public interface IPagingValidator
    {
        void ThrowIfPageOutOfRange(int pageSize, int page);
    }
}
