﻿namespace bgTeam.Queues
{
    using bgTeam.Queues.Exceptions;
    using System;
    using System.Threading.Tasks;

    public delegate Task QueueMessageReceive<TEntity>(object queue, TEntity message)
        where TEntity : IQueueMessage;

    public interface IQueueWatcher<IQueueMessage>
    {
        event QueueMessageHandler Subscribe;

        event EventHandler<ExtThreadExceptionEventArgs> Error;

        void StartWatch(string queueName);
    }
}