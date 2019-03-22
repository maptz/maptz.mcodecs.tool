using Microsoft.Extensions.DependencyInjection;
using Maptz.Coding.Analysis.CSharp.Sorting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace Maptz.MCodeCS.Engine
{
    public interface IExtensionEngine
    {
        /* #region Public Methods */
        Task AddTestAsync(string fileContents, string filePath, int cursor);
        Task ConvertToAsyncAsync(string fileContents, string filePath, int cursor);
        Task ConvertToProtectedVirtualAsync(string fileContents, string filePath, int cursor);
        Task CreateSettingsAsync(string fileContents, string filePath, int cursor);
        Task ExpandPropertyAsync(string fileContents, string filePath, int cursor);
        Task ExpressAsPropertyAsync(string fileContents, string filePath, int cursor);
        Task ExpressAsStatementAsync(string fileContents, string filePath, int cursor);
        Task ExtractClassAsync(string fileContents, string filePath, int cursor);
        Task RemoveUnusedUsingsAsync(string fileContents, string filePath, int cursor);
        Task SortAsync(string fileContents, string filepath, int cursor);
        /* #endregion Public Methods */
    }
}