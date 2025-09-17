using FlagsX0.Business.UseCases.Flags;

namespace FlagsX0.Business.UseCases;

public record class FlagsUseCases(
    AddFlagUseCase Add,
    GetPaginatedFlagsUseCase GetPaginated,
    GetSingleFlagUseCase Get,
    UpdateFlagUseCase Update,
    DeleteFlagUseCase Delete
);