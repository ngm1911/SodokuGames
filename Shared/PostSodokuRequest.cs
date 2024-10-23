using FluentValidation;

namespace BlazorSodokuApp.Shared
{
    public record PostSodokuRequest(string Board);

    public class PostSodokuRequestValidator : AbstractValidator<PostSodokuRequest>
    {
        public PostSodokuRequestValidator()
        {
            RuleFor(x => x.Board)
                .NotEmpty()
                .Length(81);
        }
    }
}
