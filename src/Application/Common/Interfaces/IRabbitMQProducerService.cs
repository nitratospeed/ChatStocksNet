using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IRabbitMQProducerService
    {
        bool Send(string room, string message); 
    }
}
