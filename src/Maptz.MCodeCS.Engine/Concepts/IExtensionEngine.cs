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
        Task<string> AddTestAsync(string fileContents, string filePath, int cursor);
        Task<string> ConvertToAsyncAsync(string fileContents, string filePath, int cursor);
        Task<string> ConvertToProtectedVirtualAsync(string fileContents, string filePath, int cursor);
        Task<string> CreateSettingsAsync(string fileContents, string filePath, int cursor);
        Task<string> ExpandPropertyAsync(string fileContents, string filePath, int cursor);
        Task<string> ExpressAsPropertyAsync(string fileContents, string filePath, int cursor);
        Task<string> ExpressAsStatementAsync(string fileContents, string filePath, int cursor);
        Task<string> ExtractClassAsync(string fileContents, string filePath, int cursor);
        Task<string> RemoveUnusedUsingsAsync(string fileContents, string filePath, int cursor);
        Task<string> SortAsync(string fileContents, string filepath, int cursor);
        /* #endregion Public Methods */
    }
}