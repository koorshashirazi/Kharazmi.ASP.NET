using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mvc.Utility.Core.Managers.ActiveDirectory
{
    public class AdUserProperty
    {
        public const string LOCAL_USER = "localuser";

        public const string DESCRIPTION = "description";

        public const string OBJECTCLASS = "objectClass";

        public const string CONTAINERNAME = "cn";
        public const string COMMON_NAME = "CN=";


        public const string LASTNAME = "sn";

        public const string COUNTRYNOTATION = "c";

        public const string CITY = "l";

        public const string STATE = "st";

        public const string TITLE = "title";

        public const string POSTALCODE = "postalCode";

        public const string PHYSICALDELIVERYOFFICENAME = "physicalDeliveryOfficeName";

        public const string FIRSTNAME = "givenName";

        public const string MIDDLENAME = "initials";

        public const string DISTINGUISHEDNAME = "distinguishedName";

        public const string INSTANCETYPE = "instanceType";

        public const string WHENCREATED = "whenCreated";

        public const string WHENCHANGED = "whenChanged";

        public const string DISPLAYNAME = "displayName";

        public const string USNCREATED = "uSNCreated";

        public const string MEMBEROF = "memberOf";

        public const string USNCHANGED = "uSNChanged";

        public const string COUNTRY = "co";

        public const string DEPARTMENT = "department";

        public const string COMPANY = "company";

        public const string STREETADDRESS = "streetAddress";

        public const string DIRECTREPORTS = "directReports";

        public const string NAME = "name";

        public const string OBJECTGUID = "objectGUID";

        public const string USERACCOUNTCONTROL = "userAccountControl";

        public const string BADPWDCOUNT = "badPwdCount";

        public const string CODEPAGE = "codePage";

        public const string COUNTRYCODE = "countryCode";

        public const string BADPASSWORDTIME = "badPasswordTime";

        public const string LASTLOGOFF = "lastLogoff";

        public const string LASTLOGON = "lastLogon";

        public const string PWDLASTSET = "pwdLastSet";

        public const string PRIMARYGROUPID = "primaryGroupID";

        public const string OBJECTSID = "objectSid";

        public const string ADMINCOUNT = "adminCount";

        public const string ACCOUNTEXPIRES = "accountExpires";

        public const string LOGONCOUNT = "logonCount";

        public const string LOGINNAME = "sAMAccountName";

        public const string SAMACCOUNTTYPE = "sAMAccountType";

        public const string SHOWINADDRESSBOOK = "showInAddressBook";

        public const string LEGACYEXCHANGEDN = "legacyExchangeDN";

        public const string USERPRINCIPALNAME = "userPrincipalName";

        public const string EXTENSION = "ipPhone";

        public const string SERVICEPRINCIPALNAME = "servicePrincipalName";

        public const string OBJECTCATEGORY = "objectCategory";

        public const string LASTLOGONTIMESTAMP = "lastLogonTimestamp";

        public const string EMAILADDRESS = "mail";

        public const string MANAGER = "manager";

        public const string MOBILE = "mobile";

        public const string PAGER = "pager";

        public const string FAX = "facsimileTelephoneNumber";

        public const string HOMEPHONE = "homePhone";

        public const string MSEXCHUSERACCOUNTCONTROL = "msExchUserAccountControl";

        public const string MDBUSEDEFAULTS = "mDBUseDefaults";

        public const string MSEXCHMAILBOXSECURITYDESCRIPTOR = "msExchMailboxSecurityDescriptor";

        public const string HOMEMDB = "homeMDB";

        public const string MSEXCHPOLICIESINCLUDED = "msExchPoliciesIncluded";

        public const string HOMEMTA = "homeMTA";

        public const string MSEXCHRECIPIENTTYPEDETAILS = "msExchRecipientTypeDetails";

        public const string MAILNICKNAME = "mailNickname";

        public const string MSEXCHHOMESERVERNAME = "msExchHomeServerName";

        public const string MSEXCHVERSION = "msExchVersion";

        public const string MSEXCHRECIPIENTDISPLAYTYPE = "msExchRecipientDisplayType";

        public const string MSEXCHMAILBOXGUID = "msExchMailboxGuid";

        public const string NTSECURITYDESCRIPTOR = "nTSecurityDescriptor";

        public const string SET_PASSWORD = "SetPassword";

        public const string USER_SCHEMA = "user";

        public const string GROUP_SCHEMA = "group";
    }

    public class User
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string LocalDomain { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
        public Manager Supervisor { get; set; }
    }

    public struct Manager
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }

    public enum ObjectClass
    {
        User,
        Group,
        Computer
    }

    public enum ReturnType
    {
        DistinguishedName,
        ObjectGuid
    }

    public static class ActiveDirectoryExtensions
    {
        public static User ToUser(this Principal principal, DirectoryEntry directoryEntry)
        {
            using (var dirSearch = new DirectorySearcher(directoryEntry)
            {
                Filter = "(&(objectClass=user)(" + AdUserProperty.LOGINNAME + "=" + principal.SamAccountName + "))"
            })
            {
                var results = dirSearch.FindOne();

                if (results == null) return null;
                using (var tempUser = new DirectoryEntry(results.Path))
                {
                    return tempUser.ToUser();
                }
            }
        }

        public static User ToUser(this DirectoryEntry directoryEntry)
        {
            var user = new User { UserName = GetProperty(directoryEntry, AdUserProperty.LOGINNAME) };
            {
                var pager = GetProperty(directoryEntry, AdUserProperty.PAGER);
                var ipPhone = GetProperty(directoryEntry, AdUserProperty.EXTENSION);
                var empId = string.IsNullOrEmpty(pager) ? ipPhone : pager;
                //In my case, the organization uses theses fields for store personal data
                empId = empId.Replace("ID", "").Replace(" ", "");
                if (long.TryParse(empId, out var result))
                    user.UserId = result;
            }
            user.DisplayName = GetProperty(directoryEntry, AdUserProperty.DISPLAYNAME);
            user.Name = GetProperty(directoryEntry, AdUserProperty.FIRSTNAME);
            user.LastName = GetProperty(directoryEntry, AdUserProperty.LASTNAME);
            user.Description = GetProperty(directoryEntry, AdUserProperty.DESCRIPTION);
            user.EMail = GetProperty(directoryEntry, AdUserProperty.EMAILADDRESS);
            return user;
        }

        public static string GetProperty(DirectoryEntry userDetail, string propertyName)
        {
            return userDetail.Properties.Contains(propertyName)
                ? userDetail.Properties[propertyName][0].ToString()
                : string.Empty;
        }
    }


    public class ActiveDirectoryManager
    {
        //public const string LOCAL_DOMAIN = "@organization.local";
        //public const string localDomain = "organization.com";
        //public const string DOMAIN_ADDRESS = "OrgAnizaTion";
        //private const string TemporalPassword = "temporal2014";

        private DirectoryEntry GetDirectoryEntry()
        {
            return new DirectoryEntry();
        }

        private DirectoryEntry GetDirectoryEntry(string path)
        {
            return new DirectoryEntry(path);
        }

        private DirectoryEntry GetDirectoryEntry(string path, string userName, string password)
        {
            return new DirectoryEntry(path, userName, password);
        }

        private DirectoryEntry GetDirectoryEntry(string path, string userName, string password,
            AuthenticationTypes authenticationTypes = AuthenticationTypes.None)
        {
            return new DirectoryEntry(path, userName, password, authenticationTypes);
        }


        public PrincipalContext Create(ContextType type, string name, string container, ContextOptions options,
            string userName, string password)
        {
            return new PrincipalContext(type, name, container, options, userName, password);
        }

        public ComputerPrincipal Create(PrincipalContext context, string displayName, string samAccountName,
            bool enable, string password)
        {
            var computer = new ComputerPrincipal(context)
            {
                DisplayName = displayName,
                SamAccountName = samAccountName,
                Enabled = enable
            };

            computer.SetPassword(password);

            return computer;
        }

        public UserPrincipal Create(PrincipalContext context)
        {
            var userObj = new UserPrincipal(context)
            {
                Enabled = true,
                PasswordNeverExpires = false
            };
            return userObj;
        }

        public GroupPrincipal Create(PrincipalContext context, string groupName)
        {
            return new GroupPrincipal(context, groupName);
        }

        public bool ValidateLogin(string username, string password)
        {
            bool valid;
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                valid = context.ValidateCredentials(username, password);
            }

            return valid;
        }

        public string CreateEmail(string domainAddress, string user)
        {
            return $"{user.ToLowerInvariant()}@{domainAddress}";
        }

        public bool Disable(PrincipalContext context, string userName)
        {
            var user = UserPrincipal.FindByIdentity(context, userName);
            if (user == null) return false;

            user.Enabled = false;
            user.Save();
            return true;
        }

        /// <summary>
        ///     Checks if exists.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool Exists(PrincipalContext context, string userName)
        {
            var userObj = UserPrincipal.FindByIdentity(context, userName);

            //  if not null, exists
            return userObj != null;
        }

        public IEnumerable<string> FilterNonExist(PrincipalContext context, IEnumerable<string> users)
        {
            return users.Where(userName => !Exists(context, userName)).ToList();
        }

        /// <summary>
        ///     Checks if exists.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserPrincipal GetUserPrincipal(PrincipalContext context, string userName)
        {
            var userObj = new UserPrincipal(context)
            {
                SamAccountName = userName
            };

            using (var searcher = new PrincipalSearcher { QueryFilter = userObj })
            {
                return searcher.FindOne() as UserPrincipal;
            }
        }

        public Principal Find(UserPrincipal user)
        {
            using (var pS = new PrincipalSearcher { QueryFilter = user })
            {
                return pS.FindOne();
            }
        }

        public Principal FindByUserName(PrincipalContext context, string userName)
        {
            var user = new UserPrincipal(context) { Name = userName };
            using (var pS = new PrincipalSearcher { QueryFilter = user })
            {
                return pS.FindOne();
            }
        }

        public IEnumerable<Principal> FindAllByUserName(PrincipalContext context, string userName)
        {
            // create a principal object representation to describe 
            // what will be searched 
            var user = new UserPrincipal(context) { Name = userName };
            // define the properties of the search (this can use wildcards) 
            // create a principal searcher for running a search operation 
            using (var pS = new PrincipalSearcher { QueryFilter = user })
            {
                // run the query 
                var results = pS.FindAll();
                foreach (var result in results) yield return result;
            }
        }

        public IEnumerable<Principal> GetAuthorizationGroupsFor(PrincipalContext context, string userName)
        {
            var userPrincipal = UserPrincipal.FindByIdentity(context, userName);

            if (userPrincipal == null) yield break;
            var results = userPrincipal.GetAuthorizationGroups();
            foreach (var result in results) yield return result;
        }


        public User GetUserByUserName(string path, string category, string userName)
        {
            using (var dirSearch = new DirectorySearcher(GetDirectoryEntry(path))
            {
                Filter = "(&((&(objectCategory=" + category + ")(objectClass=User)))(mail=" + userName + "))"
            })
            {
                var results = dirSearch.FindOne();

                if (results == null) return null;
                using (var tempUser = new DirectoryEntry(results.Path))
                {
                    return tempUser.ToUser();
                }
            }
        }

        /// <summary>
        ///     Looks for an User using the Domain as datasource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUserByUserName(string path, string userName)
        {
            using (var dirSearch = new DirectorySearcher(GetDirectoryEntry(path))
            {
                Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))"
            })
            {
                var results = dirSearch.FindOne();

                if (results == null) return null;
                using (var tempUser = new DirectoryEntry(results.Path))
                {
                    return tempUser.ToUser();
                }
            }
        }


        public User GetUserByEmail(string path, string email)
        {
            using (var dirSearch = new DirectorySearcher(GetDirectoryEntry(path))
            {
                Filter = "(&(objectClass=user)(mail=" + email + "))"
            })
            {
                var results = dirSearch.FindOne();

                if (results == null) return null;
                using (var tempUser = new DirectoryEntry(results.Path))
                {
                    return tempUser.ToUser();
                }
            }
        }

        public User GetUserByEmail(string path, string category, string email)
        {
            using (var dirSearch = new DirectorySearcher(GetDirectoryEntry(path))
            {
                Filter = "(&((&(objectCategory=" + category + ")(objectClass=User)))(mail=" + email + "))"
            })
            {
                var results = dirSearch.FindOne();

                if (results == null) return null;
                using (var tempUser = new DirectoryEntry(results.Path))
                {
                    return tempUser.ToUser();
                }
            }
        }

        /// <summary>
        ///     Looks for an User using the Domain as datasource
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="id"></param>
        /// <returns>A matched and filled User, otherwise, null.</returns>
        public User GetUserByUserId(string serverPath, string id)
        {
            var dirSearch = new DirectorySearcher(GetDirectoryEntry(serverPath))
            {
                Filter = "(&(objectClass=user)(|(ipPhone=*" + id + ")(pager=*" + id + ")))"
            };
            var results = dirSearch.FindOne();

            if (results == null) return null;
            var tempUser = new DirectoryEntry(results.Path);
            return tempUser.ToUser();
        }

        /// <summary>
        ///     Looks for an element by any of his fields.
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="name"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public IEnumerable<User> GetUsers(string serverPath, string name, string userName, string email,
            string description)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email) &&
                string.IsNullOrEmpty(description))
                return new User[0];

            var builder = new StringBuilder("(&(objectClass=user)(&");

            if (!string.IsNullOrEmpty(userName))
                builder.AppendFormat("({0}={1})", AdUserProperty.LOGINNAME, userName);
            if (!string.IsNullOrEmpty(name))
                builder.AppendFormat("({0}=*{1}*)", AdUserProperty.NAME, name);
            if (!string.IsNullOrEmpty(email))
                builder.AppendFormat("({0}={1})", AdUserProperty.EMAILADDRESS, email);
            if (!string.IsNullOrEmpty(description))
                builder.AppendFormat("({0}=*{1}*)", AdUserProperty.DESCRIPTION, description);

            //builder.AppendFormat("({0}={1})", ADUserProperty.USERACCOUNTCONTROL, 512);    //¿?

            builder.Append("))");

            var dirSearch = new DirectorySearcher(GetDirectoryEntry(serverPath)) { Filter = builder.ToString() };
            var results = dirSearch.FindAll();

            return results.Count > 0
                ? results.OfType<SearchResult>().Select(i => i.GetDirectoryEntry().ToUser())
                : new User[0];
        }

        /// <summary>
        ///     Exceptions:
        ///     System.InvalidOperationException:
        ///     The principal has not yet been associated with a System.DirectoryServices.AccountManagement.PrincipalContext
        ///     object.This type of principal cannot be inserted in the store.
        ///     System.DirectoryServices.AccountManagement.PrincipalOperationException:
        ///     An exception occurred when saving changes to the store, or updating the group
        ///     membership in the store.
        ///     System.DirectoryServices.AccountManagement.PrincipalExistsException:
        ///     The principal already occurs in the store.
        ///     System.DirectoryServices.AccountManagement.PasswordException:
        ///     The password does not meet complexity requirements.
        /// </summary>
        /// <param name="user"></param>
        public void Save(UserPrincipal user)
        {
            user.ExpirePasswordNow();
            user.Save();
        }

        public void Save(string serverPath, PrincipalContext context, User user, string officeName, string password)
        {
            //This is a easy way to create an User.
            using (var userPrincipal = Create(context))
            {
                userPrincipal.DisplayName = user.DisplayName;
                userPrincipal.EmailAddress = user.EMail;
                userPrincipal.VoiceTelephoneNumber = user.Phone;

                userPrincipal.GivenName = user.Name;
                userPrincipal.MiddleName = user.LastName;
                userPrincipal.Description = user.Description;

                userPrincipal.UserPrincipalName = user.UserName + user.LocalDomain;
                userPrincipal.SamAccountName = user.UserName;

                userPrincipal.Enabled = true;
                Save(userPrincipal);
            }

            // I wait, since sometimes the Active Directory/Server doesn't commit inmediatly.
            Task.Delay(TimeSpan.FromSeconds(5)).Wait();

            /*
             *  Update Properties
             */

            var dirSearch = new DirectorySearcher(GetDirectoryEntry(serverPath))
            {
                Filter = "(&(objectClass=user)(" + AdUserProperty.LOGINNAME + "=" + user.UserName + "))"
            };
            var results = dirSearch.FindOne();

            if (results == null) return;

            var entry = new DirectoryEntry(results.Path);

            entry.Invoke("SetPassword", password);
            entry.Properties[AdUserProperty.PAGER].Value = user.UserId.ToString();
            entry.Properties[AdUserProperty.FIRSTNAME].Value = user.Name;
            entry.Properties[AdUserProperty.LASTNAME].Value = user.LastName;
            entry.Properties[AdUserProperty.DESCRIPTION].Value = user.Description;
            entry.Properties[AdUserProperty.EMAILADDRESS].Value = user.EMail;
            entry.Properties[AdUserProperty.HOMEPHONE].Value = user.Phone;

            // This makes the user change the password at the next login
            entry.Properties[AdUserProperty.PWDLASTSET].Value = 0;

            entry.Properties[AdUserProperty.TITLE].Value = user.Description;
            entry.Properties[AdUserProperty.DEPARTMENT].Value = user.Department;
            entry.Properties[AdUserProperty.COMPANY].Value = officeName;
            //entry.Properties[ADUserProperty.MANAGER].Value = String.Format("CN={0},CN=Users,DC=organization,DC=local", user.Supervisor.Name);

            // Attributes (codes) http://www.selfadsi.org/ads-attributes/user-userAccountControl.htm
            var val = (int)entry.Properties[AdUserProperty.USERACCOUNTCONTROL].Value;
            entry.Properties[AdUserProperty.USERACCOUNTCONTROL].Value = val & ~0x2; // (512 - Active)

            entry.CommitChanges();
            entry.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="id"></param>
        /// <returns>Object containing basic data.</returns>
        public Manager GetManagerByEmpId(string serverPath, int id)
        {
            var user = GetUserByUserId(serverPath, id + "");
            return user != null ? new Manager { UserId = id, Name = user.DisplayName } : new Manager();
        }

        /// <summary>
        ///     ActiveDirectory basic's rules or policy on the password.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>true - if valid, otherwise, false.</returns>
        public bool IsValidPassword(string text)
        {
            return text.Length > 6;
        }

        /// <summary>
        ///     Rename an object and specify the domain controller and credentials directly
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newName"></param>
        public void Rename(string path, string userName, string password, string newName)
        {
            GetDirectoryEntry(path, userName, password).Rename(AdUserProperty.COMMON_NAME + newName);
        }


        /// <summary>
        ///     Rename an Object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newName"></param>
        public void Rename(string path, string newName)
        {
            var child = GetDirectoryEntry(path);
            child.Rename(AdUserProperty.COMMON_NAME + newName);
        }

        /// <summary>
        ///     create new local account
        /// </summary>
        public void CreateLocalAccount(string path, string password)
        {
            var localMachine = GetDirectoryEntry(path);
            var newUser = localMachine.Children.Add(AdUserProperty.LOCAL_USER, AdUserProperty.USER_SCHEMA);
            newUser.Invoke(AdUserProperty.SET_PASSWORD, password);
            newUser.CommitChanges();
            localMachine.Close();
            newUser.Close();
        }

        public IEnumerable<DirectoryEntry> FindLocalGroup(string path, string groupName)
        {
            var localMachine = GetDirectoryEntry(path);

            var admGroup = localMachine.Children.Find
                (groupName, AdUserProperty.GROUP_SCHEMA);
            var members = admGroup.Invoke("members", null);

            foreach (var groupMember in (IEnumerable)members)
                using (var member = new DirectoryEntry(groupMember))
                {
                    yield return member;
                }
        }

        /// <summary>
        ///     Create New Virtual Directory in IIS with DirectoryEntry()
        /// </summary>
        public void CreateIisVirtualDirectory(string directoryName, string siteName)
        {
            var wwwroot = "c:\\Inetpub\\wwwroot";

            using (var vRoot = new DirectoryEntry(siteName))
            {
                var vDir = vRoot.Children.Add(directoryName,
                    "IIsWebVirtualDir");
                vDir.CommitChanges();

                vDir.Properties["Path"].Value = wwwroot + "\\" + directoryName;
                vDir.Properties["DefaultDoc"].Value = "Default.aspx";
                vDir.Properties["DirBrowseFlags"].Value = 2147483648;
                vDir.CommitChanges();
                vRoot.CommitChanges();
            }
        }

        /// <summary>
        ///     Translate the Friendly Domain Name to Fully Qualified Domain
        /// </summary>
        /// <param name="friendlyDomainName"></param>
        /// <returns></returns>
        public static string FriendlyDomainToLdapDomain(string friendlyDomainName)
        {
            string ldapPath = null;
            try
            {
                var objContext = new DirectoryContext(
                    DirectoryContextType.Domain, friendlyDomainName);
                var objDomain = Domain.GetDomain(objContext);
                ldapPath = objDomain.Name;
            }
            catch (DirectoryServicesCOMException e)
            {
                ldapPath = e.Message;
            }

            return ldapPath;
        }

        /// <summary>
        ///     Enumerate Domains in the Current Forest
        /// </summary>
        /// <returns></returns>
        public static ArrayList EnumerateDomains()
        {
            var alDomains = new ArrayList();
            var currentForest = Forest.GetCurrentForest();
            var myDomains = currentForest.Domains;

            foreach (Domain objDomain in myDomains) alDomains.Add(objDomain.Name);

            return alDomains;
        }

        /// <summary>
        ///     Enumerate Global Catalogs in the Current Forest
        /// </summary>
        /// <returns></returns>
        public static ArrayList EnumerateForest()
        {
            var alGCs = new ArrayList();
            var currentForest = Forest.GetCurrentForest();
            foreach (GlobalCatalog gc in currentForest.GlobalCatalogs) alGCs.Add(gc.Name);

            return alGCs;
        }

        /// <summary>
        ///     Enumerate Domain Controllers in a Domain
        /// </summary>
        /// <returns></returns>
        public static ArrayList EnumerateDomainControllers()
        {
            var alDcs = new ArrayList();
            var domain = Domain.GetCurrentDomain();
            foreach (DomainController dc in domain.DomainControllers) alDcs.Add(dc.Name);

            return alDcs;
        }


        /// <summary>
        ///     Create a Trust Relationship
        /// </summary>
        /// <param name="sourceForestName"></param>
        /// <param name="targetForestName"></param>
        public void CreateTrust(string sourceForestName, string targetForestName)
        {
            var sourceForest = Forest.GetForest(new DirectoryContext(
                DirectoryContextType.Forest, sourceForestName));

            var targetForest = Forest.GetForest(new DirectoryContext(
                DirectoryContextType.Forest, targetForestName));

            // create an inbound forest trust

            sourceForest.CreateTrustRelationship(targetForest,
                TrustDirection.Outbound);
        }


        /// <summary>
        ///     Delete a Trust Relationship
        /// </summary>
        /// <param name="sourceForestName"></param>
        /// <param name="targetForestName"></param>
        public void DeleteTrust(string sourceForestName, string targetForestName)
        {
            var sourceForest = Forest.GetForest(new DirectoryContext(
                DirectoryContextType.Forest, sourceForestName));

            var targetForest = Forest.GetForest(new DirectoryContext(
                DirectoryContextType.Forest, targetForestName));

            // delete forest trust

            sourceForest.DeleteTrustRelationship(targetForest);
        }

        /// <summary>
        ///     Enumerate Objects in an OU
        ///     The parameter OuDn is the Organizational Unit distinguishedName such as OU=Users,dc=myDomain,dc=com
        /// </summary>
        /// <param name="ouDn"></param>
        /// <returns></returns>
        public ArrayList EnumerateOu(string ouDn)
        {
            var alObjects = new ArrayList();
            try
            {
                var directoryObject = GetDirectoryEntry(ouDn);
                foreach (DirectoryEntry child in directoryObject.Children)
                {
                    var childPath = child.Path;
                    alObjects.Add(childPath.Remove(0, 7));
                    //remove the LDAP prefix from the path

                    child.Close();
                    child.Dispose();
                }

                directoryObject.Close();
                directoryObject.Dispose();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new DirectoryServicesCOMException("An Error Occurred: " + e.Message);
            }

            return alObjects;
        }

        public Dictionary<string, string> DirectoryEntryConfigurationSettings(string path)
        {
            // Bind to current domain

            using (var entry = GetDirectoryEntry(path))
            {
                var entryConfiguration = entry.Options;
                var serverInfo = new Dictionary<string, string>
                {
                    {"Server: ", entryConfiguration.GetCurrentServerName()},
                    {"Page Size: ", entryConfiguration.PageSize.ToString()},
                    {"Password Encoding: ", entryConfiguration.PasswordEncoding.ToString()},
                    {"Password Port: ", entryConfiguration.PasswordPort.ToString()},
                    {"Referral: ", entryConfiguration.Referral.ToString()},
                    {"Security Masks: ", entryConfiguration.SecurityMasks.ToString()},
                    {"Is Mutually Authenticated: ", entryConfiguration.IsMutuallyAuthenticated().ToString()}
                };

                return serverInfo;
            }
        }

        /// <summary>
        ///     Check for the Existence of an Object
        ///     It should be noted that the string newLocation should NOT include the CN= value of the object.
        ///     The method will pull that from the objectLocation string for you.
        ///     So object CN=group,OU=GROUPS,DC=contoso,DC=com is sent in as the objectLocation
        ///     but the newLocation is something like: OU=NewOUParent,DC=contoso,DC=com.
        ///     The method will take care of the CN=group.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Exists(string path)
        {
            var found = DirectoryEntry.Exists(path);
            return found;
        }

        /// <summary>
        ///     Move an Object from one Location to Another
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newPath"></param>
        public void Move(string path, string newPath)
        {
            //For brevity, removed existence checks

            var eLocation = GetDirectoryEntry(path);
            var nLocation = GetDirectoryEntry(newPath);
            var newName = eLocation.Name;
            eLocation.MoveTo(nLocation, newName);
            nLocation.Close();
            eLocation.Close();
        }

        /// <summary>
        ///     Enumerate Multi-String Attribute Values of an Object
        ///     This method includes a recursive flag in case you want to recursively dig up properties
        ///     of properties such as enumerating all the member values of a group and
        ///     then getting each member group's groups all the way up the tree.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="path"></param>
        /// <param name="valuesCollection"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public ArrayList AttributeValuesMultiString(string path, string attributeName, ArrayList valuesCollection,
            bool recursive)
        {
            var ent = GetDirectoryEntry(path);
            var valueCollection = ent.Properties[attributeName];
            var en = valueCollection.GetEnumerator();

            while (en.MoveNext())
                if (en.Current != null)
                    if (!valuesCollection.Contains(en.Current.ToString()))
                    {
                        valuesCollection.Add(en.Current.ToString());
                        if (recursive)
                            AttributeValuesMultiString("LDAP://" + en.Current, attributeName, valuesCollection, true);
                    }

            ent.Close();
            ent.Dispose();
            return valuesCollection;
        }

        /// <summary>
        ///     Enumerate Single String Attribute Values of an Object
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="objectDn"></param>
        /// <returns></returns>
        public string AttributeValuesSingleString(string attributeName, string objectDn)
        {
            string strValue;
            var ent = new DirectoryEntry(objectDn);
            strValue = ent.Properties[attributeName].Value.ToString();
            ent.Close();
            ent.Dispose();
            return strValue;
        }


        /// <summary>
        ///     Enumerate an Object's Properties: The Ones with Values
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ArrayList GetUsedAttributes(string path)
        {
            var objRootDse = GetDirectoryEntry(path);
            var props = new ArrayList();

            foreach (string strAttrName in objRootDse.Properties.PropertyNames) props.Add(strAttrName);
            return props;
        }

        /// <summary>
        ///     Get an Object DistinguishedName: ADO.NET search (ADVANCED)
        /// </summary>
        /// <param name="objectCls"></param>
        /// <param name="returnValue"></param>
        /// <param name="objectName"></param>
        /// <param name="LdapDomain"></param>
        /// <returns></returns>
        public string GetObjectDistinguishedName(string path, ObjectClass objectCls, ReturnType returnValue,
            string objectName)
        {
            var distinguishedName = string.Empty;
            var entry = new DirectoryEntry(path);
            var mySearcher = new DirectorySearcher(entry);

            switch (objectCls)
            {
                case ObjectClass.User:
                    mySearcher.Filter = "(&(objectClass=user)(|(cn = " + objectName + ")(sAMAccountName = " +
                                        objectName + ")))";
                    break;
                case ObjectClass.Group:
                    mySearcher.Filter = "(&(objectClass=group)(|(cn = " + objectName + ")(dn = " + objectName + ")))";
                    break;
                case ObjectClass.Computer:
                    mySearcher.Filter =
                        "(&(objectClass=computer)(|(cn = " + objectName + ")(dn = " + objectName + ")))";
                    break;
            }

            var result = mySearcher.FindOne();

            if (result == null)
                throw new NullReferenceException
                ("unable to locate the distinguishedName for the object " +
                 objectName + " in the " + path + " domain");
            var directoryObject = result.GetDirectoryEntry();
            if (returnValue.Equals(ReturnType.DistinguishedName))
                distinguishedName = "LDAP://" + directoryObject.Properties["distinguishedName"].Value;
            if (returnValue.Equals(ReturnType.ObjectGuid)) distinguishedName = directoryObject.Guid.ToString();
            entry.Close();
            entry.Dispose();
            mySearcher.Dispose();
            return distinguishedName;
        }

        /// <summary>
        ///     Convert distinguishedName to ObjectGUID
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ConvertDNtoGuid(string path)
        {
            //Removed logic to check existence first

            var directoryObject = new DirectoryEntry(path);
            return directoryObject.Guid.ToString();
        }


        /// <summary>
        ///     Convert an ObjectGUID to OctectString: The Native ObjectGUID
        /// </summary>
        /// <param name="objectGuid"></param>
        /// <returns></returns>
        public static string ConvertGuidToOctectString(string objectGuid)
        {
            var guid = new Guid(objectGuid);
            var byteGuid = guid.ToByteArray();
            var queryGuid = "";
            foreach (var b in byteGuid) queryGuid += @"\" + b.ToString("x2");
            return queryGuid;
        }

        /// <summary>
        ///     Search by ObjectGUID or convert ObjectGUID to distinguishedName
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ConvertGuidToDn(string guid)
        {
            var ent = new DirectoryEntry();
            var ADGuid = ent.NativeGuid;
            var x = new DirectoryEntry("LDAP://{GUID=" + ADGuid + ">");
            //change the { to <>

            return x.Path.Remove(0, 7); //remove the LDAP prefix from the path
        }


        /// <summary>
        /// </summary>
        private void init()
        {
            CreateShareEntry("OU=HOME,dc=baileysoft,dc=com",
                "Music", @"\\192.168.2.1\Music", "mp3 Server Share");
            Console.ReadLine();
        }

        /// <summary>
        ///     Publish Network Shares in Active Directory
        /// </summary>
        /// <param name="ldapPath"></param>
        /// <param name="shareName"></param>
        /// <param name="shareUncPath"></param>
        /// <param name="shareDescription"></param>
        public void CreateShareEntry(string ldapPath, string shareName, string shareUncPath, string shareDescription)
        {
            var oGUID = string.Empty;
            var connectionPrefix = "LDAP://" + ldapPath;
            var directoryObject = new DirectoryEntry(connectionPrefix);
            var networkShare = directoryObject.Children.Add("CN=" +
                                                            shareName, "volume");
            networkShare.Properties["uNCName"].Value = shareUncPath;
            networkShare.Properties["Description"].Value = shareDescription;
            networkShare.CommitChanges();

            directoryObject.Close();
            networkShare.Close();
        }

        /// <summary>
        ///     Create a New Security Group
        ///     Note: by default if no GroupType property is set, the group is created as a domain security group.
        /// </summary>
        /// <param name="ouPath"></param>
        /// <param name="name"></param>
        public void Create(string ouPath, string name)
        {
            if (!DirectoryEntry.Exists("LDAP://CN=" + name + "," + ouPath))
                try
                {
                    var entry = new DirectoryEntry(ouPath);
                    var group = entry.Children.Add("CN=" + name, "group");
                    group.Properties["sAmAccountName"].Value = name;
                    group.CommitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            else
                throw new Exception(ouPath + " already exists");
        }

        public void Delete(string ouPath, string groupPath)
        {
            if (DirectoryEntry.Exists(groupPath))
                try
                {
                    var entry = GetDirectoryEntry(ouPath);
                    var group = GetDirectoryEntry(groupPath);
                    entry.Children.Remove(group);
                    group.CommitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            else
                throw new Exception(ouPath + " doesn't exist");
        }


        /// <summary>
        ///     Authenticate a User Against the Directory
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool Authenticate(string path, string userName, string password)
        {
            var authentic = false;
            try
            {
                var entry = GetDirectoryEntry(path, userName, password);
                var nativeObject = entry.NativeObject;
                authentic = true;
            }
            catch (DirectoryServicesCOMException)
            {
            }

            return authentic;
        }

        /// <summary>
        ///     Add User to Group
        /// </summary>
        /// <param name="userDn"></param>
        /// <param name="groupDnPath"></param>
        public void AddToGroup(string groupDnPath, string userDn)
        {
            try
            {
                var dirEntry = GetDirectoryEntry(groupDnPath);
                dirEntry.Properties["member"].Add(userDn);
                dirEntry.CommitChanges();
                dirEntry.Close();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new Exception(e.Message);
            }
        }

        //Remove User from Group
        public void RemoveUserFromGroup(string groupDnPath, string userDn)
        {
            try
            {
                var dirEntry = GetDirectoryEntry(groupDnPath);
                dirEntry.Properties["member"].Remove(userDn);
                dirEntry.CommitChanges();
                dirEntry.Close();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///     Get User Group Memberships of the Logged in User from ASP.NET
        /// </summary>
        /// <returns></returns>
        public ArrayList Groups()
        {
            var groups = new ArrayList();
            if (HttpContext.Current.Request.LogonUserIdentity?.Groups == null) return groups;
            foreach (var group in
                HttpContext.Current.Request.LogonUserIdentity?.Groups)
                groups.Add(group.Translate(typeof
                    (NTAccount)).ToString());

            return groups;
        }

        /// <summary>
        ///     Get User Group Memberships
        /// </summary>
        /// <param name="userDn"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public ArrayList Groups(string userDn, bool recursive)
        {
            var groupMemberships = new ArrayList();
            return AttributeValuesMultiString("memberOf", userDn,
                groupMemberships, recursive);
        }

        /// <summary>
        ///     Create User Account
        /// </summary>
        /// <param name="ldapPath"></param>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public string CreateUserAccount(string ldapPath, string userName, string userPassword)
        {
            try
            {
                var oGUID = string.Empty;
                var connectionPrefix = "LDAP://" + ldapPath;
                var dirEntry = GetDirectoryEntry(connectionPrefix);
                var newUser = dirEntry.Children.Add
                    ("CN=" + userName, "user");
                newUser.Properties["samAccountName"].Value = userName;
                newUser.CommitChanges();
                oGUID = newUser.Guid.ToString();

                newUser.Invoke("SetPassword", userPassword);
                newUser.CommitChanges();
                dirEntry.Close();
                newUser.Close();
                return oGUID;
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///     Enable a User Account
        /// </summary>
        /// <param name="userDnPath"></param>
        public void Enable(string userDnPath)
        {
            try
            {
                var user = GetDirectoryEntry(userDnPath);
                var val = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = val & ~0x2;
                //ADS_UF_NORMAL_ACCOUNT;

                user.CommitChanges();
                user.Close();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///     Disable a User Account
        /// </summary>
        /// <param name="userDnPath"></param>
        public void Disable(string userDnPath)
        {
            try
            {
                var user = GetDirectoryEntry(userDnPath);
                var val = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = val | 0x2;
                //ADS_UF_ACCOUNTDISABLE;

                user.CommitChanges();
                user.Close();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///     Unlock a User Account
        /// </summary>
        /// <param name="userDnPath"></param>
        public void Unlock(string userDnPath)
        {
            try
            {
                var uEntry = GetDirectoryEntry(userDnPath);
                uEntry.Properties["LockOutTime"].Value = 0; //unlock account

                uEntry.CommitChanges(); //may not be needed but adding it anyways

                uEntry.Close();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///     Reset a User Password
        /// </summary>
        /// <param name="userDnPath"></param>
        /// <param name="password"></param>
        public void ResetPassword(string userDnPath, string password)
        {
            var uEntry = GetDirectoryEntry(userDnPath);
            uEntry.Invoke(AdUserProperty.SET_PASSWORD, password);
            uEntry.Properties["LockOutTime"].Value = 0; //unlock account

            uEntry.Close();
        }

        /// <summary>
        ///     Gets or sets a value indicating if the user account is locked out
        /// </summary>
        public bool IsLocked(string path)
        {
            return Convert.ToBoolean(GetDirectoryEntry(path).InvokeGet("IsAccountLocked"));
        }

        public void SetLockState(string serverPath, params object[] values)
        {
            GetDirectoryEntry(serverPath).InvokeSet("IsAccountLocked", values);
        }
    }
}