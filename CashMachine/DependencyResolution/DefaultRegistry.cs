// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CashMachine.DependencyResolution
{
    using System.Web;

    using CashMachine.Dal;
    using CashMachine.Dal.Repositories;
    using CashMachine.Domain.Abstractions;
    using CashMachine.Domain.Entities;
    using CashMachine.Domain.Managers;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            For<IUnitOfWork>().Use<EfUnitOfWork>();

            For<ICardRepository>().Use<CardRepository>();
            For<IOperationRepository>().Use<OperationRepository>();
            For<IUserStore<Card, int>>().Use<CardRepository>();

            For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<CardManager>().OnCreationForAll(manager => manager.Configure());
        }

        #endregion
    }
}