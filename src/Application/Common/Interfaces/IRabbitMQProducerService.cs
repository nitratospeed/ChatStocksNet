using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IRabbitMQProducerService
    {
        void Push(string message); 
    }
}
