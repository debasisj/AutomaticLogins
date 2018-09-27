using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AutomaticLogins
{
    class Program
    {
        static void Main(string[] args)
        {
            string browserType, app, userId, pwd, url = null, envToSelect = null;


            try
            {
            
                if (args[0].ToLower(CultureInfo.InvariantCulture).IndexOf("help", System.StringComparison.Ordinal) >= 0)
                {
                    Console.WriteLine("--------------------Argument Details-------------------------\n");
                    string[] parameterNumber = { "1.", "2.", "3.", "4.","5."};
                    string[] parameterName = { "Browser", "Application", "UserId", "Password", "Url" };
                    string[] parameterOption = { "Mandatory", "Mandatory", "Mandatory", "Mandatory", "Optional" };
                    string[] possibleValues = { "Chrome, Firefox, IE", "NA", "NA", "NA", "NA" };

                    Console.WriteLine("{0,-3} {1,10} {2,10} {3,10}\n", "No.", "Name", "Option", "Values");
                    for (int ctr = 0; ctr < parameterNumber.Length; ctr++)
                    {
                        Console.WriteLine("{0,-3} {1,10:N1} {2,10:N2} {3,10:N3}", parameterNumber[ctr], parameterName[ctr], parameterOption[ctr], possibleValues[ctr]);
                    }
                       
                  
                }
                else
                {
                    var parsedArgs = args.Select(s => s.Split(new[] { '-' })).ToDictionary(s => s[0].ToLower(), s => s[1]);
                    if (args.Length < 4)
                    {
                        throw new ArgumentException("Missing arguments");
                    }
                    else
                    {
                      
                        try
                        {
                             browserType = parsedArgs.Single(x => x.Key == "browser").Value;
                        }
                        catch (Exception)
                        {

                            throw new ArgumentException("browser type is missing, for example command should be browser:chrome");
                        }


                        try
                        {
                             app = parsedArgs.Single(x => x.Key == "app").Value;
                        }
                        catch (Exception)
                        {

                            throw new ArgumentException("app type is missing, for example command should be app:jira");
                        }


                        try
                        {
                             userId = parsedArgs["userid"];
                        }
                        catch (Exception)
                        {

                            throw new ArgumentException("user id is missing, for example command should be userid:someuserid");
                        }


                        try
                        {
                             pwd = parsedArgs["pwd"];
                        }
                        catch (Exception)
                        {

                            throw new ArgumentException("password is missing, for example command should be pwd:somepassword");
                        }

                        try
                        {
                            if(args.Length >= 5)
                            {
                                if(parsedArgs.ContainsKey("url"))
                                {
                                    url = parsedArgs["url"];
                                }
                                //else
                                //{
                                //    url = "";
                                //}
                            }
                                //url = args.Length >= 5 ? parsedArgs["url"] : "";
                        }
                        catch (Exception)
                        {

                            throw new ArgumentException("url is optional but if passed it should be url:someUrl");
                        }

                        try
                        {
                            if (args.Length >= 5)
                            {
                                if (parsedArgs.ContainsKey("env"))
                                {
                                    envToSelect = parsedArgs["env"];
                                }
                     
                            }
                            //envToSelect = args.Length >= 5 ? parsedArgs["env"] : "";
                        }
                        catch (Exception)
                        {

                            throw new ArgumentException("env is optional but if passed it should be env:exact name of the env from list");
                        }



                        var builder = new ConfigurationBuilder().SetBasePath
                                            (Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json");
                        var Configuration = builder.Build();
                        AppVals appVals = new AppVals();
                        string sectionToGetVals = "Apps:" + app;

                        if (!Configuration.GetSection(sectionToGetVals).Exists())
                        {

                            throw new ArgumentException(app + " is not a valid application");
                        }
                        Configuration.GetSection(sectionToGetVals).Bind(appVals);
                        //if (url == null)
                        //{
                        //    InitDriver.Init(browserType, userId, pwd, appVals);
                        //}
                        //else
                        //{
                            InitDriver.Init(driverTyper: browserType, userId: userId, pwd: pwd, appVals:appVals, url:url, envVal:envToSelect);
                        //}
                        //if (appVals.selectEnv)
                        //{

                        //}
                       
                    }
                }
                
            }
            catch (ArgumentException ex)
            {

                Console.WriteLine(ex.ToString());
            }
            //catch if any other exception 
            catch (Exception moreExcpetion)
            {
                Console.WriteLine(moreExcpetion.ToString());
            }

        }
    }

    public class AppVals
    {
        public string alias { get; set; }
        public string url { get; set; }
        public bool selectEnv { get; set; }
        public string findBy { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string selectPath { get; set; }
        public string selectBy { get; set; }
        public string defaultSelectEnv { get; set; }
        public string submit { get; set; }
    }
}
