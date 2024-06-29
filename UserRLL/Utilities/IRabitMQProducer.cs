using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRLL.Utilities
{
    public interface IRabitMQProducer
    {
        public void SendMessage<T>(T message);
    }
}
