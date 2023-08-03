using Grpc.Core;
using Todo.Grpc.Data;
using Todo.Grpc.Models;

namespace Todo.Grpc.Services;

public class ToDoItService : ToDoIt.ToDoItBase
{
    private readonly AppDbContext _context;

    public ToDoItService(AppDbContext context)
    {
        _context = context;
    }

    public override async Task<CreateToDoResponse> CreateTodo(CreateToDoRequest request, ServerCallContext context)
    {
        if (request.Title == string.Empty || request.Description == string.Empty)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid object"));
        }


        TodoModel todoItem = new()
        {
            Title = request.Title,
            Description = request.Description,
        };

        await _context.AddAsync(todoItem);
        await _context.SaveChangesAsync();

        return await Task.FromResult(new CreateToDoResponse
        {
            Id = todoItem.Id,
        });
    }

    public override async Task<ReadToDoResponse> ReadToDo(ReadToDoRequest request, ServerCallContext context)
    {
        return base.ReadToDo(request, context);
    }

    public override async Task<GetAllResponse> ListToDo(GetAllRequest request, ServerCallContext context)
    {
        return base.ListToDo(request, context);
    }

    public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {
        return base.UpdateToDo(request, context);
    }

    public override async Task<DeleteToDoResponse> DeleteTodo(DeleteToDoRequest request, ServerCallContext context)
    {
        return base.DeleteTodo(request, context);
    }
}