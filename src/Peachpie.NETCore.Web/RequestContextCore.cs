﻿using Microsoft.AspNetCore.Http;
using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peachpie.Web
{
    /// <summary>
    /// Runtime context for ASP.NET Core request.
    /// </summary>
    sealed class RequestContextCore : Context
    {
        #region .cctor

        static RequestContextCore()
        {
            LoadScriptReferences();
        }

        /// <summary>
        /// Loads assemblies representing referenced scripts and reflects their symbols to be used by the runtime.
        /// </summary>
        static void LoadScriptReferences()
        {
            LoadScript(new System.Reflection.AssemblyName("website"));
        }

        static void LoadScript(System.Reflection.AssemblyName assname)
        {
            try
            {
                var ass = System.Reflection.Assembly.Load(assname);
                if (ass != null)
                {
                    AddScriptReference(ass.GetType(ScriptInfo.ScriptTypeName));
                }
            }
            catch
            {
            }

        }

        #endregion

        public static ScriptInfo ResolveScript(HttpRequest req)
        {
            var path = req.Path.Value.Replace('/', '\\').Trim('\\');    // TODO: normalized form
            return ScriptsMap.GetDeclaredScript(path);
        }

        /// <summary>
        /// Application physical root directory including trailing slash.
        /// </summary>
        public override string RootPath => "";

        public RequestContextCore(HttpContext context)
            : base()
        {
            this.InitOutput(context.Response.Body);

            // TODO: server variables
        }

        public override bool IsWebApplication => true;

        public override Encoding StringEncoding => Encoding.UTF8;
    }
}
