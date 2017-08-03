using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.Core.Data.Command
{
    /// <summary>
    /// Generic Interface for commands
    /// This is used for fire and forget actions
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommand<TCommand>
    {
        void Execute(TCommand command);
    }
}
