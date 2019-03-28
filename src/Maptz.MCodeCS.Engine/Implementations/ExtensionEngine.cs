using Microsoft.Extensions.DependencyInjection;
using Maptz.Coding.Analysis.CSharp.Sorting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Threading.Tasks;
using Maptz.Coding.Analysis.CSharp.TestCreator;
using Maptz.Coding.Analysis.CSharp.Misc;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Maptz.MCodeCS.Engine
{

    public class ExtensionEngine : IExtensionEngine
    {

        public ExtensionEngine(IWorkspaceProvider workspaceProvider, IServiceProvider serviceProvider, IOutputService outputService)
        {
            this.WorkspaceProvider = workspaceProvider;
            this.ServiceProvider = serviceProvider;
            this.OutputService = outputService;
        }

        public IWorkspaceProvider WorkspaceProvider { get; }
        public IServiceProvider ServiceProvider { get; }
        public IOutputService OutputService { get; }

        public async Task<string> RunCodeManipulator<T>(string fileContents, string filePath, int cursor) where T : ICodeManipulatorService
        {
            var cmp = new SimpleCodeManipulationContextProvider(fileContents, filePath, cursorPosition: cursor);
            //var cmp = new SimpleCodeManipulationContextProvider(fileContents, Path.GetFileName(filePath), cursorPosition: cursor);
            var tuple = await cmp.GetCodeManipulationContextAsync();
            var service = this.ServiceProvider.GetRequiredService<T>();

            var cr = await service.Convert(tuple);
            var implementor = this.ServiceProvider.GetRequiredService<ICodeChangeImplementorService>() as CodeChangeImplementorService;
            await implementor.ApplyChangeAsync(tuple, cr);

            var obj = new
            {
                Changes = implementor.Changes,
                Error = (string) null
            };

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(obj, serializerSettings);
            this.OutputService.Write(json);
            return json;
        }

        public async Task<string> AddTestAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<ICreateTestsService>(fileContents, filePath, cursor);
        }

        public async Task<string> ConvertToAsyncAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<IAsyncMethodConverterService>(fileContents, filePath, cursor);
        }

        public async Task<string> CreateSettingsAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<ICreateSettingsService>(fileContents, filePath, cursor);
        }

        public async Task<string> ConvertToProtectedVirtualAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<IProtectedVirtualMethodConverterService>(fileContents, filePath, cursor);
        }

        public async Task<string> ExpandPropertyAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<IExpandPropertyService>(fileContents, filePath, cursor);

        }

        public async Task<string> ExpressAsPropertyAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<IExpressPropertyService>(fileContents, filePath, cursor);


        }

        public async Task<string> ExpressAsStatementAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<IExpressStatementService>(fileContents, filePath, cursor);
        }

        public async Task<string> ExtractClassAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<IExtractClassService>(fileContents, filePath, cursor);
        }

        public async Task<string> RemoveUnusedUsingsAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<IRemoveUnusedUsingsService>(fileContents, filePath, cursor);
        }


        public async Task<string> SortAsync(string fileContents, string filePath, int cursor)
        {
            return await this.RunCodeManipulator<ICSharpSorterService>(fileContents, filePath, cursor);
        }
    }
}