﻿using FluentValidation.Results;
using MediatR;
using NSE.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ValidationResult> EnviarComado<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }
    }
}