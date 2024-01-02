using AutoMapper;
using Dapper;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Application.Interfaces.Services;
using Tasker.Domain;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Repositories;

namespace Tasker.Application.Services.Application;

public class UserService : EntityService<User, UserDto>, IUserService
{
    private readonly IUserResolver _resolver;
    private readonly IUserAuthService _userAuthService;
    private readonly IDatabaseConnectionFactory _dbConnectionFactory;

    public UserService(IEntityRepository<User> repository, IMapper mapper,
        IUserResolver resolver, IUserAuthService userAuthService,IDatabaseConnectionFactory dbConnectionFactory) 
        : base(repository, mapper)
    {
        _resolver = resolver;
        _userAuthService = userAuthService;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<UserDto> CreateAsync(UserDto createDto)
    {
        var user = Mapper.Map<User>(createDto);

        await Repository.AddAsync(user);

        return (await GetByIdAsync(user!.Id))!;
    }

    public async Task<UserDto?> UpdateAsync(UserUpdateDto dto)
    {
        var user = (await Repository.FindAsync(u => u.Title == dto.Username)).FirstOrDefault();

        if (user is null)
        {
            return null;
        }

        var (adminProjects, assignedProjects) = GetProjects(dto, user.Id);

        await _resolver.ResolveAsync(
            admin: adminProjects,
            assigned: assignedProjects,
            userId: user.Id);
        
        return (await GetByIdAsync(user!.Id))!;
    }

    private (List<UserProjectDto>?, List<UserProjectDto>?) GetProjects(UserUpdateDto dto, string userId)
    {
        var assignedProjects = (dto.AssignedProjects ?? new List<string>())
            .Select(project => new UserProjectDto { ProjectId = project, UserId = userId }).ToList();

        var adminProjects = (dto.UnderControlProjects ?? new List<string>())
            .Select(project => new UserProjectDto { ProjectId = project, UserId = userId }).ToList();
        
        if (!assignedProjects.Any()) assignedProjects = null;

        if (!adminProjects.Any()) adminProjects = null;
        
        return (adminProjects, assignedProjects);
    }
    
    public new async Task<bool> DeleteAsync(string id)
    {
        var entity = await Repository.GetByIdAsync(id);
        
        if (entity is not null)
        {
            if (entity.Title != Constant.SuperAdminEmail)
            {
                var superAdmin = (await Repository.FindAsync(u => u.Title == Constant.SuperAdminEmail)).First();
                
                await ProcessRelatedEntities(entity, superAdmin);
                
                await _userAuthService.DeleteUserAsync(id);
            
                return true;
            }
        }

        return false;
    }

    private async System.Threading.Tasks.Task ProcessRelatedEntities(User entity, User superAdmin)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        
        var updateTaskSql = "UPDATE Tasks SET CreatorId = @SuperAdminId WHERE CreatorId = @EntityId";
        
        await connection.ExecuteAsync(updateTaskSql, new { SuperAdminId = superAdmin.Id, EntityId = entity.Id });

        updateTaskSql = "UPDATE Tasks SET AssigneeId = null WHERE AssigneeId = @EntityId";
        
        await connection.ExecuteAsync(updateTaskSql, new { EntityId = entity.Id });
        
        const string projectSql = @"
            SELECT p.*
            FROM Projects p
            INNER JOIN AdminProjectUsers apu ON p.Id = apu.ProjectId
            WHERE apu.UserId = @EntityId";
        
        var projects = (await connection.QueryAsync<Project>(projectSql, new { EntityId = entity.Id })).ToList();

        foreach (var project in projects)
        {
            const string deleteAdminSql = "DELETE FROM AdminProjectUsers WHERE ProjectId = @ProjectId AND UserId = @EntityId";
            await connection.ExecuteAsync(deleteAdminSql, new { ProjectId = project.Id, EntityId = entity.Id });

            const string deleteAssignedSql = "DELETE FROM AssignedProjectUsers WHERE ProjectId = @ProjectId AND UserId = @EntityId";
            await connection.ExecuteAsync(deleteAssignedSql, new { ProjectId = project.Id, EntityId = entity.Id });
            
            const string checkSuperAdminSql = @"
                SELECT COUNT(1)
                FROM AdminProjectUsers
                WHERE ProjectId = @ProjectId AND UserId = @SuperAdminId";
            
            var count = await connection.ExecuteScalarAsync<int>(checkSuperAdminSql, 
                new { ProjectId = project.Id, SuperAdminId = superAdmin.Id });

            if (count == 0)
            {
                const string insertAdminSql = @"
                    INSERT INTO AdminProjectUsers (ProjectId, UserId)
                    VALUES (@ProjectId, @SuperAdminId)";
                
                await connection.ExecuteAsync(insertAdminSql, new { ProjectId = project.Id, SuperAdminId = superAdmin.Id });
            }

            const string deleteUser = "DELETE FROM [dbo].User WHERE Id = @Id";
            await connection.ExecuteAsync(deleteUser, new { entity.Id });
        }
    }
}