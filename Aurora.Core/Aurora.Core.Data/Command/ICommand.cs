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
    /// <remarks>https://cuttingedge.it/blogs/steven/pivot/entry.php?id=91</remarks>
    public interface ICommand<TCommand>
    {
        void Execute(TCommand command);
    }
}
