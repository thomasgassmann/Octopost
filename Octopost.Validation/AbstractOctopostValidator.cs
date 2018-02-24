namespace Octopost.Validation
{
    using FluentValidation;

    public abstract class AbstractOctopostValidator<T> : AbstractValidator<T>
    {
        public AbstractOctopostValidator() =>
            this.Initalize();

        protected abstract void Initalize();
    }
}
