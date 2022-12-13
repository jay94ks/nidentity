namespace NIdentity.Core.Server.Commands
{
    public class CommandTerraformContext
    {
        private static readonly object KEY = new();

        /// <summary>
        /// Initialize a new <see cref="CommandTerraformContext"/> instance.
        /// </summary>
        /// <param name="Http"></param>
        private CommandTerraformContext(HttpContext HttpContext)
        {
            this.HttpContext = HttpContext;
        }

        /// <summary>
        /// Make <see cref="CommandTerraformContext"/> instance from <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <returns></returns>
        internal static CommandTerraformContext FromHttpContext(HttpContext HttpContext)
        {
            HttpContext.Items.TryGetValue(KEY, out var Temp);
            if (Temp is not CommandTerraformContext Terraform)
                HttpContext.Items[KEY] = Terraform = new(HttpContext);

            return Terraform;
        }

        /// <summary>
        /// Invoke the terraform delegates.
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        internal static async Task TerraformAsync(HttpContext Http, Func<Task> Next)
        {
            var Context = FromHttpContext(Http);

            // --> set the executor instance.
            Context.Executor = Http.RequestServices.GetRequiredService<ICommandExecutor>();

            async Task NextAsync()
            {
                if (Context.Unauthorized)
                    return;

                if (Context.Terraformers.TryDequeue(out var Each))
                {
                    await Each.Invoke(Context, NextAsync);
                    return;
                }

                else
                {
                    await Next.Invoke();
                }
            }

            await NextAsync();

            if (Context.Unauthorized)
            {
                Http.Response.StatusCode = 401;
                return;
            }
        }

        /// <summary>
        /// Http Context.
        /// </summary>
        public HttpContext HttpContext { get; }

        /// <summary>
        /// Terraformers.
        /// </summary>
        internal Queue<Func<CommandTerraformContext, Func<Task>, Task>> Terraformers { get; }
           = new Queue<Func<CommandTerraformContext, Func<Task>, Task>>();

        /// <summary>
        /// Command Executor.
        /// </summary>
        public ICommandExecutor Executor { get; set; }

        /// <summary>
        /// Unauthorized or not.
        /// </summary>
        public bool Unauthorized { get; set; }
    }
}
