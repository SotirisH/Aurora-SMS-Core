using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Aurora.Core.Data.Command
{
    /// <summary>
    /// Concrete implementation of a command that wraps the execution in a transaction
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public class TransactionCommandDecorator<TCommand> : ICommand<TCommand>
    {
        private readonly ICommand<TCommand> decorated;
        public TransactionCommandDecorator(ICommand<TCommand> decorated)
        {
            this.decorated = decorated;
        }

        public void Execute(TCommand command)
        {
            using (var scope = new TransactionScope())
            {
                decorated.Execute(command);

                scope.Complete();
            }
        }
    }
}
