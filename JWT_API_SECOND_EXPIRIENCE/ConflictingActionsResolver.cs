using System.Collections.Generic;
using System.Linq;
using JWT_API_SECOND_EXPIRIENCE.Repositories.Abstract;
using JWT_API_SECOND_EXPIRIENCE.Repositories.Domain;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JWT_API_SECOND_EXPIRIENCE
{
    public class ConflictingActionsResolver : IOperationFilter
    {

        


        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor != null)
            {
                var uniqueId = $"{actionDescriptor.ControllerName}_{actionDescriptor.ActionName}";

                operation.OperationId = uniqueId;
            }
        }


        private string GetUniqueIdentifier(List<string> conflictingActionIds, string currentId)
        {
            var uniqueId = currentId;
            var counter = 1;

            while (conflictingActionIds.Contains(uniqueId))
            {
                uniqueId = $"{currentId}_{counter}";
                counter++;
            }

            return uniqueId;
        }
    }
}
