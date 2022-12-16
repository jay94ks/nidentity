using NIdentity.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Endpoints.Commands.Bases
{
    /// <summary>
    /// Base class for sensitive commands
    /// that requires authority's certificate.
    /// </summary>
    public abstract class EidSensitiveCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidSensitiveCommand"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        protected EidSensitiveCommand(string Type) : base(Type)
        {
        }
    }
}
