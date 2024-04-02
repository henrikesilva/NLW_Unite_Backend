using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register;
public class RegisterEventUseCase
{
    public ResponseRegisterEventJson Execute(RequestEventJson request)
    {
        Validate(request);

        var dbcontext = new PassInDbContext();

        var entity = new Infrastructure.Entities.Event
        {
            Title = request.Title,
            Details = request.Details,
            Maximum_Attendees = request.MaximumAttendees,
            Slug = request.Title.ToLower().Replace(" ", "-")
        };

        dbcontext.Events.Add(entity);
        dbcontext.SaveChanges();

        return new ResponseRegisterEventJson
        {
            Id = entity.Id
        };
    }

    private void Validate(RequestEventJson request)
    {
        if (request.MaximumAttendees <= 0)
        {
            throw new PassInException("The Maximium attendees is invalid.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new PassInException("The title is invalid");
        }

        if (string.IsNullOrWhiteSpace(request.Details))
        {
            throw new PassInException("The details is invalid");
        }
    }
}
