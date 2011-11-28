using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using System.Configuration.Provider;
using JobZoom.Web.Models;
using JobZoom.Business.Entities;

public class CustomRoleProvider : RoleProvider
{
    #region Properties
    private string _ApplicationName;
    #endregion

    #region Fields
    public override string ApplicationName
    {
        get { return _ApplicationName; }
        set { _ApplicationName = value; }
    }
    #endregion

    #region Override Method
    public override void Initialize(string name, NameValueCollection config)
    {
        if (config == null)
            throw new ArgumentNullException("config");

        if (name == null || name.Length == 0)
            name = "CustomRoleProvider";

        if (String.IsNullOrEmpty(config["description"]))
        {
            config.Remove("description");
            config.Add("description", "Custom Role Provider");
        }

        base.Initialize(name, config);

        _ApplicationName = GetConfigValue(config["applicationName"],
                     System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
    }

    /// <summary>
    ///     Adds the specified user names to the specified roles
    /// </summary>
    /// <param name="usernames">A string array of user names to be added to the specified roles.</param>
    /// <param name="roleNames">A string array of the role names to add the specified user names to.</param>
    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
        foreach (string rolename in roleNames)
        {
            if (rolename == null || rolename == "")
                throw new ProviderException("Role name cannot be empty or null.");
            if (!RoleExists(rolename))
                throw new ProviderException("Role name not found.");
        }

        foreach (string username in usernames)
        {
            if (username == null || username == "")
                throw new ProviderException("User name cannot be empty or null.");
            if (username.Contains(","))
                throw new ArgumentException("User names cannot contain commas.");

            foreach (string roleName in roleNames)
            {
                if (IsUserInRole(username, roleName))
                    throw new ProviderException("User is already in role.");
            }
        }

        using (JobZoomEntities db = new JobZoomEntities())
        {
            foreach (string username in usernames)
            {
                User dbuser = db.Users.First(r => r.UserId == username);
                foreach (string roleName in roleNames)
                {
                    if (!IsUserInRole(username, roleName))
                    {
                        var dbrole = db.Roles.First(r => r.RoleName == roleName);
                        dbrole.Users.Add(dbuser);
                        db.SaveChanges();
                    }
                }
            }

        }
    }

    /// <summary>
    ///     Adds a new role to the data source
    /// </summary>
    /// <param name="roleName">The name of the role to create.</param>
    public override void CreateRole(string roleName)
    {
        if (roleName == null || roleName == "")
            throw new ProviderException("Role name cannot be empty or null.");
        if (roleName.Contains(","))
            throw new ArgumentException("Role names cannot contain commas.");
        if (RoleExists(roleName))
            throw new ProviderException("Role name already exists.");
        if (roleName.Length > 255)
            throw new ProviderException("Role name cannot exceed 255 characters.");

        using (JobZoomEntities db = new JobZoomEntities())
        {
            Role dbrole = new Role();
            dbrole.RoleId = Guid.NewGuid();
            dbrole.RoleName = roleName;

            db.AddToRoles(dbrole);
            db.SaveChanges();
        }
    }

    /// <summary>
    ///     Removes a role from the data source
    /// </summary>
    /// <param name="roleName">The name of the role to delete.</param>
    /// <param name="throwOnPopulatedRole">If true, throw an exception if roleName has one or more members and do not delete roleName.</param>
    /// <returns>true if the role was successfully deleted; otherwise, false.</returns>
    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
        if (!RoleExists(roleName))
        {
            throw new ProviderException("Role does not exist.");
        }

