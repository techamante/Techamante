﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Domain.Interfaces
{
    public interface  IDomainEvent : INotification
    {
        DateTime DateTimeEventOccurred { get; }
    }
}
