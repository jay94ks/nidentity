namespace NIdentity.Core.Server.Commands
{
    public class CommandTerraformBuilder
    {
        private readonly List<Func<CommandTerraformContext, Func<Task>, Task>> m_Terraformers = new();

        /// <summary>
        /// Use a terraform delegate.
        /// </summary>
        /// <param name="Terraformer"></param>
        /// <returns></returns>
        public CommandTerraformBuilder With(Func<CommandTerraformContext, Func<Task>, Task> Terraformer)
        {
            m_Terraformers.Add(Terraformer);
            return this;
        }

        /// <summary>
        /// Get the terraformer delegates.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Func<CommandTerraformContext, Func<Task>, Task>> GetTerraformers() => m_Terraformers;
    }
}
