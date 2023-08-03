using Grpc.Core;
using Microsoft.EntityFrameworkCore;
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
        var todoItem = await _context.Todos.FindAsync(request.Id) ?? throw new RpcException(new Status(StatusCode.NotFound, $"Todo with id {request.Id} not found"));

        return await Task.FromResult(new ReadToDoResponse
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
        });
    }

    public override async Task<GetAllResponse> ListToDo(GetAllRequest request, ServerCallContext context)
    {
        var response = new GetAllResponse();

        var todoItems = await _context.Todos.ToListAsync();

        foreach (var item in todoItems)
        {
            response.ToDo.Add(new ReadToDoResponse
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                ToDoStatus = item.TodoStatus
            });
        }

        return await Task.FromResult(response);
    }

    public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {
        if (request.Id <= 0 || request.Title is null || request.Description == string.Empty)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid object"));
        }

        var todoToUpdate = await _context.Todos.FirstOrDefaultAsync(todo => todo.Id == request.Id) ?? throw new RpcException(new Status(StatusCode.NotFound, $"Todo with id {request.Id} not found"));

        todoToUpdate.Title = request.Title;
        todoToUpdate.Description = request.Description;
        todoToUpdate.TodoStatus = request.ToDoStatus;

        await _context.SaveChangesAsync();

        return await Task.FromResult(new UpdateToDoResponse
        {
            Id = todoToUpdate.Id
        });
    }

    public override async Task<DeleteToDoResponse> DeleteTodo(DeleteToDoRequest request, ServerCallContext context)
    {
        var todoToDelete = await _context.Todos.FirstOrDefaultAsync(todo => todo.Id == request.Id);

        if (todoToDelete is null || request.Id <= 0)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Todo with id {request.Id} not found"));
        }

        _context.Todos.Remove(todoToDelete);
        await _context.SaveChangesAsync();

        return await Task.FromResult(new DeleteToDoResponse
        {
            Id = todoToDelete.Id
        });
    }
}