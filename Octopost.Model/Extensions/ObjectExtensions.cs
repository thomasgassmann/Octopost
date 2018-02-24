namespace Octopost.Model.Extensions
{
    using AutoMapper;

    public static class ObjectExtensions
    {
        public static TResult MapTo<TResult>(this object obj) =>
            Mapper.Map<TResult>(obj);
    }
}
