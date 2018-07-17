using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Identity;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Users
{
    public class AuthenticateCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Username).NotEqual(default(string));
                RuleFor(request => request.Password).NotEqual(default(string));
            }            
        }

        public class Request : IRequest<Response>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Response
        {
            public string AccessToken { get; set; }
            public Guid UserId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventStore _eventStore;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ISecurityTokenFactory _securityTokenFactory;

            public Handler(
                IEventStore eventStore, 
                ISecurityTokenFactory securityTokenFactory, 
                IPasswordHasher passwordHasher)
            {
                _eventStore = eventStore;
                _securityTokenFactory = securityTokenFactory;
                _passwordHasher = passwordHasher;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {                                
                var userId = _eventStore.GetEventByEventProperyValue<UserCreated>("Username",request.Username).UserId;
                
                var user = _eventStore.Load<User>(userId);

                if (user.Password != _passwordHasher.HashPassword(user.Salt, request.Password))
                    throw new System.Exception();

                return Task.FromResult(new Response()
                {
                    AccessToken = _securityTokenFactory.Create(request.Username),
                    UserId = userId
                });
            }            
        }
    }
}
