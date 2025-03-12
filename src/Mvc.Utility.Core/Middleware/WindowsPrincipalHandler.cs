﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin;


namespace Mvc.Utility.Core.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class WindowsPrincipalHandler
    {
        private readonly AppFunc _next;

        public WindowsPrincipalHandler(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            if (context.Request.User is WindowsPrincipal windowsPrincipal && windowsPrincipal.Identity.IsAuthenticated)
            {
                await _next(env);

                if (context.Response.StatusCode != 401) return;
                // We're going no add the identifier claim
                var nameClaim = windowsPrincipal.FindFirst(ClaimTypes.Name);

                // This is the domain name
                string name = nameClaim.Value;

                // If the name is something like DOMAIN\username then
                // grab the name part
                var parts = name.Split(new[] { '\\' }, 2);

                string shortName = parts.Length == 1 ? parts[0] : parts[parts.Length - 1];

                // REVIEW: Do we want to preserve the other claims?

                // Normalize the claims here
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, name),
                    new Claim(ClaimTypes.Name, shortName),
                    new Claim(ClaimTypes.AuthenticationMethod, "Windows")
                };
                if (windowsPrincipal.Identity is WindowsIdentity wi)

                    if (wi.Groups != null)
                    {
                        var groups = wi.Groups.Translate(typeof(NTAccount));
                        var roles = groups?.Select(x => new Claim("role", x.Value)).ToList();
                        if (roles != null) claims.AddRange(roles);
                    }

                var identity = new ClaimsIdentity(claims, Constraints.Constraint.WIN_AUTH_TYPE);

                context.Authentication.SignIn(identity);

                context.Response.Redirect((context.Request.PathBase + context.Request.Path).Value);

                return;
            }

            await _next(env);
        }
    }
}