        if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0)
        {
            throw new ProviderException("Cannot delete a populated role.");
        }

        using (JobZoomEntities db = new JobZoomEntities())
        {
            try
            {
                Role dbrole = db.Roles.First(r => r.RoleName == roleName);
                db.DeleteObject(dbrole);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <summary>
    ///     Gets an array of user names in a role where the user name contains the specified user name to match.
    /// </summary>
    /// <param name="roleName">The role to search in.</param>
    /// <param name="usernameToMatch">The user name to search for</param>
    /// <returns>A string array containing the names of all the users where the user name matches usernameToMatch and the user is a member of the specified role.</returns>
    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            return (db.Users.Where(u => u.Roles.Any(r => r.RoleName == roleName) && u.UserId.Contains(usernameToMatch))).Select(u => u.UserId).ToArray();
        }
    }

    /// <summary>
    ///     Gets a list of all the roles
    /// </summary>
    /// <returns>A string array containing the names of all the roles stored in the data source</returns>
    public override string[] GetAllRoles()
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            return db.Roles.Select(r => r.RoleName).ToArray();
        }
    }

    /// <summary>
    ///     Gets a list of the roles that a specified user is in
    /// </summary>
    /// <param name="username">The user to return a list of roles for.</param>
    /// <returns>A string array containing the names of all the roles that the specified user is in</returns>
    public override string[] GetRolesForUser(string username)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            return (db.Roles.Where(r => r.Users.Any(u => u.UserId == username))).Select(r => r.RoleName).ToArray();
        }
    }

    /// <summary>
    ///     Gets a list of users in the specified role
    /// </summary>
    /// <param name="roleName">The name of the role to get the list of users for.</param>
    /// <returns>A string array containing the names of all the users who are members of the specified role</returns>
    public override string[] GetUsersInRole(string roleName)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            return (db.Users.Where(u => u.Roles.Any(r => r.RoleName == roleName))).Select(u => u.UserId).ToArray();
        }
    }

    /// <summary>
    ///     Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
    /// </summary>
    /// <param name="username">The user name to search for.</param>
    /// <param name="roleName">The role to search in.</param>
    /// <returns>true if the specified user is in the specified role; otherwise, false.</returns>
    public override bool IsUserInRole(string username, string roleName)
    {
        if (username == null || username == "")
            throw new ProviderException("User name cannot be empty or null.");
        if (roleName == null || roleName == "")
            throw new ProviderException("Role name cannot be empty or null.");

        using (JobZoomEntities db = new JobZoomEntities())
        {
            return GetRolesForUser(username).Contains(roleName);
        }
    }

    /// <summary>
    ///     Removes the specified user names from the specified roles for the configured applicationName.
    /// </summary>
    /// <param name="usernames">A string array of user names to be removed from the specified roles.</param>
    /// <param name="roleNames">A string array of role names to remove the specified user names from.</param>
    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
        foreach (string roleName in roleNames)
        {
            if (roleName == null || roleName == "")
                throw new ProviderException("Role name cannot be empty or null.");
            if (!RoleExists(roleName))
                throw new ProviderException("Role name not found.");
        }

        foreach (string username in usernames)
        {
            if (username == null || username == "")
                throw new ProviderException("User name cannot be empty or null.");

            foreach (string roleName in roleNames)
            {
                if (!IsUserInRole(username, roleName))
                    throw new ProviderException("User is not in role.");
            }
        }

        using (JobZoomEntities db = new JobZoomEntities())
        {
            foreach (string username in usernames)
            {
                foreach (string roleName in roleNames)
                {
                    User dbuser = db.Users.FirstOrDefault(u => u.UserId == username);
                    Role dbrole = db.Roles.FirstOrDefault(r => r.RoleName == roleName);
                    dbuser.Roles.Remove(dbrole);
                    db.SaveChanges();
                    
                }
            }
        }
    }

    /// <summary>
    ///     Gets a value indicating whether the specified role name already exists in the role data source
    /// </summary>
    /// <param name="roleName">The name of the role to search for in the data source</param>
    /// <returns>True if the role name already exists in the data source for the configured applicationName; otherwise, false.</returns>
    public override bool RoleExists(string roleName)
    {
        if (roleName == null || roleName == "")
            throw new ProviderException("Role name cannot be empty or null.");

        using (JobZoomEntities db = new JobZoomEntities())
        {
            return db.Roles.Any(r => r.RoleName == roleName);
        }
    }
    #endregion

    #region Methods
    /// <summary>
    ///     A helper function to retrieve config values from the configuration file.
    /// </summary>
    /// <param name="configValue">Config value to retrieve</param>
    /// <param name="defaultValue">Default value</param>
    /// <returns>Return the config value if it exists; otherwise, return the defaultValue</returns>
    private string GetConfigValue(string configValue, string defaultValue)
    {
        if (string.IsNullOrEmpty(configValue))
            return defaultValue;

        return configValue;
    }
    #endregion
}
