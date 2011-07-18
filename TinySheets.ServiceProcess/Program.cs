using Autofac;
using Autofac.Configuration;
using TinySheets.Tasks;
using Topshelf;
using Topshelf.Configuration.Dsl;

namespace TinySheets.ServiceProcess
{
    class Program
    {
        // Command-line syntax can be used to install/uninstall the service
        // see http://topshelf-project.com
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ConfigurationSettingsReader>();

            using (var container = builder.Build())
            {
                var cfg = RunnerConfigurator.New(x =>
                {
                    x.ConfigureService<ITaskRunner>(s =>
                    {
                        s.HowToBuildService(name => container.Resolve<ITaskRunner>());
                        s.WhenStarted(p => p.Start());
                        s.WhenStopped(p => p.Stop());
                    });
                    x.RunAsLocalSystem();
                    x.SetDescription("TinySheets background process for running periodic tasks.");
                    x.SetServiceName("TinySheets Service Process");
                });

                Runner.Host(cfg, args);
            }
        }
    }
}
