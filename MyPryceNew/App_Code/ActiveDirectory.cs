using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.DirectoryServices;




/// <summary>
/// Summary description for ActiveDirectory
/// </summary>
public class ActiveDirectory
{
    public ActiveDirectory()
    {
        //
        // TODO: Add constructor logic here
        //
    }



    public  class Users
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public bool isMapped { get; set; }
    }



    public  List<Users> GetADUsers(string DomainPath)
    {

        //List<Users> lstADUsers = new List<Users>();

        //DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
        //DirectorySearcher search = new DirectorySearcher(searchRoot);
        //search.Filter = "(&(objectClass=user)(objectCategory=person))";
        //search.PropertiesToLoad.Add("samaccountname");
        //search.PropertiesToLoad.Add("mail");
        //search.PropertiesToLoad.Add("usergroup");
        //search.PropertiesToLoad.Add("displayname");//first name
        //SearchResult result;
        //SearchResultCollection resultCol = search.FindAll();
        //if (resultCol != null)
        //{
        //    for (int counter = 0; counter < resultCol.Count; counter++)
        //    {
        //        string UserNameEmailString = string.Empty;
        //        result = resultCol[counter];
        //        if (result.Properties.Contains("samaccountname") &&
        //                 result.Properties.Contains("mail") &&
        //            result.Properties.Contains("displayname"))
        //        {
        //            Users objSurveyUsers = new Users();
        //            objSurveyUsers.Email = (String)result.Properties["mail"][0] +
        //              "^" + (String)result.Properties["displayname"][0];
        //            objSurveyUsers.UserName = (String)result.Properties["samaccountname"][0];
        //            objSurveyUsers.DisplayName = (String)result.Properties["displayname"][0];
        //            lstADUsers.Add(objSurveyUsers);
        //        }
        //    }
        //}
        //return lstADUsers;
        return null;

    }
}