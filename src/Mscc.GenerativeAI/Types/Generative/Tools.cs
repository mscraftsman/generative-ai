#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Tools : List<Tool>
    {
        public Tools() { }

        public Tools(Delegate[] delegates)
        {
            if (delegates == null) throw new ArgumentNullException(nameof(delegates));

            var functions = GetFunctions();
            foreach (Delegate function in delegates)
            {
                functions.Add(new (function));
            }
        }

        public void AddGoogleSearch()
        {
            if (!this.Any(t => t.GoogleSearch is not null))
            {
                this.Add(new Tool() { GoogleSearch = new() });
            }
        }

        public void AddCodeExecution()
        {
            if (!this.Any(t => t.CodeExecution is not null))
            {
                this.Add(new Tool() { CodeExecution = new() });
            }
        }

        public void AddFunction(string name, string? description)
        {
            GetFunctions().Add(new(name, description));
        }

        public void AddFunction(Delegate callback)
        {
            GetFunctions().Add(new(callback));
        }

        public void AddFunction(string name, Delegate callback)
        {
            GetFunctions().Add(new(name, callback));
        }

        public void AddFunction(string name, string? description, Delegate callback)
        {
            GetFunctions().Add(new(name, description, callback));
        }

        public bool RemoveFunction(string name)
        {
            var functions = this
                .Where(t => t.FunctionDeclarations is not null)
                .Select(t => t);
            foreach (var item in functions)
            {
                var function = item.FunctionDeclarations!.FirstOrDefault(f => f.Name == name);
                if (function is not null)
                {
                    return item.FunctionDeclarations.Remove(function);
                }
            }

            return false;
        }

        public bool RemoveFunction(Delegate callback)
        {
            var functions = this
                .Where(t => t.FunctionDeclarations is not null)
                .Select(t => t);
            foreach (var item in functions)
            {
                var function = item.FunctionDeclarations!.FirstOrDefault(f => f.Callback == callback);
                if (function is not null)
                {
                    return item.FunctionDeclarations.Remove(function);
                }
            }

            return false;
        }

        public void ClearFunctions()
        {
            var functions = this
                .Where(t => t.FunctionDeclarations is not null)
                .Select(t => t);
            foreach (var item in functions)
            {
                item.FunctionDeclarations!.Clear();
            }
        }

        private List<FunctionDeclaration> GetFunctions()
        {
            Tool functions = new() { FunctionDeclarations = new() };
            if (this.Any(t => t.FunctionDeclarations is not null))
            {
                functions = this.FirstOrDefault(t => t.FunctionDeclarations is not null);
            }
            else if (Count == 0)
            {
                Add(functions);
            }
            else
            {
                Add(functions);
            }

            return functions!.FunctionDeclarations!;
        }
    }
}