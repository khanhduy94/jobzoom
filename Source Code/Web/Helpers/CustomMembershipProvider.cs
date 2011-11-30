using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using JobZoom.Business.Entities;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mail;


public class CustomMembershipProvider : MembershipProvider
{
    //
    // Properties from web.config, default all to False
    //

    #region Properties
    private string _ApplicationName;
    private bool _EnablePasswordReset;
    private bool _EnablePasswordRetrieval = false;
    private bool _RequiresQuestionAndAnswer = false;
    private bool _RequiresUniqueEmail;
    private int _MaxInvalidPasswordAttempts;
    private int _PasswordAttemptWindow;
    private int _MinRequiredPasswordLength;
    private int _MinRequiredNonalphanumericCharacters;
    private string _PasswordStrengthRegularExpression;
    private MembershipPasswordFormat _PasswordFormat = MembershipPasswordFormat.Hashed;
    #endregion

    #region Fields
    public override int MaxInvalidPasswordAttempts
    {
        get { return _MaxInvalidPasswordAttempts; }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
        get { return _MinRequiredNonalphanumericCharacters; }
    }

    public override int MinRequiredPasswordLength
    {
        get { return _MinRequiredPasswordLength; }
    }

    public override int PasswordAttemptWindow
    {
        get { return _PasswordAttemptWindow; }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
        get { return _PasswordFormat; }
    }

    public override string PasswordStrengthRegularExpression
    {
        get { return _PasswordStrengthRegularExpression; }
    }

    public override bool RequiresQuestionAndAnswer
    {
        get { return _RequiresQuestionAndAnswer; }
    }

    public override bool RequiresUniqueEmail
    {
        get { return _RequiresUniqueEmail; }
    }

    public override bool EnablePasswordReset
    {
        get { return _EnablePasswordReset; }
    }

    public override bool EnablePasswordRetrieval
    {
        get { return _EnablePasswordRetrieval; }
    }

    public override string ApplicationName
    {
        get { return _ApplicationName; }
        set { _ApplicationName = value; }
    }
    #endregion

    #region Override Methods
    public override void Initialize(string name, NameValueCollection config)
    {
        if (config == null)
            throw new ArgumentNullException("config");

        if (name == null || name.Length == 0)
            name = "CustomMembershipProvider";

        if (String.IsNullOrEmpty(config["description"]))
        {
            config.Remove("description");
            config.Add("description", "Custom Membership Provider");
        }

        base.Initialize(name, config);

        _ApplicationName = GetConfigValue(config["applicationName"],
                      System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        _MaxInvalidPasswordAttempts = Convert.ToInt32(
                      GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
        _PasswordAttemptWindow = Convert.ToInt32(
                      GetConfigValue(config["passwordAttemptWindow"], "10"));
        _MinRequiredNonalphanumericCharacters = Convert.ToInt32(
                      GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
        _MinRequiredPasswordLength = Convert.ToInt32(
                      GetConfigValue(config["minRequiredPasswordLength"], "6"));
        _EnablePasswordReset = Convert.ToBoolean(
                      GetConfigValue(config["enablePasswordReset"], "true"));
        _PasswordStrengthRegularExpression = Convert.ToString(
                       GetConfigValue(config["passwordStrengthRegularExpression"], ""));
        _RequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

    }



    /// <summary>
    ///     Processes a request to update the password for a membership user
    /// </summary>
    /// <param name="username">The user to update the password for. </param>
    /// <param name="oldPassword">The current password for the specified user. </param>
    /// <param name="newPassword">The new password for the specified user. </param>
    /// <returns></returns>
    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
        if (ValidateUser(username, oldPassword))
            return false;

        //Validate new password
        ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
        OnValidatingPassword(args);
        if (args.Cancel)
        {
            if (args.FailureInformation != null)
                throw args.FailureInformation;
            else
                throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
        }

        using (JobZoomEntities db = new JobZoomEntities())
        {
            try
            {
                User dbuser = db.Users.First(u => u.UserId == username);
                dbuser.PasswordSalt = CreateSalt();
                dbuser.Password = CreatePasswordHash(newPassword, dbuser.PasswordSalt);
                dbuser.LastPasswordChangedDate = DateTime.Now;
                dbuser.LastActivityDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new ProviderException(ex.Message);
            }
        }
    }

    //
    //Because we will not implement password reset using security question and answer
    //
    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
        return false;
    }

    /// <summary>
    ///     Adds a new membership user to the data source.
    /// </summary>
    /// <param name="username">The user name for the new user. </param>
    /// <param name="password">The password for the new user. </param>
    /// <param name="email">The e-mail address for the new user.</param>
    /// <param name="passwordQuestion">The password question for the new user.</param>
    /// <param name="passwordAnswer">The password answer for the new user</param>
    /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
    /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
    /// <param name="status">A MembershipCreateStatus enumeration value indicating whether the user was created successfully.</param>
    /// <returns>A MembershipUser object populated with the information for the newly created user.</returns>
    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
        ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
        OnValidatingPassword(args);

        if (args.Cancel)
        {
            status = MembershipCreateStatus.InvalidPassword;
            return null;
        }

        if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
        {
            status = MembershipCreateStatus.DuplicateEmail;
            return null;
        }

        MembershipUser user = GetUser(username, false);

        if (user == null)
        {
            this.CreateUser(username, password, email);
            status = MembershipCreateStatus.Success;

            return GetUser(username, false);
        }
        else
            status = MembershipCreateStatus.DuplicateUserName;


        return null;
    }

