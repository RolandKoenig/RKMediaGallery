using System;

namespace RKMediaGallery.ViewServices;

public interface IUseCaseViewService
{
    IDisposable GetScopedUseCase<TUseCase>(out TUseCase useCase)
        where TUseCase : notnull;
    
    IDisposable GetScopedUseCase<TUseCase1, TUseCase2>(out TUseCase1 useCase1, out TUseCase2 useCase2)
        where TUseCase1 : notnull
        where TUseCase2 : notnull;
}