using FluentValidation;
using HiveWear.Application.Clothing.Commands;

namespace HiveWear.Application.Clothing.Validators
{
    public class AddClothingItemCommandValidator : AbstractValidator<AddClothingItemCommand>
    {
        public AddClothingItemCommandValidator()
        {
            RuleFor(x => x.ClothingItem)
                .NotNull()
                .WithMessage("Clothing item is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ClothingItem!.Name)
                        .NotEmpty()
                        .WithMessage("Name is required");

                    RuleFor(x => x.ClothingItem!.Description)
                        .NotEmpty()
                        .WithMessage("Description is required");

                    RuleFor(x => x.ClothingItem!.Category)
                        .NotEmpty()
                        .WithMessage("Category is required");

                    RuleFor(x => x.ClothingItem!.Color)
                        .NotEmpty()
                        .WithMessage("Color is required");

                    RuleFor(x => x.ClothingItem!.Size)
                        .NotEmpty()
                        .WithMessage("Size is required");

                    RuleFor(x => x.ClothingItem!.Brand)
                        .NotEmpty()
                        .WithMessage("Brand is required");

                    RuleFor(x => x.ClothingItem!.Season)
                        .NotNull()
                        .WithMessage("Season is required");
                });
        }
    }
}
