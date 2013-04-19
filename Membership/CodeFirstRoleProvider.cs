using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp4.Membership;
using WebApp4.Entities;
using WebApp4.Infrastructure;

namespace WebApp4.Membership
{
    public class CodeFirstRoleProvider : RoleProvider
    {

        private string _ApplicationName;
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "CodeFirstRoleProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Code-First Role Provider");
            }

            base.Initialize(name, config);

            _ApplicationName = config["applicationName"];
        }

        public override string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }

        public override bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            using (WebApp4Context context = new WebApp4Context())
            {
                dynamic result = context.Role.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool IsUserInRole(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            using (WebApp4Context context = new WebApp4Context())
            {
                dynamic user = context.User.FirstOrDefault(Usr => Usr.Username == userName);
                if (user == null)
                {
                    return false;
                }
                dynamic role = context.Role.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (role == null)
                {
                    return false;
                }
                return user.Roles.Contains(role);
            }
        }

        public override string[] GetAllRoles()
        {
            using (WebApp4Context context = new WebApp4Context())
            {
                return context.Role.Select(Rl => Rl.RoleName).ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            using (WebApp4Context context = new WebApp4Context())
            {
                var role = context.Role.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (role == null)
                {
                    throw new InvalidOperationException("Role not found");
                }
                return role.Users.Select(Usr => Usr.Username).ToArray();
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            if (string.IsNullOrEmpty(usernameToMatch))
            {
                throw CreateArgumentNullOrEmptyException("usernameToMatch");
            }
            using (WebApp4Context context = new WebApp4Context())
            {
                var query = from Rl in context.Role from Usr in Rl.Users where Rl.RoleName == roleName && Usr.Username.Contains(usernameToMatch) select Usr.Username;
                return query.ToArray();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            using (WebApp4Context context = new WebApp4Context())
            {
                dynamic role = context.Role.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (role == null)
                {
                    throw new InvalidOperationException("Role not found");
                }
                if (throwOnPopulatedRole)
                {
                    dynamic usersInRole = role.Users.Any;
                    if (usersInRole)
                    {
                        throw new InvalidOperationException(string.Format("Role populated: {0}", roleName));
                    }
                }
                else
                {
                    foreach (User usr_loopVariable in role.Users)
                    {
                        var usr = usr_loopVariable;
                        context.User.Remove(usr);
                    }
                }
                context.Role.Remove(role);
                context.SaveChanges();
                return true;
            }
        }

        public override string[] GetRolesForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            using (WebApp4Context context = new WebApp4Context())
            {
                var user = context.User.FirstOrDefault(Usr => Usr.Username == userName);
                if (user == null)
                {
                    throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                }
                return user.Roles.Select(Rl => Rl.RoleName).ToArray();
            }
        }

        public override void CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            using (WebApp4Context context = new WebApp4Context())
            {
                dynamic role = context.Role.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (role != null)
                {
                    throw new InvalidOperationException(string.Format("Role exists: {0}", roleName));
                }
                Role NewRole = new Role
                {
                    //Id = Guid.NewGuid(),    
                    RoleName = roleName
                };
                NewRole.GenerateNewIdentity();
                context.Role.Add(NewRole);
                context.SaveChanges();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (WebApp4Context context = new WebApp4Context())
            {
                var users = context.User.Where(usr => usernames.Contains(usr.Username)).ToList();
                var roles = context.Role.Where(rl => roleNames.Contains(rl.RoleName)).ToList();
                foreach (User user_loopVariable in users)
                {
                    var user = user_loopVariable;
                    foreach (Role role_loopVariable in roles)
                    {
                        var role = role_loopVariable;
                        if (!user.Roles.Contains(role))
                        {
                            user.Roles.Add(role);
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (WebApp4Context context = new WebApp4Context())
            {
                foreach (string username_loopVariable in usernames)
                {
                    var username = username_loopVariable;
                    string us = username;
                    User user = context.User.FirstOrDefault(u => u.Username == us);
                    if (user != null)
                    {
                        foreach (string rolename_loopVariable in roleNames)
                        {
                            var rolename = rolename_loopVariable;
                            var rl = rolename;
                            Role role = user.Roles.FirstOrDefault(r => r.RoleName == rl);
                            if (role != null)
                            {
                                user.Roles.Remove(role);
                            }
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        private ArgumentException CreateArgumentNullOrEmptyException(string paramName)
        {
            return new ArgumentException(string.Format("Argument cannot be null or empty: {0}", paramName));
        }

    }

}