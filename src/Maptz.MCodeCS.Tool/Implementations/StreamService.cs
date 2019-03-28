using Maptz;
using Maptz.CliTools;
using Maptz.MCodeCS.Engine;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Maptz.MCodeCS.Tool
{

    public class StreamService : IStreamService
    {
        public IExtensionEngine ExtensionEngine { get; }

        public StreamService(IExtensionEngine extensionEngine)
        {
            this.ExtensionEngine = extensionEngine;
        }

        public static string GetRequestPostData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return null;
            }
            using (System.IO.Stream body = request.InputStream) // here we have data
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public void Do()
        {
            var shouldExit = false;
            using (var shouldExitWaitHandle = new ManualResetEvent(shouldExit))
            using (var listener = new HttpListener())
            {
                Console.CancelKeyPress += (
                    object sender,
                    ConsoleCancelEventArgs e
                ) =>
                {
                    e.Cancel = true;
                    /*
                    this here will eventually result in a graceful exit
                    of the program
                     */
                    shouldExit = true;
                    shouldExitWaitHandle.Set();
                };

                listener.Prefixes.Add("http://+:8089/");

                listener.Start();
                Console.WriteLine("Server listening at port 8089");

                /*
                This is the loop where everything happens, we loop until an
                exit is requested
                 */
                while (!shouldExit)
                {
                    /*
                    Every request to the http server will result in a new
                    HttpContext
                     */
                    var contextAsyncResult = listener.BeginGetContext(
                        async (IAsyncResult asyncResult) =>
                        {
                            var context = listener.EndGetContext(asyncResult);
                            try
                            {
                                var json = GetRequestPostData(context.Request);
                                var pipedInputModel = JsonConvert.DeserializeObject<PipedInputModelEx>(json);
                                var engineMethod = this.GetEngineMethod(pipedInputModel);
                                var retval = await engineMethod(pipedInputModel.FileContents, pipedInputModel.FilePath, pipedInputModel.Cursor);

                                using (var writer = new StreamWriter(context.Response.OutputStream))
                                {
                                    writer.WriteLine(retval);
                                }
                            }
                            catch(Exception ex)
                            {
                                using (var writer = new StreamWriter(context.Response.OutputStream))
                                {
                                    writer.WriteLine("ERROR: " + ex.ToString());
                                }
                            }
                        },
                        null
                    );

                    /*
                    Wait for the program to exit or for a new request 
                     */
                    WaitHandle.WaitAny(new WaitHandle[]{
                        contextAsyncResult.AsyncWaitHandle,
                        shouldExitWaitHandle
                    });
                }

                listener.Stop();
                Console.WriteLine("Server stopped");
            }
        }

        private EngineMethod GetEngineMethod(PipedInputModelEx pipedInputModelEx)
        {
            switch (pipedInputModelEx.CommandName)
            {
                case "add-test":
                    return this.ExtensionEngine.AddTestAsync;
                case "convert-to-async":
                    return this.ExtensionEngine.ConvertToAsyncAsync;
                case "convert-to-protected-virtual":
                    return this.ExtensionEngine.ConvertToProtectedVirtualAsync;
                case "create-settings":
                    return this.ExtensionEngine.CreateSettingsAsync;
                case "extract-class":
                    return this.ExtensionEngine.ExtractClassAsync;
                case "expand-property":
                    return this.ExtensionEngine.ExpandPropertyAsync;
                case "express-as-property":
                    return this.ExtensionEngine.ExpressAsPropertyAsync;
                case "express-as-statement":
                    return this.ExtensionEngine.ExpressAsStatementAsync;
                case "remove-unused-usings":
                    return this.ExtensionEngine.RemoveUnusedUsingsAsync;
                case "sort":
                    return this.ExtensionEngine.SortAsync;
                default:
                    throw new NotImplementedException();
            }

        }

       
    }
}