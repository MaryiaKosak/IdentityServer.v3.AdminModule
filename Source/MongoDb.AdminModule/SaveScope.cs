/*
 * Copyright 2014, 2015 James Geall
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using IdentityServer3.Core.Models;
using Newtonsoft.Json;

namespace IdentityServer3.Admin.MongoDb.Powershell
{
    class ScopesSingleton
    {
        private static ScopesSingleton _instance;
        public List<Scope> Scopes { get; set; }

        private ScopesSingleton()
        {
            Scopes = new List<Scope>();
        }
 
        public static ScopesSingleton GetInstance()
        {
            return _instance ?? (_instance = new ScopesSingleton());
        }
        
        ~ScopesSingleton()
        {
            // var scopesJson = JsonConvert.SerializeObject(Scopes);
            // File.WriteAllText(@"c:\temp\MyTest.txt", scopesJson);
        }
    }

    [Cmdlet(VerbsCommon.Set, "Scope")]
    public class SaveScope : MongoCmdlet
    {
        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNull]
        public Scope Scope { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                ScopesSingleton.GetInstance().Scopes.Add(Scope);
                var scopesJson = JsonConvert.SerializeObject(ScopesSingleton.GetInstance().Scopes);
                File.WriteAllText(@"c:\temp\Scopes.json", scopesJson);
                
                Console.WriteLine($"Scope {Scope.Name} saved");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}\r\n Message: {e.Message}\r\n Inner ex: {e.InnerException}\r\n Stacktrace: {e.StackTrace}");
                throw;
            }
        }
    }
}