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
            private readonly IAppDbContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ISecurityTokenFactory _securityTokenFactory;

            public Handler(
                IAppDbContext context, 
                ISecurityTokenFactory securityTokenFactory, 
                IPasswordHasher passwordHasher)
            {
                _context = context;
                _securityTokenFactory = securityTokenFactory;
                _passwordHasher = passwordHasher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var domainEvents = await _context.DomainEvents.Where(x
                    => x.DotNetType == typeof(UserCreatedEvent).AssemblyQualifiedName)
                    .Select(x => JsonConvert.DeserializeObject(x.Data, Type.GetType(x.DotNetType)))
                    .ToArrayAsync();

                var user = new User(Guid.NewGuid());
                
                user = user.Reduce(user, domainEvents);
                
                if (user.Password != _passwordHasher.HashPassword(user.Salt, request.Password))
                    throw new System.Exception();

                return new Response()
                {
                    AccessToken = _securityTokenFactory.Create(request.Username),
                    UserId = user.UserId
                };
            }            
        }
    }
}
