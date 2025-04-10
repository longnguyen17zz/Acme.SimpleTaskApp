using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.Tasks;
using System;

namespace Acme.SimpleTaskApp.Tasks.Dto;
public class GetAllTasksInput
{
    public TaskState? State { get; set; }
}

[AutoMapFrom(typeof(Task))]
public class TaskListDto : EntityDto, IHasCreationTime
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreationTime { get; set; }

    public TaskState State { get; set; }

    public Guid? AssignedPersonId { get; set; }

    public string AssignedPersonName { get; set; }
}