//using Maptz.MCodeCS.Tool.Engine
using Maptz.CliTools;
using Maptz.Coding.Analysis.CSharp;
using Maptz.Coding.Analysis.CSharp.Misc;
using Maptz.Coding.Analysis.CSharp.Sorting;
using Maptz.Coding.Analysis.CSharp.TestCreator;
using Maptz.MCodeCS.Engine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Maptz.MCodeCS.Tool
{

    public class Program : CliProgramBase<AppSettings>
    {
        public static int Main(string[] args)
        {
            
            var program = new Program(args);
            if (program.ExitCode != 0)
            {
                
            }
            return program.ExitCode;
        }

        public Program(string[] args) : base(args)
        {

        }

        protected override void AddServices(IServiceCollection services)
        {
            base.AddServices(services);
            services.AddTransient<ICliProgramRunner, CLIProgramRunner>();
            services.AddTransient<IExtensionEngine, ExtensionEngine>();
            services.AddTransient<IWorkspaceProvider, WorkspaceProvider>();

            services.AddTransient<ICSharpSorterService, CSharpSorterService>();
            services.AddTransient<ISortGroupingOrderingService, DefaultSortGroupingService>();
            services.AddTransient<ISortGroupingService, DefaultSortGroupingService>();
            services.AddTransient<IMemberDeclarationOrderingService, DefaultSortGroupingService>();


            services.AddTransient<ICreateTestsService, CreateTestsService>();
            services.AddTransient<IAsyncMethodConverterService, AsyncMethodConverterService>();
            services.AddTransient<IProtectedVirtualMethodConverterService, ProtectedVirtualMethodConverterService>();
            services.AddTransient<IExpandPropertyService, ExpandPropertyService>();

            services.AddTransient<IRemoveUnusedUsingsService, RemoveUnusedUsingsService>();
            services.AddTransient<IExpressPropertyService, ExpressPropertyService>();
            services.AddTransient<IExpressStatementService, ExpressStatementService>();
            services.AddTransient<IExtractClassService, ExtractClassService>();
            services.AddTransient<IOutputService, OutputService>();
            services.AddTransient<ICodeChangeImplementorService, CodeChangeImplementorService>();
            services.AddTransient<ICSharpFormatterService, NullCSharpFormatterService>();
            services.AddTransient<ICreateSettingsService, CreateSettingsService>();
            services.AddTransient<IInputPipe, DefaultInputPipe>();
            services.AddTransient<IStreamService, StreamService>();
            //services.AddLogging(loggingBuilder => loggingBuilder.AddConfiguration(Configuration.GetSection("Logging")).AddConsole().AddDebug());
        }
    }

    public class NullCSharpFormatterService : ICSharpFormatterService
    {
        public string Format(string code)
        {
            return code;
        }
    }
}



