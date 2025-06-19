using FluentValidation;
using MamaFit.BusinessObjects.DTO.BranchDto;

namespace MamaFit.Services.Validator.Branch
{
    public class BranchRequestValiator : AbstractValidator<BranchCreateDto>
    {
        public BranchRequestValiator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");

            RuleFor(x => x.BranchManagerId)
                .NotEmpty().WithMessage("BranchManagerId is required.");

            RuleFor(x => x.OpeningHour)
                .NotEmpty().WithMessage("Opening hour is required.");
        }
    }
}
