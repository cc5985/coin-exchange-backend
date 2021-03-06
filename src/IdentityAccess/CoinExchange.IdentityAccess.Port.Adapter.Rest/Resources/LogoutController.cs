/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* Coin Exchange is a high performance exchange system specialized for
* Crypto currency trading. It has different general purpose uses such as
* independent deposit and withdrawal channels for Bitcoin and Litecoin,
* but can also act as a standalone exchange that can be used with
* different asset classes.
* Coin Exchange uses state of the art technologies such as ASP.NET REST API,
* AngularJS and NUnit. It also uses design patterns for complex event
* processing and handling of thousands of transactions per second, such as
* Domain Driven Designing, Disruptor Pattern and CQRS With Event Sourcing.
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web.Http;
using System.Web.Http.Description;
using CoinExchange.Common.Utility;
using CoinExchange.IdentityAccess.Application.AccessControlServices;
using CoinExchange.IdentityAccess.Application.AccessControlServices.Commands;
using CoinExchange.IdentityAccess.Domain.Model.SecurityKeysAggregate;
using CoinExchange.IdentityAccess.Port.Adapter.Rest.DTO;

namespace CoinExchange.IdentityAccess.Port.Adapter.Rest.Resources
{
    [RoutePrefix("v1")]
    public class LogoutController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
              (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ILogoutApplicationService _logoutApplicationService;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="logoutApplicationService"></param>
        public LogoutController(ILogoutApplicationService logoutApplicationService)
        {
            _logoutApplicationService = logoutApplicationService;
        }

        /// <summary>
        /// Call for activating user account
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("private/admin/logout")]
        [FilterIP]
        [Authorize]
        [ResponseType(typeof(bool))]
        public IHttpActionResult Logout()
        {
            try
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("Logout Call Recevied");
                }
                return
                    Ok(_logoutApplicationService.Logout(new LogoutCommand(HeaderParamUtility.GetApikey(Request))));
            }
            catch (InstanceNotFoundException exception)
            {
                if (Log.IsErrorEnabled)
                {
                    Log.Error("Logout Call Exception ", exception);
                }
                return BadRequest(exception.Message);
            }
            catch (InvalidCredentialException exception)
            {
                if (Log.IsErrorEnabled)
                {
                    Log.Error("Logout Call Exception ", exception);
                }
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                if (Log.IsErrorEnabled)
                {
                    Log.Error("Logout Call Exception ", exception);
                }
                return InternalServerError();
            }
        }
    }
}