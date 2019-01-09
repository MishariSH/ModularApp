using Microsoft.EntityFrameworkCore;

namespace ModularApp.Modules.Core.Globals
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}
