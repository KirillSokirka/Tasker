using AutoMapper;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.EntitiesExtension;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Repositories;

namespace Tasker.Application.Services.Application;

public class UserService : EntityService<User, UserDto>, IUserService
{
    private readonly IUserResolver _resolver;

    public UserService(IEntityRepository<User> repository, IMapper mapper, 
        IUserResolver resolver) : base(repository, mapper)
    {
        _resolver = resolver;
    }

    public async Task<UserDto> CreateAsync(UserDto createDto)
    {
        var user = Mapper.Map<User>(createDto);

        await Repository.AddAsync(user);
        
        return (await GetByIdAsync(user!.Id))!;
    }

    public async Task<UserDto?> UpdateAsync(UserUpdateDto dto)
    {
        var user = await Repository.GetByIdAsync(dto.Id);

        if (user is null)
        {
            return null;
        }

        var resolvedProperties = await _resolver.ResolveAsync(dto);

        user.Update(dto, resolvedProperties);
        
        await Repository.UpdateAsync(user);
        
        return (await GetByIdAsync(user!.Id))!;
    }
}