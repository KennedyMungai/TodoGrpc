using Grpc.Core;
using Todo.Grpc.Data;

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
        return base.CreateTodo(request, context);
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