    /// <summary>
    ///     Removes a user from the membership data source. 
    /// </summary>
    /// <param name="username">The name of the user to delete.</param>
    /// <param name="deleteAllRelatedData">True to delete data related to the user from the database; false to leave data related to the user in the database.</param>
    /// <returns>true if the user was successfully deleted; otherwise, false.</returns>
    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
        if (!deleteAllRelatedData)
            throw new ProviderException("You must delete all data that related to the user in the database.");

        using (JobZoomEntities db = new JobZoomEntities())
        {
            try
            {
                User dbuser = db.Users.First(u => u.UserId == username);
                db.DeleteObject(dbuser);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new ProviderException("Please set CASCADE to all table that related to the user in the database. " + ex.Message);
            }
        }
    }

    /// <summary>
    ///     Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
    /// </summary>
    /// <param name="emailToMatch">The e-mail address to search for.</param>
    /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
    /// <param name="pageSize">The size of the page of results to return.</param>
    /// <param name="totalRecords">The total number of matched users.</param>
    /// <returns>A MembershipUserCollection collection that contains a page of pageSize MembershipUser objects beginning at the page specified by pageIndex.</returns>
    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Gets a collection of membership users where the user name contains the specified user name to match.
    /// </summary>
    /// <param name="usernameToMatch">The user name to search for.</param>
    /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
    /// <param name="pageSize">The size of the page of results to return.</param>
    /// <param name="totalRecords">The total number of matched users.</param>
    /// <returns>A MembershipUserCollection collection that contains a page of pageSize MembershipUser objects beginning at the page specified by pageIndex.</returns>
    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        totalRecords = 0;
        using (JobZoomEntities db = new JobZoomEntities())
        {
            var allMatchUsers = (from u in db.Users where u.UserId.Contains(usernameToMatch) orderby u.UserId ascending select u);
            totalRecords = allMatchUsers.Count();

            var results = allMatchUsers.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            MembershipUserCollection _users = new MembershipUserCollection();

            foreach (User user in results)
            {
                _users.Add(new MembershipUser("CustomMembershipProvider",
                                                              user.UserId,
                                                              user.UserId,
                                                              user.Email,
                                                              "",
                                                              user.Comments,
                                                              user.IsActivated,
                                                              user.IsLockedOut,
                                                              user.CreatedDate,
                                                              user.LastLoginDate,
                                                              user.LastActivityDate,
                                                              user.LastPasswordChangedDate,
                                                              user.LastLockedOutDate));

            }
            return _users;
        }
    }

    /// <summary>
    ///     Gets a collection of all the users in the data source in pages of data.
    /// </summary>
    /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
    /// <param name="pageSize">The size of the page of results to return.</param>
    /// <param name="totalRecords">The total number of matched users.</param>
    /// <returns></returns>
    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
        totalRecords = 0;
        using (JobZoomEntities db = new JobZoomEntities())
        {
            var allusers = (from u in db.Users orderby u.UserId ascending select u);
            totalRecords = allusers.Count();

            var results = allusers.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            MembershipUserCollection _users = new MembershipUserCollection();

            foreach (User user in results)
            {
                _users.Add(new MembershipUser("CustomMembershipProvider",
                                                              user.UserId,
                                                              user.UserId,
                                                              user.Email,
                                                              "",
                                                              user.Comments,
                                                              user.IsActivated,
                                                              user.IsLockedOut,
                                                              user.CreatedDate,
                                                              user.LastLoginDate,
                                                              user.LastActivityDate,
                                                              user.LastPasswordChangedDate,
                                                              user.LastLockedOutDate));

            }
            return _users;
        }
    }

    /// <summary>
    ///     Gets the number of users currently accessing the application.
    /// </summary>
    /// <returns>The number of users currently accessing the application.</returns>
    public override int GetNumberOfUsersOnline()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Gets the password for the specified user name from the data source.
    /// </summary>
    /// <param name="username">The user to retrieve the password for. </param>
    /// <param name="answer">The password answer for the user. </param>
    /// <returns>The password for the specified user name.</returns>
    public override string GetPassword(string username, string answer)
    {
        throw new ProviderException("Get password function by the security answer is not support!");
    }

    /// <summary>
    ///     Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
    /// </summary>
    /// <param name="username">The name of the user to get information for. </param>
    /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user. </param>
    /// <returns>A MembershipUser object populated with the specified user's information from the data source.</returns>
    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            var result = from u in db.Users where (u.UserId == username) select u;

            if (result.Count() != 0)
            {
                try
                {
                    User dbuser = result.FirstOrDefault();

                    string _username = dbuser.UserId;
                    string _providerUserKey = dbuser.UserId;
                    string _email = dbuser.Email;
                    string _passwordQuestion = "";
                    string _comment = dbuser.Comments;
                    bool _isApproved = dbuser.IsActivated;
                    bool _isLockedOut = dbuser.IsLockedOut;
                    DateTime _creationDate = dbuser.CreatedDate;
                    DateTime _lastLoginDate = dbuser.LastLoginDate;
                    DateTime _lastActivityDate = dbuser.LastActivityDate;
                    DateTime _lastPasswordChangedDate = dbuser.LastPasswordChangedDate;
                    DateTime _lastLockedOutDate = dbuser.LastLockedOutDate;

                    MembershipUser user = new MembershipUser("JobZoomMembershipProvider",
                                                              _username,
                                                              _providerUserKey,
                                                              _email,
                                                              _passwordQuestion,
                                                              _comment,
                                                              _isApproved,
                                                              _isLockedOut,
                                                              _creationDate,
                                                              _lastLoginDate,
                                                              _lastActivityDate,
                                                              _lastPasswordChangedDate,
                                                              _lastLockedOutDate);

                    //update the user activity date
                    if (userIsOnline)
                    {
                        dbuser.LastActivityDate = DateTime.Now;
                        db.SaveChanges();
                    }

                    return user;
                }
                catch (Exception ex)
                {
                    throw new ProviderException(ex.Message);
                }
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    ///     Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
    /// </summary>
    /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
    /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
    /// <returns>A MembershipUser object populated with the specified user's information from the data source.</returns>
    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
        if (providerUserKey == null)
        {
            return null;
        }
        else
        {
            using (JobZoomEntities db = new JobZoomEntities())
            {
                var result = from u in db.Users where (u.UserId == providerUserKey.ToString()) select u;

                if (result.Count() != 0)
                {
                    try
                    {
                        User dbuser = result.FirstOrDefault();

                        string _username = dbuser.UserId;
                        string _providerUserKey = dbuser.UserId;
                        string _email = dbuser.Email;
                        string _passwordQuestion = "";
                        string _comment = dbuser.Comments;
                        bool _isApproved = dbuser.IsActivated;
                        bool _isLockedOut = dbuser.IsLockedOut;
                        DateTime _creationDate = dbuser.CreatedDate;
                        DateTime _lastLoginDate = dbuser.LastLoginDate;
                        DateTime _lastActivityDate = dbuser.LastActivityDate;
                        DateTime _lastPasswordChangedDate = dbuser.LastPasswordChangedDate;
                        DateTime _lastLockedOutDate = dbuser.LastLockedOutDate;

                        MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                                  _username,
                                                                  _providerUserKey,
                                                                  _email,
                                                                  _passwordQuestion,
                                                                  _comment,
                                                                  _isApproved,
                                                                  _isLockedOut,
                                                                  _creationDate,
                                                                  _lastLoginDate,
                                                                  _lastActivityDate,
                                                                  _lastPasswordChangedDate,
                                                                  _lastLockedOutDate);

                        //update the user activity date
                        if (userIsOnline)
                        {
                            dbuser.LastActivityDate = DateTime.Now;
                            db.SaveChanges();
                        }

                        return user;
                    }
                    catch (Exception ex)
                    {
                        throw new ProviderException(ex.Message);
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }

    /// <summary>
    ///     Gets the user name associated with the specified e-mail address.
    /// </summary>
    /// <param name="email">The e-mail address to search for. </param>
    /// <returns>The user name associated with the specified e-mail address. If no match is found, return null.</returns>
    public override string GetUserNameByEmail(string email)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            var result = from u in db.Users where (u.Email == email) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.FirstOrDefault();

                return dbuser.UserId;
            }
            else
            {
                return "";
            }
        }
    }

    /// <summary>
    ///     Resets a user's password to a new, automatically generated password.
    /// </summary>
    /// <param name="username">The user to reset the password for. </param>
    /// <param name="answer">The password answer for the specified user. </param>
    /// <returns>The new password for the specified user.</returns>
    public override string ResetPassword(string username, string answer)
    {
        throw new ProviderException("The reset password function is not support!");
    }

    /// <summary>
    ///     Clears a lock so that the membership user can be validated.
    /// </summary>
    /// <param name="userName">The membership user whose lock status you want to clear.</param>
    /// <returns>true if the membership user was successfully unlocked; otherwise, false.</returns>
    public override bool UnlockUser(string userName)
    {
        if (this.GetUser(userName, false) == null)
        {
            throw new ProviderException(userName + "doesn't exists!");
        }

        try
        {
            using (JobZoomEntities db = new JobZoomEntities())
            {
                User dbuser = db.Users.First(u => u.UserId == userName);
                dbuser.IsLockedOut = true;
                dbuser.LastLockedOutDate = DateTime.Now;
                dbuser.LastModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            throw new ProviderException("Can't unlock. " + ex.Message);
        }
    }

    /// <summary>
    ///     Updates information about a user in the data source.
    /// </summary>
    /// <param name="user">A MembershipUser object that represents the user to update and the updated information for the user. </param>
    public override void UpdateUser(MembershipUser user)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            User dbuser = db.Users.First(u => u.UserId == user.ProviderUserKey.ToString());
            dbuser.UserId = user.UserName;
            dbuser.Email = user.Email;
            dbuser.Comments = user.Comment;
            dbuser.CreatedDate = user.CreationDate;
            dbuser.LastActivityDate = user.LastActivityDate;
            dbuser.LastLoginDate = user.LastLoginDate;
            dbuser.LastLockedOutDate = user.LastLockoutDate;
            dbuser.LastPasswordChangedDate = user.LastPasswordChangedDate;
            dbuser.IsOnline = user.IsOnline;
            dbuser.IsActivated = user.IsApproved;

            //continue.....
            db.SaveChanges();
        }
    }

    /// <summary>
    ///     Verifies that the specified user name and password exist in the data source.
    /// </summary>
    /// <param name="username">The name of the user to validate. </param>
    /// <param name="password">The password for the specified user. </param>
    /// <returns>true if the specified username and password are valid; otherwise, false.</returns>
    public override bool ValidateUser(string username, string password)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            var result = from u in db.Users where (u.UserId == username) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.First();

                if (dbuser.Password == CreatePasswordHash(password, dbuser.PasswordSalt))
                {
                    if (dbuser.IsActivated && !dbuser.IsLockedOut)
                    {
                        //Update LastLoginDate
                        dbuser.LastLoginDate = dbuser.LastActivityDate;
                        dbuser.LastActivityDate = DateTime.Now;
                        db.SaveChanges();
                        return true;
                    }
                    else if (!dbuser.IsActivated)
                        throw new ProviderException("You must activate your account before logging!");
                    else
                        throw new ProviderException("Your account is being locked by administrator! Reason: " + dbuser.LastLockedOutReason);
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
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

    /// <summary>
    ///     Adds a new membership user to the data source.
    /// </summary>
    /// <param name="username">The user name for the new user.</param>
    /// <param name="password">The password for the new user. </param>
    /// <param name="email">The email address for the new user. </param>
    /// <returns></returns>
    public MembershipUser CreateUser(string username, string password, string email)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            User user = new User();

            user.UserId = username;
            user.Email = email;
            user.PasswordSalt = CreateSalt(); //MembershipPasswordFormat _PasswordFormat = MembershipPasswordFormat.Hashed;
            user.Password = CreatePasswordHash(password, user.PasswordSalt);
            user.CreatedDate = DateTime.Now;
            user.IsActivated = false;
            user.IsLockedOut = false;
            user.LastLockedOutDate = DateTime.Now;
            user.LastLoginDate = DateTime.Now;
            user.LastActivityDate = DateTime.Now;
            user.LastPasswordChangedDate = DateTime.Now;
            user.NewEmailKey = GenerateKey();
            db.AddToUsers(user);
            db.SaveChanges();

            sendActivatedEmail(user);

            return GetUser(username, false);

        }
    }

    /// <summary>
    ///     Create a password salt
    /// </summary>
    /// <returns>A password salt</returns>
    private static string CreateSalt()
    {
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        byte[] buff = new byte[32];
        rng.GetBytes(buff);

        return Convert.ToBase64String(buff);
    }

    /// <summary>
    ///     Create a hashed password with a password salt
    /// </summary>
    /// <param name="password">The password to create</param>
    /// <param name="salt">The password salt</param>
    /// <returns>A hashed password</returns>
    private static string CreatePasswordHash(string password, string salt)
    {
        string saltAndPwd = String.Concat(password, salt);
        string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
        return hashedPwd;
    }

    /// <summary>
    ///     Generate a guid key
    /// </summary>
    /// <returns>New guid key</returns>
    private static string GenerateKey()
    {
        Guid emailKey = Guid.NewGuid();
        return emailKey.ToString();
    }

    /// <summary>
    ///     Activate user to log in
    /// </summary>
    /// <param name="username">Username to activate</param>
    /// <param name="key">Key to valid</param>
    /// <returns>true if user was activated successfully; otherwise, false.</returns>
    public bool ActivateUser(string username, string key)
    {
        using (JobZoomEntities db = new JobZoomEntities())
        {
            var result = (from u in db.Users where (u.UserId == username) select u);

            if (result.Count() != 0)
            {
                var dbuser = result.First();
                if (dbuser.NewEmailKey == key)
                {
                    dbuser.IsActivated = true;
                    dbuser.LastModifiedDate = DateTime.Now;
                    dbuser.LastActivityDate = DateTime.Now;
                    dbuser.NewEmailKey = null;

                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
    }


    /// <summary>
    ///     Send activated email to user
    /// </summary>
    /// <param name="user">User to send the activated email</param>
    public void sendActivatedEmail(User user)
    {
        //send activation link to email
        Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~/Web.config");

        MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

        if (mailSettings != null)
        {
            string ActivationLink = "http://localhost:29204/Account/Activate/" +
                              user.UserId + "/" + user.NewEmailKey;

            var message = new MailMessage(mailSettings.Smtp.From, user.Email)
            {
                Subject = "Activate your account!",
                Body = ActivationLink
            };

            var client = new SmtpClient(mailSettings.Smtp.Network.Host, mailSettings.Smtp.Network.Port);
            client.Credentials = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);
            client.EnableSsl = mailSettings.Smtp.Network.EnableSsl;

            client.Send(message);
        }
    }


    /// <summary>
    ///     Resend the activated email to user
    /// </summary>
    /// <param name="email">The user email</param>
    public bool resendActivatedEmail(string email)
    {

        using (JobZoomEntities db = new JobZoomEntities())
        {
            var result = from u in db.Users where (u.Email == email) select u;

            if (result.Count() > 0)
            {
                sendActivatedEmail(result.FirstOrDefault());
                return true;
            }
            else
                return false;
        }
    }
    #endregion

}