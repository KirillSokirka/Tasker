using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tasker.Domain.Models.Identity;

namespace Tasker.Domain.Models.Application;

public abstract class UserOperationBase<T> where T : class
{
    public async Task<IActionResult> ExecuteAsync(T model, ControllerBase controller)
    {
        if (!controller.ModelState.IsValid)
        {
            return controller.BadRequest(controller.ModelState);
        }

        var operationResult = await PerformUserOperationAsync(model);

        if (operationResult.IsSuccess)
        {
            return controller.Ok(GetSuccessResult(operationResult));
        }

        AddErrorsToModelState(operationResult, controller.ModelState);
        return controller.BadRequest(controller.ModelState);
    }

    protected abstract Task<OperationResult> PerformUserOperationAsync(T model);
    protected abstract object GetSuccessResult(OperationResult operationResult);
    protected abstract void AddErrorsToModelState(OperationResult operationResult, ModelStateDictionary modelState);
}