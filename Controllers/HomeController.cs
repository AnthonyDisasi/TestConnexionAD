using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using TestConnexionAD.Models;
using System.Security.Principal;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;

namespace TestConnexionAD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult HomePage()
        {
            return View();
        }
        //public List<GroupPrincipal> GetGroups(string userName = "Antoine.Disasi")
        public List<string> GetGroups(string userName = "Antoine.Disasi")
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();
            List<string> groupsp = new List<string>();

            // establish domain context
            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain);

            // find your user
            UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, userName);

            // if found - grab its groups
            if (user != null)
            {
                PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

                // iterate over all groups
                foreach (Principal p in groups)
                {
                    // make sure to add only group principals
                    if (p is GroupPrincipal)
                    {
                        groupsp.Add(p.Name  );
                        result.Add((GroupPrincipal)p);
                    }
                }
            }

            return groupsp;
        }
        public List<string> GetGroupse(string userName = "Antoine.Disasi")
        {
            List<string> result = new List<string>();
            WindowsIdentity wi = new WindowsIdentity(userName = "Antoine.Disasi");

            foreach (IdentityReference group in wi.Groups)
            {
                try
                {
                    result.Add(group.Translate(typeof(NTAccount)).ToString());
                }
                catch (Exception ex) { }
            }
            result.Sort();
            return result;
        }

        public string GetDepartment(string username = "Antoine.Disasi")
        {
            string result = string.Empty;

            // if you do repeated domain access, you might want to do this *once* outside this method, 
            // and pass it in as a second parameter!
            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain);

            // find the user
            UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, username);

            // if user is found
            if (user != null)
            {
                // get DirectoryEntry underlying it
                DirectoryEntry de = (user.GetUnderlyingObject() as DirectoryEntry);

                if (de != null)
                {
                    if (de.Properties.Contains("department"))
                    {
                        result = de.Properties["department"][0].ToString();
                    }
                }
            }

            return result;
        }

        public string ValidateCredentials(string userName = "Antoine.Diksasi", string password = "Dis@si2304a")
        {
            try
            {
                using (var adContext = new PrincipalContext(ContextType.Domain, "CONGO-EQUIPMENT"))
                {
                    return (adContext.ValidateCredentials(userName, password)).ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
